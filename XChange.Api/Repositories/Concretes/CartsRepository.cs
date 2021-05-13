using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class CartsRepository : BaseRepository<Carts>, ICartsRepository
    {

        private static string ModuleName = "CartsRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();


        public CartsRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
  

        public async Task<bool> AddProductToCart(Carts cart)
        {
            try
            {
                await InsertAsync(cart);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddProductToCart", "Error Adding Product to Cart" + ex + "\n");
                throw;
            }
        }

        public async Task<bool> RemoveProductFromCart(int productId, int userId)
        {
            try
            {
                var cartItem = Query().Where(o => o.ProductId == productId && o.UserId == userId);

                if (cartItem.Count() > 0)
                {
                    DeleteRange(cartItem);
                    await Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "RemoveProductFromCart", "Error Removing product from cart " + ex + "\n");
                throw;
            }
        }

        public async Task<int> GetCartCount()
        {
            try
            {
                return Query().Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetCartCount", "Error getting count of cart: error: " + ex + "/n");
                return 0;
            }
        }

        public async Task<int> GetCountOfUserCart(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetCountOfUserCart", "Error getting count of product in user cart: error: " + ex + "/n");
                return 0;
            }
        }

        public async Task<List<Carts>> GetUserCart(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserCart", "Error Fetching cart of user: " + userId  + " error : " + ex + "\n");
                throw;
            }
        }
     

        public async Task<bool> DeleteUserCart(int userId)
        {
            try
            {
                var cart = Query().Where(o => o.UserId == userId);

                if (cart.Count() > 0)
                {
                    DeleteRange(cart);
                    await Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "DeleteUserCart", "Error Deleting cart " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Carts>> GetCarts()
        {
            try
            {
                return Query().ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetCarts", "Error Fetching carts: " + ex + "\n");
                throw;
            }
        }


        public async Task<bool> IsProductInUserCart(int productId, int userId)
        {
            try
            {
                var query = Query().Where(o => o.ProductId == productId && o.UserId == userId).Count();

                if(query > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsProductInUserCart", "Error checking if product Exist in user's cart: " + ex + "/n");
                return false;

            }
        }


        public async Task<bool> UpdateUsercart(Carts cart)
        {
            try
            {
                Update(cart);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateUserCart", "Error Updating User Cart. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<Carts> GetSingleProductInCart(int productId, int userId)
        {
            try
            {
                return Query().Where(o => o.ProductId == productId && o.UserId == userId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleProductInCart", "Error getting product from cart: " + ex + "/n");
                throw;
            }
        }
    }
}
