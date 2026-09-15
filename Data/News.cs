using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenApp.Data
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
