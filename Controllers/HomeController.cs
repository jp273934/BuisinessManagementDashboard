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
                    .ToListAsync()
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
