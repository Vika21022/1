namespace DeviceDashboard;

public sealed record DeviceInfo(string Name, string Address, Dictionary<string, string> Metrics);
