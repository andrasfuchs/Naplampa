using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.NaplampaService;
using System.IO;
using MagmaLightWeb.Common;
using System.Threading;

namespace MagmaLightWeb.Billing
{
    public partial class ShowInvoice : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                string invoiceIdStr = Context.Request.QueryString["InvoiceID"];
                string passCode = Context.Request.QueryString["PassCode"];
                int invoiceId = -1;

                lblError.Visible = true;

                if (invoiceIdStr == null) return;
                if (!Int32.TryParse(invoiceIdStr, out invoiceId)) return;

                Invoice invoice = ServiceManager.NaplampaService.GetInvoice(invoiceId);

                if (invoice != null)
                {
                    if (invoice.PassCode != passCode) return;

                    lblError.Visible = false;

                    byte[] jpegData = new byte[0]; 
                    ServiceManager.NaplampaService.GetInvoiceImage(invoiceId, Thread.CurrentThread.CurrentCulture.Name);

                    Response.ContentType = "image/jpeg";
                    Response.OutputStream.Write(jpegData, 0, jpegData.Length);
                }
        }
    }
}
