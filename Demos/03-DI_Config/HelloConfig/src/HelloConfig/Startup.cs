using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelloConfig
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Add json settings files
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath) // Required
                .AddJsonFile("appsettings.json")
                // Last addition wins
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets()
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Plug config values into DI system
            services.AddOptions();
            services.Configure<Copyright>(options =>
            {
                options.Year = int.Parse(Configuration.GetSection("copyright:year").Value);
                options.Company = Configuration.GetSection("copyright:company").Value;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IOptions<Copyright> copyrightOpts)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //IConfigurationSection copyright = Configuration.GetSection("copyright");
            //int year = int.Parse(Configuration.GetSection("copyright:year").Value);
            //string company = Configuration.GetSection("copyright:company").Value;

            var secret = Configuration.GetSection("MySecret").Value;
            var copyrightText = $"Copyright {copyrightOpts.Value.Year} {copyrightOpts.Value.Company}";

            var message = $"Hello ASP.NET Core!" +
                $"\n\nSecret: {secret}\nEnvironment: {env.EnvironmentName}\n{copyrightText}";

            app.Run(async context =>
            {
                await context.Response.WriteAsync(message);
            });
        }
    }
}
