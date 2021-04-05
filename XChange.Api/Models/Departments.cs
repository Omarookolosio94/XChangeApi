using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Departments
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public string ContactFullName { get; set; }
    }
}
