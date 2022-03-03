using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using DotnetTodoApiLab.Api.Extensions;
using DotnetTodoApiLab.Infrastructure.Context;

namespace DotnetTodoApiLab.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = SerilogExtension.CreateLogger();
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                Log.Logger.Information("Application starting up...");
                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                await host.RunAsync();
            }
            catch(Exception ex)
            {
                Log.Logger.Fatal(ex, "Application startup failed.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
