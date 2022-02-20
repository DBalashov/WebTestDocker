using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

#pragma warning disable CS8603

namespace WebTest.Controllers
{
    public class RemoteController : Controller
    {
        readonly string RemoteHostName;

        public RemoteController(IConfiguration config)
        {
            RemoteHostName = config["WebTestServiceAddress"];
        }

        public IActionResult Index() => View();

        public async Task<ResponseModel> GetCourse(string ids)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var r = await client.GetFromJsonAsync<ResponseModel>(RemoteHostName + "/Home/GetCourse?ids=" + ids);
                if (!r!.Success)
                {
                    r.Error = "Error from remote service: " + r.Error;
                }

                return r;
            }
            catch (Exception e)
            {
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
        }
    }
}