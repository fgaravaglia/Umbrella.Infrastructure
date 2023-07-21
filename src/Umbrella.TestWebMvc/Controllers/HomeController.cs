using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Umbrella.Infrastructure.Cache;
using Umbrella.TestWebMvc.Models;

namespace Umbrella.TestWebMvc.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;
        readonly IMemoryCache _Cache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
            this._Cache = cache ?? throw new ArgumentNullException(nameof(cache));  
        }

        public IActionResult Index()
        {
            DateTime cachedDate;
            if (this._Cache.TryGetObject<DateTime>("TEST", out cachedDate))
            {
                ViewBag.Date = cachedDate;
            }
            else
                ViewBag.Date = DateTime.Now.AddYears(10);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}