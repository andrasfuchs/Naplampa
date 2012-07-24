using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagmaLightWeb.Common
{
    public partial class ProductPanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetProductButton(1, "ES004");
            SetProductButton(2, "ES002");
            SetProductButton(3, "ES015");
        }

        private void SetProductButton(int index, string code)
        {
            HyperLink link = (HyperLink)pnlProducts.FindControl("lnkProduct" + index);
            link.NavigateUrl = "~/Products/Products.aspx?code=" + code;

            Image image = (Image)pnlProducts.FindControl("imgProduct" + index);
            image.ImageUrl = "~/images/product_" + code + "_mini.png";
            image.AlternateText = code + "_mini";

            Label labelName = (Label)pnlProducts.FindControl("lblProductName" + index);
            labelName.Text = Resources.Products.ResourceManager.GetString("LOC_" + code + "_NAME");

            Label labelOneLiner = (Label)pnlProducts.FindControl("lblProductOneLiner" + index);
            labelOneLiner.Text = Resources.Products.ResourceManager.GetString("LOC_" + code + "_ONELINER");
        }
    }
}