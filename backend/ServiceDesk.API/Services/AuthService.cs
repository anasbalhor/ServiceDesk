using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.Data;
using ServiceDesk.API.DTOs.Auth;
using ServiceDesk.API.Helpers;
using ServiceDesk.API.Models;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Services;
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerRequest)
        {
            //1. Verifier si l'email existe déjà
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email.ToLower());
            if (existingUser != null)
            {
                throw new InvalidOperationException("un compte avec cet email existe déjà");
            }

            // 2. creer un nouvel utilisateur
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                Role = UserRole.Employee // par défaut, tous les nouveaux utilisateurs sont des employés
            };

            // 3. sauvegarder l'utilisateur dans la base de données
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 4. générer un token JWT pour l'utilisateur
            var token = _jwtHelper.GenerateToken(user);

            // 5. retourner la réponse
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginRequest)
        {

            // 1. chercher l'utilisateur par email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email.ToLower());

            // 2. vérifier que l'utilisateur existe et que le mot de passe est correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");

            // 3. verifier que le compte est actif
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Ce compte a été désactivé.");

            // 4. générer un token JWT pour l'utilisateur
            var token = _jwtHelper.GenerateToken(user);

            // 5. retourner la réponse
            return new AuthResponseDto 
            { 
                Token = token , 
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }
    }