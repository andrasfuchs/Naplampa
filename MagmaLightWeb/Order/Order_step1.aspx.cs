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
using MagmaLightWeb.Common;
using System.Resources;
using MagmaLightWeb.NaplampaService;
using System.Collections.Generic;
using System.Globalization;

namespace MagmaLightWeb.Order
{
    public partial class Order1 : System.Web.UI.Page
    {
        private Country deliveryCountry = null;

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            // Do the calculations
            ProductQuantity[] basket = (ProductQuantity[])Session["Basket"];
            if ((basket == null) || (basket.Length == 0)) return;

            short paymentMethod = Int16.Parse(rblPayment.SelectedValue);
            Session["PaymentMethod"] = paymentMethod;

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            Country[] countryList = cm.CheckAndLoadCountries();
            Session["DeliveryCountry"] = countryList.First(c => c.CountryId == Int32.Parse(ddlCountry.SelectedValue));

            Session["Basket"] = basket;

            Session["InvoicingCurrency"] = Int32.Parse(ddlCurrency.SelectedValue);

            Session["CouponCode"] = txtCoupon.Text.ToUpper();
            
            Response.Redirect("Order_step2.aspx");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!String.IsNullOrEmpty(this.Request["CouponCode"])) Session["CouponCode"] = this.Request["CouponCode"];
            if (!String.IsNullOrEmpty(Session["CouponCode"] as string))
            {
                TextBox textBox = (TextBox)pnlProducts.FindControl("txtCoupon");
                textBox.Text = Session["CouponCode"] as string;
            }

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            Country[] countryList = cm.CheckAndLoadCountries();

            if (!Page.IsPostBack)
            {
                ddlCountry.DataSource = countryList;
                ddlCountry.DataBind();
            }

            deliveryCountry = countryList.First(c => c.CountryId == Int32.Parse(ddlCountry.SelectedValue));

            Currency[] currencyList = cm.CheckAndLoadCurrencies(deliveryCountry.Currency.ISO);
            Product[] products = cm.CheckAndLoadProducts();

            // fill the product descriptions
            int prodES004Count = SetProduct(1, products.First<Product>(p => p.Code == "ES004"), Session["LastViewedProductCode"] as string == "ES004" ? -1 : -1);
            int prodES002Count = SetProduct(2, products.First<Product>(p => p.Code == "ES002"), Session["LastViewedProductCode"] as string == "ES002" ? 1 : -1);
            int prodES015Count = SetProduct(3, products.First<Product>(p => p.Code == "ES015"), Session["LastViewedProductCode"] as string == "ES015" ? -1 : -1);
            int prodES002WCount = SetProduct(4, products.First<Product>(p => p.Code == "ES002W"), Session["LastViewedProductCode"] as string == "ES002W" ? 1 : -1);
            int prodES015WCount = SetProduct(5, products.First<Product>(p => p.Code == "ES015W"), Session["LastViewedProductCode"] as string == "ES015W" ? -1 : -1);
            Session["LastViewedProductCode"] = null;

            // calculate price
            if (!Page.IsPostBack)
            {
                ddlCurrency.DataSource = currencyList;
                ddlCurrency.DataBind();
            }

            int currencyId = Int32.Parse(ddlCurrency.SelectedValue);
            Currency curr = currencyList.First(c => c.CurrencyId == currencyId);

            List<ProductQuantity> basket = new List<ProductQuantity>();
            if (prodES004Count > 0) basket.Add(new ProductQuantity() { ProductId = products.First<Product>(p => p.Code == "ES004").ProductId, Quantity = prodES004Count });
            if (prodES002Count > 0) basket.Add(new ProductQuantity() { ProductId = products.First<Product>(p => p.Code == "ES002").ProductId, Quantity = prodES002Count });
            if (prodES015Count > 0) basket.Add(new ProductQuantity() { ProductId = products.First<Product>(p => p.Code == "ES015").ProductId, Quantity = prodES015Count });
            if (prodES002WCount > 0) basket.Add(new ProductQuantity() { ProductId = products.First<Product>(p => p.Code == "ES002W").ProductId, Quantity = prodES002WCount });
            if (prodES015WCount > 0) basket.Add(new ProductQuantity() { ProductId = products.First<Product>(p => p.Code == "ES015W").ProductId, Quantity = prodES015WCount });
            Session["Basket"] = basket.ToArray();

            OrderCosts orderCosts = new OrderCosts();
            if (basket.Count > 0)
            {
                orderCosts = cm.CheckAndLoadOrderCosts(null, deliveryCountry.CountryId, Int16.Parse(rblPayment.SelectedValue), basket.ToArray(), currencyId, txtCoupon.Text.ToUpper());
            }

            lblPackageWeightText.Text = orderCosts.TotalWeight.ToString("0") + "g";
            lblProductsSumText.Text = PriceFormatter.Format(orderCosts.ProductCost, curr);
            lblTransactionCostText.Text = PriceFormatter.Format(orderCosts.TransactionCost, curr);
            lblPostCostText.Text = PriceFormatter.Format(orderCosts.SendingCost, curr);
            lblPackageCostText.Text = PriceFormatter.Format(orderCosts.PackageCost, curr);
            lblQuantityDiscountText.Text = (orderCosts.QuantityDiscount > 0 ? "-" : "") + PriceFormatter.Format(orderCosts.QuantityDiscount, curr);
            lblCouponDiscountText.Text = (orderCosts.CouponDiscount > 0 ? "-" : "") + PriceFormatter.Format(orderCosts.CouponDiscount, curr);

            lblTotalCostsText.Text = PriceFormatter.Format(orderCosts.Total, curr);

            // payment methods filter
            string[] allowedPaymentMethods = deliveryCountry.AvailablePaymentMethods.Split(new char[] { ',' });

            foreach (ListItem li in rblPayment.Items)
            {
                li.Enabled = false;
            }

            bool anythingSelected = false;
            foreach (string paymentId in allowedPaymentMethods)
            {
                ListItem li = rblPayment.Items.FindByValue(paymentId);
                if (li != null) li.Enabled = true;
                if (!anythingSelected)
                {
                    li.Selected = true;
                    anythingSelected = true;
                }
            }

            ((MasterPage)this.Master).DisableCountryDropDown();
        }



        private int SetProduct(int index, Product product, int count = -1)
        {
            int result = 0;

            Image image = (Image)pnlProducts.FindControl("imgProduct" + index);
            if (image != null)
            {
                image.ImageUrl = "../images/" + product.ThumbnailURI;
                image.AlternateText = product.Name;
            }

            Label labelName = (Label)pnlProducts.FindControl("lblProduct" + index);
            if (labelName != null)
            {
                labelName.Text = product.Name + " (" + Resources.Products.ResourceManager.GetString("LOC_" + product.Code + "_ONELINER") + ")";
            }

            TextBox textBox = (TextBox)pnlProducts.FindControl("txtProduct" + index);
            if (textBox != null)
            {
                if (count != -1) textBox.Text = count.ToString();

                Int32.TryParse(textBox.Text, out result);
                textBox.Text = result.ToString();
            }

            return result;
        }



        public void btnProd1Minus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct1.Text, out i);
            if (i > 0) txtProduct1.Text = (i - 1).ToString();
        }
        public void btnProd1Plus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct1.Text, out i);
            txtProduct1.Text = (i + 1).ToString();
        }

        public void btnProd2Minus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct2.Text, out i);
            if (i > 0) txtProduct2.Text = (i - 1).ToString();
        }
        public void btnProd2Plus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct2.Text, out i);
            txtProduct2.Text = (i + 1).ToString();
        }

        public void btnProd3Minus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct3.Text, out i);
            if (i > 0) txtProduct3.Text = (i - 1).ToString();
        }
        public void btnProd3Plus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct3.Text, out i);
            txtProduct3.Text = (i + 1).ToString();
        }

        public void btnProd4Minus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct4.Text, out i);
            if (i > 0) txtProduct4.Text = (i - 1).ToString();
        }
        public void btnProd4Plus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct4.Text, out i);
            txtProduct4.Text = (i + 1).ToString();
        }

        public void btnProd5Minus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct5.Text, out i);
            if (i > 0) txtProduct5.Text = (i - 1).ToString();
        }
        public void btnProd5Plus_Click(object sender, EventArgs e)
        {
            int i = -1;
            Int32.TryParse(txtProduct5.Text, out i);
            txtProduct5.Text = (i + 1).ToString();
        }
    }
}
