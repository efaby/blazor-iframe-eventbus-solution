# Blazor Host + Iframe (Typed C# Event Bus)

Run (requires .NET 8 SDK):

Terminal A:
```bash
dotnet run --project IframeApp
```

Terminal B:
```bash
dotnet run --project HostApp
```

Open:
- Host: https://localhost:5001/iframe-host
- Iframe (direct): https://localhost:5002/iframe-client

Notes:
- Origin allowlist is enforced in `wwwroot/iframeBusInterop.js`.
- Host sends: `links:update`, `nav:go`
- Iframe sends: `ready`, `link:clicked`, `nav:request`
