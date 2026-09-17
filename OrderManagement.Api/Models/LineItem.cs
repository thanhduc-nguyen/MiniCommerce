namespace OrderManagement.Api.Models;

public class LineItem
{
    public Guid ProductGuid { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
