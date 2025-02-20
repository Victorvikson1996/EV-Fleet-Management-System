public class MovementActuator
{
    private readonly GpsDriver _gpsDriver;

    public MovementActuator(GpsDriver gpsDriver)
    {
        _gpsDriver = gpsDriver;
    }

    public void Move(double lat, double lng) => _gpsDriver.UpdateLocation(lat, lng);
}