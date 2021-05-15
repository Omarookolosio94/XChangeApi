using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Reciept
    {
        public int Order_Id { get; set; }
        public int User_Id { get; set; }
        public string Order_Status { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Total_Tax { get; set; }
        public List<OrderedProducts> OrderedProducts { get; set; }
    }


    public class OrderedProducts
    {
        public int Porduct_Id { get; set; }
        //public int Seller_Id { get; set; }
        public decimal Unit_Price { get; set; }
        public int Quantity_Ordered { get; set; }
        public decimal Price { get; set; }
    }
}
