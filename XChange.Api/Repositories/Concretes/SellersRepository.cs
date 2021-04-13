using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class SellersRepository : BaseRepository<Sellers>, ISellersRepository
    {

        private static string ModuleName = "SellersRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();



        public SellersRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddSeller(Sellers seller)
        {
            try
            {
                await InsertAsync(seller);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddSeller", "Error Adding Seller" + ex + "\n");
                throw;
            }
        }

        public async Task<Sellers> GetSeller(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSeller", "Error Fetching Seller with userId: " + userId + "exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Sellers>> GetSellers()
        {
            try
            {
                return Query().OrderByDescending(seller => seller.SellerId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSellers", "Error Fetching Sellers. " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> UpdateSeller(Sellers seller)
        {
            try
            {
                Update(seller);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateSeller", "Error Updating Seller. Exception error: " + ex + "\n");
                throw;
            }
        }
    }
}
