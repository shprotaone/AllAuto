namespace AllAuto.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string CarName{ get; set; }

        public string Model{ get; set; }

        public double MaxSpeed { get; set; }

        public string DateCreate { get; set; }

        public byte[]? Image { get; set; }
    }
}
