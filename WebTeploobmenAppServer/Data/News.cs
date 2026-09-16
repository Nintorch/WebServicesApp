using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenAppServer.Data
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
    }
}
