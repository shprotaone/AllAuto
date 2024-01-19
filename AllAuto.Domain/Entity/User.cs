using AllAuto.Domain.Enum;

namespace AllAuto.Domain.Entity
{
    public class User //свойства для регистрации 
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public Profile Profile { get; set; }

        public Basket? Basket { get; set; }
    }
}
