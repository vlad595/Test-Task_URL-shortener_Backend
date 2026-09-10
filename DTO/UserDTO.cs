using System;
using Test_Task_URL_shortener_Backend.Models;

namespace Test_Task_URL_shortener_Backend.DTO
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Token { get; set; }
        public UserResponseDTO(User userModel, string token)
        {
            this.Id = userModel.Id;
            this.Username = userModel.Username;
            this.Email = userModel.Email;
            this.Role = userModel.Role;
            this.Token = token;
        } 
    }
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}