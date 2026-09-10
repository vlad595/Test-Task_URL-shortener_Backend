using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Test_Task_URL_shortener_Backend.Models
{
    [Index(nameof(ShortenedUrl), IsUnique = true)]
    [Index(nameof(OriginalUrl), IsUnique = true)]
    public class Url
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int ClickCount { get; set; }
        public Guid AuthorId { get; set; }
        [JsonIgnore]
        public User Author { get; set; }

        public Url()
        {
            CreatedAt = DateTime.UtcNow;
            ExpirationDate = CreatedAt.AddDays(30);
            ClickCount = 0;
        }
    }
}