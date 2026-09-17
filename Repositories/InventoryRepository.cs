using InventoryServiceAPI.Data;
using InventoryServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryServiceAPI.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(int productId)
    {
        return await _context.Inventories
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<Inventory> AddAsync(Inventory inventory)
    {
        await _context.Inventories.AddAsync(inventory);
        await _context.SaveChangesAsync();

        return inventory;
    }

    public async Task UpdateAsync(Inventory inventory)
    {
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
    }
}