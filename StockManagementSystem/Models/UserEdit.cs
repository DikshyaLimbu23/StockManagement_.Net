using System.ComponentModel.DataAnnotations;

namespace StockManagementSystem.Models
{
    public class UserEdit
    {
        public int Userid { get; set; }

        public string Username { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string Locations { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public bool? Isactive { get; set; }

        public DateOnly? RegisterDate { get; set; }

        public string? ResonforDelete { get; set; }

        public DateOnly? DeleteDate { get; set; }

        public string? Userfile { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Userphoto {  get; set; }

        public string? UserPassword { get; set; }

        public string? EmailToken { get; set; }

    }
}
