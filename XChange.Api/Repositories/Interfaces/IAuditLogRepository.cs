using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetAuditLogs();
        Task<List<AuditLog>> GetAuditLogByUser(int userId);
        void AddAuditLog(AuditLog auditLog);
        
    }
}
