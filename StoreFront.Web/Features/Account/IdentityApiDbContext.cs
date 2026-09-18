using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreFront.Web.Models.Account;

namespace StoreFront.Web.Features.Account;

public class IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
