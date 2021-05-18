using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface IOrdersLogService
    {
        Task<List<OrdersLog>> GetOrdersLog();
        Task<List<OrdersLog>> GetOrdersLogByUser(int userId);
        void AddOrdersLog(OrdersLog ordersLog);
    }
}
