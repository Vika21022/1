using DeviceDashboard;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DeviceOptions>(builder.Configuration.GetSection("Devices"));
builder.Services.AddHttpClient("device", client =>
{
    client.Timeout = TimeSpan.FromSeconds(3);
});
builder.Services.AddSingleton<DeviceMonitorService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<DeviceMonitorService>());

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/devices", (DeviceMonitorService monitor) =>
{
    var snapshots = monitor.GetSnapshots();
    return Results.Ok(new
    {
        onlineCount = snapshots.Count(x => x.Online),
        totalCount = snapshots.Count,
        devices = snapshots
    });
});

app.Run();
