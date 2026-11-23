using BuisinessManagementDashboard.Models;

namespace BuisinessManagementDashboard.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<(List<Customer> Customers, int TotalCustomers, int TotalPages, int CurrentPage)>
            GetCustomersAsync(string? search, int page, int pageSize = 10);

        Task<Customer?> GetCustomerByIdAsync(int id);

        Task CreateCustomerAsync(Customer customer);

        Task<bool> UpdateCustomerAsync(int id, Customer input);

        Task<CustomerDeleteResult> DeleteCustomerAsync(int id);
    }

    public enum CustomerDeleteResult
    {
        Success,
        NotFound,
        HasWorkOrders
    }
}
