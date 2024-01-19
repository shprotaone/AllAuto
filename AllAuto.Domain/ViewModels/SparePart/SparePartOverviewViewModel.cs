using AllAuto.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.ViewModels.SparePart
{
    public class SparePartOverviewViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Производитель")]
        public string Model { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Дата изготовления")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Тип")]
        public TypePart TypeSparePart { get; set; }
        [Display(Name = "Количество")]
        public int Amount { get; set; }
        [Display(Name = "Изображение")]
        public byte[]? Avatar { get; set; }
    }
}
