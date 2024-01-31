using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.Enum
{
    public enum DeliveryType
    {
        [Display(Name = "Без доставки")]
        None = 0,
        [Display(Name = "Быстрая доставка")]
        Fast =1,
        [Display(Name = "Обычная доставка")]
        Common=2,
        [Display(Name ="Доставка до терминала")]
        Terminal=3
    }
}
