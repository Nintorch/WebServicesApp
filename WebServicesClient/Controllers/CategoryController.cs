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
            HttpResponseMessage response = await new HttpClient().GetAsync(_config.Value.ServerRequestUri + "Category/GetCategories");
            return View(await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CategoryViewModel model)
        {
            HttpResponseMessage response = await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "Category/SearchCategories", model);
            return View(await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>());
        }

        public async Task<IActionResult> Parameters(int id)
        {
            if (id > 0)
            {
                HttpResponseMessage response = await new HttpClient().GetAsync(_config.Value.ServerRequestUri + "Category/GetCategories/?id=" + id);
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
                await new HttpClient().PutAsJsonAsync(_config.Value.ServerRequestUri + "Category/CreateCategory", model);
            }
            else
            {
                await new HttpClient().PatchAsJsonAsync(_config.Value.ServerRequestUri + "Category/ModifyCategory", model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await new HttpClient().DeleteAsync(_config.Value.ServerRequestUri + "Category/DeleteCategory/?id=" + id);
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
