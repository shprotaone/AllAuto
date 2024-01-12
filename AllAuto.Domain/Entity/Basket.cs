namespace AllAuto.Domain.Entity
{
    public class Basket
    {
        public long Id { get; set; }

        public User User { get; set; } //навигационное свойство

        public long UserId { get; set; } //Навигационное свойство

        public List<Order> Orders { get; set; }
    }
}
