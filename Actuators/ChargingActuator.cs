public class ChargingActuator
{
    private readonly BatteryDriver _batteryDriver;

    public ChargingActuator(BatteryDriver batteryDriver)
    {
        _batteryDriver = batteryDriver;
    }

    public void Charge(double amount) => _batteryDriver.Charge(amount);
}