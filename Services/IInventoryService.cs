using InventoryServiceAPI.DTOs;

namespace InventoryServiceAPI.Services;

public interface IInventoryService
{
    Task<InventoryResponse?> GetInventoryAsync(int productId);

    Task<InventoryResponse> CreateInventoryAsync(
        CreateInventoryRequest request);

    Task<InventoryResponse> AddStockAsync(
        int productId,
        UpdateStockRequest request);

    Task<InventoryResponse> RemoveStockAsync(
        int productId,
        UpdateStockRequest request);

    Task<InventoryResponse> ReserveStockAsync(
        int productId,
        int quantity);

    Task<InventoryResponse> ReleaseStockAsync(
        int productId,
        int quantity);
}