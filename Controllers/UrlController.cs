using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test_Task_URL_shortener_Backend.DTO;
using Test_Task_URL_shortener_Backend.Models;
using Test_Task_URL_shortener_Backend.Services;

namespace Test_Task_URL_shortener_Backend.Controllers
{
    [ApiController]
    [Authorize]
    public class UrlController: ControllerBase
    {
        private readonly IUrlService _service;
        public UrlController(IUrlService service)
        {
            _service = service; 
        }
        [HttpPost("api/[controller]/")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateUrl(UrlCreationDTO url)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _service.CreateUrl(url.OriginalUrl, userId);
                return Ok(result);
            }catch(Exception ex) {
                return Forbid(); //To replace
            }
        }
        [HttpGet("{shortenedUrl}")]
        [AllowAnonymous]
        public async Task<IActionResult> RedirectToOriginal(string shortenedUrl)
        {
            try
            {
                UrlResponseDTO urlResponse = await _service.RedirectTo(shortenedUrl);
                var originalUrl = urlResponse.OriginalUrl;
                return Redirect(originalUrl);
            }catch(Exception ex)
            {
                return Forbid(); //To replace
            }
        }
        [HttpGet("api/[controller]/")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUrls()
        {
            try
            {
                var result = await _service.GetAllUrls();
                return Ok(result);
            }catch(Exception ex)
            {
                if (ex.Message == "404") return NotFound();
                else return Forbid();
            }
        }
        [HttpDelete("api/[controller]/")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteUrl(string id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                UserRole userRole = Enum.Parse<UserRole>(User.FindFirstValue(ClaimTypes.Role));
                var result = await _service.DeleteUrl(Guid.Parse(id), userId, userRole);
                return Ok(result);
            }catch(Exception ex)
            {
                if (ex.Message == "403") return Forbid();
                else return NotFound();
            }
        }
    }
}