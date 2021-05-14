using System.Collections.Generic;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Provider.Interfaces
{
    public interface ICartsProvider
    {
        Task<List<Carts>> GetCarts();
    }
}
