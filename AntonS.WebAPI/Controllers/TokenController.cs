using AntonS.Abstractions.Services;
using AntonS.WebAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntonS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IUSerService _userService;
        private readonly ITokenService _tokenService;
        public TokenController(IUSerService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (await _userService.IsUserExistsAsync(request.Login) &&
                await _userService.IsPasswordCorrectAsync(request.Login, request.Password))
            {
                var user = await _userService.GetByEmailAsync(request.Login);
                if (user != null)
                {
                    var jwtTokenString = await _tokenService.GetJwtTokenAsync(user);
                    var refreshToken = Guid.NewGuid();
                    await _tokenService.AddRefreshTokenAsync();
                }
            }
        }
    }
}
