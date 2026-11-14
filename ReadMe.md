Run the device APIs:
```cmd
dotnet run --launch-profile Device1

dotnet run --launch-profile Device2
```

Open `http://localhost:1235/info` and `http://localhost:1237/info`. You should see the json output.

Run the device monitoring dashboard:
```cmd
dotnet run --project DeviceDashboard
```

Open `http://localhost:5000`. The dashboard polls the configured devices every 5 seconds and shows
the status of each device, its key information (address, uptime, temperature) and the total number
of online devices. The device list is configured in `DeviceDashboard/appsettings.json`.
