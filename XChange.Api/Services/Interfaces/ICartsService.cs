using System.Collections.Generic;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface ICartsService
    {
        Task<bool> AddProductToCart(Carts cart);
        Task<bool> RemoveProductFromCart(int productId, int userId);
        Task<int> GetCountOfUserCart(int userId);
        Task<int> GetCartCount();
        Task<List<Carts>> GetUserCart(int userId);
        Task<List<Carts>> GetCarts();
        Task<bool> DeleteUserCart(int userId);
        Task<bool> UpdateUserCart(int userId, Carts cart);
        Task<bool> IsProductInUserCart(int productId , int userId);
        Task<Carts> GetSingleProductInCart(int productId, int userId);
    }
}
