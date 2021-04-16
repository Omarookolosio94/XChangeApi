using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.DTO;
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
        private readonly IRegistrationLogService _registrationLogService;
        private readonly XChangeDatabaseContext dbGeneralContext = new XChangeDatabaseContext();



        public UsersRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _registrationLogService = new RegistrationLogService(new RegistrationLogRepository(dbGeneralContext));

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
                User logUser = new User
                {
                    Email = user.Email,
                    Password = user.Password,
                    UserType = user.UserType
                };

                //log exception
                new Logger().LogError(ModuleName, "RegisterUser", "Error Registering User" + user + ex + "\n");

                RegistrationLog registrationSuccessLog = Utility.Utility.AddRegistrationLog(logUser, false, ex.Message.ToString());
                _registrationLogService.AddRegistrationLog(registrationSuccessLog);

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

        public async Task<Users> GetUserByEmail(string email)
        {
            try
            {
                var user = Query().Where(o => o.Email.ToLower() == email.ToLower()).FirstOrDefault();
                Users result = new Users{};

                if (user != null)
                {
                    return result = user;
                }

                return result;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetUserByEmail", "Error Fetching User By Email" + email + "exception error: " +  ex + "\n");
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
                new Logger().LogError(ModuleName, "GetUsers", "Error Fetching All Users" + ex + "\n");
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

        public async Task<bool> UpdateUser(Users user)
        {
            try
            {
                Update(user);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateUser", "Error Updating User " + user.UserId + " exception error: "+ ex + "/n");
                throw;
            }
        }



    }

}