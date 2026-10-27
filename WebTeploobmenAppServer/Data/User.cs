using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTeploobmenAppServer.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
