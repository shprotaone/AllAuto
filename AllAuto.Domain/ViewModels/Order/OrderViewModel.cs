using AllAuto.Domain.ViewModels.SparePart;

namespace AllAuto.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public List<SparePartViewModel> PartList{ get; set; }

        public string DateCreate { get; set; }

        public byte[]? Image { get; set; }
    }
}
