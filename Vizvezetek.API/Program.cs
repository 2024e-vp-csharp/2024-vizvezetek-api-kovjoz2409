using Microsoft.EntityFrameworkCore;
using Vizvezetek.API.Models;

namespace Vizvezetek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<vizvezetekContext>(options =>
            {
                var connnectionString = builder.Configuration.GetConnectionString("Default");
                options.UseMySql(connnectionString, ServerVersion.AutoDetect(connnectionString));
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}