namespace EVFleetManagement.ChargingStations
{
    public class ChargingStation
    {
        public int Id { get; set; }
        public string LocationJson { get; set; } = "{\"Lat\": 0, \"Lng\": 0}"; // Store as JSON
        public bool IsAvailable { get; set; } = true;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}