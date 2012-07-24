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
using System.Threading;

namespace MagmaLightWeb.Order
{
    public partial class Order2 : System.Web.UI.Page
    {
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            if (Session["DeliveryCountry"] == null) Response.Redirect("Order_step1.aspx");

            Person customer = new Person() { Email = txtEmail.Text, FirstName = txtFirstName.Text, LastName = txtLastName.Text, Newsletter = chkNewsletter.Checked, CultureName = Thread.CurrentThread.CurrentCulture.Name };
            customer.Base = new Partner() { Phone = txtPhone.Text };
            string referer = txtReferer.Text;


            string fullname = (String.IsNullOrEmpty(customer.Title) ? "" : customer.Title + " ") + customer.FirstName + " " + customer.LastName;

            if (Thread.CurrentThread.CurrentCulture.Name == "hu-HU")
            {
                fullname = (String.IsNullOrEmpty(customer.Title) ? "" : customer.Title + " ") + customer.LastName + " " + customer.FirstName;
            }

            Address deliveryAddress = new Address() { Name = fullname, AddressLine = txtAddressLine.Text, Province = "", Town = txtTown.Text, PostalCode = txtPostalCode.Text };
            int deliveryCountryId = Int32.Parse(ddlCountry.SelectedValue);

            Address invoiceAddress = new Address() { Name = txtInvoiceName.Text, AddressLine = txtInvoiceAddressLine.Text, Province = "", Town = txtInvoiceTown.Text, PostalCode = txtInvoicePostalCode.Text };
            int? invoiceCountryId = Int32.Parse(ddlInvoiceCountry.SelectedValue);

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            Country[] countryList = cm.CheckAndLoadCountries();


            Country deliveryCountry = (Country)Session["DeliveryCountry"];
            ProductQuantity[] basket = (ProductQuantity[])Session["Basket"];
            short paymentMethod = (short)Session["PaymentMethod"];
            int invoicingCurrency = (int)Session["InvoicingCurrency"];
            string couponCode = (string)Session["CouponCode"];

            string invoiceProvince = null;
            string invoiceTown = null;
            string invoicePostalCode = null;
            string invoiceAddressLine = null;
            string invoiceFullName = null;

            if (!String.IsNullOrEmpty(invoiceAddress.AddressLine))
            {
                invoiceProvince = invoiceAddress.Province;
                invoiceTown = invoiceAddress.Town;
                invoicePostalCode = invoiceAddress.PostalCode;
                invoiceAddressLine = invoiceAddress.AddressLine;
                invoiceFullName = invoiceAddress.Name;
            }
            else
            {
                invoiceCountryId = null;
            }


            ValidationResult[] validationResults = ServiceManager.NaplampaService.ValidateOrder((String.IsNullOrEmpty(customer.Title) ? "" : customer.Title), customer.FirstName, customer.LastName, customer.Email, customer.Base.Phone, customer.Newsletter, customer.CultureName,
                deliveryCountry.CountryId, deliveryAddress.Province, deliveryAddress.Town, deliveryAddress.PostalCode, deliveryAddress.AddressLine,
                paymentMethod, referer, basket.ToArray(), invoicingCurrency,
                invoiceCountryId, invoiceProvince, invoiceTown, invoicePostalCode, invoiceAddressLine, invoiceFullName);

            int errorNum = SetValidationErrors(validationResults);
            
            if (errorNum > 0)
            {
                return;
            }

            int orderId = ServiceManager.NaplampaService.CreateNewOrderWithoutLogin(
                (String.IsNullOrEmpty(customer.Title) ? "" : customer.Title), customer.FirstName, customer.LastName, customer.Email, customer.Base.Phone, customer.Newsletter, customer.CultureName,
                deliveryCountry.CountryId, deliveryAddress.Province, deliveryAddress.Town, deliveryAddress.PostalCode, deliveryAddress.AddressLine,
                paymentMethod, referer, basket.ToArray(), invoicingCurrency,
                invoiceCountryId, invoiceProvince, invoiceTown, invoicePostalCode, invoiceAddressLine, invoiceFullName, null, couponCode);

            if ((paymentMethod == 2) || (paymentMethod == 3))
            {
                NaplampaService.Order order = ServiceManager.NaplampaService.GetOrder(orderId);

                if (!String.IsNullOrEmpty(order.PayPalToken))
                {
                    Response.Redirect("https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + order.PayPalToken);
                }
            }
            else
            {
                Response.Redirect("OrderCompleted.aspx?OrderId=" + orderId);
            }
        }

        private int SetValidationErrors(ValidationResult[] results)
        {
            int result = 0;

            HtmlGenericControl div = null;

            foreach (ValidationResult vr in results)
            {
                if (vr.ResultType == ValidationResultType.Error) result++;

                string rootControlName = "div" + vr.PropertyName;
                
                div = (HtmlGenericControl)pnlOrderStep2.FindControl(rootControlName);
                SetDivCssClass(div, vr, false);

                div = (HtmlGenericControl)pnlOrderStep2.FindControl(rootControlName + "Label");
                SetDivCssClass(div, vr, false);

                rootControlName = rootControlName + "_Error";
                div = (HtmlGenericControl)pnlOrderStep2.FindControl(rootControlName + "_" + vr.Type.ToString());
                if (div == null)
                {
                    div = (HtmlGenericControl)pnlOrderStep2.FindControl(rootControlName);
                }
                SetDivCssClass(div, vr, true);

                if ((vr.Parameters != null) && (div.Controls.Count > 0))
                {
                    foreach (Control control in div.Controls)
                    {
                        if (control is Label) ((Label)control).Text = String.Format(((Label)control).Text, vr.Parameters);
                    }
                }
            }

            return result;
        }

        private void SetDivCssClass(HtmlGenericControl div, ValidationResult vr, bool setVisibility)
        {
            if (div != null)
            {
                string value = div.Attributes["class"];

                if (!String.IsNullOrEmpty(value))
                {
                    if ((vr.ResultType == ValidationResultType.Pass) && (value.EndsWith("_error"))) value = value.Substring(0, value.Length - 6);
                    if ((vr.ResultType == ValidationResultType.Error) && (!value.EndsWith("_error"))) value = value + "_error";

                    div.Attributes.Add("class", value);
                    if (setVisibility) div.Visible = (vr.ResultType != ValidationResultType.Pass);
                }
            }
        }


        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Session["DeliveryCountry"] == null) Response.Redirect("Order_step1.aspx");

            if (!Page.IsPostBack)
            {
                CacheManager cm = new CacheManager(this.Cache, this.Session);
                Country[] countryList = cm.CheckAndLoadCountries();

                ddlCountry.DataSource = countryList;
                ddlCountry.DataBind();
                ddlCountry.SelectedValue = ((Country)Session["DeliveryCountry"]).CountryId.ToString();
                ddlCountry.Enabled = false;
                ddlInvoiceCountry.DataSource = ddlCountry.DataSource;
                ddlInvoiceCountry.DataBind();
            }

            ((MasterPage)this.Master).DisableCountryDropDown();
        }
    }
}
