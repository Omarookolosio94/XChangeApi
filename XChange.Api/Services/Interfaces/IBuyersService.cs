using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface IBuyersService
    {
        Task<bool> AddBuyer(Buyers buyer);
        Task<List<Buyers>> GetBuyers();
        Task<Buyers> GetBuyer(int userId);
        Task<bool> UpdateBuyer(int userId, Buyers buyer);
        Task<bool> IsBuyerRegistered(int userId);
    }
}
