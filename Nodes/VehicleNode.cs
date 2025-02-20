using Stateless;

public class VehicleNode
{
    private readonly string _id;
    private readonly StateMachine<string, string> _stateMachine;
    private readonly GpsDriver _gpsDriver;
    private readonly BatteryDriver _batteryDriver;
    private readonly ChargingActuator _chargingActuator;
    private readonly MovementActuator _movementActuator;
    private readonly EventBus _eventBus;
    private readonly ServiceRegistry _serviceRegistry;

    public VehicleNode(EventBus eventBus, ServiceRegistry serviceRegistry, FsmService fsmService)
    {
        _eventBus = eventBus;
        _serviceRegistry = serviceRegistry;
        _id = Utils.GenerateId();
        _gpsDriver = new GpsDriver(eventBus);
        _batteryDriver = new BatteryDriver(eventBus);
        _chargingActuator = new ChargingActuator(_batteryDriver);
        _movementActuator = new MovementActuator(_gpsDriver);
        _stateMachine = fsmService.CreateVehicleStateMachine();

        _stateMachine.OnTransitioned((transition) =>
        {
            Console.WriteLine($"Vehicle {_id} state: {transition.Destination}");
            UpdateStatus();
            HandleStateChange();
        });

        SubscribeToEvents();
        RegisterWithRegistry();
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<DriveCommand>("driveCommand", data =>
        {
            if (data.VehicleId == _id)
                _stateMachine.Fire("START");
        });

        _eventBus.Subscribe<ChargeCommand>("chargeCommand", data =>
        {
            if (data.VehicleId == _id)
                _stateMachine.Fire("CHARGE");
        });
    }

    private void RegisterWithRegistry()
    {
        _serviceRegistry.Register(new ServiceRegistry.Service(
            _id, "vehicle", _gpsDriver.GetLocation(), _stateMachine.State));
    }

    private void UpdateStatus()
    {
        var service = _serviceRegistry.GetService(_id);
        if (service != null)
        {
            service = service with { Status = _stateMachine.State };
            _serviceRegistry.Register(service);
        }
    }

    private void HandleStateChange()
    {
        if (_stateMachine.State == "driving")
        {
            _movementActuator.Move(_gpsDriver.GetLocation().Lat + 0.1, _gpsDriver.GetLocation().Lng + 0.1);
            _batteryDriver.Drain(5);
            _stateMachine.Fire("DRIVE");
        }
        else if (_stateMachine.State == "charging")
        {
            _chargingActuator.Charge(10);
        }

        if (_batteryDriver.GetBatteryLevel() < 20)
        {
            _eventBus.Publish("lowBattery", new { VehicleId = _id });
        }
    }

    public void Drive() => _stateMachine.Fire("START");
    public void Charge() => _stateMachine.Fire("CHARGE");

    public record DriveCommand(string VehicleId);
    public record ChargeCommand(string VehicleId);
}