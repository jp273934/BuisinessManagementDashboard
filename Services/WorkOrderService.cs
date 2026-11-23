using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ApplicationDbContext _context;

        private static readonly string[] Statuses =
        {
            "Open",
            "In Progress",
            "Completed"
        };

        public WorkOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(
            List<WorkOrder> WorkOrders,
            int TotalOrders,
            int TotalPages,
            int CurrentPage)> GetWorkOrdersAsync(
                string? search,
                string? status,
                int page,
                int pageSize = 10)
        {
            if (page < 1)
                page = 1;

            var orders = _context.WorkOrders
                .Include(w => w.Customer)
                .AsNoTracking()
                .AsQueryable();

            // Search by customer name or service
            if (!string.IsNullOrWhiteSpace(search))
            {
                orders = orders.Where(w =>
                    w.Customer.Name.Contains(search) ||
                    w.Service.Contains(search));
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status) &&
                IsValidStatus(status))
            {
                orders = orders.Where(w => w.Status == status);
            }

            int totalOrders = await orders.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalOrders / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var orderList = await orders
                .OrderByDescending(w => w.CreatedDate)
                .ThenByDescending(w => w.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (
                orderList,
                totalOrders,
                totalPages,
                page);
        }

        public async Task<WorkOrder?> GetWorkOrderByIdAsync(int id)
        {
            return await _context.WorkOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WorkOrder?> GetWorkOrderWithCustomerAsync(int id)
        {
            return await _context.WorkOrders
                .Include(w => w.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            return await _context.Customers
                .AnyAsync(c => c.Id == customerId);
        }

        public bool IsValidStatus(string? status)
        {
            return status != null && Statuses.Contains(status);
        }

        public async Task CreateWorkOrderAsync(WorkOrder order)
        {
            order.CreatedDate = DateTime.UtcNow;

            order.CompletedDate =
                order.Status == "Completed"
                    ? DateTime.UtcNow
                    : null;

            _context.WorkOrders.Add(order);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateWorkOrderAsync(
            int id,
            WorkOrder input)
        {
            var order = await _context.WorkOrders.FindAsync(id);

            if (order == null)
                return false;

            order.CustomerId = input.CustomerId;
            order.Service = input.Service;
            order.Description = input.Description;
            order.Status = input.Status;
            order.Price = input.Price;

            // Preserve the original completion date
            // unless the work order is reopened.
            if (input.Status == "Completed")
            {
                order.CompletedDate ??= DateTime.UtcNow;
            }
            else
            {
                order.CompletedDate = null;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteWorkOrderAsync(int id)
        {
            var order = await _context.WorkOrders.FindAsync(id);

            if (order == null)
                return false;

            _context.WorkOrders.Remove(order);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
