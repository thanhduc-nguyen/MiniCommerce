namespace Agent.Api.Tools.Catalog;

public class CheckoutProductTool(MiniCommerceClient miniCommerceClient) : ITool
{
    public string Name => "checkout_products";

    public string Description => throw new NotImplementedException();

    public Task<string> InvokeAsync(string input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}