using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class BuyersRepository : BaseRepository<Buyers>, IBuyersRepository
    {

        private static string ModuleName = "BuyersRepository";

        public BuyersRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddBuyer(Buyers buyer)
        {
            try
            {
                await InsertAsync(buyer);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddBuyer", "Error Adding Buyer" + ex + "\n");
                throw;
            }
        }

        public async Task<Buyers> GetBuyer(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetBuyer", "Error Fetching Buyer with userId: " + userId + "exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Buyers>> GetBuyers()
        {
            try
            {
                return Query().OrderByDescending(buyer => buyer.BuyerId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetBuyers", "Error Fetching Buyers. " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> UpdateBuyer(Buyers buyer)
        {
            try
            {
                Update(buyer);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateBuyer", "Error Updating Buyer. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Buyers>> SearchBuyers(string searchParam)
        {
            try
            {
                var _queryList = new List<Buyers>();
                var _query = Query().ToList();
                if (!string.IsNullOrEmpty(searchParam))
                {
                    if (_query != null)
                    {
                        _queryList = _query = _query.Where(x => x.FirstName.ToLower().Contains(searchParam)
                                                || x.LastName.ToLower().Contains(searchParam) || x.CompanyName.ToLower().Contains(searchParam) || x.Email.ToLower().Contains(searchParam)).ToList();
                    }
                }

                _queryList = _query.AsQueryable().ToList();
                return _queryList.ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "SearchBuyers", "Error Searching for Buyers" + ex + "\n");
                throw;
            }
        }

        public async Task<int> GetBuyersCount()
        {
            try
            {
                return Query().Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetBuyersCount", "Error getting buyers count exception error: " + ex + "/n");
                return 0;
            }
        }

    }
}
