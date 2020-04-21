using System.Security.Claims;
using Artemis.Web.Server.Config;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Employee;
using Artemis.Web.Server.Messaging;
using Artemis.Web.Server.Messaging.Adapters;
using Artemis.Web.Server.Users;
using Artemis.Web.Server.Users.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Twilio;

namespace Artemis.Web.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("HelpUs"));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.Configure<UserConfig>(_configuration.GetSection("User"));
            services.Configure<Captcha>(_configuration.GetSection("Captcha"));
            services.Configure<TwilioConfig>(_configuration.GetSection("Twilio"));
            services.Configure<SendGridConfiguration>(_configuration.GetSection("SendGrid"));

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<DataSeeder>();
            services.AddTransient<UserAdapter>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<TwilioMessageAdapterFactory>();

            services.AddTransient(sp => sp.GetService<TwilioMessageAdapterFactory>()
                .GetMessagingClient());


            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
