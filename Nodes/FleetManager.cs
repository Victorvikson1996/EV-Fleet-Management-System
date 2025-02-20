public class FleetManager
{
    private readonly List<VehicleNode> _vehicles = new();
    private readonly List<ChargingStation> _stations = new();
    private readonly EventBus _eventBus;

    public FleetManager(EventBus eventBus)
    {
        _eventBus = eventBus;
        InitializeFleet();
        SubscribeToEvents();
    }

    private void InitializeFleet()
    {
        for (int i = 0; i < 2; i++)
        {
            _vehicles.Add(new VehicleNode(_eventBus, new ServiceRegistry(), new FsmService()));
            _stations.Add(new ChargingStation(_eventBus, new ServiceRegistry()));
        }
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<LocationUpdate>("locationUpdate", data =>
        {
            Console.WriteLine($"Vehicle {data.VehicleId} at location: {data.Location}");
        });

        _eventBus.Subscribe<LowBattery>("lowBattery", data =>
        {
            Console.WriteLine($"Vehicle {data.VehicleId} has low battery. Sending to charge.");
            SendToCharge(data.VehicleId);
        });
    }

    public void SendToCharge(string vehicleId)
    {
        var stations = _stations.Where(s => s._isAvailable).ToList();
        if (stations.Any())
        {
            _eventBus.Publish("chargeRequest", new ChargingStation.ChargeRequest(vehicleId, stations[0]._id));
        }
    }

    public void StartDriving(string vehicleId)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v._id == vehicleId);
        vehicle?.Drive();
    }

    public record LocationUpdate(string VehicleId, (double Lat, double Lng) Location);
    public record LowBattery(string VehicleId);
}