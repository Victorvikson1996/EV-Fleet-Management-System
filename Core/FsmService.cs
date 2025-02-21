using Stateless;


// Finite State Machine for vehicles using Stateless.

public class FsmService
{
    public StateMachine<string, string> CreateVehicleStateMachine()
    {
        var machine = new StateMachine<string, string>("idle");

        machine.Configure("idle")
            .Permit("START", "driving")
            .Permit("CHARGE", "charging");

        machine.Configure("driving")
            .Permit("STOP", "idle")
            .Permit("LOW_BATTERY", "charging")
            .OnEntry(() => Console.WriteLine("Vehicle is driving"))
            .OnExit(() => Console.WriteLine("Vehicle stopped driving"));

        machine.Configure("charging")
            .Permit("FINISH", "idle")
            .OnEntry(() => Console.WriteLine("Vehicle is charging"))
            .OnExit(() => Console.WriteLine("Charging finished"));
        return machine;
    }
}