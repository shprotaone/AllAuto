using AllAuto.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.Car
{
    public class CarViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2,ErrorMessage ="Минимальная длинна должна быть больше двух символов")]
        public string Name { get; set; }

        [Display(Name ="Описание")]
        public string Description { get; set; }

        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Укажите модель")]
        [MinLength(2, ErrorMessage = "Минимальная длинна должна быть больше двух символов")]
        public string Model { get; set; }

        [Display(Name = "Максимаальная скорость")]
        [Required(ErrorMessage = "Укадите скорость")]
        [Range(30,450, ErrorMessage = "Укажите скорость в пределах от 30 до 450км/ч")]
        public double MaxSpeed { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Укажите стоимость")]
        public decimal Price { get; set; }

        public DateTime DateCreate { get; set; }

        [Display(Name = "Тип автомобиля")]
        [Required(ErrorMessage = "Выберете тип")]
        public string TypeCar { get; set; }

        public IFormFile Avatar { get; set; }

        public byte[]? Image { get; set; }
    }
}