public class BatterySensor
{
    private readonly BatteryDriver _batteryDriver;

    public BatterySensor(BatteryDriver batteryDriver)
    {
        _batteryDriver = batteryDriver;
    }

    public double GetBatteryLevel() => _batteryDriver.GetBatteryLevel();
}