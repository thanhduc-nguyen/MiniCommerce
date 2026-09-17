using Catalog.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/products", async (IProductRepository productRepository, CancellationToken cancellationToken) =>
{
    var products = await productRepository.GetProducts(cancellationToken);
    return Results.Ok(products);
});

app.Run();
