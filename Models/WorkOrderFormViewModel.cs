using System.ComponentModel.DataAnnotations;

namespace BuisinessManagementDashboard.Models
{
    public class WorkOrderFormViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a valid customer.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Enter a service name.")]
        [StringLength(150)]
        public string Service { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Select a valid status.")]
        [StringLength(30)]
        public string Status { get; set; } = WorkOrderStatus.Open;

        [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000.")]
        public decimal Price { get; set; }
    }
}
