namespace DeviceDashboard;

public sealed class DeviceSnapshot
{
    public required string Url { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public bool Online { get; set; }

    public int UptimeSeconds { get; set; }

    public double Temperature { get; set; }

    public DateTimeOffset? LastOnline { get; set; }
}
