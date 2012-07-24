using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;
using MagmaLightWeb.Common;
using System.IO;

namespace MagmaLightWeb
{
    public partial class Index : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string langISO = Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName;
            string mappedFilename = Server.MapPath("./images/pic_Landing_"+langISO+".png");

            if (File.Exists(mappedFilename))
            {
                divLandingPic.Style.Add(HtmlTextWriterStyle.BackgroundImage, "./images/pic_Landing_" + langISO + ".png");
            }

            // This must be done with IIS 7 URLRewrite (1.1)
            //if ((Request.ServerVariables["SERVER_NAME"].ToLower().IndexOf("magmalight.com") >= 0) || (Request.ServerVariables["SERVER_NAME"].ToLower().IndexOf("magmalights.com") >= 0))
            //{
            //    string redirectURL = "http://www.naplampa.hu" + Request.ServerVariables["URL"];
            //    if (!String.IsNullOrEmpty(Request.ServerVariables["QUERY_STRING"])) redirectURL += "?" + Request.ServerVariables["QUERY_STRING"];
            //    Response.Redirect(redirectURL);
            //}
        }
    }
}