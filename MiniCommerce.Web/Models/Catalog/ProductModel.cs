namespace MiniCommerce.Web.Models.Catalog;

public class ProductModel
{
    public int ProductId { get; set; }
    public Guid ProductGuid { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal DiscountRate { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Stock { get; set; }

    public decimal DiscountedPrice => Price - (Price * DiscountRate);
}
