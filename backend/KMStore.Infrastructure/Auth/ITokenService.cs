using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Infrastructure.Auth;

public interface ITokenService
{
    (string token, DateTime expiresAt) CreateToken(int userId, string email, string role);
}