using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class ProductsService : IProductsService
    {
        private static string ModuleName = "ProductsService";

        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;

        }

        public async Task<bool> AddProduct(Products product)
        {
            try
            {
                var status = await _productsRepository.AddProduct(product);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<List<Products>> GetProducts()
        {
            try
            {
                var status = await _productsRepository.GetProducts();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Products>> GetProductsBySeller(int sellerId)
        {
            try
            {
                var status = await _productsRepository.GetProductsBySeller(sellerId);
                return status;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Products>> SearchProducts(string searchParams)
        {
            try
            {
                var status = await _productsRepository.SearchProducts(searchParams);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Products> GetProduct(int productId)
        {

            try
            {
                var status = await _productsRepository.GetProduct(productId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<decimal> GetProductPrice(int productId)
        {

            try
            {
                var status = await _productsRepository.GetProduct(productId);

                if (status != null)
                {
                    return status.UnitPrice;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> UpdateProduct(int sellerId, Products product)
        {
            try
            {
                bool result = false;
                Products updateProduct = await _productsRepository.GetSingleProductBySeller(sellerId, product.ProductId);

                if (updateProduct != null)
                {
                    updateProduct.ProductName = product.ProductName;
                    updateProduct.ProductDescription = product.ProductDescription;
                    updateProduct.Category = product.Category;
                    updateProduct.Quantity = product.Quantity;
                    updateProduct.Quantity = product.Quantity;
                    updateProduct.UnitPrice = product.UnitPrice;
                    updateProduct.UnitsInOrder = product.UnitsInOrder;
                    updateProduct.UnitsInStock = product.UnitsInStock;
                    updateProduct.PictureUrl = product.PictureUrl;
                    updateProduct.PictureName = product.PictureName;
                    updateProduct.LastUpdateTime = DateTime.Now;

                    result = await _productsRepository.UpdateProduct(updateProduct);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int sellerId, int productId)
        {
            try
            {
                var status = await _productsRepository.DeleteProduct(sellerId, productId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetProductsCount()
        {
            try
            {
                int count = await _productsRepository.GetProductsCount();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateProductRating(int rating, int productId)
        {
            try
            {
                bool result = true;
                Products updateProduct = await _productsRepository.GetProduct(productId);

                if (updateProduct != null)
                {

                    var newRating = Utility.Utility.CalculateProductRating(updateProduct.Rating, rating);

                    updateProduct.Rating = newRating;
                    updateProduct.LastUpdateTime = DateTime.Now;

                    result = await _productsRepository.UpdateProduct(updateProduct);
                }

                return result;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateProductRating", "Error Updating ProductRating. Exception error: " + ex + "\n");
                return true;
                //throw;
            }
        }

        public async Task<bool> UpdateProductRatingReview(string rating, int productId)
        {
            try
            {
                bool result = true;
                Products updateProduct = await _productsRepository.GetProduct(productId);

                if (updateProduct != null)
                {
                    updateProduct.Rating = rating;
                    updateProduct.LastUpdateTime = DateTime.Now;

                    result = await _productsRepository.UpdateProduct(updateProduct);
                }

                return result;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateProductRatingReview", "Error Updating ProductRatingReview. Exception error: " + ex + "\n");
                return true;
            }
        }

        public async Task<bool> IsProduct(int productId)
        {
            try
            {
                var status = await _productsRepository.IsProduct(productId);
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
