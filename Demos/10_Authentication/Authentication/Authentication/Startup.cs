using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authentication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Require authenticated users
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Add Mvc types
            services.AddMvc(options =>
            {
                // Add global auth filter
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(typeof(ValidateAntiForgeryTokenAttribute));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true,
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                SignInScheme = "Cookies",
                ClientId = "585966796531-oc9ab04c396lrj3lea09c51i7njb9gpj.apps.googleusercontent.com",
                ClientSecret = "oPmCaX1KsRUMl193Y9zwVnsP",
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
