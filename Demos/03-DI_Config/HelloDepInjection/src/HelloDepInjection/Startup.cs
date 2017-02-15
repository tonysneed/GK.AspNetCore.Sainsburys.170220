using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloDepInjection
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Register interfaces with DI container
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Get repository interface from DI container
            var name = "Peter";
            //var customerRepo = app.ApplicationServices.GetService<ICustomerRepository>();
            //var city = customerRepo.GetCity(name);

            app.Run(async context =>
            {
                // Get repository interface from DI container
                var customerRepo = context.RequestServices.GetService<ICustomerRepository>();
                var city = customerRepo.GetCity(name);
                await context.Response.WriteAsync($"Hello {name} from {city}! - {env.EnvironmentName}");
            });
        }
    }
}
