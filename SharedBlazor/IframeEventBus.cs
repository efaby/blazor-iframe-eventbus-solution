using System;
using System.Text.Json;
using Microsoft.JSInterop;
using Shared;

namespace SharedBlazor;

public class IframeEventBus : IIframeEventBus
{
    private readonly IJSRuntime _js;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private IJSObjectReference? _module;
    private DotNetObjectReference<IframeEventBus>? _dotNetRef;

    private readonly Dictionary<string, Type> _typeMap = new();
    private readonly Dictionary<string, List<Func<object, Task>>> _handlers = new();

    public IframeEventBus(IJSRuntime js) => _js = js;

    public void Register<TPayload>(string type) => _typeMap[type] = typeof(TPayload);

    public void On<TPayload>(string type, Func<TPayload, Task> handler)
    {
        if (!_handlers.TryGetValue(type, out var list))
            _handlers[type] = list = new();
        list.Add(obj => handler((TPayload)obj));
    }

    public async Task StartAsync(IEnumerable<string> allowedOrigins)
    {
        _module = await _js.InvokeAsync<IJSObjectReference>("import", "./iframeBusInterop.js");
        _dotNetRef = DotNetObjectReference.Create(this);

        await _module.InvokeVoidAsync("configure", allowedOrigins.ToArray());
        await _module.InvokeVoidAsync("start", _dotNetRef);
    }

    public async Task PublishAsync<TPayload>(
    string type,
    TPayload payload,
    string targetOrigin
)
    {
        if (_module is null) throw new InvalidOperationException("Event bus not started.");
        await _module.InvokeVoidAsync("send", type, payload, targetOrigin);
    }

    [JSInvokable]
    public async Task ReceiveAsync(string type, JsonElement payload)
    {
        if (!_typeMap.TryGetValue(type, out var clrType)) return;
        var obj = payload.Deserialize(clrType, _json);
        if (obj is null) return;

        if (_handlers.TryGetValue(type, out var list))
            foreach (var h in list)
                await h(obj);
    }

    public async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        if (_module is not null) await _module.DisposeAsync();
    }

    public async Task PublishToIframeAsync<T>(
    string type,
    T payload,
    string targetOrigin)
    {
        await _module!.InvokeVoidAsync(
            "sendToIframe",
            type,
            payload,
            targetOrigin
        );
    }

    public async Task PublishToParentAsync<T>(
        string type,
        T payload,
        string targetOrigin)
    {
        await _module!.InvokeVoidAsync(
            "sendToParent",
            type,
            payload,
            targetOrigin
        );
    }

}
