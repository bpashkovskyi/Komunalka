using Komunalka.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Accidents.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<KomunalkaContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("KomunalkaDb")));

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

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

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.Run();
    }
}