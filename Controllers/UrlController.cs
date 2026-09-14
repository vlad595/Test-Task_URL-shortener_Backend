using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test_Task_URL_shortener_Backend.DTO;
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
    }
}