using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebTeploobmenAppServer.Data;
using WebTeploobmenAppServer.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace WebTeploobmenAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly WebServicesContext _context;

        public AccountController(ILogger<AccountController> logger, WebServicesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("Register")]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            User? checkUser = _context.Users.FirstOrDefault(u => u.Login == model.Login);
            if (checkUser != null)
            {
                return Forbid();
            }

            User user = new()
            {
                Login = model.Login,
                PasswordHash = PasswordHasher.Hash(model.Password),
                RoleId = 1, // "user" role
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == model.Login);
            if (user == null || !PasswordHasher.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
            };
            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var identity = new ClaimsIdentity(claims, scheme);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtAuthOptions.Issuer,
                audience: JwtAuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtAuthOptions.LifeTime)),
                signingCredentials: new SigningCredentials(JwtAuthOptions.SecurityKey,
                SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var result = new AccountLoginSuccessViewModel()
            {
                Token = encodedJwt,
                RoleName = user.Role.Name,
            };
            return Ok(result);
        }
    }
}
