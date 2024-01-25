namespace AllAuto.Domain.Entity
{
    public class CompleteOrder
    {
        public long Id { get; set; }

        public User User { get; set; } //навигационное свойство

        public long UserId { get; set; } //Навигационное свойство

        public ICollection<ItemEntry> ItemEntries { get; set; }
    }
}
