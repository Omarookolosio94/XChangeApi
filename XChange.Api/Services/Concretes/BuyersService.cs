using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class BuyersService : IBuyersService
    {
        private readonly IBuyersRepository _buyersRepository;

        public BuyersService(IBuyersRepository buyersRepository)
        {
            _buyersRepository = buyersRepository;

        }

        public async Task<bool> AddBuyer(Buyers buyer)
        {
            try
            {
                var status = await _buyersRepository.AddBuyer(buyer);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<Buyers> GetBuyer(int userId)
        {
            try
            {
                var status = await _buyersRepository.GetBuyer(userId);
                return status;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Buyers>> GetBuyers()
        {
            try
            {
                var status = await _buyersRepository.GetBuyers();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> UpdateBuyer(int userId, Buyers buyer)
        {
            try
            {
                bool result = false;
                Buyers updateBuyer = await _buyersRepository.GetBuyer(userId);

                if (updateBuyer != null)
                {
                    updateBuyer.Gender = buyer.Gender;
                    updateBuyer.FirstName = buyer.FirstName;
                    updateBuyer.LastName = buyer.LastName;
                    updateBuyer.Phone = buyer.Phone;
                    updateBuyer.CompanyName = buyer.CompanyName;

                    result = await _buyersRepository.UpdateBuyer(updateBuyer);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsBuyerRegistered(int userId)
        {
            try
            {
                bool result = false;
                Buyers buyer = await _buyersRepository.GetBuyer(userId);

                if (buyer != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return true;
            }
        }


        public async Task<List<Buyers>> SearchBuyers(string searchParams)
        {
            try
            {
                var status = await _buyersRepository.SearchBuyers(searchParams);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> GetBuyersCount()
        {
            try
            {
                int count = await _buyersRepository.GetBuyersCount();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
