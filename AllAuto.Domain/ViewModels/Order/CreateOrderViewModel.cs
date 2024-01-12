namespace AllAuto.Domain.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public long CarId { get; set; }
        public string? LoginId { get; set; }
        public int Quantity { get; set; }
    }
}
