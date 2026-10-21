using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTeploobmenAppClient.Helpers;
using WebTeploobmenAppClient.Models;

namespace WebTeploobmenAppClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MyConfig> _config;

        public AccountController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                // TODO
                throw new NotImplementedException();
            }

            HttpResponseMessage response = await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "Account/Register", model);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Login");
            }

            // TODO: Show error messages
            // ModelState.AddModelError
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            HttpResponseMessage response = await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "Account/Login", model);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string token = await response.Content.ReadAsStringAsync();

                var claims = new List<Claim>
                {
                    new(ClaimsIdentity.DefaultNameClaimType, model.Login),
                    new("Token", token),
                };
                var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                var identity = new ClaimsIdentity(claims, scheme);
                await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }

            // TODO: Show error messages
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
