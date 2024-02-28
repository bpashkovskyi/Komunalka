using Komunalka.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Protocols.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<KomunalkaContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("KomunalkaDb")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}