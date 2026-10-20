using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebTeploobmenAppServer.Data;
using WebTeploobmenAppServer.Models;

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
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Login == model.Login);
            if (user == null)
            {
                return NotFound();
            }
            if (!PasswordHasher.Verify(model.Password, user.PasswordHash))
            {
                return Forbid();
            }

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, model.Login)
            };
            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var identity = new ClaimsIdentity(claims, scheme);
            await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity));

            return Ok();
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
