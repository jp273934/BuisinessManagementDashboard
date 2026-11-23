using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ApplicationDbContext _context;

        

        public WorkOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task ValidateWorkOrderAsync(WorkOrder order)
        {
            if (!await CustomerExistsAsync(order.CustomerId))
            {
                throw new ArgumentException(
                    "The selected customer does not exist.",
                    nameof(order.CustomerId));
            }

            if (!WorkOrderStatus.IsValid(order.Status))
            {
                throw new ArgumentException(
                    "The selected work order status is invalid.",
                    nameof(order.Status));
            }
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
                WorkOrderStatus.IsValid(status))
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
            return WorkOrderStatus.IsValid(status);
        }


        public async Task CreateWorkOrderAsync(WorkOrder order)
        {
            await ValidateWorkOrderAsync(order);

            order.CreatedDate = DateTime.UtcNow;

            order.CompletedDate =
                order.Status == WorkOrderStatus.Completed
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

            await ValidateWorkOrderAsync(input);

            order.CustomerId = input.CustomerId;
            order.Service = input.Service;
            order.Description = input.Description;
            order.Status = input.Status;
            order.Price = input.Price;

            // Preserve the original completion date
            // unless the work order is reopened.
            if (input.Status == WorkOrderStatus.Completed)
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
