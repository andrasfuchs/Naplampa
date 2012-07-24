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
using MagmaLightWeb.NaplampaService;

namespace MagmaLightWeb.Billing
{
    public partial class OrderInfo : System.Web.UI.Page
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

        protected override void OnPreLoad(EventArgs e)
        {
            string passCode = Context.Request.QueryString["PassCode"];

            lblError.Visible = true;
            pnlDetails.Visible = false;


            MagmaLightWeb.NaplampaService.Order order = (MagmaLightWeb.NaplampaService.Order)Session["Order"];
            
            if (order != null)
            {
                if (order.PassCode != passCode) return;

                lblError.Visible = false;
                pnlDetails.Visible = true;

                string orderStatus = Resources.OrderStatuses.ResourceManager.GetString("ORDERSTATUS_" + order.OrderStatus);
                if (String.IsNullOrEmpty(orderStatus))
                {
                    orderStatus = Resources.OrderStatuses.ResourceManager.GetString("ORDERSTATUS_UNKNOWN") + " (" + order.OrderStatus + ")";
                }

                lblOrderIdValue.Text = order.OrderId.ToString();
                lblStatusText.Text = orderStatus;
                lblTitleValue.Text = order.Partner.ContactPerson.Title;
                lblFirstNameValue.Text = order.Partner.ContactPerson.FirstName;
                lblLastNameValue.Text = order.Partner.ContactPerson.LastName;

                lblPaymentMethodValue.Text = Resources.PaymentMethods.ResourceManager.GetString("PAYMENTMETHOD_" + order.PaymentMethod);

                lblAddressNameValue.Text = order.DeliveryAddress.Name;
                lblAddressLineValue.Text = order.DeliveryAddress.AddressLine;
                lblTownValue.Text = order.DeliveryAddress.Town;
                lblPostalCodeValue.Text = order.DeliveryAddress.PostalCode;
                lblCountryValue.Text = Resources.Countries.ResourceManager.GetString(order.DeliveryAddress.Country.Name);

                CacheManager cm = new CacheManager(this.Cache, this.Session);
                Product[] products = cm.CheckAndLoadProducts();

                // fill the product descriptions
                int prodES004Count = SetProduct(1, products.First<Product>(p => p.Code == "ES004"));
                int prodES002Count = SetProduct(2, products.First<Product>(p => p.Code == "ES002"));
                int prodES015Count = SetProduct(3, products.First<Product>(p => p.Code == "ES015"));
                int prodES002WCount = SetProduct(4, products.First<Product>(p => p.Code == "ES002W"));
                int prodES015WCount = SetProduct(5, products.First<Product>(p => p.Code == "ES015W"));

                foreach (OrderProductXRef opx in order.OrderProductXRefs)
                {
                    switch (opx.Product.Code)
                    {
                        case "ES004":
                            txtProduct1.Text = opx.Quantity.ToString();
                            break;
                        case "ES002":
                            txtProduct2.Text = opx.Quantity.ToString();
                            break;
                        case "ES015":
                            txtProduct3.Text = opx.Quantity.ToString();
                            break;
                        case "ES002W":
                            txtProduct4.Text = opx.Quantity.ToString();
                            break;
                        case "ES015W":
                            txtProduct5.Text = opx.Quantity.ToString();
                            break;
                    }
                }

                lblProductsSumText.Text = PriceFormatter.Format(order.ProductsCost, order.Currency);
                lblPostCostText.Text = PriceFormatter.Format(order.SendingCost, order.Currency);
                lblPackageCostText.Text = PriceFormatter.Format(order.PackageCost, order.Currency);
                lblTransactionCostText.Text = PriceFormatter.Format(order.TransactionCost, order.Currency);
                lblQuantityDiscountText.Text = (order.QuantityDiscount > 0 ? "-" : "") + PriceFormatter.Format(order.QuantityDiscount, order.Currency);
                lblCouponDiscountText.Text = (order.CouponDiscount > 0 ? "-" : "") + PriceFormatter.Format(order.CouponDiscount, order.Currency);
                lblTotalCostsText.Text = PriceFormatter.Format(order.OrderTotal, order.Currency);
                
                Invoice lastInvoice = order.Invoices.OrderBy(i => i.DateOfInvoice).Last(); 
                ViewState["LastInvoice"] = lastInvoice;
                //if ((lastInvoice == null) || ((order.OrderStatus & 16) != 16)) btnInvoice.Enabled = false;
            }
        }

        private int SetProduct(int index, Product product)
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
                Int32.TryParse(textBox.Text, out result);
                textBox.Text = result.ToString();
            }
            return result;
        }

        protected void btnInvoice_OnClick(object sender, EventArgs e)
        {
            Invoice invoice = (Invoice)ViewState["LastInvoice"];

            Response.Redirect("ShowInvoice.aspx?InvoiceID=" + invoice.InvoiceId + "&PassCode=" + invoice.PassCode + "&Language=" + invoice.Order.Partner.ContactPerson.CultureName);
        }
    }
}