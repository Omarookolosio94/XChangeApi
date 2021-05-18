using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IOrdersLogRepository
    {
        Task<List<OrdersLog>> GetOrdersLog();
        Task<List<OrdersLog>> GetOrdersLogByUser(int userId);
        void AddOrdersLog(OrdersLog ordersLog);
    }
}
