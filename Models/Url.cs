using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Test_Task_URL_shortener_Backend.DTO;

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
        public DateTime ExpirationDate { get; set; }
        public int ClickCount { get; set; }
        public Guid AuthorId { get; set; }
        [JsonIgnore]
        public User Author { get; set; }
    }
}