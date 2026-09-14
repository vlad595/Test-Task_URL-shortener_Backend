using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test_Task_URL_shortener_Backend.DTO;
using Test_Task_URL_shortener_Backend.Services;

namespace Test_Task_URL_shortener_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO dto)
        {
            try
            {
                var result = await _service.CreateUserAsync(dto);
                return Created("", result);
            }catch(Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            try
            {
                var result = await _service.AuthenticateUserAsync(dto);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> FindUserByEmail([FromRoute]string email)
        {
            try
            {
                var result = await _service.GetUserByEmailAsync(email);
                return Ok(result);
            }catch(Exception ex)
            {
                return NotFound("User does not found");
            }
        }
        [HttpGet("id/{id}")]
        [Authorize]
        public async Task<IActionResult> FindUserById(string id)
        {
            try
            {
                var result = await _service.GetUserByIdAsync(id);
                return Ok(result);
            }catch(Exception ex)
            {
                return NotFound("User does not found");
            }
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _service.GetAllUsers();
            return Ok(result);
        }
    }
}