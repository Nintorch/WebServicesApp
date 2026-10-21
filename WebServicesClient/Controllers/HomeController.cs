using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebTeploobmenAppClient.Helpers;
using WebTeploobmenAppClient.Models;

namespace WebTeploobmenAppClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MyConfig> _config;

        public HomeController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        private async Task<List<CategoryViewModel>> GetCategories()
        {
            Uri uri = new(_config.Value.ServerRequestUri + "Category/GetCategories");
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            HttpResponseMessage response = await client.GetAsync(uri);
            List<CategoryViewModel> categories = await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>() ?? [];
            return categories;
        }

        public async Task<IActionResult> Index()
        {
            Uri uri = new(_config.Value.ServerRequestUri + "News/GetNews");
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            HttpResponseMessage response = await client.GetAsync(uri);
            return View(new NewsCategoriesViewModel()
            {
                News = await response.Content.ReadFromJsonAsync<List<NewsViewModel>>(),
                Categories = await GetCategories()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewsViewModel model)
        {
            Uri uri = new(_config.Value.ServerRequestUri + "News/SearchNews");
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, model);
            return View(new NewsCategoriesViewModel()
            {
                News = await response.Content.ReadFromJsonAsync<List<NewsViewModel>>(),
                Categories = await GetCategories()
            });
        }

        public async Task<IActionResult> Parameters(int id)
        {
            if (id > 0)
            {
                Uri uri = new(_config.Value.ServerRequestUri + "News/GetNews/?id=" + id);
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                HttpResponseMessage response = await client.GetAsync(uri);
                List<NewsViewModel>? value = await response.Content.ReadFromJsonAsync<List<NewsViewModel>>();
                if (value == null || value.Count == 0)
                {
                    return View();
                }

                return View(new NewsModifyViewModel() { News = value[0], Categories = await GetCategories()});
            }
            return View(new NewsModifyViewModel() { News = null, Categories = await GetCategories() });
        }

        [HttpPost]
        public async Task<IActionResult> Parameters(NewsViewModel model)
        {
            if (model.Id == 0)
            {
                Uri uri = new(_config.Value.ServerRequestUri + "News/CreateNews");
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                var a = await client.PutAsJsonAsync(uri, model);
            }
            else
            {
                Uri uri = new(_config.Value.ServerRequestUri + "News/ModifyNews");
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                await client.PatchAsJsonAsync(uri, model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Uri uri = new(_config.Value.ServerRequestUri + "News/DeleteNews/?id=" + id);
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            await client.DeleteAsync(uri);
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
