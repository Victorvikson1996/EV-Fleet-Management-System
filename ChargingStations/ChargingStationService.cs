using Microsoft.EntityFrameworkCore;

public class ChargingStationService
{
    private readonly AppDbContext _context;

    public ChargingStationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChargingStation> CreateAsync(ChargingStation station)
    {
        var entity = await _context.ChargingStations.AddAsync(station);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<List<ChargingStation>> FindAllAsync()
    {
        return await _context.ChargingStations.ToListAsync();
    }

    public async Task<ChargingStation> UpdateAsync(int id, ChargingStation station)
    {
        var existing = await _context.ChargingStations.FindAsync(id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(station);
            await _context.SaveChangesAsync();
        }
        if (existing == null)
        {
            throw new KeyNotFoundException($"ChargingStation with id {id} not found.");
        }
        return existing;
    }
}