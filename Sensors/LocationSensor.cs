public class LocationSensor
{
    private readonly GpsDriver _gpsDriver;

    public LocationSensor(GpsDriver gpsDriver)
    {
        _gpsDriver = gpsDriver;
    }

    public (double Lat, double Lng) GetLocation() => _gpsDriver.GetLocation();

    public void UpdateLocation(double lat, double lng) => _gpsDriver.UpdateLocation(lat, lng);
}