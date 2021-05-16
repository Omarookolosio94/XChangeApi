using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {

        private static string ModuleName = "RefreshTokenRepository";

        public RefreshTokenRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                await InsertAsync(refreshToken);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddRefreshToken", "Error Adding Refresh Token" + ex + "\n");
                throw;
            }
        }

        public async Task<RefreshToken> GetRefreshTokenByUser(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).LastOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetRefreshTokenByUser", "Error Fetching RefershToken by user:  " + userId + "  " + ex + "\n");
                throw;
            }
        }

        public async Task<List<RefreshToken>> GetRefreshTokens()
        {
            try
            {
                return Query().OrderByDescending(o => o.TokenId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetRefreshTokens", "Error Fetching All RefreshTokens" + ex + "\n");
                throw;
            }
        }


        public async Task<bool> IsRefreshTokenValid(int userId, string token)
        {
            try
            {
                RefreshToken refreshToken = Query().Where(o => o.UserId == userId && o.Token.ToLower() == token.ToLower()).FirstOrDefault();

                if (refreshToken != null)
                {
                   return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsRefreshTokenValid", "Error Validating refresh token " + token + " for user " + userId + "error: " + ex + "/n");
                throw;
            }
        }



    }

}
