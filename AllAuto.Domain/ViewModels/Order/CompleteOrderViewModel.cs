using AllAuto.Domain.Entity;
using System.ComponentModel.DataAnnotations;


namespace AllAuto.Domain.ViewModels.Order
{
    public class CompleteOrderViewModel
    {
        public long Id { get; set; }

        public AllAuto.Domain.Entity.User User { get; set; } //навигационное свойство

        public long UserId { get; set; } //Навигационное свойство

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Минимальная длина 2 символа")]
        public string FullName { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Минимальная длина 2 символа")]
        public string Address { get; set; }

        public DateTime DateTime { get; set; }
        [Display(Name = "Тип доставки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string DeliveryType { get; set; }

        public List<ItemEntryViewModel>ItemEntries { get; set; }
    }
}
