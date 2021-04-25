using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class ProductsService : IProductsService
    {
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
            } catch(Exception ex)
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

            } catch(Exception ex)
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

        public async  Task<bool> UpdateProduct(int sellerId, Products product)
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
                    //updateProduct.Ranking = product.Ranking;
                    updateProduct.Quantity = product.Quantity;
                    updateProduct.UnitPrice = product.UnitPrice;
                    updateProduct.UnitsInOrder = product.UnitsInOrder;
                    updateProduct.UnitsInStock = product.UnitsInStock;
                    updateProduct.Picture = product.Picture;
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
                var status = await _productsRepository.DeleteProduct(sellerId ,productId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
