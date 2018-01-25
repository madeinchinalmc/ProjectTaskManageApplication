using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.DependencyInjection;
using WorkingTask.Data;

namespace ProjectTaskManageApplication
{
    public class Program
    {
        private ContainerBuilder container;
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TaskManageContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private void fun1()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<MyComponent>();
            builder.RegisterType<ConsoleLogger>().As<ILogger>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                //var component = container.Resolve<MyComponent>();
            }
        }
    }
}
