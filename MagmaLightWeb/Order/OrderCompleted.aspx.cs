using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using System.Globalization;
using MagmaLightWeb.NaplampaService;
using System.Web.UI.HtmlControls;

namespace MagmaLightWeb.Order
{
    public partial class OrderCompleted : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            string payPalSuccess = Context.Request.QueryString["PayPalSuccess"] == null ? "false" : Context.Request.QueryString["PayPalSuccess"].ToLower();
            string payPalToken = Context.Request.QueryString["Token"];
            string orderIdStr = Context.Request.QueryString["OrderID"];
            int orderId = -1;

            if (orderIdStr == null) return;
            if (!Int32.TryParse(orderIdStr, out orderId)) return;


            MagmaLightWeb.NaplampaService.Order order = ServiceManager.NaplampaService.GetOrder(orderId);

            IFormatProvider format = new CultureInfo("en-US");

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            Currency huf = cm.CheckAndLoadCurrencies(null).FirstOrDefault(c => c.ISO == "HUF");

            // Value is in Hungarian Forints at the moment
            ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$orderID")).Value = order.OrderId.ToString("0");
            ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$orderTotal")).Value = ((order.OrderTotal * (decimal)order.Currency.ConversionToEur) / (decimal)huf.ConversionToEur).ToString("0.00", format);
            Invoice invoice = order.Invoices.OrderBy(i => i.DateOfInvoice).LastOrDefault();
            if (invoice != null) invoice = ServiceManager.NaplampaService.GetInvoice(invoice.InvoiceId);

            InvoiceLine invoiceLine = null;
            if (invoice != null)
            {
                invoiceLine = invoice.InvoiceLines.FirstOrDefault(il => (il.Product != null) && (il.Product.Code == "ES002"));
                if ((invoiceLine != null) && (invoiceLine.Quantity > 0))
                {
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es002Price")).Value = (invoiceLine.UnitPrice * (decimal)invoice.Currency.ConversionToEur).ToString("0.00", format);
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es002Quantity")).Value = invoiceLine.Quantity.ToString("0");
                }
                invoiceLine = invoice.InvoiceLines.FirstOrDefault(il => (il.Product != null) && (il.Product.Code == "ES004"));
                if ((invoiceLine != null) && (invoiceLine.Quantity > 0))
                {
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es004Price")).Value = (invoiceLine.UnitPrice * (decimal)invoice.Currency.ConversionToEur).ToString("0.00", format);
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es004Quantity")).Value = invoiceLine.Quantity.ToString("0");
                }
                invoiceLine = invoice.InvoiceLines.FirstOrDefault(il => (il.Product != null) && (il.Product.Code == "ES015"));
                if ((invoiceLine != null) && (invoiceLine.Quantity > 0))
                {
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es015Price")).Value = (invoiceLine.UnitPrice * (decimal)invoice.Currency.ConversionToEur).ToString("0.00", format);
                    ((HtmlInputHidden)Page.FindControl("ctl00$endOfPage$es015Quantity")).Value = invoiceLine.Quantity.ToString("0");
                }
            }

            lblPayPalError.Visible = (payPalSuccess == "false") && (order.PaymentMethod == 2);

            if (payPalSuccess == "true")
            {
                if (order.PayPalToken == payPalToken)
                {
                    ServiceManager.NaplampaService.OrderPaymentReceived(orderId, payPalToken);
                }
            }
        }
    }
}