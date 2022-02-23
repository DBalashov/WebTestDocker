using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebTest.Controllers
{
    public class LocalController : Controller
    {
        readonly CoursesHandler ch;
        readonly MetricHandler  mh;

        public LocalController(CoursesHandler ch, MetricHandler mh)
        {
            this.ch = ch;
            this.mh = mh;
        }

        public IActionResult Index() => View();

        public async Task<ResponseModel> GetCourse(string ids, [FromServices] IDatabaseHandler db)
        {
            var sw = Stopwatch.StartNew();
            Thread.Sleep(new Random().Next(200));
            try
            {
                var _ids   = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = new ResponseModel(await ch.Get(_ids), Request.Headers);

                mh.Set(MetricHandler.LocalItemsCount, result.Data.Length);
                mh.Increment(MetricHandler.LocalRequestSuccess);
                mh.Increment(MetricHandler.LocalRequestCount);
                mh.Set(MetricHandler.LocalRequestDuration, sw.ElapsedMilliseconds);

                db.Put(new DBLogItem()
                {
                    data = JsonConvert.SerializeObject(result),
                    host = "LOCAL",
                    dt   = DateTime.UtcNow
                });
                
                return result;
            }
            catch (Exception e)
            {
                mh.Increment(MetricHandler.LocalRequestFailed);
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
            finally
            {
                sw.Stop();
            }
        }
    }
}