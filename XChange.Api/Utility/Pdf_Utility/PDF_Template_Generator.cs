﻿using System.Linq;
using System.Text;
using XChange.Api.DTO;

namespace XChange.Api.Utility
{
    public class PDF_Template_Generator
    {
        public static string Get_Orders_Receipt_HTML_Template(Reciept reciept)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div id='invoice-POS'>
                                   <center id='top'>
                                      <div class='logo'></div>
                                      <div class='info'> 
                                         <h2>XChange ECommerce</h2>
                                     </div>
                                 </center>
            ");

            sb.AppendFormat(@"
                                <div id='mid'>
                                     <div class='info'>
                                           <h2>Contact Info</h2>
                                            <p> 
                                                Billing Address : {0}</br>
                                                Billing Phone Number: {1}</br>
                                            </p>
                                      </div>
                                    </div>
        
                                 <div id='bot'>
    
                                    <div id='table'>
                                        <table>
                                            <tr class='tabletitle'>
                                                <td class='item'><h2>Item</h2></td>
                                                <td class='Hours'><h2>Unit Price</h2></td>
                                                <td class='Hours'><h2>Quantity</h2></td>
                                                <td class='Rate'><h2>Sub Total</h2></td>
                                            </tr>
            ", reciept.Billing_Address , reciept.Billing_Phone);


            foreach (var order in reciept.OrderedProducts)
            {
                sb.AppendFormat(@"
                                 <tr class='service'>
                                    <td class='tableitem'><p class='itemtext'>{0}</p></td>
                                    <td class='tableitem'><p class='itemtext'>{1}</p></td>
                                    <td class='tableitem'><p class='itemtext'>{2}</p></td>
                                    <td class='tableitem'><p class='itemtext'>{3}</p></td>
                                </tr>

                ", order.Product_Id , order.Unit_Price , order.Quantity_Ordered , order.Price);
            }

            sb.AppendFormat(@"
                                                <tr class='tabletitle'>
                                                    <td></td>
                                                    <td></td>
                                                    <td class='Rate'><h2>Tax</h2></td>
                                                    <td class='payment'>{0}<h2></h2></td>
                                                </tr>
    
                                                <tr class='tabletitle'>
                                                    <td></td>
                                                    <td></td>
                                                    <td class='Rate'><h2>Total</h2></td>
                                                    <td class='payment'><h2>{1}</h2></td>
                                                </tr>
                 
                                            </table>
                                        </div>
                       
                                        <div id='legalcopy'>
                                            <p class='legal'><strong>Thank you for your business!</strong>  
                                            Please, confirm order and proceed to payment. <br/>
                                            For enquiry contact +234 000 000 000
                                            </p>
                                        </div>
                                    </div>
                             </div>
                    </body>
                </html>
            ", reciept.Total_Tax , reciept.Total_Price);

            return sb.ToString();
        }
    }
}
