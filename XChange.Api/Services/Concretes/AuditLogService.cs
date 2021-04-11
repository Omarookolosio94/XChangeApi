using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;

        }

        public async void AddAuditLog(AuditLog auditLog)
        {
            try
            {
                _auditLogRepository.AddAuditLog(auditLog);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogs()
        {
            try
            {
                var status = await _auditLogRepository.GetAuditLogs();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogByUser(int userId)
        {
            try
            {
                var status = await _auditLogRepository.GetAuditLogByUser(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
