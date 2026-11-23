
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuisinessManagementDashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WorkOrdersController : Controller
    {
        private readonly IWorkOrderService _workOrderService;


        public WorkOrdersController(
            IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        private async Task LoadFormOptionsAsync()
        {
            var customers = await _workOrderService
                .GetCustomersAsync();

            ViewBag.Customers = new SelectList(
                customers, "Id", "Name");

            ViewBag.Statuses = new SelectList(WorkOrderStatus.All);
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            int page = 1)
        {
            var result = await _workOrderService
                .GetWorkOrdersAsync(search, status, page);

            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;
            ViewBag.CurrentPage = result.CurrentPage;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.TotalOrders = result.TotalOrders;

            return View(result.WorkOrders);
        }

        // GET: WorkOrders/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormOptionsAsync();

            return View(new WorkOrderFormViewModel());
        }

        // POST: WorkOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CustomerId,Service,Description,Status,Price")]
            WorkOrder order)
        {
            ModelState.Remove(nameof(WorkOrder.Customer));

            if (!await _workOrderService
                .CustomerExistsAsync(order.CustomerId))
            {
                ModelState.AddModelError(
                    nameof(WorkOrder.CustomerId),
                    "Select a valid customer.");
            }

            if (!_workOrderService.IsValidStatus(order.Status))
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

            await _workOrderService.CreateWorkOrderAsync(order);

            TempData["Success"] =
                "Work order created successfully.";

            return RedirectToAction(nameof(Index));
        }


        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _workOrderService
                .GetWorkOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            var model = new WorkOrderFormViewModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Service = order.Service,
                Description = order.Description,
                Status = order.Status,
                Price = order.Price
            };

            await LoadFormOptionsAsync();

            return View(model);
        }



        // POST: WorkOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            WorkOrderFormViewModel input)
        {
            if (id != input.Id)
                return NotFound();

            if (!await _workOrderService
                .CustomerExistsAsync(input.CustomerId))
            {
                ModelState.AddModelError(
                    nameof(input.CustomerId),
                    "Select a valid customer.");
            }

            if (!_workOrderService.IsValidStatus(input.Status))
            {
                ModelState.AddModelError(
                    nameof(input.Status),
                    "Select a valid status.");
            }

            if (!ModelState.IsValid)
            {
                await LoadFormOptionsAsync();

                return View(input);
            }

            var order = new WorkOrder
            {
                CustomerId = input.CustomerId,
                Service = input.Service,
                Description = input.Description,
                Status = input.Status,
                Price = input.Price
            };

            bool updated = await _workOrderService
                .UpdateWorkOrderAsync(id, order);

            if (!updated)
                return NotFound();

            TempData["Success"] =
                "Work order updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        // GET: WorkOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _workOrderService
                .GetWorkOrderWithCustomerAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _workOrderService
                .DeleteWorkOrderAsync(id);

            if (!deleted)
                return NotFound();

            TempData["Success"] =
                "Work order deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
