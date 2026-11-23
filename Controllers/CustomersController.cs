
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuisinessManagementDashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index(
            string? search,
            int page = 1)
        {
            var result = await _customerService
                .GetCustomersAsync(search, page);

            ViewBag.Search = search;
            ViewBag.CurrentPage = result.CurrentPage;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalCustomers = result.TotalCustomers;

            return View(result.Customers);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Email,Phone,Address")] Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            await _customerService.CreateCustomerAsync(customer);

            TempData["Success"] = "Customer created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService
                .GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Email,Phone,Address")] Customer input)
        {
            if (id != input.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(input);

            bool updated = await _customerService
                .UpdateCustomerAsync(id, input);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Customer updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService
                .GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService
                .DeleteCustomerAsync(id);

            switch (result)
            {
                case CustomerDeleteResult.NotFound:
                    return NotFound();

                case CustomerDeleteResult.HasWorkOrders:
                    TempData["Error"] =
                        "This customer cannot be deleted because they have " +
                        "existing work orders. Delete the associated work " +
                        "orders first.";

                    return RedirectToAction(nameof(Index));

                case CustomerDeleteResult.Success:
                    TempData["Success"] =
                        "Customer deleted successfully.";

                    return RedirectToAction(nameof(Index));

                default:
                    return StatusCode(500);
            }
        }
    }
}
