public class GpsDriver
{
    private (double Lat, double Lng) _location = (0, 0);
    private readonly EventBus _eventBus;

    public GpsDriver(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public (double Lat, double Lng) GetLocation() => _location;

    public void UpdateLocation(double lat, double lng)
    {
        _location = (lat, lng);
        _eventBus.Publish("locationUpdate", new FleetManager.LocationUpdate("vehicle1", _location)); // Hardcoded for demo
    }
}