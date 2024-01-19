using AllAuto.Domain.ViewModels.SparePart;

namespace AllAuto.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string Name{ get; set; }

        public string Manufactor { get; set; }

        public string DateCreate { get; set; }

        public decimal Price { get; set; }
    }
}
