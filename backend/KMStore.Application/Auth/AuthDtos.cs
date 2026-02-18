using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Application.Auth;

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);

public record AuthResponse(
    string AccessToken,
    DateTime ExpiresAt,
    int UserId,
    string Email,
    string Role
);
