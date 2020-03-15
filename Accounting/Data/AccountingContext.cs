using Accounting.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Accounting.Data
{
    public class AccountingContext : IdentityDbContext<User>
    {
        public AccountingContext(DbContextOptions<AccountingContext> options) : base(options)
        {
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<CustomerType> CutomerType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Restrict cascading deletes
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
