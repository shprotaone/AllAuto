using AllAuto;
using AllAuto.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRTelegramBot.Core;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connection = builder.Configuration.GetConnectionString("DefaultConnection");


services.AddControllersWithViews();
services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connection,ma => ma.MigrationsAssembly("AllAuto.DAL")),ServiceLifetime.Singleton);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

StartupInit.InitializeRepos(services);
StartupInit.InitializeServices(services);
StartupInit.InitLogger(builder);



//services.AddScoped(sp => _bot);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DBInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var _bot = new PRBot(config =>
{
    config.Token = "6612601470:AAE2fBc9wCbNhEoyMoIhTXnZdsDdlSSlY_M";
    config.ClearUpdatesOnStart = true;
    config.WhiteListUsers = new List<long>() { };
    config.Admins = new List<long>();
    config.BotId = 0;
}, app.Services.GetRequiredService<IServiceProvider>());


await _bot.Start();
app.Run();

