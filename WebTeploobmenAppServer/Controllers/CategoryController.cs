using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTeploobmenAppServer.Data;
using WebTeploobmenAppServer.Models;

namespace WebTeploobmenAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly WebServicesContext _context;

        public CategoryController(ILogger<CategoryController> logger, WebServicesContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Получить список всех категорий, или информацию о конкретной категории
        /// </summary>
        /// <param name="id">ID категории, или 0 для просмотра списка всех категорий</param>
        /// <returns></returns>
        [HttpGet("GetCategories")]
        public List<Category> GetCategories(int id)
        {
            if (id == 0)
            {
                return _context.Categories.ToList();
            }
            Category? category = _context.Categories.Find(id);
            if (category == null)
            {
                return [];
            }
            return [category];
        }

        /// <summary>
        /// Фильтрация категорий по параметрам
        /// </summary>
        /// <param name="model">Параметры фильтрации</param>
        /// <returns></returns>
        [HttpPost("SearchCategories")]
        public List<Category> SearchCategories(CategoryViewModel model)
        {
            var categoriesQuery = _context.Categories.AsQueryable();
            if (model.Name != null)
                categoriesQuery = categoriesQuery.Where(v => v.Name.Contains(model.Name));
            return categoriesQuery.ToList();
        }

        /// <summary>
        /// Создание категории
        /// </summary>
        /// <param name="model">Информация о категории</param>
        /// <returns></returns>
        [HttpPut("CreateCategory")]
        public IActionResult CreateCategory(Category model)
        {
            Category category = new()
            {
                Name = model.Name,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Изменение данных о категории
        /// </summary>
        /// <param name="model">Данные о категории, включая её ID</param>
        /// <returns></returns>
        [HttpPatch("ModifyCategory")]
        public IActionResult ModifyCategory(Category model)
        {
            Category? category = _context.Categories.Find(model.Id);
            if (category != null)
            {
                category.Name = model.Name;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <returns></returns>
        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            Category? category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
