using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class RefreshTokenService :  IRefreshTokenService
    {

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;

        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                var status = await _refreshTokenRepository.AddRefreshToken(refreshToken);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }


        public async Task<List<RefreshToken>> GetRefreshTokens()
        {
            try
            {
                var status = await _refreshTokenRepository.GetRefreshTokens();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RefreshToken> GetRefreshTokenByUser(int userId)
        {
            try
            {
                RefreshToken refreshToken = await _refreshTokenRepository.GetRefreshTokenByUser(userId);
                return refreshToken;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsRefreshTokenValid(int userId , string token)
        {
            try
            {
                bool isRefreshTokenValid = await _refreshTokenRepository.IsRefreshTokenValid(userId, token);
                return isRefreshTokenValid;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
