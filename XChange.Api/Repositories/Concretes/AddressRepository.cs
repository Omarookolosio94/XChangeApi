using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {

        private static string ModuleName = "AddressRepository";
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();



        public AddressRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAddress(Address address)
        {
            try
            {
                await InsertAsync(address);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddAddress", "Error Adding Address" + ex + "\n");
                throw;
            }
        }

        public async Task<List<Address>> GetAllAddress()
        {
            try
            {
                return Query().OrderByDescending(address => address.AddressId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetAllAddress", "Error Fetching All Address. " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Address>> GetAddressOfUser(int userId)
        {
            try
            {
                return Query().OrderByDescending(address => address.AddressId).Where(o => o.UserId == userId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetAddressOfUser", "Error Fetching Address of user: " + userId + "error : " + ex + "\n");
                throw;
            }
        }

        public async Task<Address> GetSingleAddress(int addressId)
        {
            try
            {
                return Query().Where(o => o.AddressId == addressId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleAddress", "Error Fetching single Address: " + addressId + "error : " + ex + "\n");
                throw;
            }
        }


        public async Task<Address> GetSingleAddressOfUser(int userId, int addressId)
        {
            try
            {
                return Query().Where(o => o.AddressId == addressId && o.UserId == userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleAddressOfUser", "Error Fetching single adress of user: " + userId + "address: " + addressId + "error : " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            try
            {
                Update(address);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateAddress", "Error Updating Address. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Address>> SearchAddress(string searchParam)
        {
            try
            {
                var _queryList = new List<Address>();
                var _query = Query().ToList();
                if (!string.IsNullOrEmpty(searchParam))
                {
                    if (_query != null)
                    {
                        _queryList = _query = _query.Where(x => x.Street.ToLower().Contains(searchParam)
                                                || x.State.ToLower().Contains(searchParam) || x.City.ToLower().Contains(searchParam)).ToList();
                    }
                }

                _queryList = _query.AsQueryable().ToList();
                return _queryList.ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "SearchAddress", "Error Searching for Address " + ex + "\n");
                throw;
            }

        }

        public async Task<bool> DeleteAddress(int userId, int addressId)
        {
            try
            {
                var address = Query().Where(o => o.AddressId == addressId && o.UserId == userId);

                if (address.Count() > 0)
                {
                    DeleteRange(address);
                    await Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "DeleteAddress", "Error Deleting Address " + ex + "\n");
                throw;
            }
        }

    }
}
