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
        public Task<UrlResponseDTO> CreateUrl(string originalUrl, string authorId);
        public Task<List<UrlResponseDTO>> GetAllUrls();
        public Task<UrlResponseDTO> RedirectTo(string shortenedUrl);
        public Task<UrlResponseDTO> DeleteUrl(Guid urlId, string authorId, UserRole role);
    }
    public class UrlService: IUrlService
    {
        private readonly URLShortenerContext _context;
        public UrlService(URLShortenerContext context)
        {
            _context = context;
        }
        public async Task<UrlResponseDTO> CreateUrl(string originalUrl, string authorId)
        {
            var newUrl = new Url
            {
                Id = Guid.NewGuid(),
                OriginalUrl = originalUrl,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                ClickCount = 0,
                AuthorId = Guid.Parse(authorId),
                ShortenedUrl = EncodeUrl(originalUrl)
            };

            _context.Urls.Add(newUrl);
            await _context.SaveChangesAsync();

            var dto = new UrlResponseDTO(newUrl);
            dto.ShortenedUrl = "http://localhost:5059/" + dto.ShortenedUrl;
            return dto;
        }
        public async Task<List<UrlResponseDTO>> GetAllUrls()
        {
            var urls = await _context.Urls.ToListAsync();
            var urlsResponse = urls.Select(url => {
                var dto = new UrlResponseDTO(url);
                dto.ShortenedUrl = "http://localhost:5059/" + dto.ShortenedUrl;
                return dto;
            }).ToList();
            return urlsResponse;
        }
        public async Task<UrlResponseDTO> RedirectTo(string shortenedUrl)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(url => url.ShortenedUrl == shortenedUrl);
            if (url != null && (url.ExpirationDate != null || url.ExpirationDate > DateTime.UtcNow))
            {
                url.ClickCount++;
                await _context.SaveChangesAsync();
                return new UrlResponseDTO(url);
            }
            else
            {
                throw new Exception("404");
            }
        }
        public async Task<UrlResponseDTO> DeleteUrl(Guid urlId, string authorId, UserRole role)
        {
            Url? url = await _context.Urls.FirstOrDefaultAsync(url => url.Id == urlId);
            if (url != null)
            {
                if (authorId != url.AuthorId.ToString() && role != UserRole.Admin)
                {
                    throw new Exception("403");
                }
                else if (url.ExpirationDate > DateTime.UtcNow)
                {
                    _context.Urls.Remove(url);
                    await _context.SaveChangesAsync();
                    var dto = new UrlResponseDTO(url);
                    dto.ShortenedUrl = "http://localhost:5059/" + dto.ShortenedUrl;
                    return dto;
                }
                else throw new Exception("404");
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