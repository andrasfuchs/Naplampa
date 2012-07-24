using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;

namespace MagmaLightWeb.Order
{
    public partial class OrderReview : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string payPalToken = Context.Request.QueryString["Token"];
            string payPalPayerId = Context.Request.QueryString["PayerID"];
            string orderIdStr = Context.Request.QueryString["OrderID"];
            int orderId = -1;

            if (orderIdStr == null) return;
            if (!Int32.TryParse(orderIdStr, out orderId)) return;

            NaplampaService.OrderReview orderReview = ServiceManager.NaplampaService.GetOrderReview(orderId, payPalToken);
            bool paypalSuccess = ServiceManager.NaplampaService.ConfirmOrder(orderId, payPalToken, payPalPayerId);

            Response.Redirect("OrderCompleted.aspx?PayPalSuccess=" + paypalSuccess + "&OrderID=" + orderId + "&Token=" + payPalToken);
        }
    }
}
