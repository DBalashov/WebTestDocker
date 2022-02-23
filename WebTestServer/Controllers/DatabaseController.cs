using System.Linq;
using Handler;
using Microsoft.AspNetCore.Mvc;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class DatabaseController : Controller
    {
        readonly IDatabaseHandler db;

        public DatabaseController(IDatabaseHandler db) => this.db = db;

        public IActionResult Index()
        {
            var items = db.GetLast(100).Select(p => new LogItemModel(p)).ToArray();
            return View(items);
        }
    }
}