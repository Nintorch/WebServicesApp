using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenAppServer.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
