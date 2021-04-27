using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<bool> RegisterUser(Users user);
        Task<Users> GetUser(int userId);
        Task<List<Users>> GetUsers();
        Task<bool> IsEmailRegistered(string email);
        Task<Users> GetUserByEmail(string email);
        Task<bool> UpdateUser(Users user);
        Task<int> GetUsersCount();
        //Task<Users> SearchUser(string searchParams);

    }
}
