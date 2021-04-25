using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class ProductsRepository : BaseRepository<Products>, IProductsRepository
    {

        private static string ModuleName = "ProductsRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();



        public ProductsRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AddProduct(Products product)
        {
            try
            {
                await InsertAsync(product);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddProducts", "Error Adding Product" + ex + "\n");
                throw;
            }
        }

        public async Task<List<Products>> GetProducts()
        {

            try
            {
                return Query().OrderByDescending(product => product.ProductId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetProducts", "Error Fetching Products " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Products>> GetProductsBySeller(int sellerId)
        {

            try
            {
                return Query().OrderByDescending(product => product.ProductId).Where(o => o.SellerId == sellerId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetProductsBySeller", "Error Fetching products of seller: " + sellerId + "error : " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Products>> SearchProducts(string searchParams)
        {
            try
            {
                var _queryList = new List<Products>();
                var _query = Query().ToList();
                if (!string.IsNullOrEmpty(searchParams))
                {
                    if (_query != null)
                    {
                        _queryList = _query = _query.Where(x => x.ProductName.ToLower().Contains(searchParams)
                                                || x.ProductDescription.ToLower().Contains(searchParams) || x.Category.ToLower().Contains(searchParams)).ToList();
                    }
                }

                _queryList = _query.AsQueryable().ToList();
                return _queryList.ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "SearchProducts", "Error Searching for Products " + ex + "\n");
                throw;
            }

        }

        public async Task<Products> GetProduct(int productId)
        {
            try
            {
                return Query().Where(o => o.ProductId == productId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetProduct", "Error Fetching single product: " + productId + "error : " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> UpdateProduct(Products product)
        {
            try
            {
                Update(product);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateProduct", "Error updating product. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int sellerId , int productId)
        {
            try
            {
                var product = Query().Where(o => o.ProductId == productId && o.SellerId == sellerId);
                DeleteRange(product);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "DeleteProduct", "Error Deleting Product" + ex + "\n");
                throw;
            }
        }

        public async Task<Products> GetSingleProductBySeller(int sellerId, int productId)
        {
            try
            {
                return Query().Where(o => o.ProductId == productId && o.SellerId == sellerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleProductBySeller", "Error Fetching single product by seller: " + sellerId + "product: " + productId + "error : " + ex + "\n");
                throw;
            }
        }
    }
}
