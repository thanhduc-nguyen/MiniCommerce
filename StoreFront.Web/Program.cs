using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreFront.Web;
using StoreFront.Web.Features.Account;
using StoreFront.Web.Features.Account.Services;
using StoreFront.Web.Models.Account;
using StoreFront.Web.Services.Account;
using StoreFront.Web.Services.Catalog;
using StoreFront.Web.Services.Orders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDb")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityApiDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ServiceUri:Catalog"]!);
});
builder.Services.AddHttpClient<IOrderService, OrderService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ServiceUri:OrderManagement"]!);
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
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IdentityApiDbContext>();
    await context.Database.EnsureCreatedAsync();
    await SeedData.AddSampleRoles(services.GetRequiredService<RoleManager<IdentityRole>>());
    await SeedData.AddSampleUsers(services.GetRequiredService<UserManager<ApplicationUser>>());
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
