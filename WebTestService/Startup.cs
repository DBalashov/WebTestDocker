using System;
using Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebTestService
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAppMetricsSystemMetricsCollector();
            
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
            
            services.AddScoped<CoursesHandler>();
            services.AddSingleton<MetricHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseHandler db, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            if (db.Enabled)
            {
                logger.LogInformation("Using database");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}