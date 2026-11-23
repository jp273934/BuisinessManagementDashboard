using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(
     string? search,
     int page = 1)
        {
            const int pageSize = 10;

            // Prevent invalid page numbers
            if (page < 1)
                page = 1;

            var customers = _context.Customers
                .AsNoTracking()
                .AsQueryable();

            // Apply search before pagination
            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search));
            }

            // Count the filtered records
            int totalCustomers = await customers.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalCustomers / (double)pageSize);

            // Keep the page within the available range
            if (page > totalPages && totalPages > 0)
                page = totalPages;

            // Retrieve only the current page
            var customerList = await customers
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCustomers = totalCustomers;

            return View(customerList);
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

            customer.CreatedDate = DateTime.UtcNow;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

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

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            customer.Name = input.Name;
            customer.Email = input.Email;
            customer.Phone = input.Phone;
            customer.Address = input.Address;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            // Check whether the customer has any work orders
            bool hasWorkOrders = await _context.WorkOrders
                .AnyAsync(w => w.CustomerId == id);

            if (hasWorkOrders)
            {
                TempData["Error"] =
                    "This customer cannot be deleted because they have " +
                    "existing work orders. Delete the associated work " +
                    "orders first.";

                return RedirectToAction(nameof(Index));
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
