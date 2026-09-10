using System;
using Microsoft.EntityFrameworkCore;

namespace Test_Task_URL_shortener_Backend.Data
{
    public class URLShortenerContext : DbContext
    {
        public URLShortenerContext(DbContextOptions<URLShortenerContext> options) : base(options) {}
    }
}