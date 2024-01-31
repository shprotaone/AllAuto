namespace AllAuto.Domain.ViewModels.Order
{
    public class CompleteOrderView
    {
        public long Id { get; set; } //должен присваиватся автоматически

        public long UserId { get; set; } //Навигационное свойство

        public string FullName { get; set; }

        public string Address { get; set; }

        public decimal Sum { get; set; }

        public DateTime DateTime { get; set; }

        public string DeliveryType { get; set; }
    }
}
