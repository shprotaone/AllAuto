using AllAuto.Domain.Entity;
using AllAuto.Domain.ViewModels.Order;
using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Неверно заполнен возраст")]
        [Range(0, 150, ErrorMessage = "Неверно заполнен возраст")]
        public int? Age { get; set; }

        [Required(ErrorMessage ="Укажите адрес")]
        [MinLength(5,ErrorMessage ="Минимальная длина должна быть больше 5 символов")]
        [MaxLength(250,ErrorMessage ="Максимальная длина должна быть меньше 250 символов")]
        public string? Address {  get; set; }

        public string UserName { get; set; }

        public string NewPassword { get; set; }

        public List<CompleteOrderView> CompleteOrders { get; set; }
    }
}
