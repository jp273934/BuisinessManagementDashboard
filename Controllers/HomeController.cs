using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BuisinessManagementDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentMonth = new DateTime(
    DateTime.UtcNow.Year,
    DateTime.UtcNow.Month,
    1);

            var startMonth = currentMonth.AddMonths(-5);
            var endMonth = currentMonth.AddMonths(1);

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

            var model = new DashboardViewModel
            {
                TotalCustomers = await _context.Customers.CountAsync(),

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

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
