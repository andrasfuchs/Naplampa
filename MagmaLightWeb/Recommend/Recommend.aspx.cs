using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;
using System.Threading;
using System.Globalization;

namespace MagmaLightWeb.Recommend
{
    public partial class Recommend : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);

            string orderIdStr = Context.Request.QueryString["OrderID"];
            int orderId = -1;

            if (orderIdStr == null) return;
            if (!Int32.TryParse(orderIdStr, out orderId)) return;

            MagmaLightWeb.NaplampaService.Order order = ServiceManager.NaplampaService.GetOrder(orderId);

            Session["Order"] = order;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(order.Partner.ContactPerson.CultureName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(order.Partner.ContactPerson.CultureName);

        }

        protected override void OnPreRender(EventArgs e)
        {
            MagmaLightWeb.NaplampaService.Order order = (MagmaLightWeb.NaplampaService.Order)Session["Order"];
            if (order != null) txtSignature.Text = order.Partner.ContactPerson.FirstName;

            base.OnPreRender(e);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            MagmaLightWeb.NaplampaService.Order order = (MagmaLightWeb.NaplampaService.Order)Session["Order"];
            List<string> recipients = new List<string>();

            for (int i = 1; i < 10; i++)
            {
                TextBox txt = (TextBox)pnlMessage.FindControl("txtRecommendTo" + i.ToString());
                if ((txt != null) && (!String.IsNullOrEmpty(txt.Text)))
                {
                    recipients.Add(txt.Text);
                }
            }

            string message = txtMessage.Text + Environment.NewLine + txtSignature.Text + " [www.magmalight.com]";

            ServiceManager.NaplampaService.SendRecommendation((order == null ? (int?)null : order.OrderId), recipients.ToArray(), message);

            pnlMessage.Visible = false;
            lblThankYou.Visible = true;
        }
    }
}
