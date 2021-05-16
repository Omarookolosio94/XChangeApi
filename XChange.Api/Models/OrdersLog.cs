using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class OrdersLog
    {
        public int OrdersLogId { get; set; }
        public string Activity { get; set; }
        public string Receipt { get; set; }
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime? TimeLogged { get; set; }
    }
}
