using System.Collections.Concurrent;

//Service Registry for tracking services.

public class ServiceRegistry
{
    private readonly ConcurrentDictionary<string, Service> _services = new();

    public record Service(string Id, string Type, (double Lat, double Lng) Location, string Status);

    public void Register(Service service)
    {
        _services[service.Id] = service;
        BroadcastUpdate();
    }

    public void Unregister(string id)
    {
        _services.TryRemove(id, out _);
        BroadcastUpdate();
    }

    public Service? GetService(string id)
    {
        return _services.TryGetValue(id, out var service) ? service : null;
    }

    public IEnumerable<Service> GetAllServices()
    {
        return _services.Values;
    }

    private void BroadcastUpdate()
    {
        // Simulate broadcasting via Event Bus
        var eventBus = new EventBus();
        eventBus.Publish("serviceUpdate", GetAllServices());
    }
}