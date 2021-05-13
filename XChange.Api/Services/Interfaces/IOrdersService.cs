using System.Collections.Generic;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<bool> MakeOrder(Orders order);
        Task<List<Orders>> GetOrders();
        Task<Orders> GetOrder(int orderId);
        Task<List<Orders>> GetUserOrders(int userId);
        Task<List<Orders>> GetUserValidOrders(int userId);
        Task<Orders> GetSingleUserOrder(int orderId, int userId);
        Task<int> GetOrdersCount();
        Task<int> GetUserOrdersCount(int userId);
        Task<int> GetUserValidOrdersCount(int userId);
        Task<List<Orders>> QueryOrderByOrderStatus(string query);
        Task<List<Orders>> QueryUserOrderByOrderStatus(int userId, string query);
        Task<bool> IsOrderValid(int orderId);
        Task<bool> IsUserOrder(int userid, int orderId);
        Task<bool> CancelOrder(int orderId, int userId , string reason);
        //Task<bool> CloseOrder(int orderId , int userId);
        //Task<bool> OpenOrder(int orderId);
    }
}
