using Microsoft.EntityFrameworkCore;

public class VehicleService
{
    private readonly AppDbContext _context;

    public VehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle)
    {
        var entity = await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<List<Vehicle>> FindAllAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task<Vehicle> UpdateAsync(int id, Vehicle vehicle)
    {
        var existing = await _context.Vehicles.FindAsync(id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(vehicle);
            await _context.SaveChangesAsync();
        }
        if (existing == null)
        {
            throw new KeyNotFoundException($"Vehicle with id {id} not found.");
        }
        return existing;
    }
}