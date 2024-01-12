using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string UserName { get;set; }

        [Required(ErrorMessage ="Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(5,ErrorMessage ="Пароль должен быть больше 5 символов")]
        public string NewPassword { get;set; }
    }
}
