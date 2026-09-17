using InventoryServiceAPI.Models;

namespace InventoryServiceAPI.Repositories;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(int productId);

    Task<Inventory> AddAsync(Inventory inventory);

    Task UpdateAsync(Inventory inventory);
}