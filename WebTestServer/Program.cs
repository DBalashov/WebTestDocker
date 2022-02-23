using System;
using System.Linq;
using System.Net;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebTest
{
    public class Program
    {
        static readonly IMetricsRoot Metrics = AppMetrics.CreateDefaultBuilder()
                                                         .OutputMetrics.AsPrometheusPlainText()
                                                         .Build();

        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
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
}