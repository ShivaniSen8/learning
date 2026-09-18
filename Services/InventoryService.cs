using InventoryServiceAPI.DTOs;
using InventoryServiceAPI.Models;
using InventoryServiceAPI.Repositories;

namespace InventoryServiceAPI.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<InventoryResponse?> GetInventoryAsync(int productId)
    {
        var inventory = await _repository.GetByProductIdAsync(productId);

        if (inventory == null)
        {
            return null;
        }

        return MapToResponse(inventory);
    }

    public async Task<InventoryResponse> CreateInventoryAsync(
        CreateInventoryRequest request)
    {
        if (request.ProductId <= 0)
        {
            throw new ArgumentException("Invalid product ID.");
        }

        if (request.Quantity < 0)
        {
            throw new ArgumentException(
                "Quantity cannot be negative.");
        }

        var existingInventory =
            await _repository.GetByProductIdAsync(request.ProductId);

        if (existingInventory != null)
        {
            throw new InvalidOperationException(
                "Inventory already exists for this product.");
        }

        var inventory = new Inventory
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            ReservedQuantity = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdInventory =
            await _repository.AddAsync(inventory);

        return MapToResponse(createdInventory);
    }

    public async Task<InventoryResponse> AddStockAsync(
        int productId,
        UpdateStockRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var inventory =
            await GetInventoryOrThrow(productId);

        inventory.Quantity += request.Quantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(inventory);

        return MapToResponse(inventory);
    }

    public async Task<InventoryResponse> RemoveStockAsync(
        int productId,
        UpdateStockRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var inventory =
            await GetInventoryOrThrow(productId);

        var availableQuantity =
            inventory.Quantity - inventory.ReservedQuantity;

        if (request.Quantity > availableQuantity)
        {
            throw new InvalidOperationException(
                "Insufficient available stock.");
        }

        inventory.Quantity -= request.Quantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(inventory);

        return MapToResponse(inventory);
    }

    public async Task<InventoryResponse> ReserveStockAsync(
        int productId,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var inventory =
            await GetInventoryOrThrow(productId);

        var availableQuantity =
            inventory.Quantity - inventory.ReservedQuantity;

        if (quantity > availableQuantity)
        {
            throw new InvalidOperationException(
                "Insufficient available stock.");
        }

        inventory.ReservedQuantity += quantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(inventory);

        return MapToResponse(inventory);
    }

    public async Task<InventoryResponse> ReleaseStockAsync(
        int productId,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var inventory =
            await GetInventoryOrThrow(productId);

        if (quantity > inventory.ReservedQuantity)
        {
            throw new InvalidOperationException(
                "Cannot release more stock than reserved.");
        }

        inventory.ReservedQuantity -= quantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(inventory);

        return MapToResponse(inventory);
    }

    private async Task<Inventory> GetInventoryOrThrow(int productId)
    {
        var inventory =
            await _repository.GetByProductIdAsync(productId);

        if (inventory == null)
        {
            throw new KeyNotFoundException(
                "Inventory not found for this product.");
        }

        return inventory;
    }

    private static InventoryResponse MapToResponse(
        Inventory inventory)
    {
        return new InventoryResponse
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity,
            ReservedQuantity = inventory.ReservedQuantity,
            AvailableQuantity =
                inventory.Quantity - inventory.ReservedQuantity,
            CreatedAt = inventory.CreatedAt,
            UpdatedAt = inventory.UpdatedAt
        };
    }
}