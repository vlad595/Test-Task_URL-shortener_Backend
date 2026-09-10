using System;
using Microsoft.EntityFrameworkCore;
using Test_Task_URL_shortener_Backend.Models;

namespace Test_Task_URL_shortener_Backend.Data
{
    public class URLShortenerContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AboutText> AboutTexts { get; set; }
        public URLShortenerContext(DbContextOptions<URLShortenerContext> options) : base(options) {}
    }
}