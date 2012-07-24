using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;
using System.Threading;

namespace MagmaLightWeb.Contact
{
    public partial class Contact : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Session["Country"] != null)
            {
                int representativePersonId = (int)((Country)Session["Country"]).RepresentativeReference.EntityKey.EntityKeyValues[0].Value;
                Person representative = ServiceManager.NaplampaService.GetPersonById(representativePersonId);

                lblContactNameText.Text = representative.Base.FullName;
                lblContactEmailText.Text = representative.Email.Replace("@", Resources.Common.ResourceManager.GetString("AT")).Replace(".", Resources.Common.ResourceManager.GetString("DOT"));
                lblContactPhoneText.Text = representative.Base.Phone;
                lblContactFaxText.Text = representative.Base.Fax;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ServiceManager.NaplampaService.SendEmail("WEBMAIL", Thread.CurrentThread.CurrentUICulture.ToString(), null, new object[] { txtSenderEmail.Text, txtMessage.Text, DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss") });

            pnlSendMessage.Visible = false;
            lblThankYou.Visible = true;
        }
    }
}
