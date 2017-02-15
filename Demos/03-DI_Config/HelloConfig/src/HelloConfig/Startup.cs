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

            var year = copyrightOpts.Value.Year;
            var company = copyrightOpts.Value.Company;

            var copyrightText = $"Copyright {year} {company}";

            app.Run(async context =>
            {
                await context.Response.WriteAsync($"Hello Config! {copyrightText} ({env.EnvironmentName})");
            });
        }
    }
}
