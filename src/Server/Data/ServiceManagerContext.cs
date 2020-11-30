using ServiceManager.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceManager.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ServiceManager.Shared.Models;
using ServiceManager.Shared.Models.Setup;
using ServiceManager.Shared.Models.ProductManagement;
using ServiceManager.Shared.Models.Procurement;
using ServiceManager.Shared.Models.SalesManagement;


namespace ServiceManager.Server.Data
{
    public class ServiceManagerContext : ApiAuthorizationDbContext<ServiceManagerUser>
    {
        public ServiceManagerContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ServiceManagerUser>(b =>
            {
                b.ToTable("ServiceManagerUser");
            });
            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("Tokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

        }

        public DbSet<ServiceManager.Server.Areas.Identity.Data.ServiceManagerUser> ServiceManagerUser { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<TodoItem> TodoItem { get; set; }
        public DbSet<DocumentSetup> DocumentSetup { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<PurchaseHeader> PurchaseHeader { get; set; }
        public DbSet<PurchaseLine> PurchaseLine{ get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Variant> Variant { get; set; }
        public DbSet<VariantImage> VariantImage { get; set; }
        public DbSet<ProductGroup> ProductGroup { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategory { get; set; }
        public DbSet<ProductCost> ProductCost { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<SalesHeader> SalesHeader { get; set; }
        public DbSet<SalesLine> SalesLine { get; set; }
        public DbSet<InvoiceHeader> InvoiceHeader { get; set; }
        public DbSet<InvoiceLine> InvoiceLine { get; set; }
    }
}
