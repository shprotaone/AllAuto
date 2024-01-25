using AllAuto.DAL;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Helpers;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AllAuto
{
    public static class DBInitializer
    {
        public async static Task Initialize(ApplicationDbContext context)
        {
            //await AddParts(context);
            //await AddUsers(context);
            //await AddBaskets(context);

            //await AddOrders(context);
            await context.SaveChangesAsync();
        }

        private async static Task AddParts(ApplicationDbContext context)
        {
            IExcelReaderService<SparePart> excelReaderService = context.GetService<IExcelReaderService<SparePart>>();
            await excelReaderService.ReadExcelFile();
        }

        private async static Task AddOrders(ApplicationDbContext context)
        {
            var orders = new List<ItemEntry>()
            {
                new ItemEntry{BasketId=1,SparePartId=9,Quantity=1},
                new ItemEntry{BasketId=1,SparePartId=10,Quantity=1},
                new ItemEntry{BasketId=1,SparePartId=11,Quantity = 1},
                new ItemEntry{BasketId=2,SparePartId=1,Quantity=1},
                new ItemEntry{BasketId=2,SparePartId=2,Quantity=1},
                new ItemEntry{BasketId=2,SparePartId=3, Quantity = 1},

            };
            context.ItemEntries.AddRange(orders);
            await context.SaveChangesAsync();
        }

        private async static Task AddBaskets(ApplicationDbContext context)
        {
            var baskets = new List<Basket>()
            {
                new Basket{UserId=2},
                new Basket{UserId=3},
                new Basket{UserId=4},
                new Basket{UserId=5},
                new Basket{UserId=6},
            };

            context.Baskets.AddRange(baskets);
            await context.SaveChangesAsync();
        }

        private async static Task AddUsers(ApplicationDbContext context)
        {
            var users = new List<User>()
            {
                new User{Name="Николай",Password=HashPasswordHelper.HasPassword("666666"),Role=0},
                new User{Name="Вася",Password=HashPasswordHelper.HasPassword("111111"),Role=0},
                new User{Name="Петя",Password=HashPasswordHelper.HasPassword("222222"),Role=0},
                new User{Name="Маша",Password=HashPasswordHelper.HasPassword("333333"),Role=0},
                new User{Name="Саша",Password=HashPasswordHelper.HasPassword("444444"),Role=0},
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}
