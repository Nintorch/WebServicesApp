using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebTeploobmenAppClient.Models;

namespace WebTeploobmenAppClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MyConfig> _config;

        public HomeController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            return View(); // TODO
        }

        [HttpPost]
        public IActionResult Index(NewsViewModel model)
        {
            return View(); // TODO
        }

        public IActionResult Parameters(int id)
        {
            return View(); // TODO
        }

        [HttpPost]
        public IActionResult Parameters(NewsViewModel model)
        {
            return View(); // TODO
        }

        public IActionResult Delete(int id)
        {
            return View(); // TODO
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
