using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class RegistrationLogRepository : BaseRepository<Models.RegistrationLog>, IRegistrationLogRepository
    {

        private static string ModuleName = "RegistrationLogRepository";

        public RegistrationLogRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void AddRegistrationLog(RegistrationLog registrationLog)
        {
            try
            {
                await InsertAsync(registrationLog);
                await Commit();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddRegistrationLog", "Error  Adding Registration Log" + ex + "\n");
              
            }
        }

        public async Task<RegistrationLog> GetRegistrationLog(int registrationLogId)
        {
            try
            {
                return Query().Where(log => log.RegistrationLogId == registrationLogId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetRegistrationLogId", "Error RegistrationLog" + registrationLogId + ex + "\n");
                throw;
            }
        }

        public async Task<List<RegistrationLog>> GetRegistrationLogs()
        {
            try
            {
                return Query().OrderByDescending(log => log.RegistrationLogId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetRegistrationLogs", "Error Fetching All Registration Logs"  + ex + "\n");
                throw;
            }
        }

    }

}