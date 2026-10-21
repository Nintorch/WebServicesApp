using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenAppClient.Models
{
    public class AccountLoginViewModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
