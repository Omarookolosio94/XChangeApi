using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Services.Interfaces
{
    public interface IAddressService
    {
        Task<bool> AddAddress(Address address);
        Task<List<Address>> GetAllAddress();
        Task<List<Address>> GetAddressOfUser(int userId);
        Task<Address> GetSingleAddress(int addressId);
        Task<Address> GetSingleAddressOfUser(int userId, int addressId);
        Task<bool> UpdateAddress(int userId , Address address);
        Task<List<Address>> SearchAddress(string searchParam);
    }
}
