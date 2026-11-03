using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTeploobmenAppServer.Data
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int ChatId { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageText { get; set; }
        public User Author { get; set; }
    }
}
