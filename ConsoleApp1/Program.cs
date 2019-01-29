using ClassLibrary1;
using Hangfire;
using Hangfire.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RouteDelivery.Hangfire;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile(path: "/opt/Secrets.json", optional: false, reloadOnChange: true)
            .AddJsonFile(path: "AppSettings.json", optional: false, reloadOnChange: true)
            .Build();
            var serviceProvider = new ServiceCollection()          
           .AddScoped<IEmailService, EmailService>()
           .BuildServiceProvider();

            GlobalConfiguration.Configuration.UseSqlServerStorage(config.GetConnectionString("DefaultConnection"));
            GlobalJobFilters.Filters.Add(new LogEverythingAttribute());
            LogProvider.SetCurrentLogProvider(new CustomHangfireLogProvider());
            var options = new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 5, //  this is how default works
                //WorkerCount = 2,
                Queues = new[] { "critical","default" },
                Activator = new HangfireActivator(serviceProvider) 
            };
            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    public class HangfireActivator : Hangfire.JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
