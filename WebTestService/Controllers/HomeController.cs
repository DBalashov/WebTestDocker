using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebTestService.Controllers
{
    public class HomeController : Controller
    {
        static string UID = Environment.GetEnvironmentVariable("HOSTNAME");

        readonly CoursesHandler ch;
        readonly MetricHandler  mh;

        public HomeController(CoursesHandler ch, MetricHandler mh)
        {
            if (string.IsNullOrWhiteSpace(UID))
                UID = Guid.NewGuid().ToString();

            this.ch = ch;
            this.mh = mh;
        }

        public async Task<ResponseModel> GetCourse(string ids, [FromServices] IDatabaseHandler db)
        {
            switch (ids)
            {
                case "crash":
                    Environment.Exit(-1);
                    break;
              
                case "500":
                    throw new InvalidOperationException("FAILED INPUT");
            }

            Thread.Sleep(new Random().Next(200)); // fake delay for emulate slow network / long response 
            try
            {
                var _ids = (ids ?? throw new ArgumentNullException(nameof(ids)))
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var result = new ResponseModel(await ch.Get(_ids), Request.Headers);
                result.RequestHeaders = result.RequestHeaders.Append(new RequestHeader() { Name = "X-Host-UID", Value = UID }).ToArray();

                mh.Set(MetricHandler.LocalItemsCount, result.Data.Length);
                mh.Increment(MetricHandler.LocalRequestSuccess);
                mh.Increment(MetricHandler.LocalRequestCount);

                db.Put(new DBLogItem()
                {
                    data = JsonConvert.SerializeObject(result),
                    host = UID,
                    dt   = DateTime.UtcNow
                });

                return result;
            }
            catch (Exception e)
            {
                mh.Increment(MetricHandler.LocalRequestFailed);
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
        }
    }
}