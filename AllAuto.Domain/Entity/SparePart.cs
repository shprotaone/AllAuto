using AllAuto.Domain.Enum;

namespace AllAuto.Domain.Entity
{
    public class SparePart
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreate { get; set; }
        public TypePart TypeSparePart {get;set;}
        public int Amount { get; set; }
        public byte[]? Avatar { get; set; }

    }
}
