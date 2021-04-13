using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface ISellersService
    {
        Task<bool> AddSeller(Sellers seller);
        Task<List<Sellers>> GetSellers();
        Task<Sellers> GetSeller(int userId);
        Task<bool> UpdateSeller(int userId, Sellers seller);
    }
}
