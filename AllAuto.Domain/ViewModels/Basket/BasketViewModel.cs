using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Domain.ViewModels.Basket
{
    public class BasketViewModel
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime DateTime { get; set; }

        public string DeliveryType {get;set; }

        public List<ItemEntryViewModel> Items { get; set; }
    }
}
