using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.NaplampaService;
using System.Threading;
using MagmaLightWeb.Common;
using System.Web.Caching;

namespace MagmaLightWeb.Products
{
    public partial class Products : System.Web.UI.Page
    {
        private ProductDiscountPrice[] prices = null;
        private Currency currency = null;
        private Country country = null;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string code = Request.QueryString["code"];
            currency = (Currency)Session["Currency"];
            country = (Country)Session["Country"];
            if (!String.IsNullOrEmpty(this.Request["CouponCode"])) Session["CouponCode"] = this.Request["CouponCode"];

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            Product[] products = cm.CheckAndLoadProducts();
            prices = cm.CheckAndLoadProductPrices(products, currency.CurrencyId, country.CountryId);

             // fill the product descriptions
            Product product = products.FirstOrDefault<Product>(p => p.Code == code);

            if (product == null)
            {
                product = products.FirstOrDefault<Product>(p => p.Code == "ES002");
            }
            Session["LastViewedProductCode"] = product.Code;

            pnlIonizer.Visible = (product.Code != "ES004");

            lblProductName.Text = product.Name;
            lblProductDescription.Text = product.Description;
            
            productCard.Style.Add("background-image", "url(../images/product_" + product.Code + ".jpg)");


            ProductDiscountPrice price = prices.First(p => p.Product.Code == product.Code);
            double multiplier = (price.Discount == null ? 1.0 : price.Discount.Multiplier);

            int daysRemaining = ((price.Discount == null) || (price.Discount.ValidUntil == null) || (price.Discount.ValidUntil.Value > DateTime.Today.AddDays(7)) ? -1 : (int)Math.Ceiling((price.Discount.ValidUntil.Value - DateTime.Today).TotalDays));
            lblSaleRemaining.Text = (daysRemaining > 0 ? String.Format(lblSaleRemaining.Text, daysRemaining) : "");
            lblMSRPPriceText.Text = PriceFormatter.Format(price.MSRP, currency);
            lblProductPriceText.Text = PriceFormatter.Format(price.Price, currency) + " (-" + (100 - (multiplier * 100)).ToString("0") + "%)";


            List<string> otherProducts = new List<string>();
            if (product.Code.ToUpper() != "ES004") otherProducts.Add("ES004");
            if (product.Code.ToUpper() != "ES002") otherProducts.Add("ES002");
            if (product.Code.ToUpper() != "ES015") otherProducts.Add("ES015");

            for (int i = 1; i <= otherProducts.Count; i++)
            {
                SetProductButton(i, otherProducts[i-1]);
            }
        }

        private void SetProductButton(int index, string code)
        {
            HyperLink link = (HyperLink)pnlProducts.FindControl("lnkProduct" + index);
            link.NavigateUrl = "../products/products.aspx?code=" + code;

            Image image = (Image)pnlProducts.FindControl("imgProduct" + index);
            image.ImageUrl = "../images/product_" + code + "_mini.png";
            image.AlternateText = code + "_mini";

            Label labelName = (Label)pnlProducts.FindControl("lblProductName" + index);
            labelName.Text = Resources.Products.ResourceManager.GetString("LOC_" + code + "_NAME");

            Label labelOneLiner = (Label)pnlProducts.FindControl("lblProductOneLiner" + index);
            labelOneLiner.Text = Resources.Products.ResourceManager.GetString("LOC_" + code + "_ONELINER");
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected void redirectToOrder1(object sender, EventArgs e)
        {
            Response.Redirect("../order/order_step1.aspx", true);
        }
    }
}
