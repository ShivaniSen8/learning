using InventoryServiceAPI.DTOs;
using InventoryServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace InventoryServiceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // GET: api/inventory/1
    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetInventory(int productId)
    {
        var inventory =
            await _inventoryService.GetInventoryAsync(productId);

        if (inventory == null)
        {
            return NotFound(new
            {
                message = "Inventory not found for this product."
            });
        }

        return Ok(inventory);
    }

    // POST: api/inventory
    [HttpPost]
    public async Task<IActionResult> CreateInventory(
        [FromBody] CreateInventoryRequest request)
    {
        try
        {
            var inventory =
                await _inventoryService.CreateInventoryAsync(request);

            return CreatedAtAction(
                nameof(GetInventory),
                new { productId = inventory.ProductId },
                inventory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: api/inventory/1/stock
    [HttpPut("{productId:int}/stock")]
    public async Task<IActionResult> AddStock(
        int productId,
        [FromBody] UpdateStockRequest request)
    {
        try
        {
            var inventory =
                await _inventoryService.AddStockAsync(
                    productId,
                    request);

            return Ok(inventory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: api/inventory/1/remove-stock
    [HttpPut("{productId:int}/remove-stock")]
    public async Task<IActionResult> RemoveStock(
        int productId,
        [FromBody] UpdateStockRequest request)
    {
        try
        {
            var inventory =
                await _inventoryService.RemoveStockAsync(
                    productId,
                    request);

            return Ok(inventory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // POST: api/inventory/1/reserve
    [HttpPost("{productId:int}/reserve")]
    public async Task<IActionResult> ReserveStock(
        int productId,
        [FromBody] int quantity)
    {
        try
        {
            var inventory =
                await _inventoryService.ReserveStockAsync(
                    productId,
                    quantity);

            return Ok(inventory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // POST: api/inventory/1/release
    [HttpPost("{productId:int}/release")]
    public async Task<IActionResult> ReleaseStock(
        int productId,
        [FromBody] int quantity)
    {
        try
        {
            var inventory =
                await _inventoryService.ReleaseStockAsync(
                    productId,
                    quantity);

            return Ok(inventory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}