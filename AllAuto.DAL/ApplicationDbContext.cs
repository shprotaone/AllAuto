using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

            Database.EnsureCreated();
        }

        public DbSet<SparePart> SpareParts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<ItemEntry> ItemEntries { get; set; }

        public DbSet<CompleteOrder> CompleteOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //маппинг
            UserMapping(modelBuilder);
            ProfileMapping(modelBuilder);
            BasketMapping(modelBuilder);
            ItemEntriesMapping(modelBuilder);
            CompleteOrdersMapping(modelBuilder);
        }

        private void CompleteOrdersMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompleteOrder>(modelBuilder =>
            modelBuilder.ToTable("CompleteOrders").HasKey(x => x.Id)        
            );
        }

        private void ItemEntriesMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntry>(modelBuilder =>
            {
                modelBuilder.ToTable("ItemEntries").HasKey(x => x.Id);
                modelBuilder.HasOne(r => r.Basket)
                .WithMany(t => t.ItemEntries)
                .HasForeignKey(r => r.BasketId);

                modelBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            });           
        }

        private void BasketMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(modelBuilder =>
            {
                modelBuilder.ToTable("Baskets").HasKey(x => x.Id);
                modelBuilder.HasData(new Basket()
                {
                    Id = 1,
                    UserId = 1
                });
            });
        }

        private void ProfileMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>(builder =>
            {
                builder.ToTable("Profiles").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Age);
                builder.Property(x => x.Address).HasMaxLength(250).IsRequired(false);

                builder.HasData(new Profile
                {
                    Id = 1,
                    UserId = 1
                });
            });
        }

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasData(new User
                {
                    Id = 1,
                    Name = "Admin",
                    Password = HashPasswordHelper.HasPassword("123456"),
                    Role = Role.Admin
                });

                builder.ToTable("Users").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();//Автогенерация ID
                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

                builder.HasOne(x => x.Profile)
                .WithOne(x => x.User)
                .HasPrincipalKey<User>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
