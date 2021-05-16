using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class CartsService : ICartsService
    {
        private readonly ICartsRepository _cartsRepository;

        public CartsService(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;

        }

        public async Task<bool> AddProductToCart(Carts cart)
        {
            try
            {
                var status = await _cartsRepository.AddProductToCart(cart);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> RemoveProductFromCart(int productId, int userId)
        {
            try
            {
                var status = await _cartsRepository.RemoveProductFromCart(productId, userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetCountOfUserCart(int userId)
        {
            try
            {
                int count = await _cartsRepository.GetCountOfUserCart(userId);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<int> GetCartCount()
        {
            try
            {
                int count = await _cartsRepository.GetCartCount();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<List<Carts>> GetUserCart(int userId)
        {
            try
            {
                var status = await _cartsRepository.GetUserCart(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserCart(int userId)
        {
            try
            {
                var status = await _cartsRepository.DeleteUserCart(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Carts>> GetCarts()
        {
            try
            {
                var status = await _cartsRepository.GetCarts();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Carts> GetSingleProductInCart(int productId , int userId)
        {
            try
            {
                var status = await _cartsRepository.GetSingleProductInCart(productId, userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<bool> UpdateUserCart(int userId, Carts cart)
        {

            try
            {
                bool result = false;
                Carts updateCart = await _cartsRepository.GetSingleProductInCart(cart.ProductId , userId);


                if (updateCart != null)
                {
                    updateCart.QuantityOrdered = cart.QuantityOrdered;

                    result = await _cartsRepository.UpdateUsercart(updateCart);
                }
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsProductInUserCart(int productId, int userId)
        {
            try
            {
                var status = await _cartsRepository.IsProductInUserCart(productId, userId);
                return status;
            }
            catch (Exception ex)
            {
               return false;
            }
        }

        public async Task<bool> DeleteCarts(List<int> cartIds)
        {
            try
            {
                var status = await _cartsRepository.DeleteCarts(cartIds);
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
