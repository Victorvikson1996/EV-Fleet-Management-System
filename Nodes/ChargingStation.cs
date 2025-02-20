public class ChargingStation
{
    private readonly string _id;
    private readonly (double Lat, double Lng) _location;
    private bool _isAvailable = true;
    private readonly GpsDriver _gpsDriver;
    private readonly EventBus _eventBus;
    private readonly ServiceRegistry _serviceRegistry;

    public ChargingStation(EventBus eventBus, ServiceRegistry serviceRegistry)
    {
        _eventBus = eventBus;
        _serviceRegistry = serviceRegistry;
        _id = Utils.GenerateId();
        _gpsDriver = new GpsDriver(eventBus);
        _location = _gpsDriver.GetLocation();
        RegisterWithRegistry();
        SubscribeToEvents();
    }

    private void RegisterWithRegistry()
    {
        _serviceRegistry.Register(new ServiceRegistry.Service(
            _id, "chargingStation", _location, _isAvailable ? "available" : "busy"));
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<ChargeRequest>("chargeRequest", data =>
        {
            if ((string.IsNullOrEmpty(data.StationId) || data.StationId == _id) && _isAvailable)
            {
                _isAvailable = false;
                UpdateStatus();
                _eventBus.Publish("chargeStarted", new { StationId = _id, VehicleId = data.VehicleId });
                Task.Delay(5000).ContinueWith(_ =>
                {
                    _isAvailable = true;
                    UpdateStatus();
                    _eventBus.Publish("chargeCompleted", new { StationId = _id, VehicleId = data.VehicleId });
                });
            }
        });
    }

    private void UpdateStatus()
    {
        var service = _serviceRegistry.GetService(_id);
        if (service != null)
        {
            service = service with { Status = _isAvailable ? "available" : "busy" };
            _serviceRegistry.Register(service);
        }
    }

    public record ChargeRequest(string VehicleId, string? StationId);
}