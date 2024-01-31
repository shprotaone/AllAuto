using AllAuto.Domain.ViewModels.SparePart;

namespace AllAuto.Domain.ViewModels.Order
{
    public class ItemEntryViewModel
    {
        public long Id { get; set; }

        public string Name{ get; set; }

        public string Manufactor { get; set; }

        public string DateCreate { get; set; }

        public decimal Sum { get; set; }

        public int Quantity { get; set; }
    }
}
