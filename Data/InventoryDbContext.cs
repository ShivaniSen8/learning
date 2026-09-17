using InventoryServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryServiceAPI.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Inventory> Inventories { get; set; }
}