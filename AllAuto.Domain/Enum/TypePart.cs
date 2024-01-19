using System.ComponentModel.DataAnnotations;

namespace AllAuto.Domain.Enum
{
    public enum TypePart
    {
        [Display(Name = "Двигатель")]
        Engine = 0,
        [Display(Name = "Ходовая и управление")]
        Handling = 1,
        [Display(Name = "Трансмиссия")]
        Transmission = 2,

    }
}
