using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Buyer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }

    public class UpdateBuyer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}
