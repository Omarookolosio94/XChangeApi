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
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;

        }

        public async Task<bool> MakeOrder(Orders order)
        {
            try
            {
                var status = await _ordersRepository.MakeOrder(order);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        /*
        public async Task<bool> UpdateOrder(int userid, Orders order)
        {
            try
            {
                bool result = true;
                Orders updateOrder = await _ordersRepository.GetOrder(order.OrderId);

                if (updateOrder != null)
                {
                    updateOrder.

                    result = await _productsRepository.UpdateProduct(updateProduct);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        */

        public async Task<List<Orders>> GetOrders()
        {
            try
            {
                var status = await _ordersRepository.GetOrders();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Orders> GetOrder(int orderId)
        {
            try
            {
                var status = await _ordersRepository.GetOrder(orderId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Orders>> GetUserOrders(int userId)
        {
            try
            {
                var status = await _ordersRepository.GetUserOrders(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Orders>> GetUserValidOrders(int userId)
        {
            try
            {
                var status = await _ordersRepository.GetUserValidOrders(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Orders> GetSingleUserOrder(int orderId, int userId)
        {
            try
            {
                var status = await _ordersRepository.GetSingleUserOrder(orderId, userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetOrdersCount()
        {
            try
            {
                int count = await _ordersRepository.GetOrdersCount();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetUserOrdersCount(int userId)
        {
            try
            {
                int count = await _ordersRepository.GetUserOrdersCount(userId);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetUserValidOrdersCount(int userId)
        {
            try
            {
                int count = await _ordersRepository.GetUserValidOrdersCount(userId);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> CancelOrder(int orderId, int userId, string reason)
        {
            try
            {
                bool result = false;
                Orders updateOrder = await _ordersRepository.GetSingleUserOrder(orderId, userId);

                if (updateOrder != null)
                {
                    updateOrder.CancelReason = reason;
                    updateOrder.OrderStatus = "Cancelled";
                    updateOrder.CancelledAt = DateTime.Now;

                    result = await _ordersRepository.UpdateOrder(updateOrder);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Orders>> QueryOrderByOrderStatus(string query)
        {
            try
            {
                var status = await _ordersRepository.QueryOrderByOrderStatus(query);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Orders>> QueryUserOrderByOrderStatus(int userId, string query)
        {
            try
            {
                var status = await _ordersRepository.QueryUserOrderByOrderStatus(userId, query);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsOrderValid(int orderId)
        {
            try
            {
                var status = await _ordersRepository.IsOrderValid(orderId);
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsUserOrder(int userid, int orderId)
        {
            try
            {
                var status = await _ordersRepository.IsUserOrder(userid, orderId);
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderReceiptUrl(int orderId, string receiptUrl , string receiptName)
        {
            try
            {
                bool result = true;
                Orders updateOrder = await _ordersRepository.GetOrder(orderId);

                if (updateOrder != null)
                {
                    updateOrder.OrderRecieptUrl = receiptUrl;
                    updateOrder.OrderReceiptName = receiptName;

                    result = await _ordersRepository.UpdateOrder(updateOrder);
                }

                return result;
            }
            catch (Exception ex)
            {
                new Logger().LogError("OrdersService", "UpdateOrderReceiptUrl", "Error Updating order receipt Url. Exception error: " + ex + "\n");
                return true;
            }
        }

        public async Task<bool> UpdateReceiptUrl(Orders order)
        {
            try
            {
                 return await _ordersRepository.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                new Logger().LogError("OrdersService", "UpdateReceiptUrl", "Error Updating order receipt Url. Exception error: " + ex + "\n");
                return true;
            }
        }
    }
}
