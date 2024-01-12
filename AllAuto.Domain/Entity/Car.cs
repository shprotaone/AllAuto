using AllAuto.Domain.Enum;

namespace AllAuto.Domain.Entity
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public double MaxSpeed { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreate { get; set; }
        public TypeCar TypeCar {get;set;}
        public byte[]? Avatar { get; set; }

    }
}
