using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IRegistrationLogRepository
    {
        void AddRegistrationLog(RegistrationLog registrationLog);
        Task<List<RegistrationLog>> GetRegistrationLogs();
        Task<RegistrationLog> GetRegistrationLog(int registrationLogId);

    }
}
