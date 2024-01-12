namespace AllAuto.Domain.Entity
{
    public class Profile //подробная информация о пользователе
    {
        public long Id { get; set; }
        public string Address { get; set; }
        public byte Age { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
