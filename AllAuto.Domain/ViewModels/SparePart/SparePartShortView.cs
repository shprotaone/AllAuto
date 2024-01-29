
using AllAuto.Domain.Enum;

namespace AllAuto.Domain.ViewModels.SparePart
{
    public class SparePartShortView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public TypePart TypeSparePart { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
