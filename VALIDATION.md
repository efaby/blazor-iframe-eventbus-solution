# Validation (what I can validate in this sandbox)

This environment does **not** include the .NET SDK, so I cannot run `dotnet build` here.

Validated:
- ✅ `.csproj` files are well-formed XML
- ✅ key source files exist

## csproj XML parse
- ✅ Shared/Shared.csproj
- ✅ HostApp/HostApp.csproj
- ✅ IframeApp/IframeApp.csproj

## Missing files
- (none)

## Full validation on your machine
```bash
dotnet build BlazorIframeEventBus.sln
dotnet run --project IframeApp
dotnet run --project HostApp
```
