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

        builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.LoginPath = "/login";
            });

        builder.Services.AddAuthorization();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHealthChecks().AddDbContextCheck<KomunalkaContext>();

        builder.Services.AddScoped<IProtocolParser, ProtocolParser>();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
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