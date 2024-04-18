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
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly FlightsDbContext _flightsDbContext;
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountService(FlightsDbContext context, AuthenticationSettings authenticationSettings, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _flightsDbContext = context;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Login = dto.Login,
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.Password = hashedPassword;
            _flightsDbContext.Add(newUser);
            _flightsDbContext.SaveChanges();
        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _flightsDbContext.Users
                .FirstOrDefault(u => u.Login == dto.Login);

            if(user is null)
            {
                throw new BadRequestExeption("Invalid login or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
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
