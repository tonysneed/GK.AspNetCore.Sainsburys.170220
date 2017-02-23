using HelloWebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;

namespace HelloWebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                options.Filters.Add(
                    new TypeFilterAttribute(typeof(ExceptionLoggingFilter)));
            })
            .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(new ConsoleLoggerSettings
            {
                Switches = new Dictionary<string, LogLevel>
                {
                    { "Microsoft", LogLevel.None },
                    { "HelloWebApi", LogLevel.Debug },
                }
            });
            app.UseMvc();
        }
    }
}
