using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class SellersService : ISellersService
    {
        private readonly ISellersRepository _sellersRepository;

        public SellersService(ISellersRepository sellersRepository)
        {
            _sellersRepository = sellersRepository;

        }

        public async Task<bool> AddSeller(Sellers seller)
        {
            try
            {
                var status = await _sellersRepository.AddSeller(seller);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<Sellers> GetSeller(int userId)
        {
            try
            {
                var status = await _sellersRepository.GetSeller(userId);
                return status;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Sellers>> GetSellers()
        {
            try
            {
                var status = await _sellersRepository.GetSellers();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> UpdateSeller(int userId, Sellers seller)
        {
            try
            {
                bool result = false;
                Sellers updateSeller = await _sellersRepository.GetSeller(userId);

                if (updateSeller != null)
                {
                    updateSeller.CompanyName = seller.CompanyName;
                    updateSeller.ContactFirstName = seller.ContactFirstName;
                    updateSeller.ContactLastName = seller.ContactLastName;
                    updateSeller.ContactPosition = seller.ContactPosition;
                    updateSeller.Phone = seller.Phone;
                    updateSeller.Logo = seller.Logo;

                    result = await _sellersRepository.UpdateSeller(updateSeller);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsSellerRegistered(int userId)
        {
            try
            {
                bool result = false;
                Sellers seller = await _sellersRepository.GetSeller(userId);

                if (seller != null)
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

    }
}
