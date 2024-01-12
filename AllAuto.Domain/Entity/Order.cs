namespace AllAuto.Domain.Entity
{
    public class Order
    {
        public long Id { get; set; }

        public long? CardId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Address { get; set; }

        public long? BasketId { get; set; }

        public virtual Basket Basket { get; set; } //virtual из за LazyLoad

    }
}
