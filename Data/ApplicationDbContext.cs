using BuisinessManagementDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
          DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkOrder>()
                .HasOne(w => w.Customer)
                .WithMany(c => c.WorkOrders)
                .HasForeignKey(w => w.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
