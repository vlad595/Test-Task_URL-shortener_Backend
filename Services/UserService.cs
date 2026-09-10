using System;
using Microsoft.EntityFrameworkCore;
using Test_Task_URL_shortener_Backend.DTO;
using Test_Task_URL_shortener_Backend.Models;
using BCrypt.Net;
using Test_Task_URL_shortener_Backend.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Test_Task_URL_shortener_Backend.Services
{
    public interface IUserService
    {
        public Task<UserResponseDTO> CreateUserAsync(UserRegistrationDTO userRegistrationDTO);
        public Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO userLoginDTO);
        public Task<UserResponseDTO> GetUserByEmailAsync(string email);
        public Task<UserResponseDTO> GetUserByIdAsync(string userId);
    }
    class UserService : IUserService
    {
        private readonly URLShortenerContext  _context;
        private readonly IConfiguration _config;
        public UserService(URLShortenerContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<UserResponseDTO> CreateUserAsync(UserRegistrationDTO userRegistrationDTO)
        {
            bool isUnique = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegistrationDTO.Email) != null;
            if (isUnique)
            {
                throw new Exception("409");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userRegistrationDTO.Username,
                Email = userRegistrationDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationDTO.Password),
                Role = UserRole.User
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var token = GenerateToken(user);

            var userResponse = new UserResponseDTO(user, new JwtSecurityTokenHandler().WriteToken(token));
            return userResponse;
        }
        public async Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);
            if (user == null)
            {
                throw new Exception("404");
            }

            if (BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.PasswordHash))
            {
                var token = GenerateToken(user);
                return new UserResponseDTO(user, new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                throw new Exception("401");
            }
        }
        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("404");
            }
            return new UserResponseDTO(user, "");
        }
        public async Task<UserResponseDTO> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                throw new Exception("404");
            }
            return new UserResponseDTO(user, "");
        }
        JwtSecurityToken GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())  
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            return token;
        }
    }
}