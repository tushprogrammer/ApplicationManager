using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.AuthApp
{
    public class UserLogin
    {
        [Required, MaxLength(20)]
        public string LoginProp { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } //пароль

        public string ReturnUrl { get; set; } //возвратная ссылка после авторизации
    }
}
