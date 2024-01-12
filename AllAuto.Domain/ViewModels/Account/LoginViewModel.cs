using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Введите имя")]
        [MaxLength(20, ErrorMessage ="Имя должно иметь длину меньше 20 символов")]
        [MinLength(3,ErrorMessage = "Имя должно иметь длину больше 3х символов")]

        public string Name { get; set; }

        [Required(ErrorMessage ="введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
