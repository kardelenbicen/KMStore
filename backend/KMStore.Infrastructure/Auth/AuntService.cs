using KMStore.Application.Auth;
using KMStore.Domain.Entities;
using KMStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KMStore.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly KMStoreDbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(KMStoreDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(x => x.Email == email))
            throw new InvalidOperationException("Bu email zaten kayıtlı.");

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = 2, // seed'e göre: 2 = User
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var roleName = await _db.Roles
            .Where(r => r.Id == user.RoleId)
            .Select(r => r.Name)
            .FirstAsync();

        var (token, expiresAt) = _tokenService.CreateToken(user.Id, user.Email, roleName);

        return new AuthResponse(token, expiresAt, user.Id, user.Email, roleName);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        var (token, expiresAt) = _tokenService.CreateToken(user.Id, user.Email, user.Role.Name);

        return new AuthResponse(token, expiresAt, user.Id, user.Email, user.Role.Name);
    }
}
