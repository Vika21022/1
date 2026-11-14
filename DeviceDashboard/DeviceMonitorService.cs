using System.Collections.Concurrent;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace DeviceDashboard;

public sealed class DeviceMonitorService(IHttpClientFactory httpClientFactory, IOptions<DeviceOptions> options) : BackgroundService
{
    private readonly ConcurrentDictionary<string, DeviceSnapshot> _snapshots = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var endpoint in options.Value.Endpoints)
        {
            _snapshots.TryAdd(endpoint.Url, new DeviceSnapshot
            {
                Url = endpoint.Url,
                Name = endpoint.Name,
                Address = endpoint.Url
            });
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.WhenAll(options.Value.Endpoints.Select(e => PollAsync(e, stoppingToken)));
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, options.Value.PollIntervalSeconds)), stoppingToken);
        }
    }

    private async Task PollAsync(DeviceEndpoint endpoint, CancellationToken token)
    {
        var client = httpClientFactory.CreateClient("device");
        var snapshot = _snapshots[endpoint.Url];
        try
        {
            var info = await client.GetFromJsonAsync<DeviceInfo>($"{endpoint.Url.TrimEnd('/')}/info", token);
            snapshot.Name = info?.Name ?? endpoint.Name;
            snapshot.Address = info?.Address ?? endpoint.Url;
            snapshot.Online = true;
            if (info?.Metrics is not null)
            {
                if (int.TryParse(info.Metrics.GetValueOrDefault("UptimeSeconds"), out var uptime))
                {
                    snapshot.UptimeSeconds = uptime;
                }

                if (double.TryParse(info.Metrics.GetValueOrDefault("Temperature"), System.Globalization.CultureInfo.InvariantCulture, out var temperature))
                {
                    snapshot.Temperature = temperature;
                }
            }

            snapshot.LastOnline = DateTimeOffset.Now;
        }
        catch
        {
            snapshot.Online = false;
        }
    }

    public IReadOnlyCollection<DeviceSnapshot> GetSnapshots()
    {
        return _snapshots.Values.OrderBy(x => x.Name).ToList();
    }
}
