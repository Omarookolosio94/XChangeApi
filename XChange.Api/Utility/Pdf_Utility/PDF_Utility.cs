using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using XChange.Api.DTO;

namespace XChange.Api.Utility.Pdf_Utility
{
    public class PDF_Utility
    {
        public static HtmlToPdfDocument Create_Order_PDF(int orderId , int userId , string receiptTemplate)
        {
            //var directory = Path.Combine(Directory.GetCurrentDirectory(), "PDF_Receipts");
            //var reportName = @"\Order_Receipt_" + orderId +"_" + userId + "_" + DateTime.Now.ToString("ddMMMMyyyyHHmm") + ".pdf";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Order Receipt Report for order -" + orderId,
                //Out = directory + reportName
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = receiptTemplate,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Utility", "PDF_Style.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return pdf;
        }
    }
}
