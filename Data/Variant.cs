using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenApp.Data
{
    public class Variant
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Number1 { get; set; }
        public int Number2 { get; set; }
    }
}
