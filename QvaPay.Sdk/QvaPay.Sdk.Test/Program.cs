using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;

namespace QvaPay.Sdk.Test
{
    public class Program
    {
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            // Set up the objects we need to get to configuration settings
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .ReadFrom.Configuration(config)
               .WriteTo.Console(theme: SystemConsoleTheme.Colored)
               .WriteTo.Async(a => a.File($"logs/log [{DateTime.Now:yyyyMMdd HHmmss}] {Guid.NewGuid()}.txt", rollingInterval: RollingInterval.Infinite, buffered: true))
               .CreateLogger();

            services.AddLogging(logging =>
            {
                //logging.AddConfiguration(config);
                //logging.AddConsole();
                logging.AddSerilog(Log.Logger, dispose: true);
            });
            services.AddSingleton(config);
            //services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory(Log.Logger, true));

            // QvaPay SDK
            services.AddQvaPayClient();

            services.AddTransient<Program>();
            return services;
        }

        private static async Task Main()
        {
            var services = ConfigureServices();

            var provider = services.BuildServiceProvider();

            Log.Information("TEST START");
            try
            {
                await RunTests(provider);
            }
            catch (Exception ex)
            {
                Log.Fatal("FATAL EXCEPTION {ex}", ex);
            }
            finally
            {
                Log.Information("TEST END");
                Log.CloseAndFlush();
                Console.ReadLine();
            }
        }

        private static async Task RunTests(ServiceProvider provider)
        {
            await TestGetAppInfo(provider);
            await TestGetBalance(provider);
            await TestCreateInvoice(provider);
            await TestGetTransactions(provider);
        }

        private static async Task TestGetAppInfo(ServiceProvider provider)
        {
            var client = provider.GetQvaPayClient();

            var appInfo = await client.GetAppInfo();

            Log.Information("{@AppInfo}", appInfo);
        }

        private static async Task TestGetBalance(ServiceProvider provider)
        {
            var client = provider.GetQvaPayClient();

            var balance = await client.GetBalance();

            Log.Information("{@Balance}", balance);
        }

        private static async Task TestCreateInvoice(ServiceProvider provider)
        {
            var client = provider.GetQvaPayClient();

            var invoice = await client.CreateInvoice(20, "test", "remoteid", true);

            Log.Information("{@Invoice}", invoice);
        }

        private static async Task TestGetTransactions(ServiceProvider provider)
        {
            var client = provider.GetQvaPayClient();

            var transactions = await client.GetTransactions();

            Log.Information("{@Transactions}", transactions);
        }
    }
}
