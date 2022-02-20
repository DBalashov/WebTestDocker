using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            var envs = Environment.GetEnvironmentVariables();

            var m = new HomeIndexModel() { Env = new Dictionary<string, string>() };
            foreach (var key in envs.Keys.OfType<string>())
                m.Env.Add(key, envs[key]!.ToString()!);

            m.RequestHeaders = Request.Headers.ToDictionary(p => p.Key, p => p.Value.ToString());
            m.Interfaces = NetworkInterface.GetAllNetworkInterfaces()
                                           .Select(p =>
                                           {
                                               var a = p.GetIPProperties().UnicastAddresses;
                                               return new NetworkInterfaceItem()
                                               {
                                                   Name      = p.Name + ": " + p.NetworkInterfaceType,
                                                   Addresses = !a.Any() ? "-" : string.Join("<br/>", a.Select(add => add.Address.ToString()))
                                               };
                                           })
                                           .ToArray();
            return View(m);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}