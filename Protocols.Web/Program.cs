using HealthChecks.UI.Client;

using Komunalka.Persistence;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Protocols.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<KomunalkaContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("KomunalkaDb"),
            sqlServerDbContextOptionsBuilder => sqlServerDbContextOptionsBuilder.MigrationsHistoryTable("__MigrationsHistory", "komunalka")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.UseHealthChecks(
            "/healthchecks",
            new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

        app.Run();
    }
}