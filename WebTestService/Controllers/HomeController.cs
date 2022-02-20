using System;
using System.Threading;
using System.Threading.Tasks;
using Handler;
using Microsoft.AspNetCore.Mvc;

namespace WebTestService.Controllers
{
    public class HomeController : Controller
    {
        readonly CoursesHandler ch;

        public HomeController(CoursesHandler ch) => this.ch = ch;

        public async Task<ResponseModel> GetCourse(string ids)
        {
            Thread.Sleep(200);
            try
            {
                var _ids = (ids ?? throw new ArgumentNullException(nameof(ids)))
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                
                return new ResponseModel(await ch.Get(_ids), Request.Headers);
            }
            catch (Exception e)
            {
                return new ResponseModel((e.InnerException ?? e).Message, Request.Headers);
            }
        }
    }
}