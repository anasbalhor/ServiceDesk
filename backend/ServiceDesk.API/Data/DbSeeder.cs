using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.Models;

namespace ServiceDesk.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        var seedUsers = new List<(string Email, string FirstName, string LastName, string Password, UserRole Role)>
        {
            ("admin@servicedesk.com", "Admin", "ServiceDesk", "Admin123!", UserRole.Admin),
            ("manager@servicedesk.com", "Manager", "ServiceDesk", "Manager123!", UserRole.Manager),
            ("agent@servicedesk.com", "Agent", "ServiceDesk", "Agent123!", UserRole.Agent)
        };

        foreach (var (email, firstName, lastName, password, role) in seedUsers)
        {
            if (!await context.Users.AnyAsync(u => u.Email == email))
            {
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = role,
                    IsActive = true
                });
            }
        }

        await context.SaveChangesAsync();
    }
}