using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;

namespace MagmaLightWeb.Feedback
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

    }
}
