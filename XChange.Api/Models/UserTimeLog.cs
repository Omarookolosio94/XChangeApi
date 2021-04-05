using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class UserTimeLog
    {
        public int UserTimeLogId { get; set; }
        public DateTime? TimeLogged { get; set; }
        public string Error { get; set; }
        public bool? IsSuccessful { get; set; }
        public int? UserId { get; set; }
    }
}
