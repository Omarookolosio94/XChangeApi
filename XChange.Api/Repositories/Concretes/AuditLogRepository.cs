using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class AuditLogRepository : BaseRepository<AuditLog> , IAuditLogRepository
    {
        private static string ModuleName = "AuditLogRepository";

        public AuditLogRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AuditLog>> GetAuditLogs()
        {
            try
            {
                return Query().OrderByDescending(o => o.TimeLogged).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetAuditLogs", "Error Getting Audit Logs" + ex + "\n");
                throw;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogByUser(int userId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetAuditLogByUser", "Error Getting User Audit Logs" + ex + "\n");
                throw;
            }
        }

        public async void AddAuditLog(AuditLog auditLog)
        {
            try
            {
                await InsertAsync(auditLog);
                await Commit();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddAuditLog", "Error Getting User Audit Logs" + ex + "\n");
                throw;
            }
        }

    }
}
