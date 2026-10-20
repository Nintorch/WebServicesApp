using System.ComponentModel.DataAnnotations;

namespace WebTeploobmenAppServer.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
