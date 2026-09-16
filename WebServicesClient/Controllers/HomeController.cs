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
            HttpResponseMessage response = await new HttpClient().GetAsync(_config.Value.ServerRequestUri + "News/GetNews");
            return View(await response.Content.ReadFromJsonAsync<List<NewsViewModel>>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewsViewModel model)
        {
            HttpResponseMessage response = await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "News/SearchNews", model);
            return View(await response.Content.ReadFromJsonAsync<List<NewsViewModel>>());
        }

        public async Task<IActionResult> Parameters(int id)
        {
            if (id > 0)
            {
                HttpResponseMessage response = await new HttpClient().GetAsync(_config.Value.ServerRequestUri + "News/GetNews/?id=" + id);
                List<NewsViewModel>? value = await response.Content.ReadFromJsonAsync<List<NewsViewModel>>();
                if (value == null || value.Count == 0)
                {
                    return View();
                }
                return View(value[0]);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Parameters(NewsViewModel model)
        {
            if (model.Id == 0)
            {
                await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "News/CreateNews", model);
            }
            else
            {
                await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "News/ModifyNews", model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await new HttpClient().GetAsync(_config.Value.ServerRequestUri + "News/DeleteNews/?id=" + id);
            return RedirectToAction("Index");
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
