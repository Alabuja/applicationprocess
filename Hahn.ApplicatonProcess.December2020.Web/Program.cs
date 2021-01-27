using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hahn.ApplicatonProcess.December2020.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

             Log.Logger = new LoggerConfiguration()
                .WriteTo.File(configSettings["LogPath"], rollOnFileSizeLimit: true, fileSizeLimitBytes: 100000)
                .CreateLogger();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddFilter("Microsoft", LogLevel.Information);
                        logging.AddFilter("System", LogLevel.Error);
                        logging.SetMinimumLevel(LogLevel.Trace);
                        logging.AddSerilog();
                        logging.AddConfiguration(configSettings);
                    });
                });
        }
    }
}
