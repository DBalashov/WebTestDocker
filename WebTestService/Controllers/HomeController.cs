using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;

namespace WebTestService.Controllers
{
    public class HomeController : Controller
    {
        static string UID = Guid.NewGuid().ToString();

        readonly CoursesHandler ch;
        readonly MetricHandler  mh;

        public HomeController(CoursesHandler ch, MetricHandler mh)
        {
            this.ch = ch;
            this.mh = mh;
        }

        public async Task<ResponseModel> GetCourse(string ids)
        {
            Thread.Sleep(new Random().Next(200));
            try
            {
                var _ids = (ids ?? throw new ArgumentNullException(nameof(ids)))
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var result = new ResponseModel(await ch.Get(_ids), Request.Headers);
                result.RequestHeaders = result.RequestHeaders.Append(new RequestHeader() { Name = "X-Host-UID", Value = UID }).ToArray();

                mh.Set(MetricHandler.LocalItemsCount, result.Data.Length);
                mh.Increment(MetricHandler.LocalRequestSuccess);
                mh.Increment(MetricHandler.LocalRequestCount);

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