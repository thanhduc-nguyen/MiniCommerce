using MiniCommerce.Web.Data.Entities;
using MiniCommerce.Web.Repositories;

namespace MiniCommerce.Web.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/", async (IProductRepository productRepository, CancellationToken cancellationToken) =>
        {
            var products = await productRepository.GetProducts(cancellationToken);
            return Results.Ok(products);
        });

        group.MapGet("/{productId:int}", async (
            int productId,
            IProductRepository productRepository,
            CancellationToken cancellationToken) =>
        {
            var product = await productRepository.GetProductById(productId, cancellationToken);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        group.MapGet("/category/{categoryName}", async (
            string categoryName,
            IProductRepository productRepository,
            CancellationToken cancellationToken) =>
        {
            var products = await productRepository.GetProductsByCategory(categoryName, cancellationToken);
            return Results.Ok(products);
        });

        group.MapPost("/", async (
            Product product,
            IProductRepository productRepository,
            CancellationToken cancellationToken) =>
        {
            var productId = await productRepository.CreateProduct(product, cancellationToken);
            return Results.Created($"/products/{productId}", product);
        });

        group.MapPut("/{productId:int}", async (
            int productId,
            Product product,
            IProductRepository productRepository,
            CancellationToken cancellationToken) =>
        {
            product.ProductId = productId;
            var updated = await productRepository.UpdateProduct(product, cancellationToken);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{productId:int}", async (
            int productId,
            IProductRepository productRepository,
            CancellationToken cancellationToken) =>
        {
            var deleted = await productRepository.DeleteProduct(productId, cancellationToken);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
