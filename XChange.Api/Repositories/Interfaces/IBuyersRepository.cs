using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IBuyersRepository
    {
        Task<bool> AddBuyer(Buyers buyer);
        Task<List<Buyers>> GetBuyers();
        Task<Buyers> GetBuyer(int userId);
        Task<bool> UpdateBuyer(Buyers buyer);
    }
}
