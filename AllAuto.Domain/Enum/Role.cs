using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.Enum
{
    public enum Role
    {
        [Display(Name ="Пользователь")]
        User,
        [Display(Name ="Модератор")]
        Moderator,
        [Display(Name ="Админ")]
        Admin
    }
}