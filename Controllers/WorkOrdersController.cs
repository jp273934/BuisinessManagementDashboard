using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private static readonly string[] Statuses =
        {
            "Open", "In Progress", "Completed"
        };

        private async Task LoadFormOptionsAsync()
        {
            var customers = await _context.Customers
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Customers = new SelectList(
                customers, "Id", "Name");

            ViewBag.Statuses = new SelectList(Statuses);
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index(
     string? search,
     string? status,
     int page = 1)
        {
            const int pageSize = 10;

            if (page < 1)
                page = 1;

            var orders = _context.WorkOrders
                .Include(w => w.Customer)
                .AsNoTracking()
                .AsQueryable();

            // Search by customer or service
            if (!string.IsNullOrWhiteSpace(search))
            {
                orders = orders.Where(w =>
                    w.Customer.Name.Contains(search) ||
                    w.Service.Contains(search));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status) &&
                Statuses.Contains(status))
            {
                orders = orders.Where(w => w.Status == status);
            }

            // Count filtered records
            int totalOrders = await orders.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalOrders / (double)pageSize);

            // Prevent requests for nonexistent pages
            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            // Retrieve only the current page
            var orderList = await orders
                .OrderByDescending(w => w.CreatedDate)
                .ThenByDescending(w => w.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalOrders = totalOrders;

            return View(orderList);
        }

        // GET: WorkOrders/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormOptionsAsync();
            return View(new WorkOrder());
        }

        // POST: WorkOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CustomerId,Service,Description,Status,Price")]
            WorkOrder order)
        {
            ModelState.Remove(nameof(WorkOrder.Customer));

            if (!await _context.Customers
                .AnyAsync(c => c.Id == order.CustomerId))
            {
                ModelState.AddModelError(
                    nameof(WorkOrder.CustomerId),
                    "Select a valid customer.");
            }

            if (!Statuses.Contains(order.Status))
            {
                ModelState.AddModelError(
                    nameof(WorkOrder.Status),
                    "Select a valid status.");
            }

            if (!ModelState.IsValid)
            {
                await LoadFormOptionsAsync();
                return View(order);
            }

            order.CreatedDate = DateTime.UtcNow;

            order.CompletedDate = order.Status == "Completed" ? DateTime.UtcNow : null;

            _context.WorkOrders.Add(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Work order created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.WorkOrders.FindAsync(id);

            if (order == null)
                return NotFound();

            await LoadFormOptionsAsync();
            return View(order);
        }

        // POST: WorkOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,CustomerId,Service,Description,Status,Price")]
            WorkOrder input)
        {
            if (id != input.Id)
                return NotFound();

            ModelState.Remove(nameof(WorkOrder.Customer));

            if (!await _context.Customers
                .AnyAsync(c => c.Id == input.CustomerId))
            {
                ModelState.AddModelError(
                    nameof(WorkOrder.CustomerId),
                    "Select a valid customer.");
            }

            if (!Statuses.Contains(input.Status))
            {
                ModelState.AddModelError(
                    nameof(WorkOrder.Status),
                    "Select a valid status.");
            }

            if (!ModelState.IsValid)
            {
                await LoadFormOptionsAsync();
                return View(input);
            }

            var order = await _context.WorkOrders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.CustomerId = input.CustomerId;
            order.Service = input.Service;
            order.Description = input.Description;
            order.Status = input.Status;
            if (input.Status == "Completed")
            {
                order.CompletedDate ??= DateTime.UtcNow;
            }
            else
            {
                order.CompletedDate = null;
            }
            order.Price = input.Price;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Work order updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: WorkOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.WorkOrders
                .Include(w => w.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.WorkOrders.FindAsync(id);

            if (order == null)
                return NotFound();

            _context.WorkOrders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Work order deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
