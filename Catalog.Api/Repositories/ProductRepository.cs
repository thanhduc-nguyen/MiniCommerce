using Catalog.Api.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Catalog.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(IConfiguration configuration, ILogger<ProductRepository> logger)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("CatalogDb")!;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        var products = new List<Product>();

        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("Product_Get", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            await Task.Run(() => da.Fill(dt));

            if (dt.Rows.Count > 0)
            {
                products = dt.AsEnumerable().Select(p => new Product
                {
                    ProductId = p.Field<int>("ProductId"),
                    ProductGuid = p.Field<Guid>("ProductGuid"),
                    ProductName = p.Field<string>("ProductName"),
                    Description = p.Field<string>("Description"),
                    ImageUrl = $"{_configuration["HostUrl"]}{p.Field<string>("ImageUrl")}",
                    Price = p.Field<decimal>("Price"),
                    DiscountRate = p.Field<decimal>("DiscountRate"),
                    CategoryName = p.Field<string>("CategoryName"),
                    Stock = p.Field<int>("Stock")
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception: {exception}", ex.Message);
            return [];
        }

        return products;
    }

    public async Task<Product> GetProductById(int productId, CancellationToken cancellationToken)
    {
        Product? product = null;

        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product WHERE ProductId = @ProductId", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter(@"ProductId", productId));

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            await Task.Run(() => da.Fill(dt));

            if (dt.Rows.Count > 0)
            {
                product = dt.AsEnumerable().Select(p => new Product
                {
                    ProductId = p.Field<int>("ProductId"),
                    ProductGuid = p.Field<Guid>("ProductGuid"),
                    ProductName = p.Field<string>("ProductName"),
                    Description = p.Field<string>("Description"),
                    ImageUrl = $"{_configuration["HostUrl"]}{p.Field<string>("ImageUrl")}",
                    Price = p.Field<decimal>("Price"),
                    DiscountRate = p.Field<decimal>("DiscountRate"),
                    CategoryName = p.Field<string>("CategoryName"),
                    Stock = p.Field<int>("Stock")
                }).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception: {exception}", ex.Message);
        }

        return product ?? new Product();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken)
    {
        string sql = "SELECT * FROM dbo.Product WHERE CategoryName = @CategoryName";

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@CategoryName", categoryName));

        using SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        await Task.Run(() => da.Fill(dt));

        return dt.AsEnumerable().Select(p => new Product
        {
            ProductId = p.Field<int>("ProductId"),
            ProductGuid = p.Field<Guid>("ProductGuid"),
            ProductName = p.Field<string>("ProductName"),
            Description = p.Field<string>("Description"),
            ImageUrl = $"{_configuration["HostUrl"]}{p.Field<string>("ImageUrl")}",
            Price = p.Field<decimal>("Price"),
            DiscountRate = p.Field<decimal>("DiscountRate"),
            CategoryName = p.Field<string>("CategoryName"),
            Stock = p.Field<int>("Stock")
        }).ToList();
    }

    public async Task<int> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        int productId = 0;

        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            using SqlCommand cmd = new SqlCommand("Product_Create", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ProductId",
                Value = productId,
                IsNullable = false,
                DbType = DbType.Int32,
                Direction = ParameterDirection.Output
            });
            cmd.Parameters.Add(new SqlParameter("@ProductGuid", product.ProductGuid));
            cmd.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
            cmd.Parameters.Add(new SqlParameter("@Description", product.Description));
            cmd.Parameters.Add(new SqlParameter("@ImageUrl", product.ImageUrl));
            cmd.Parameters.Add(new SqlParameter("@Price", product.Price));
            cmd.Parameters.Add(new SqlParameter("@DiscountRate", product.DiscountRate));
            cmd.Parameters.Add(new SqlParameter("@CategoryName", product.CategoryName));
            cmd.Parameters.Add(new SqlParameter("@Stock", product.Stock));

            await cmd.ExecuteScalarAsync(cancellationToken);
            productId = (int)cmd.Parameters["@ProductId"].Value;

            connection.Close();
            connection.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception: {exception}", ex.Message);
            return productId;
        }

        return productId;
    }


    public async Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            using SqlCommand cmd = new SqlCommand("Product_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ProductId", product.ProductId));
            cmd.Parameters.Add(new SqlParameter("@ProductGuid", product.ProductGuid));
            cmd.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
            cmd.Parameters.Add(new SqlParameter("@Description", product.Description));
            cmd.Parameters.Add(new SqlParameter("@ImageUrl", product.ImageUrl));
            cmd.Parameters.Add(new SqlParameter("@Price", product.Price));
            cmd.Parameters.Add(new SqlParameter("@DiscountRate", product.DiscountRate));
            cmd.Parameters.Add(new SqlParameter("@CategoryName", product.CategoryName));
            cmd.Parameters.Add(new SqlParameter("@Stock", product.Stock));

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            connection.Close();
            connection.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception: {exception}", ex.Message);
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            using SqlCommand cmd = new SqlCommand("Product_Delete", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            connection.Close();
            connection.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception: {exception}", ex.Message);
            return false;
        }

        return true;
    }
}

