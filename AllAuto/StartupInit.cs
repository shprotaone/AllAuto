using AllAuto.DAL.Interfaces;
using AllAuto.DAL.Repositories;
using AllAuto.Domain.Entity;
using AllAuto.Service.Implementations;
using AllAuto.Service.Interfaces;
using NLog;
using NLog.Web;

namespace AllAuto
{
    public static class StartupInit
    {
        public static void InitializeRepos(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<SparePart>,SparePartRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Profile>, ProfileRepository>();
            services.AddScoped<IBaseRepository<ItemEntry>, ItemEntryRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
            services.AddScoped<IBaseRepository<CompleteOrder>, CompleteOrderRepository>();
        }

        public static void InitializeServices(IServiceCollection services)
        {
            services.AddScoped<ISparePartService, SparePartService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IExcelReaderService<SparePart>, ExcelReaderService>();
            services.AddScoped<ICompleteOrderService, CompleteOrderService>();
        }

        public static void InitLogger(WebApplicationBuilder builder)
        {
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            }).UseNLog();

            LogManager.Configuration.Variables["myDir"] = builder.Environment.ContentRootPath + "/Logs";
        }
    }
}
