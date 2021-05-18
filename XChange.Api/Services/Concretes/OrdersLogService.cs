using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class OrdersLogService : IOrdersLogService
    {
        private readonly IOrdersLogRepository _ordersLogRepository;

        public OrdersLogService(IOrdersLogRepository ordersLogRepository)
        {
            _ordersLogRepository = ordersLogRepository;

        }

        public async void AddOrdersLog(OrdersLog ordersLog)
        {
            try
            {
                _ordersLogRepository.AddOrdersLog(ordersLog);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<OrdersLog>> GetOrdersLog()
        {
            try
            {
                var status = await _ordersLogRepository.GetOrdersLog();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<OrdersLog>> GetOrdersLogByUser(int userId)
        {
            try
            {
                var status = await _ordersLogRepository.GetOrdersLogByUser(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
