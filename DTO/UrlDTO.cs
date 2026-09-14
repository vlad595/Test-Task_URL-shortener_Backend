using System;
using Test_Task_URL_shortener_Backend.Models;

namespace Test_Task_URL_shortener_Backend.DTO
{
    public class UrlCreationDTO
    {
        public string OriginalUrl {get;set;}
    }
    public class UrlResponseDTO
    {
        public string OriginalUrl {get;set;}
        public string ShortenedUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate {get;set;}
        public int ClickCount {get;set;}
        public Guid AuthorId {get;set;}
        public UrlResponseDTO(Url url)
        {
            OriginalUrl = url.OriginalUrl;
            ShortenedUrl = url.ShortenedUrl;
            CreatedAt = url.CreatedAt;
            ExpirationDate = url.ExpirationDate;
            ClickCount = url.ClickCount;
            AuthorId = url.AuthorId;
        }
    }
}