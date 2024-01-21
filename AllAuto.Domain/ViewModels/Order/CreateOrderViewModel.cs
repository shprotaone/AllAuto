namespace AllAuto.Domain.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public long? Id { get; set; }
        public int SparePartIdToAdd { get; set; }
        public string? LoginId { get; set; }
        public int Quantity { get; set; }

    }
}
