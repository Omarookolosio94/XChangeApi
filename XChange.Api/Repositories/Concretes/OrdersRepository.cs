using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class OrdersRepository : BaseRepository<Orders>, IOrdersRepository
    {

        private static string ModuleName = "OrdersRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();


        public OrdersRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
  
 
        public async Task<bool> MakeOrder(Orders order)
        {
            try
            {
                await InsertAsync(order);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "MakeOrder", "Error Making Order" + ex + "\n");
                throw;
            }
        }

        public  async Task<bool> UpdateOrder(Orders order)
        {
            try
            {
                Update(order);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateOrder", "Error Updating order. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Orders>> GetOrders()
        {
            try
            {
                return Query().ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOrders", "Error Fetching orders. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<Orders> GetOrder(int orderId)
        {
            try
            {
                return Query().Where(o => o.OrderId == orderId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOrder", "Error Fetching order: " + orderId + " Error : " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Orders>> GetUserOrders(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserOrders", "Error getting list of user's orders. Error: " + ex + "/n");
                throw;
            }
        }

        public async Task<List<Orders>> GetUserValidOrders(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId && o.OrderStatus.ToLower() != "Cancelled".ToLower()).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserValidOrders", "Error getting list of user's orders. Error: " + ex + "/n");
                throw;
            }
        }


        public async Task<Orders> GetSingleUserOrder(int orderId, int userId)
        {
            try
            {
                return Query().Where(o => o.OrderId == orderId && o.UserId == userId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleUserOrder", "Error getting order: " + orderId + ". Error: " + ex + "/n");
                throw;
            }
        }

        public async Task<int> GetOrdersCount()
        {
            try
            {
                return Query().Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOrdersCount", "Error getting count of orders: Error: " + ex + "/n");
                return 0;
            }
        }

        public  async Task<int> GetUserOrdersCount(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserOrdersCount", "Error getting count of user's orders. userId: " + userId + " Error: " + ex + " "+ "/n");
                return 0;
            }
        }

        public async Task<int> GetUserValidOrdersCount(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId && o.OrderStatus.ToLower() != "Cancelled".ToLower()).Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserValidOrdersCount", "Error getting count of user's valid orders. userId: " + userId + " Error: " + ex + " " + "/n");
                return 0;
            }
        }


        public async Task<List<Orders>> QueryOrderByOrderStatus(string query)
        {
            try
            {
                return Query().Where(o => o.OrderStatus.ToLower() == query.ToLower()).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "QueryOrderByOrderStatus", "Error getting order query using order status. Error: " + ex + "/n");
                throw;
            }
        }

        public async Task<List<Orders>> QueryUserOrderByOrderStatus(int userId, string query)
        {
            try
            {
                return Query().Where(o => o.UserId == userId && o.OrderStatus.ToLower() == query.ToLower()).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "QueryUserOrderByOrderStatus", "Error getting user order query using order status. Error: " + ex + "/n");
                throw;
            }
        }

        public async Task<bool> IsUserOrder(int userId, int orderId)
        {
            try
            {
                var query = Query().Where(o => o.UserId == userId && o.OrderId == orderId).Count();

                if (query > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsUserOrder", "Error checking if order was made by user: " + ex + "/n");
                return false;

            }
        }

        public async Task<bool> IsOrderValid(int orderId)
        {
            try
            {
                var query = Query().Where(o => o.OrderId == orderId).Count();

                if (query > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsOrderValid", "Error checking order exits" + ex + "/n");
                return false;

            }
        }

    }
}
