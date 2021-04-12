using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class RegistrationLogService : IRegistrationLogService
    {
        private readonly IRegistrationLogRepository _registrationLogRepository;
        //private static string ModuleName = "UsersService";

        public RegistrationLogService(IRegistrationLogRepository registrationLogRepository)
        {
            _registrationLogRepository = registrationLogRepository;
        }

        public async void AddRegistrationLog(RegistrationLog registrationLog)
        {
            try
            {
                 _registrationLogRepository.AddRegistrationLog(registrationLog);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public async Task<RegistrationLog> GetRegistrationLog(int registrationLogId)
        {
            try
            {
                var status = await _registrationLogRepository.GetRegistrationLog(registrationLogId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RegistrationLog>> GetRegistrationLogs()
        {
            try
            {
                var status = await _registrationLogRepository.GetRegistrationLogs();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
