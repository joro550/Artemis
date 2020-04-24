using System;
using System.IO;
using System.Reflection;
using MediatR;
using AutoMapper;
using System.Security.Claims;
using Artemis.Web.Server.Data;
using Artemis.Web.Server.Users;
using Artemis.Web.Server.Config;
using Artemis.Web.Server.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Artemis.Web.Server.Users.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Artemis.Web.Server.Messaging.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi.Models;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Artemis Api",
                    Description = "Api for the artemis project",
                    Contact = new OpenApiContact
                    {
                        Name = "Mark Davies",
                        Email = "markdavies550@gmail.com",
                        Url = new Uri("https://twitter.com/joro550"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

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
            app.UseSwagger();

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

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artemis Api v1"));

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
