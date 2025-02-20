public class BatteryDriver
{
    private double _level = 100;
    private readonly EventBus _eventBus;

    public BatteryDriver(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public double GetBatteryLevel() => _level;

    public void SetBatteryLevel(double level)
    {
        _level = Math.Max(0, Math.Min(100, level));
        _eventBus.Publish("batteryUpdate", new { Level = _level });
        if (_level < 20)
        {
            _eventBus.Publish("lowBattery", new FleetManager.LowBattery("vehicle1")); // Hardcoded for demo
        }
    }

    public void Charge(double amount) => SetBatteryLevel(_level + amount);

    public void Drain(double amount) => SetBatteryLevel(_level - amount);
}