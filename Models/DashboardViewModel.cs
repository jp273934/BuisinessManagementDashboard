namespace BuisinessManagementDashboard.Models
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int OpenOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<RecentWorkOrderViewModel> RecentOrders
        { get; set; } = new();

        public List<MonthlyRevenueViewModel> MonthlyRevenue { get; set; } = new();

    }
}
