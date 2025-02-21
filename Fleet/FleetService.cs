public class FleetService : FleetManager
{
    public FleetService(EventBus eventBus) : base(eventBus) { }

    public object GetFleetStatus()
    {
        return new
        {
            Vehicles = _vehicles.Select(v => new { Id = v._id, Status = v.StateMachine.State }),
            Stations = _stations.Select(s => new { Id = s.id, IsAvailable = s.IsAvailable })

        };
    }
}