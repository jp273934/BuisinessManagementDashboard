namespace BuisinessManagementDashboard.Models
{
    public static class WorkOrderStatus
    {
        public const string Open = "Open";
        public const string InProgress = "In Progress";
        public const string Completed = "Completed";

        public static readonly IReadOnlyList<string> All =
            Array.AsReadOnly(new[]
            {
                Open,
                InProgress,
                Completed
            });

        public static bool IsValid(string? status)
        {
            return status != null && All.Contains(status);
        }
    }
}
