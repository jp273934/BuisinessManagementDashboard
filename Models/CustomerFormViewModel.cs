using System.ComponentModel.DataAnnotations;

namespace BuisinessManagementDashboard.Models
{
    public class CustomerFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a customer name.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter an email address.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [StringLength(25)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
