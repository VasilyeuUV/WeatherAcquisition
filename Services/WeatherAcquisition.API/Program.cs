using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Serilog;

namespace WeatherAcquisition.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)) // подключение Serilog с чтением конфигурации из хоста
                .ConfigureWebHostDefaults(host => host
                    .UseStartup<Startup>()
                );
    }
}
