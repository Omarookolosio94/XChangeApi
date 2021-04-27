using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<bool> AddProduct(Products product);
        Task<List<Products>> GetProducts();
        Task<List<Products>> GetProductsBySeller(int sellerId);
        Task<Products> GetSingleProductBySeller(int sellerId, int productId);
        Task<List<Products>> SearchProducts(string searchParams);
        Task<Products> GetProduct(int productId);
        Task<bool> UpdateProduct(Products product);
        Task<bool> DeleteProduct(int sellerId , int productId);
        Task<int> GetProductsCount();
    }
}
