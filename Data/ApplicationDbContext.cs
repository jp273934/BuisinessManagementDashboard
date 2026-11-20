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
    }
}
