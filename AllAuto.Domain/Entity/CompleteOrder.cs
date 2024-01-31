using AllAuto.Domain.Enum;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Domain.Entity
{
    public class CompleteOrder
    {
        public long Id { get; set; } //должен присваиватся автоматически

        public User User { get; set; } //навигационное свойство

        public long UserId { get; set; } //Навигационное свойство

        public string FullName { get; set; }

        public string Address { get; set; }

        public decimal Sum { get; set; }

        public DateTime DateTime { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public ICollection<ItemEntry> ItemEntries { get; set; }
    }
}
