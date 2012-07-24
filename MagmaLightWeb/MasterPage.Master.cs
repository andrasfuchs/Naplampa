using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.NaplampaService;
using System.Threading;
using MagmaLightWeb.Common;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace MagmaLightWeb
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string cultureName = CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
            InitializeMeta();

            string countryISO = cultureName.Substring(cultureName.IndexOf('-') + 1);
            Country selectedCountry = null;

            DropDownList ddlCountries = (DropDownList)this.FindControl("ddlCountries");

            CacheManager cm = new CacheManager(this.Cache, this.Session);
            if (Session["Country"] == null)
            {
                Country[] countryList = cm.CheckAndLoadCountries();

                Session["Country"] = countryList[0];
            }

            selectedCountry = (Country)Session["Country"];

            if (!Page.IsPostBack)
            {
                ddlCountries.DataSource = cm.CheckAndLoadCountries();
                ddlCountries.DataBind();

                ddlCountries.SelectedValue = selectedCountry.CountryId.ToString();
                ddlCountries_SelectedIndexChanged(this, new EventArgs());
            }

            if (selectedCountry.CountryId != Int32.Parse(ddlCountries.SelectedValue))
            {
                ddlCountries_SelectedIndexChanged(this, new EventArgs());
                selectedCountry = (Country)Session["Country"];
            }

            // =================================

            List<string> languagesToDisplay = new List<string>();

            AddLanguageToList(selectedCountry.DefaultCultureName, languagesToDisplay);

            foreach (string cn in selectedCountry.AdditionalCultureNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AddLanguageToList(cn, languagesToDisplay);
            }
            
            AddLanguageToList("en-GB", languagesToDisplay);
            AddLanguageToList("de-DE", languagesToDisplay);
            AddLanguageToList("fr-FR", languagesToDisplay);
            AddLanguageToList("es-ES", languagesToDisplay);
            AddLanguageToList("it-IT", languagesToDisplay);
            AddLanguageToList("hu-HU", languagesToDisplay);

            string baseUri = Page.Request.Url.AbsoluteUri.Substring(0, Page.Request.Url.AbsoluteUri.Length - Page.Request.Url.Query.Length);

            for (int i = 1; i < 5; i++)
            {
                HyperLink lnkLang = (HyperLink)this.FindControl("lnkLang" + i.ToString("00"));

                if (languagesToDisplay.Count < i)
                {
                    lnkLang.Visible = false;
                }
                else
                {
                    lnkLang.Visible = true;
                    lnkLang.Text = languagesToDisplay[i - 1].Substring(0, 2).ToUpper();
                    lnkLang.ToolTip = new CultureInfo(languagesToDisplay[i - 1]).Parent.DisplayName;
                    lnkLang.NavigateUrl = baseUri + "?Language=" + languagesToDisplay[i - 1];
                    foreach (string key in Page.Request.QueryString.AllKeys)
                    {
                        if (key == null) continue;
                        if (key.ToLower() == "language") continue;
                        lnkLang.NavigateUrl += "&" + key + "=" + Page.Request.QueryString[key];
                    }
                }
            }

            // =================================

            // Check browser version
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string updateURL = "";

            if ((browser.Browser == "Firefox") && ((browser.MajorVersion < 3) || (browser.MajorVersion == 3) && (browser.MinorVersion < 0.5)))
            {
                pnlOldBrowser.Visible = true;
                updateURL = "http://www.mozilla.com/firefox/";
            }
            
            if ((browser.Browser == "IE") && (browser.MajorVersion < 8))
            {
                pnlOldBrowser.Visible = true;
                updateURL = "http://www.microsoft.com/windows/Internet-explorer/default.aspx";
            }

            if ((browser.Browser == "Opera") && ((browser.MajorVersion < 9) || (browser.MajorVersion == 9) && (browser.MinorVersion < 0.8)))
            {
                pnlOldBrowser.Visible = true;
                updateURL = "http://www.opera.com/download/";
            }

            if ((browser.Browser == "AppleMAC-Safari") && (browser.MajorVersion < 4))
            {
                pnlOldBrowser.Visible = true;
                updateURL = "http://www.apple.com/safari/download/";
            }

            lblOldBrowser.Text = String.Format(lblOldBrowser.Text, browser.Browser + " " + browser.Version, updateURL);
        }

        protected void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CurrencyList"] = null;

            CacheManager cm = new CacheManager(this.Cache, this.Session);

            Country country = cm.CheckAndLoadCountries().First(c => c.CountryId.ToString() == ddlCountries.SelectedValue);
            Currency[] currencyList = cm.CheckAndLoadCurrencies(country.Currency.ISO);

            Session["Country"] = country;
            Session["CountryList"] = null;
            Session["Currency"] = currencyList[0];

            
            // we need this for the output caching
            if (Response.Cookies["CountryId"] == null)
            {
                HttpCookie cookie = new HttpCookie("CountryId");
                Response.Cookies.Add(cookie);
            }

            Response.Cookies["CountryId"].Expires = DateTime.Now.AddDays(1);
            Response.Cookies["CountryId"].Value = country.CountryId.ToString();
            Response.Cookies.Set(Request.Cookies["CountryId"]);
        }

        private void AddLanguageToList(string cultureName, List<string> languageList)
        {
            string currentCulture = (Thread.CurrentThread.CurrentUICulture.Name.Length >= 3 ? Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 3) : Thread.CurrentThread.CurrentUICulture.Name);
            string cultureNameSub = (cultureName.Length >= 3 ? cultureName.Substring(0, 3) : cultureName);

            if ((languageList.Count < 3)
                && (!languageList.Any(cn => cn.StartsWith(cultureName.Substring(0, 2))))
                && (currentCulture != cultureNameSub)) languageList.Add(cultureName);
        }

        public void DisableCountryDropDown()
        {
            ddlCountries.Enabled = false;
        }

        protected void InitializeMeta()
        {
            string keywords = this.GetLocalResourceObject("meta.Keywords") as string;
            if (!String.IsNullOrEmpty(keywords))
            {
                HtmlMeta metaKeywords = new HtmlMeta();
                metaKeywords.Name = "keywords";
                metaKeywords.Content = keywords;
                this.head1.Controls.Add(metaKeywords);
            }

            string description = this.GetLocalResourceObject("meta.Description") as string;
            if (!String.IsNullOrEmpty(description))
            {
                HtmlMeta metaKeywords = new HtmlMeta();
                metaKeywords.Name = "description";
                metaKeywords.Content = description;
                this.head1.Controls.Add(metaKeywords);
            }
        }

    }
}
