using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebTeploobmenAppServer
{
    public static class JwtAuthOptions
    {
        public static string Issuer = "IssuerName";
        public static string Audience = "AudienceName";
        public static string Key = "KeyKeyKeyKeyKeyKeyKeyKeyKey1234567890?!+-=";
        public static int LifeTime = 1;
        public static SymmetricSecurityKey SecurityKey =>
        new(Encoding.ASCII.GetBytes(Key));
    }
}
