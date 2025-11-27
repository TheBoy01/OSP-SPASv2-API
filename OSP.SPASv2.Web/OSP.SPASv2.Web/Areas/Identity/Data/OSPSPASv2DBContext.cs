using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Web.Areas.Identity.Data;
using System.Reflection.Emit;

namespace OSP.SPASv2.Web.Data;

public class OSPSPASv2DBContext : IdentityDbContext<OSPSPASv2ApplicationUser>
{
    public OSPSPASv2DBContext(DbContextOptions<OSPSPASv2DBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        //builder.Entity<OSPSPASv2ApplicationUser>().ToTable("TblUser", "dbo");
        //builder.Entity<OSPSPASv2ApplicationUser>()
        //    .Property(u => u.Id)
        //    .HasColumnName("UserId");

        //builder.Entity<IdentityRole>().ToTable("RefRole");
        //builder.Entity<IdentityUserRole<string>>().ToTable("TblUserRole");
        //builder.Entity<IdentityUserClaim<string>>().ToTable("TblClaim");
        //builder.Entity<IdentityUserLogin<string>>().ToTable("TblLogin");
    }
}
