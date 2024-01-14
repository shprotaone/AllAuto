using AllAuto.DAL.Interfaces;
using AllAuto.DAL.Repositories;
using AllAuto.Domain.Entity;
using AllAuto.Service.Implementations;
using AllAuto.Service.Interfaces;
using NLog.Web;

namespace AllAuto
{
    public static class StartupInit
    {
        public static void InitializeRepos(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Car>,CarRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Profile>, ProfileRepository>();
            services.AddScoped<IBaseRepository<Order>, OrderRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
        }

        public static void InitializeServices(IServiceCollection services)
        {
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
        }

        public static void InitLogger(WebApplicationBuilder builder)
        {
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();
            
        }
    }
}
