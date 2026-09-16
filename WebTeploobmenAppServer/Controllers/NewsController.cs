using Microsoft.AspNetCore.Mvc;
using WebTeploobmenAppClient.Models;
using WebTeploobmenAppServer.Data;
using WebTeploobmenAppServer.Models;

namespace WebTeploobmenAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly WebServicesContext _context;

        public NewsController(ILogger<NewsController> logger, WebServicesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetNews")]
        public List<News> GetNews(int id)
        {
            if (id == 0)
            {
                return _context.News.ToList();
            }
            News? news = _context.News.Find(id);
            if (news == null)
            {
                return [];
            }
            return [news];
        }

        [HttpPost("SearchNews")]
        public List<News> SearchNews(NewsViewModel model)
        {
            var newsQuery = _context.News.AsQueryable();
            if (model.Name != null)
                newsQuery = newsQuery.Where(v => v.Name.Contains(model.Name));
            if (model.Category != null)
                newsQuery = newsQuery.Where(v => v.Category.Contains(model.Category));
            if (model.Content != null)
                newsQuery = newsQuery.Where(v => v.Content.Contains(model.Content));
            return newsQuery.ToList();
        }

        [HttpPost("CreateNews")]
        public IActionResult CreateNews(News model)
        {
            News variant = new()
            {
                Name = model.Name,
                Category = model.Category,
                Content = model.Content,
            };
            _context.News.Add(variant);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("ModifyNews")]
        public IActionResult ModifyNews(News model)
        {
            News? variant = _context.News.Find(model.Id);
            if (variant != null)
            {
                variant.Name = model.Name;
                variant.Category = model.Category;
                variant.Content = model.Content;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("DeleteNews")]
        public IActionResult DeleteNews(int id)
        {
            News? variant = _context.News.Find(id);
            if (variant != null)
            {
                _context.News.Remove(variant);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
