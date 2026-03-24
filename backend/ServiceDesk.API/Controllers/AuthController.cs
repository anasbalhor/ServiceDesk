using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.Auth;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerRequest)
    {
        var authResponse = await _authService.RegisterAsync(registerRequest);
        return CreatedAtAction(nameof(Register), authResponse);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
    {
        var authResponse = await _authService.LoginAsync(loginRequest);
        return Ok(authResponse);
    }
}