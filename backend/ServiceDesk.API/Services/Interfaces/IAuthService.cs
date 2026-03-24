using ServiceDesk.API.DTOs.Auth;

namespace ServiceDesk.API.Services.Interfaces;
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerRequest);
        Task<AuthResponseDto> LoginAsync(LoginDto loginRequest);
    }

