using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Data.Services.Concretes;

namespace XChange.Api.Repositories.Concretes
{
    public class UsersRepository : BaseRepository<Models.Users>, IUsersRepository
    {

        private static string ModuleName = "UsersRepository";

        public UsersRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> RegisterUser(Users user)
        {
            try
            {
                await InsertAsync(user);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Users> GetUser(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUser", "Error Fetching User" + userId + ex + "\n");
                throw;
            }
        }

        public async Task<List<Users>> GetUsers()
        {
            try
            {
                return Query().OrderByDescending(user => user.UserId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUsers", "Error Fetching All Users"  + ex + "\n");
                throw;
            }
        }


        public async Task<bool> IsEmailRegistered(string email)
        {
            try
            {
                var mailList = Query().Where(o => o.Email.ToString().ToLower() == email.ToString().ToLower()).FirstOrDefault();

                if (mailList != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsEmailRegistered", "Error Getting Is Email Registered" + ex + "/n");
                throw;
            }
        }


    }

}