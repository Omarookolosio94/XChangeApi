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

    }
}
