using BuisinessManagementDashboard.Models;

namespace BuisinessManagementDashboard.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}
