using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;

        }

        public async Task<bool> AddAddress(Address address)
        {
            try
            {
                var status = await _addressRepository.AddAddress(address);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<List<Address>> GetAllAddress()
        {
            try
            {
                var status = await _addressRepository.GetAllAddress();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Address>> GetAddressOfUser(int userId)
        {
            try
            {
                var status = await _addressRepository.GetAddressOfUser(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<Address> GetSingleAddress(int addressId)
        {
            try
            {
                var status = await _addressRepository.GetSingleAddress(addressId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<Address> GetSingleAddressOfUser(int userId, int addressId)
        {
            try
            {
                var status = await _addressRepository.GetSingleAddressOfUser(userId , addressId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> UpdateAddress(int userId, Address address)
        {
            try
            {
                bool result = false;
                Address updateAddress = await _addressRepository.GetSingleAddressOfUser(userId, address.AddressId);

                if (updateAddress != null)
                {
                    updateAddress.AdressType = address.AdressType;
                    updateAddress.Street = address.Street;
                    updateAddress.City = address.City;
                    updateAddress.State = address.State;
                    updateAddress.Country = address.Country;
                    updateAddress.PostalCode = address.PostalCode;

                    result = await _addressRepository.UpdateAddress(updateAddress);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Address>> SearchAddress(string searchParam)
        {
            try
            {
                var status = await _addressRepository.SearchAddress(searchParam);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> DeleteAddress(int userId, int addressId)
        {
            try
            {
                var status = await _addressRepository.DeleteAddress(userId, addressId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
