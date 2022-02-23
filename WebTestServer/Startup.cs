using System;
using App.Metrics;
using Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#else
            services.AddControllersWithViews();
#endif

            var connectionString = Configuration["DatabaseConnectionString"];
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddScoped<DBContext>(d => new DBContext(connectionString));
                services.AddScoped<IDatabaseHandler, DatabaseHandler>();
            }
            else
            {
                services.AddScoped<IDatabaseHandler, DatabaseHandlerStub>();
            }

            services.AddAppMetricsSystemMetricsCollector();
            services.AddScoped<CoursesHandler>();
            services.AddSingleton<MetricHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            var db = serviceProvider.GetRequiredService<IDatabaseHandler>();
            if (db.Enabled)
            {
                logger.LogInformation("Using database");
                db.PrepareDatabase();
            }

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}