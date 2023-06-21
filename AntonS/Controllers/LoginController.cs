using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Serilog;

namespace AntonS.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUSerService _userService;
        
        public LoginController(IUSerService userService)
        {
            _userService = userService;            
            
        }
        [HttpGet]
        public async Task<IActionResult> OpenLoginPage()
        {
           return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginPageModel model) 
        {
            try
            {
                var isexists = await _userService.IsUserExistsAsync(model.UserName);
                var isexists2 = await _userService.IsPasswordCorrectAsync(model.UserName, model.Password);
                if (isexists && isexists2)
                {
                    var user = await _userService.GetByEmailAsync(model.UserName);
                    await AuthenticateAsync(user);
                    return RedirectToAction("Index", "Home");

                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> OpenRegisterPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterPageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _userService.IsUserExistsAsync(model.Email))
                    {
                        var user = await _userService.AddUserAsync(model.Name, model.Surname, model.Email, model.Password);
                        if (user != null)
                        {
                            await AuthenticateAsync(user);
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("", "");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return View(model);
        }
        public async Task<IActionResult> IsEmailAlreadyUsed(string email)
        {
            return Ok(!await _userService.IsUserExistsAsync(email));
        }
        private async Task AuthenticateAsync(UserDTO dto)
        {
            
                const string authType = "Application Cookie";

            var claims = await _userService.GetUserClaimsAsync(dto);

                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            
            
        }
    }
}
