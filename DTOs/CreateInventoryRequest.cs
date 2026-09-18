namespace InventoryServiceAPI.DTOs;

public class CreateInventoryRequest
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}