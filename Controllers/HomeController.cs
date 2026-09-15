using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTeploobmenApp.Data;
using WebTeploobmenApp.Models;

namespace WebTeploobmenApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebServicesContext _context;

        public HomeController(ILogger<HomeController> logger, WebServicesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.News.ToList());
        }

        [HttpPost]
        public IActionResult Index(NewsViewModel model)
        {
            var newsQuery = _context.News.AsQueryable();
            if (model.Name != null)
                newsQuery = newsQuery.Where(v => v.Name.Contains(model.Name));
            if (model.Content != null)
                newsQuery = newsQuery.Where(v => v.Content.Contains(model.Content));
            return View(newsQuery.ToList());
        }

        public IActionResult Parameters(int id)
        {
            News? variant = _context.News.Find(id);
            NewsViewModel? model = null;
            if (variant != null)
            {
                model = new()
                {
                    Id = id,
                    Name = variant.Name,
                    Content = variant.Content,
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Parameters(NewsViewModel model)
        {
            if (model.Id == 0)
            {
                News variant = new()
                {
                    Name = model.Name,
                    Content = model.Content,
                };
                _context.News.Add(variant);
            }
            else
            {
                News? variant = _context.News.Find(model.Id);
                if (variant != null)
                {
                    variant.Name = model.Name;
                    variant.Content = model.Content;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            News? variant = _context.News.Find(id);
            if (variant != null)
            {
                _context.News.Remove(variant);
                _context.SaveChanges();
            }
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
