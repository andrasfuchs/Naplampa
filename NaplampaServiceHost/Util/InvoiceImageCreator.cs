using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Web.Hosting;
using System.Globalization;
using System.Drawing.Text;

namespace NaplampaWcfHost.Util
{
    public class InvoiceImageCreator
    {
        private static PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private static Stream CreateInvoiceImage(Invoice invoice, CultureInfo ci)
        {
            if (privateFontCollection.Families.Count() == 0)
            {
                privateFontCollection.AddFontFile(HostingEnvironment.MapPath("~/Fonts/consola.ttf"));
            }
            FontFamily consolasFontFamily = privateFontCollection.Families[0];

            Stream result = new MemoryStream();

            string fullPath = HostingEnvironment.MapPath("~/Invoice_Template.jpg");

            Image templateImage = Image.FromFile(fullPath);
            Graphics graph = Graphics.FromImage(templateImage);
            Brush brush = new SolidBrush(System.Drawing.Color.Black);

            Font myFont = new System.Drawing.Font("Arial", 26, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            Font myConsoleFont = new System.Drawing.Font(consolasFontFamily, 26, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            if (ci.ThreeLetterISOLanguageName != "eng")
            {
                // headers
                graph.DrawString(Resources.Invoice.ISSUER, myFont, brush, 153, 772);
                graph.DrawString(Resources.Invoice.BUYER, myFont, brush, 770, 772);
                graph.DrawString(Resources.Invoice.NOOFINVOICE, myFont, brush, 1305, 772);

                graph.DrawString(Resources.Invoice.NOOFORDER, myFont, brush, 153, 1092);
                graph.DrawString(Resources.Invoice.PAYMENT, myFont, brush, 374, 1092);
                graph.DrawString(Resources.Invoice.DATEOFDELIVERY, myFont, brush, 529, 1092);
                graph.DrawString(Resources.Invoice.DATEOFINVOICE, myFont, brush, 769, 1092);
                graph.DrawString(Resources.Invoice.DEADLINEOFPAYMENT, myFont, brush, 995, 1092);
                graph.DrawString(Resources.Invoice.CURRENCY, myFont, brush, 1305, 1092);

                graph.DrawString(Resources.Invoice.OBSERVATION, myFont, brush, 153, 1235);

                graph.DrawString(Resources.Invoice.NAMEOFPRODUCT, myFont, brush, new RectangleF(153, 1412, 370, 36), sf);
                graph.DrawString(Resources.Invoice.UNITOFMEASURE, myFont, brush, new RectangleF(529, 1412, 230, 36), sf);
                graph.DrawString(Resources.Invoice.QUANTITY, myFont, brush, new RectangleF(769, 1412, 220, 36), sf);
                graph.DrawString(Resources.Invoice.UNITPRICE, myFont, brush, new RectangleF(995, 1412, 305, 36), sf);
                graph.DrawString(Resources.Invoice.VALUE, myFont, brush, new RectangleF(1305, 1412, 210, 36), sf);

                graph.DrawString(Resources.Invoice.TOTAL, myFont, brush, 1100, 1944);
                graph.DrawString(Resources.Invoice.VAT, myFont, brush, 1100, 1980);
                graph.DrawString(Resources.Invoice.SUBTOTAL, myFont, brush, 1100, 2016);
            }

            // data
            graph.DrawString(invoice.IssuerBillingAddress.Name, myFont, brush, 153, 808);
            graph.DrawString(invoice.IssuerBillingAddress.PostalCode, myFont, brush, 153, 844);
            graph.DrawString(invoice.IssuerBillingAddress.AddressLine, myFont, brush, 153, 880);
            graph.DrawString(invoice.IssuerBillingAddress.Town, myFont, brush, 153, 916);
            graph.DrawString(Resources.Countries.ResourceManager.GetString(invoice.IssuerBillingAddress.Country.Name), myFont, brush, 153, 952);

            graph.DrawString(invoice.BuyerBillingAddress.Name, myFont, brush, 770, 808);
            graph.DrawString(invoice.BuyerBillingAddress.PostalCode, myFont, brush, 770, 844);
            graph.DrawString(invoice.BuyerBillingAddress.AddressLine, myFont, brush, 770, 880);
            graph.DrawString(invoice.BuyerBillingAddress.Town, myFont, brush, 770, 916);
            graph.DrawString(Resources.Countries.ResourceManager.GetString(invoice.BuyerBillingAddress.Country.Name), myFont, brush, 770, 952);

            graph.DrawString(invoice.InvoiceNo, myFont, brush, new RectangleF(1305, 916, 210, 36), sf);
            graph.DrawString(invoice.Order.OrderId.ToString(), myFont, brush, new RectangleF(153, 1128, 220, 36), sf);
            graph.DrawString(Resources.PaymentMethods.ResourceManager.GetString("PAYMENTMETHOD_" + invoice.Order.PaymentMethod.ToString()), myFont, brush, new RectangleF(377, 1128, 150, 36), sf);
            graph.DrawString(invoice.DateOfDelivery.ToShortDateString(), myFont, brush, new RectangleF(529, 1128, 230, 36), sf);
            graph.DrawString(invoice.DateOfInvoice.ToShortDateString(), myFont, brush, new RectangleF(769, 1128, 220, 36), sf);
            graph.DrawString(invoice.DeadlineOfPayment.ToShortDateString(), myFont, brush, new RectangleF(995, 1128, 305, 36), sf);
            graph.DrawString(invoice.Currency.ISO, myFont, brush, new RectangleF(1305, 1128, 210, 36), sf);


            graph.DrawString(String.Format("{0,12:0.00}", invoice.Subtotal), myConsoleFont, brush, new RectangleF(1305, 1944, 210, 36), sf);
            graph.DrawString(String.Format("{0,12:0.00}", invoice.VAT), myConsoleFont, brush, new RectangleF(1305, 1980, 210, 36), sf);
            graph.DrawString(String.Format("{0,12:0.00}", invoice.Total), myConsoleFont, brush, new RectangleF(1305, 2016, 210, 36), sf);

            // invoice lines
            int coorY = 1446;
            foreach (InvoiceLine invoiceLine in invoice.InvoiceLines.OrderBy(i => i.LineNumber))
            {
                graph.DrawString(Resources.Products.ResourceManager.GetString(invoiceLine.Description), myFont, brush, 153, coorY);
                graph.DrawString(Resources.Invoice.PIECE, myFont, brush, new RectangleF(529, coorY, 230, 36), sf);
                graph.DrawString(String.Format("{0,3:0}", invoiceLine.Quantity), myConsoleFont, brush, new RectangleF(769, coorY, 220, 36), sf);
                graph.DrawString(String.Format("{0,12:0.00}", invoiceLine.UnitPrice), myConsoleFont, brush, new RectangleF(995, coorY, 305, 36), sf);
                graph.DrawString(String.Format("{0,12:0.00}", (invoiceLine.Quantity * invoiceLine.UnitPrice)), myConsoleFont, brush, new RectangleF(1305, coorY, 210, 36), sf);
                
                coorY += 36;
            }
            
            templateImage.Save(result, System.Drawing.Imaging.ImageFormat.Jpeg);

            return result;
        }

        public static byte[] GetInvoiceImage(Invoice invoice, string cultureName)
        {
            byte[] result = null;

            CultureInfo ci = new System.Globalization.CultureInfo(cultureName);

            string folder = HostingEnvironment.MapPath("~/Invoices");
            string filename = "Invoice_" + invoice.InvoiceNo + "_" + ci.ThreeLetterISOLanguageName + "_(OrderId_" + invoice.Order.OrderId + ").jpg";
            string fullPath = folder + '\\' + filename;

            if (File.Exists(fullPath))
            {
                result = File.ReadAllBytes(fullPath);
            }
            else
            {
                Stream jpgStream = InvoiceImageCreator.CreateInvoiceImage(invoice, ci);
                result = new byte[jpgStream.Length];
                jpgStream.Seek(0, SeekOrigin.Begin);
                jpgStream.Read(result, 0, (int)jpgStream.Length);

                File.WriteAllBytes(fullPath, result);
            }

            return result;
        }
    }
}
