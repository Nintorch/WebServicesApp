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
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IOptions<MyConfig> _config;

        public CategoryController(ILogger<CategoryController> logger, IOptions<MyConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            Uri uri = new(_config.Value.ServerRequestUri + "Category/GetCategories");
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            HttpResponseMessage response = await client.GetAsync(uri);
            return View(await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CategoryViewModel model)
        {
            Uri uri = new(_config.Value.ServerRequestUri + "Category/SearchCategories");
            HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, model);
            return View(await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>());
        }

        public async Task<IActionResult> Parameters(int id)
        {
            if (id > 0)
            {
                Uri uri = new(_config.Value.ServerRequestUri + "Category/GetCategories/?id=" + id);
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                HttpResponseMessage response = await client.GetAsync(uri);
                List<CategoryViewModel>? value = await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>();
                if (value == null || value.Count == 0)
                {
                    return View();
                }
                return View(value[0]);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Parameters(CategoryViewModel model)
        {
            if (model.Id == 0)
            {
                Uri uri = new(_config.Value.ServerRequestUri + "Category/CreateCategory");
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                await client.PutAsJsonAsync(uri, model);
            }
            else
            {
                Uri uri = new(_config.Value.ServerRequestUri + "Category/ModifyCategory");
                HttpClient client = RequestHelper.PrepareClient(uri, RequestHelper.GetToken(HttpContext));
                await client.PatchAsJsonAsync(uri, model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Uri uri = new(_config.Value.ServerRequestUri + "Category/DeleteCategory/?id=" + id);
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
