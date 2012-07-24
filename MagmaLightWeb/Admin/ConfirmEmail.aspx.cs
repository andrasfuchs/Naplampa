using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;

namespace MagmaLightWeb.Admin
{
    public partial class ConfirmEmail : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            string email = Context.Request.QueryString["Email"];
            string confirmationCode = Context.Request.QueryString["ConfirmationCode"];

            lblError.Visible = true;
            lblAlready.Visible = false;
            lblSuccess.Visible = false;

            int errorCode = ServiceManager.NaplampaService.ConfirmEmail(email, confirmationCode);

            if (errorCode == 0)
            {
                lblError.Visible = false;
                lblAlready.Visible = false;
                lblSuccess.Visible = true;
            }
            else if (errorCode == 2)
            {
                lblError.Visible = false;
                lblAlready.Visible = true;
                lblSuccess.Visible = false;
            }
        }
    }
}
