using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LOT_Project.Services
{

    public interface IAccountService
    {
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly FlightsDbContext _flightsDbContext;
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountService(FlightsDbContext context, AuthenticationSettings authenticationSettings)
        {
            _flightsDbContext = context;
            _authenticationSettings = authenticationSettings;
        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _flightsDbContext.Users
                .FirstOrDefault(u => u.Login == dto.Login && u.Password == dto.Password);

            if(user is null)
            {
                throw new BadRequestExeption("Invalid login or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Login}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);


            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }
    }
}
