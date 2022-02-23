using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebTestService
{
    public class Program
    {
        static readonly IMetricsRoot Metrics = AppMetrics.CreateDefaultBuilder()
                                                         .OutputMetrics.AsPrometheusPlainText()
                                                         .Build();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseMetrics(o =>
                   {
                       o.EndpointOptions = eo =>
                       {
                           eo.MetricsEndpointOutputFormatter = Metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                       };
                   })
                   .ConfigureMetrics(o =>
                   {
                       o.Configuration.Configure(c =>
                       {
                           c.Enabled          = true;
                           c.ReportingEnabled = true;
                       });
                   })
                   .ConfigureAppConfiguration(cfg => cfg.AddEnvironmentVariables())
                   .UseStartup<Startup>()
                   .Build();
    }
}