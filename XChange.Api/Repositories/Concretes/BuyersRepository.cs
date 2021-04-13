﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class BuyersRepository : BaseRepository<Buyers> , IBuyersRepository
    {

        private static string ModuleName = "BuyersRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();



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

            } catch(Exception ex)
            {
                new Logger().LogError(ModuleName, "AddBuyer", "Error Adding Buyer"  + ex + "\n");
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
    }
}
