using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<bool> AddAddress(Address address);
        Task<List<Address>> GetAllAddress();
        Task<List<Address>> GetAddressOfUser(int userId);
        Task<Address> GetSingleAddress(int addressId);
        Task<Address> GetSingleAddressOfUser(int userId, int addressId);
        Task<bool> UpdateAddress(Address address);
        Task<List<Address>> SearchAddress(string searchParam);
        Task<bool> DeleteAddress(int userId, int addressId);
    }
}
