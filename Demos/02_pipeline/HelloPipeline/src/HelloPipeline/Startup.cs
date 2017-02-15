using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloPipeline
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Logging middleware
            app.Use(async (ctx, next) =>
            {
                Console.WriteLine($"Pre-processing request: {ctx.Request.Path}");
                await next();
                Console.WriteLine($"Post-processing response: {ctx.Response.StatusCode}");
            });

            // Branch the pipeline
            app.Map("/hello", builder =>
            {
                builder.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("Hello ASP.NET 5 App");
                });
            });

            // Add support for static files
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = ""
            });

            // Add an endpoint
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
