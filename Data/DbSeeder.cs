using BuisinessManagementDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {
            // Only seed an empty database.
            // Preserve existing customer and work order data.
            if (await context.Customers.AnyAsync() ||
                await context.WorkOrders.AnyAsync())
            {
                return;
            }

            var customers = new List<Customer>
            {
                new()
                {
                    Name = "Acme Corporation",
                    Email = "contact@acme.example",
                    Phone = "555-0101",
                    Address = "Austin, TX"
                },
                new()
                {
                    Name = "Northstar Solutions",
                    Email = "info@northstar.example",
                    Phone = "555-0102",
                    Address = "Dallas, TX"
                },
                new()
                {
                    Name = "Bluebird Technologies",
                    Email = "support@bluebird.example",
                    Phone = "555-0103",
                    Address = "Houston, TX"
                },
                new()
                {
                    Name = "Summit Industries",
                    Email = "office@summit.example",
                    Phone = "555-0104",
                    Address = "San Antonio, TX"
                },
                new()
                {
                    Name = "Pioneer Systems",
                    Email = "hello@pioneer.example",
                    Phone = "555-0105",
                    Address = "Fort Worth, TX"
                }
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            var today = DateTime.UtcNow.Date;

            var workOrders = new List<WorkOrder>
            {
                new()
                {
                    CustomerId = customers[0].Id,
                    Service = "Network Maintenance",
                    Description = "Network infrastructure maintenance.",
                    Status = "Completed",
                    Price = 850m,
                    CreatedDate = today.AddMonths(-4),
                    CompletedDate = today.AddMonths(-4).AddDays(3)
                },
                new()
                {
                    CustomerId = customers[1].Id,
                    Service = "Software Installation",
                    Description = "Install and configure business software.",
                    Status = "Completed",
                    Price = 1200m,
                    CreatedDate = today.AddMonths(-3),
                    CompletedDate = today.AddMonths(-3).AddDays(5)
                },
                new()
                {
                    CustomerId = customers[2].Id,
                    Service = "System Inspection",
                    Description = "Inspect and evaluate IT systems.",
                    Status = "Completed",
                    Price = 450m,
                    CreatedDate = today.AddMonths(-2),
                    CompletedDate = today.AddMonths(-2).AddDays(2)
                },
                new()
                {
                    CustomerId = customers[3].Id,
                    Service = "Database Optimization",
                    Description = "Improve SQL Server query performance.",
                    Status = "Completed",
                    Price = 1750m,
                    CreatedDate = today.AddMonths(-1),
                    CompletedDate = today.AddMonths(-1).AddDays(4)
                },
                new()
                {
                    CustomerId = customers[4].Id,
                    Service = "Website Maintenance",
                    Description = "Perform website updates and maintenance.",
                    Status = "Completed",
                    Price = 650m,
                    CreatedDate = today.AddDays(-12),
                    CompletedDate = today.AddDays(-9)
                },
                new()
                {
                    CustomerId = customers[0].Id,
                    Service = "Security Audit",
                    Description = "Review application security.",
                    Status = "Open",
                    Price = 950m,
                    CreatedDate = today.AddDays(-5)
                },
                new()
                {
                    CustomerId = customers[1].Id,
                    Service = "Server Migration",
                    Description = "Migrate services to a new server.",
                    Status = "In Progress",
                    Price = 2400m,
                    CreatedDate = today.AddDays(-8)
                },
                new()
                {
                    CustomerId = customers[2].Id,
                    Service = "Application Upgrade",
                    Description = "Upgrade an internal business application.",
                    Status = "Open",
                    Price = 1300m,
                    CreatedDate = today.AddDays(-2)
                },
                new()
                {
                    CustomerId = customers[3].Id,
                    Service = "API Integration",
                    Description = "Integrate an external API.",
                    Status = "In Progress",
                    Price = 1800m,
                    CreatedDate = today.AddDays(-6)
                },
                new()
                {
                    CustomerId = customers[4].Id,
                    Service = "Performance Testing",
                    Description = "Evaluate application performance.",
                    Status = "Open",
                    Price = 700m,
                    CreatedDate = today.AddDays(-1)
                }
            };

            context.WorkOrders.AddRange(workOrders);
            await context.SaveChangesAsync();
        }
    }
}
