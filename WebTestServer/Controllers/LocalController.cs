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

        public LocalController(CoursesHandler ch) => this.ch = ch;

        public IActionResult Index() => View();

        public async Task<ResponseModel> GetCourse(string ids)
        {
            Thread.Sleep(200);
            try
            {
                var _ids = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return new ResponseModel(await ch.Get(_ids), Request.Headers);
            }
            catch (Exception e)
            {
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
        }
    }
}