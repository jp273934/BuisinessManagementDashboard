using BuisinessManagementDashboard.Data;
using BuisinessManagementDashboard.Models;
using BuisinessManagementDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuisinessManagementDashboard.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(
            List<Customer> Customers,
            int TotalCustomers,
            int TotalPages,
            int CurrentPage)> GetCustomersAsync(
                string? search,
                int page,
                int pageSize = 10)
        {
            if (page < 1)
                page = 1;

            var customers = _context.Customers
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search));
            }

            int totalCustomers = await customers.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalCustomers / (double)pageSize);

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            var customerList = await customers
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (
                customerList,
                totalCustomers,
                totalPages,
                page);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            customer.CreatedDate = DateTime.UtcNow;

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCustomerAsync(
            int id,
            Customer input)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return false;

            customer.Name = input.Name;
            customer.Email = input.Email;
            customer.Phone = input.Phone;
            customer.Address = input.Address;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CustomerDeleteResult> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return CustomerDeleteResult.NotFound;

            bool hasWorkOrders = await _context.WorkOrders
                .AnyAsync(w => w.CustomerId == id);

            if (hasWorkOrders)
                return CustomerDeleteResult.HasWorkOrders;

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return CustomerDeleteResult.Success;
        }
    }
}
