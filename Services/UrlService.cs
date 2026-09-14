using System;
using Test_Task_URL_shortener_Backend.Data;
using Test_Task_URL_shortener_Backend.DTO;
using Test_Task_URL_shortener_Backend.Models;
using System.IO.Hashing;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Test_Task_URL_shortener_Backend.Services
{
    public interface IUrlService
    {
        public Task<UrlResponseDTO> CreateUrl(string originalUrl, Guid authorId);
        public Task<List<UrlResponseDTO>> GetAllUrls();
        public Task<UrlResponseDTO> FindUrl(string originalUrl);
    }
    public class UrlService: IUrlService
    {
        private readonly URLShortenerContext _context;
        public UrlService(URLShortenerContext context)
        {
            _context = context;
        }
        public async Task<UrlResponseDTO> CreateUrl(string originalUrl, Guid authorId)
        {
            var newUrl = new Url
            {
                Id = Guid.NewGuid(),
                OriginalUrl = originalUrl,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                ClickCount = 0,
                AuthorId = authorId,
                ShortenedUrl = EncodeUrl(originalUrl)
            };

            _context.Urls.Add(newUrl);
            await _context.SaveChangesAsync();

            return new UrlResponseDTO(newUrl);
        }
        public async Task<List<UrlResponseDTO>> GetAllUrls()
        {
            var urls = await _context.Urls.ToListAsync();
            var urlsResponse = urls.Select(url => new UrlResponseDTO(url)).ToList();
            return urlsResponse;
        }
        public async Task<UrlResponseDTO> FindUrl(string originalUrl)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(url => url.OriginalUrl == originalUrl);
            if (url != null)
            {
                return new UrlResponseDTO(url);
            }
            else
            {
                throw new Exception("404");
            }
        }
        public string EncodeUrl(string originalUrl)
        {
            var hashBytes = Crc32.Hash(Encoding.UTF8.GetBytes(originalUrl));
            string hashString = BitConverter.ToString(hashBytes, 0);
            return hashString;
        }
    }
}