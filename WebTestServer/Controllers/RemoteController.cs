using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

#pragma warning disable CS8603

namespace WebTest.Controllers
{
    public class RemoteController : Controller
    {
        readonly string        RemoteHostName;
        readonly MetricHandler mh;

        public RemoteController(IConfiguration config, MetricHandler mh)
        {
            RemoteHostName = config["WebTestServiceAddress"];
            this.mh        = mh;
        }

        public IActionResult Index() => View();

        public async Task<ResponseModel> GetCourse(string ids)
        {
            var       sw     = Stopwatch.StartNew();
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var r = await client.GetFromJsonAsync<ResponseModel>(RemoteHostName + "/Home/GetCourse?ids=" + ids);
                if (!r!.Success)
                {
                    r.Error = "Error from remote service: " + r.Error;
                }

                mh.Set(MetricHandler.RemoteItemsCount, r.Data.Length);
                mh.Increment(MetricHandler.RemoteRequestSuccess);
                mh.Increment(MetricHandler.RemoteRequestCount);
                mh.Set(MetricHandler.RemoteRequestDuration, sw.ElapsedMilliseconds);

                return r;
            }
            catch (Exception e)
            {
                mh.Increment(MetricHandler.RemoteRequestFailed);
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
            finally
            {
                sw.Stop();
            }
        }
    }
}