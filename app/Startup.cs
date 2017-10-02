using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DienstenCheques.Data;
using DienstenCheques.Data.Repositories;
using DienstenCheques.Filters;
using DienstenCheques.Models.Domain;
using DienstenCheques.Services;
using ApplicationUser = DienstenCheques.Models.Domain.ApplicationUser;

namespace DienstenCheques
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            var connString = $"Data Source={hostname};Initial Catalog=demoapp;User ID=demouser;Password=DemoPass12;";

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequiredLength = 8;
                    //redirect for authentication
                    config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options => {
                options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, "customer"));
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddScoped<IGebruikersRepository, GebruikersRepository>();
            services.AddScoped<GebruikerFilter>();
            services.AddTransient<DienstenChequesInitializer>();

            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DienstenChequesInitializer dienstenChequesInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();


            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            dienstenChequesInitializer.InitializeData().Wait();
        }
    }
}
