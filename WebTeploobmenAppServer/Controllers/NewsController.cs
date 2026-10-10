using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Получить список всех новостей, или информацию о конкретной новости
        /// </summary>
        /// <param name="id">ID новости, или 0 для просмотра списка всех новостей</param>
        /// <returns></returns>
        [HttpGet("GetNews")]
        public List<NewsViewModel> GetNews(int id)
        {
            if (id == 0)
            {
                return _context.News.Select(n => n.CreateViewModel(_context)).ToList();
            }
            News? news = _context.News.Find(id);
            if (news == null)
            {
                return [];
            }
            return [news.CreateViewModel(_context)];
        }

        /// <summary>
        /// Фильтрация новостей по параметрам
        /// </summary>
        /// <param name="model">Параметры фильтрации</param>
        /// <returns></returns>
        [HttpPost("SearchNews")]
        public List<NewsViewModel> SearchNews(NewsViewModel model)
        {
            var newsQuery = _context.News.AsQueryable();
            if (model.Name != null)
                newsQuery = newsQuery.Where(v => v.Name.Contains(model.Name));
            if (model.CategoryId > 0)
                newsQuery = newsQuery.Where(v => v.CategoryId == model.CategoryId);
            if (model.Content != null)
                newsQuery = newsQuery.Where(v => v.Content.Contains(model.Content));
            return newsQuery.Select(n => n.CreateViewModel(_context)).ToList();
        }

        /// <summary>
        /// Создание новости
        /// </summary>
        /// <param name="model">Информация о новости</param>
        /// <returns></returns>
        [HttpPut("CreateNews")]
        public IActionResult CreateNews(News model)
        {
            News variant = new()
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Content = model.Content,
            };
            _context.News.Add(variant);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Изменение данных о новости
        /// </summary>
        /// <param name="model">Данные о новости, включая её ID</param>
        /// <returns></returns>
        [HttpPatch("ModifyNews")]
        public IActionResult ModifyNews(NewsModifyViewModel model)
        {
            News? variant = _context.News.Find(model.Id);
            if (variant != null)
            {
                if (model.Name != null)
                    variant.Name = model.Name;
                if (model.CategoryId != null)
                    variant.CategoryId = model.CategoryId ?? 0;
                if (model.Content != null)
                    variant.Content = model.Content;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Удаление новости
        /// </summary>
        /// <param name="id">ID новости</param>
        /// <returns></returns>
        [HttpDelete("DeleteNews")]
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
