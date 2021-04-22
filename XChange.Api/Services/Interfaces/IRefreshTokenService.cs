using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<bool> AddRefreshToken(RefreshToken refreshToken);
        Task<List<RefreshToken>> GetRefreshTokens();
        Task<RefreshToken> GetRefreshTokenByUser(int userId);
        Task<bool> IsRefreshTokenValid(int userId, string token);
    }
}
