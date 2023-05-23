using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AntonS.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUSerService _userService;
        private readonly IConfiguration _configuration;
        private readonly IAcessLevelService _levelService;
        private readonly IMapper _mapper;

        public LoginController(IUSerService userService, IConfiguration configuration, IMapper mapper, IAcessLevelService levelService)
        {
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
            _levelService = levelService;
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
            var isexists = await _userService.IsUserExistsAsync(model.UserName);
            var isexists2 = await _userService.IsPasswordCorrectAsync(model.UserName, model.Password);
            if (isexists && isexists2)
            {
                var user = await _userService.GetByEmailAsync(model.UserName);
                await AuthenticateAsync(user);
                return RedirectToAction("Index","Home");

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
            if(ModelState.IsValid) 
            {
                if (!await _userService.IsUserExistsAsync(model.Email))
                {
                    var user = await _userService.AddUserAsync(model.Name, model.Surname, model.Email, model.Password);
                    if (user != null)
                    {
                        await AuthenticateAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("","");
                }
                return RedirectToAction("Index", "Home");
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Email),
                };
                var acessLevel = await _levelService.GetRoleName(dto.Id);
                if (string.IsNullOrEmpty(acessLevel))
                {
                    throw new ArgumentException("Incorrect user or role", nameof(dto));
                }
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, acessLevel));

                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            
            
        }
    }
}
