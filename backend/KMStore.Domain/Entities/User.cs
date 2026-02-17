using System;

namespace KMStore.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool EmailConfirmed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
}
