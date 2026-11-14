namespace DeviceDashboard;

public sealed class DeviceOptions
{
    public int PollIntervalSeconds { get; set; } = 5;

    public List<DeviceEndpoint> Endpoints { get; set; } = [];
}

public sealed class DeviceEndpoint
{
    public string Url { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
