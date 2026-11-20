using System.Diagnostics;
using BuisinessManagementDashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuisinessManagementDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalCustomers = 128,
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
