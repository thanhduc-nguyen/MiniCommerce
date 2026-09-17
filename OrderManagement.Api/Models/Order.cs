namespace OrderManagement.Api.Models;

public class Order
{
    public int OrderId { get; set; }
    public Guid OrderGuid { get; set; }
    public List<LineItem> LineItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
}
