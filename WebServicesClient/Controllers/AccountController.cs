using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebTeploobmenAppClient.Models;

namespace WebTeploobmenAppClient.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            // TODO
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
            // TODO
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
