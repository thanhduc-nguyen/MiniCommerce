using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.Web;
using MiniCommerce.Web.Data;
using MiniCommerce.Web.Endpoints;
using MiniCommerce.Web.Models.Account;
using MiniCommerce.Web.Repositories;
using MiniCommerce.Web.Services.Account;
using MiniCommerce.Web.Services.Agent;
using MiniCommerce.Web.Services.Catalog;
using MiniCommerce.Web.Services.Orders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddHttpClient<IAgentService, AgentService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ServiceUri:Agent"]!);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Constants.MyBearerScheme;
    options.DefaultChallengeScheme = Constants.MyBearerScheme;
    options.DefaultSignInScheme = Constants.MyBearerScheme;
}).AddCookie(Constants.MyBearerScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/NoPermission";
});

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapAuthEndpoints();

app.Run();
