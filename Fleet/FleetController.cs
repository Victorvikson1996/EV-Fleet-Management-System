using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FleetController : ControllerBase
{
    private readonly FleetManager _fleetManager;

    public FleetController(FleetManager fleetManager)
    {
        _fleetManager = fleetManager;
    }

    [HttpGet]
    public IActionResult GetFleetStatus()
    {
        return Ok(_fleetManager.GetFleetStatus());
    }

    [HttpPost("drive")]
    public IActionResult DriveVehicle([FromBody] DriveCommand command)
    {
        _fleetManager.StartDriving(command.VehicleId);
        return Ok(new { message = "Vehicle started driving" });
    }

    [HttpPost("charge")]
    public IActionResult ChargeVehicle([FromBody] ChargeCommand command)
    {
        _fleetManager.SendToCharge(command.VehicleId);
        return Ok(new { message = "Vehicle sent to charge" });
    }

    public record DriveCommand(string VehicleId);
    public record ChargeCommand(string VehicleId);
}