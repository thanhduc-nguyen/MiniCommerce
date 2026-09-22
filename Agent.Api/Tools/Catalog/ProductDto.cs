namespace Agent.Api.Tools.Catalog;

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal DiscountRate { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Stock { get; set; }
}
