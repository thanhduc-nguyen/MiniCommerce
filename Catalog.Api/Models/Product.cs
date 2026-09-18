namespace Catalog.Api.Models;

public class Product
{
    public int ProductId { get; set; }
    public Guid ProductGuid { get; set; }
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal DiscountRate { get; set; }
    public string CategoryName { get; set; } = default!;
    public int Stock { get; set; }
}
