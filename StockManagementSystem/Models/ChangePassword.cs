using System.ComponentModel.DataAnnotations;

namespace StockManagementSystem.Models
{
    public class ChangePassword
    {
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Enter your new Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Enter your current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Please, Enter your Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public string? EmailToken { get; set; }



    }
}
