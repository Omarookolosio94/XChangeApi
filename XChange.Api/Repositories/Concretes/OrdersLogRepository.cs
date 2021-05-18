using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class OrdersLogRepository : BaseRepository<OrdersLog>, IOrdersLogRepository
    {
        private static string ModuleName = "OrdersLogRepository";

        public OrdersLogRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrdersLog>> GetOrdersLog()
        {
            try
            {
                return Query().OrderByDescending(o => o.TimeLogged).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOrdersLogs", "Error Getting Orders Logs" + ex + "\n");
                throw;
            }
        }

        public async Task<List<OrdersLog>> GetOrdersLogByUser(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOrdersLogByUser", "Error Getting User Orders Logs" + ex + "\n");
                throw;
            }
        }

        public async void AddOrdersLog(OrdersLog ordersLog)
        {
            try
            {
                await InsertAsync(ordersLog);
                await Commit();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "OrdersLog", "Error adding order log" + ex + "\n");
                throw;
            }
        }

    }
}
