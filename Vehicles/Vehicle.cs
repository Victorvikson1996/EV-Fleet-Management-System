using Microsoft.EntityFrameworkCore;

public class Vehicle
{
    public int Id { get; set; }
    public string LocationJson { get; set; } = "{\"Lat\": 0, \"Lng\": 0}"; // Store as JSON
    public double BatteryLevel { get; set; } = 100;
    public string Status { get; set; } = "idle"; // idle, driving, charging
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    internal static object Select(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}