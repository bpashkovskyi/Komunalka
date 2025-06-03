using HealthChecks.UI.Client;
using Komunalka.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Advertising.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<KomunalkaContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("KomunalkaDb"),
        sqlServerDbContextOptionsBuilder => sqlServerDbContextOptionsBuilder.MigrationsHistoryTable("__MigrationsHistory", "komunalka")));

        builder.Services.AddControllers(options => options.EnableEndpointRouting = false);
        builder.Services.AddHealthChecks().AddDbContextCheck<KomunalkaContext>();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMvc();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "CORS",
                policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
        });
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseCors("CORS");
        app.MapControllers();
        app.UseMvc();

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