namespace AllAuto.Domain.Entity
{
    public class Order
    {
        public long Id { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsClosed { get; set; }

        public long? BasketId { get; set; }    

        public virtual Basket Basket { get; set; } //virtual из за LazyLoad

        public int SparePartId { get; set; }

        public SparePart SparePart { get; set; }

        public int Quantity { get; set; }

    }
}
