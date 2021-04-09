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
    }
}