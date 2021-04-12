using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class AuditLog
    {
        public long AuditLogId { get; set; }
        public int? UserId { get; set; }
        public string Activity { get; set; }
        public DateTime? TimeLogged { get; set; }
        public string Email { get; set; }
    }
}
