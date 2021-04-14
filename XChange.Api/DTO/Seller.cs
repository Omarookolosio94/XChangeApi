using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Seller
    {
        public string CompanyName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPosition { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateSeller
    {
        public string CompanyName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPosition { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
    }
}
