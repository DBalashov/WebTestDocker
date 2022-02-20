using System;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ResponseModel> GetCourse(string ids)
        {
            Thread.Sleep(new Random().Next(200));
            try
            {
                var _ids = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = new ResponseModel(await ch.Get(_ids), Request.Headers);
                mh.Increment(MetricHandler.RequestSuccess);
                mh.Increment(MetricHandler.RequestCount);
                return result;
            }
            catch (Exception e)
            {
                mh.Increment(MetricHandler.RequestFailed);
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
        }
    }
}