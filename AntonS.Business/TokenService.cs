using AntonDB.Entities;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AntonS.DB.CQS.Commands;
using AntonS.DB.CQS.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Business
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public TokenService(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<string> GetJwtTokenAsync(UserDTO dto)
        {
            var token = await GenerateJwtTokenAsync(dto);
            return token;
        }
        public async Task AddRefreshTokenAsync(int userId, Guid refreshToken)
        {
            await _mediator.Send(new AddRefreshTokenCommand()
            {
                UserId= userId,
                RefreshToken= refreshToken
            });
        }
        private async Task<string> GenerateJwtTokenAsync(UserDTO dto)
        {
            var role = await _mediator.Send(new GetUserRoleByUserNameQuery() { UserId = dto.Id });
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim(JwtRegisteredClaimNames.Sub, dto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("R"))
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:SecurityKey"]));

            var signIn = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpireInMinutes"])),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
