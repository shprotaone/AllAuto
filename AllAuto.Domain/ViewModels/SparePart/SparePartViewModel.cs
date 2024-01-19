using AllAuto.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.SparePart
{
    public class SparePartViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2,ErrorMessage ="Минимальная длинна должна быть больше двух символов")]
        public string Name { get; set; }

        [Display(Name ="Описание")]
        public string ShortDesription { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Укажите модель")]
        [MinLength(2, ErrorMessage = "Минимальная длинна должна быть больше двух символов")]
        public string Manufacture { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Укажите стоимость")]
        public decimal Price { get; set; }

        [Display(Name = "Тип запчасти")]
        [Required(ErrorMessage = "Выберете тип")]
        public string TypeSparePart { get; set; }

        [Display(Name = "Колличество")]
        [Range(1,int.MaxValue, ErrorMessage ="Количество не введено")]
        public int Amount { get; set; }
        public IFormFile Avatar { get; set; }
    }
}