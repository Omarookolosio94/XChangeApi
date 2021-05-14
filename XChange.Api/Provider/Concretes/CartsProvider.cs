using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Provider.Interfaces;

namespace XChange.Api.Provider.Concretes
{
    public class CartsProvider : ICartsProvider
    {
        private static string ModuleName = "CartsProvider";
        private readonly XChangeDatabaseContext _dbContext = new XChangeDatabaseContext();

        public CartsProvider(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Carts>> GetCarts()
        {
            return _dbContext.Carts.AsQueryable().ToList();
        }
    }
}
