using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.AuthApp
{
    public class UserRegistration
    {
        [Required, MaxLength(20)]
        public string LoginProp { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
    }
}
