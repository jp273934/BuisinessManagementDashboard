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

                OpenOrders = 24,
                CompletedOrders = 86,
                TotalRevenue = 18450.00m,

                RecentOrders = new()
                {
                    new RecentWorkOrderViewModel
                    {
                        CustomerName = "Acme Co.",
                        Service = "Maintenance",
                        Status = "Open"
                    },
                    new RecentWorkOrderViewModel
                    {
                        CustomerName = "Northstar",
                        Service = "Installation",
                        Status = "In Progress"
                    },
                    new RecentWorkOrderViewModel
                    {
                        CustomerName = "Bluebird",
                        Service = "Inspection",
                        Status = "Completed"
                    }
                }
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
