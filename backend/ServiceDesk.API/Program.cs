using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceDesk.API.Data;
using ServiceDesk.API.Helpers;
using ServiceDesk.API.Services;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ───── SERVICES ─────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connexion PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("ServiceDesk.API")
    ));


// JwtHelper disponible par injection de dépendances
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuration de l'authentification JWT
var jwtkey = builder.Configuration["Jwt:SecretKey"]!;
var jwtissuer = builder.Configuration["Jwt:Issuer"]!;
var jwtaudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtissuer,
        ValidAudience = jwtaudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey)),
        ClockSkew = TimeSpan.Zero // optionnel : réduire le délai de tolérance pour l'expiration des tokens
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// ───── DATA SEEDING ─────
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// ───── PIPELINE HTTP ─────
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();