using BuisinessManagementDashboard.Models;

namespace BuisinessManagementDashboard.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<(
           List<WorkOrder> WorkOrders,
           int TotalOrders,
           int TotalPages,
           int CurrentPage)> GetWorkOrdersAsync(
               string? search,
               string? status,
               int page,
               int pageSize = 10);

        Task<WorkOrder?> GetWorkOrderByIdAsync(int id);

        Task<WorkOrder?> GetWorkOrderWithCustomerAsync(int id);

        Task<List<Customer>> GetCustomersAsync();

        Task<bool> CustomerExistsAsync(int customerId);

        bool IsValidStatus(string? status);

        Task CreateWorkOrderAsync(WorkOrder order);

        Task<bool> UpdateWorkOrderAsync(
            int id,
            WorkOrder input);

        Task<bool> DeleteWorkOrderAsync(int id);
    }
}
