using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTeploobmenAppServer.Data
{
    public class PrivateChat
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Person1))]
        public int Person1Id { get; set; }
        [ForeignKey(nameof(Person2))]
        public int Person2Id { get; set; }

        public User Person1 { get; set; }
        public User Person2 { get; set; }
    }
}
