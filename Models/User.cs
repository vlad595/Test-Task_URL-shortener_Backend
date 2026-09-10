using System;
using Microsoft.EntityFrameworkCore;

namespace Test_Task_URL_shortener_Backend.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public  UserRole Role { get; set; }
        public List<Url> Urls { get; set; } = new List<Url>();

        public User()
        {
            Id = Guid.NewGuid();
        }
    }

    public enum UserRole
    {
        Admin,
        User,
        Guest
    }
}