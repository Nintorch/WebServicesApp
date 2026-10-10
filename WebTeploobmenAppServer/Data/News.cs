using System.ComponentModel.DataAnnotations;
using WebTeploobmenAppServer.Models;

namespace WebTeploobmenAppServer.Data
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Content { get; set; }

        public NewsViewModel CreateViewModel(WebServicesContext context)
        {
            Category? category = context.Categories.Find(CategoryId);
            string categoryName = category?.Name ?? string.Empty;
            return new()
            {
                Id = Id,
                Name = Name,
                CategoryId = CategoryId,
                CategoryName = categoryName,
                Content = Content,
            };
        }
    }
}
