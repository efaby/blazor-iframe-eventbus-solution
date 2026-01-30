using System.Text.Json;

namespace Shared;

public interface IIframeEventBus : IAsyncDisposable
{
    void Register<TPayload>(string type);
    void On<TPayload>(string type, Func<TPayload, Task> handler);
    Task StartAsync(IEnumerable<string> allowedOrigins);
    Task PublishAsync<TPayload>(string type, TPayload payload, string targetOrigin);

    Task PublishToIframeAsync<T>(
    string type,
    T payload,
    string targetOrigin);

    Task PublishToParentAsync<T>(
        string type,
        T payload,
        string targetOrigin);
}


