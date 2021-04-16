using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface ISellersRepository
    {
        Task<bool> AddSeller(Sellers seller);
        Task<List<Sellers>> GetSellers();
        Task<Sellers> GetSeller(int userId);
        Task<bool> UpdateSeller(Sellers seller);
        Task<List<Sellers>> SearchSellers(string searchParams);
    }
}
