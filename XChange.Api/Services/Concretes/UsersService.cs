using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Data.Services.Concretes
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository )
        {
            _usersRepository = usersRepository;
         
        }

        public async Task<bool> RegisterUser(Users user)
        {
            try
            {
                var status = await _usersRepository.RegisterUser(user);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }


        public async Task<Users> GetUser(int userId)
        {
            try
            {
                var status = await _usersRepository.GetUser(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Users> GetUserByEmail(string email)
        {
            try
            {
                var status = await _usersRepository.GetUserByEmail(email);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<Users>> GetUsers()
        {
            try
            {
                var status = await _usersRepository.GetUsers();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsEmailRegistered(string email)
        {
            try
            {
                bool isEmailAvailable = await _usersRepository.IsEmailRegistered(email);
                return isEmailAvailable;

            }
            catch (Exception ex)
            {
                throw;
                  
            }
        }

        /*
        public async Task<bool> UpdateUser(int userId, Users user)
        {
            try
            {
                Users updateUser = await _usersRepository.GetUser(userId);
                bool result = false;

                if(updateUser != null)
                {
                    updateUser.UserFirstName = user.UserFirstName;
                    updateUser.UserLastName = user.UserLastName;
                    updateUser.Gender = user.Gender;
                    //updateUser.Password = user.Password;
                    //updateUser.Email = user.Email;

                   return result = await _usersRepository.UpdateUser(updateUser);
                }
                 
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    */

        public async Task<bool> VerifyUser(int userId, bool isVerified)
        {
            try
            {
                Users user = await _usersRepository.GetUser(userId);
                bool result = false;

                if (user != null)
                {
                    user.IsVerified = isVerified;
                    result = await _usersRepository.UpdateUser(user);
                } 

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ResetPassword(string email ,string password)
        {
            try
            {
                bool result = false;
                Users user = await _usersRepository.GetUserByEmail(email);

                if (user != null)
                {
                    user.Password = password;
                    result = await _usersRepository.UpdateUser(user);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    

    }
}