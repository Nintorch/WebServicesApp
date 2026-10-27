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
using System.Net;

namespace WebTeploobmenAppClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MyConfig> _config;

        // TODO: Roles

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
                ModelState.AddModelError("register", "Пароли не совпадают.");
                return View();
            }

            HttpResponseMessage response = await new HttpClient().PostAsJsonAsync(_config.Value.ServerRequestUri + "Account/Register", model);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToAction("Login");
                case HttpStatusCode.Forbidden:
                    ModelState.AddModelError("register", "Аккаунт с данным логином уже существует.");
                    return View();
                default:
                    ModelState.AddModelError("register", "Неизвестная ошибка, свяжитесь с администратором.");
                    return View();
            }
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
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    AccountLoginSuccessViewModel result = await response.Content.ReadFromJsonAsync<AccountLoginSuccessViewModel>();

                    var claims = new List<Claim>
                    {
                        new(ClaimsIdentity.DefaultNameClaimType, model.Login),
                        new(ClaimsIdentity.DefaultRoleClaimType, result.RoleName),
                        new("Token", result.Token),
                    };
                    var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    var identity = new ClaimsIdentity(claims, scheme);
                    await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                case HttpStatusCode.Unauthorized:
                    ModelState.AddModelError("login", "Неверный логин или пароль.");
                    return View();
                default:
                    ModelState.AddModelError("login", "Неизвестная ошибка, свяжитесь с администратором.");
                    return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
