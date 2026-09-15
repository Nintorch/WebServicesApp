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
        private readonly TeploobmenContext _context;

        public HomeController(ILogger<HomeController> logger, TeploobmenContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Variants.ToList());
        }

        public IActionResult Parameters(int id)
        {
            Variant? variant = _context.Variants.Find(id);
            VariantViewModel? model = null;
            if (variant != null)
            {
                model = new()
                {
                    Id = id,
                    Name = variant.Name,
                    Number1 = variant.Number1,
                    Number2 = variant.Number2,
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Parameters(VariantViewModel model)
        {
            if (model.Id == 0)
            {
                Variant variant = new()
                {
                    Name = model.Name,
                    Number1 = model.Number1,
                    Number2 = model.Number2,
                };
                _context.Variants.Add(variant);
            }
            else
            {
                Variant? variant = _context.Variants.Find(model.Id);
                if (variant != null)
                {
                    variant.Name = model.Name;
                    variant.Number1 = model.Number1;
                    variant.Number2 = model.Number2;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Variant? variant = _context.Variants.Find(id);
            if (variant != null)
            {
                _context.Variants.Remove(variant);
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
