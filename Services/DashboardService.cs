using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            // Determine the six-month reporting period.
            var currentMonth = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1);

            var startMonth = currentMonth.AddMonths(-5);
            var endMonth = currentMonth.AddMonths(1);

            // Retrieve completed revenue grouped by year and month.
            var revenueData = await _context.WorkOrders
                .AsNoTracking()
                .Where(w =>
                    w.Status == "Completed" &&
                    w.CompletedDate.HasValue &&
                    w.CompletedDate.Value >= startMonth &&
                    w.CompletedDate.Value < endMonth)
                .GroupBy(w => new
                {
                    Year = w.CompletedDate!.Value.Year,
                    Month = w.CompletedDate!.Value.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Revenue = g.Sum(w => w.Price)
                })
                .ToListAsync();

            // Include months with no completed revenue.
            var monthlyRevenue = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = startMonth.AddMonths(i);

                    return new MonthlyRevenueViewModel
                    {
                        Month = month.ToString("MMM yyyy"),
                        Revenue = revenueData
                            .Where(r =>
                                r.Year == month.Year &&
                                r.Month == month.Month)
                            .Sum(r => r.Revenue)
                    };
                })
                .ToList();

            // Build the dashboard ViewModel.
            var model = new DashboardViewModel
            {
                TotalCustomers = await _context.Customers
                    .CountAsync(),

                OpenOrders = await _context.WorkOrders
                    .CountAsync(w => w.Status == "Open"),

                CompletedOrders = await _context.WorkOrders
                    .CountAsync(w => w.Status == "Completed"),

                TotalRevenue = await _context.WorkOrders
                    .Where(w => w.Status == "Completed")
                    .SumAsync(w => (decimal?)w.Price) ?? 0m,

                RecentOrders = await _context.WorkOrders
                    .AsNoTracking()
                    .OrderByDescending(w => w.CreatedDate)
                    .ThenByDescending(w => w.Id)
                    .Take(5)
                    .Select(w => new RecentWorkOrderViewModel
                    {
                        CustomerName = w.Customer.Name,
                        Service = w.Service,
                        Status = w.Status
                    })
                    .ToListAsync(),

                MonthlyRevenue = monthlyRevenue
            };

            return model;
        }
    }
}
