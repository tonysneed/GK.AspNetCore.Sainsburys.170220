using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HelloConfig
{
    public class Program
    {
        // set ASPNETCORE_ENVIRONMENT=Production
        // dotnet run server.urls=http://0.0.0.0:5001
        public static void Main(string[] args)
        {
            // Plug command-line args into config
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "environment", "Development" }
                })
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var host = new WebHostBuilder()
                // Plug config into web host
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
