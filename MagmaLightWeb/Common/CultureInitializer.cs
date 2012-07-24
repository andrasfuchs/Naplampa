using System;
using System.Data;
using System.Configuration;
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
using System.Web.SessionState;
using System.Net;
using System.IO;

namespace MagmaLightWeb.Common
{
    public static class CultureInitializer
    {
        public static string InitializeCulture(HttpRequest request, HttpResponse response, HttpSessionState session)
        {
            //string cultureName = request["Language"];
            string cultureName = "hu-HU";

            if (String.IsNullOrEmpty(cultureName) && (session != null) && (session["SelectedCulture"] != null))
            {
                cultureName = session["SelectedCulture"] as string;
            }

            if (String.IsNullOrEmpty(cultureName) && (request.Cookies["SelectedCulture"] != null))
            {
                cultureName = request.Cookies["SelectedCulture"].Value;
            }

            if (String.IsNullOrEmpty(cultureName))
            {
                HttpWebRequest req = null;
                HttpWebResponse resp = null;

                string publicIpUrl = "http://repeater.smartftp.com";

                try
                {
                    string ipCountryISO = "XX";

                    // get our public IP
                    req = (HttpWebRequest)HttpWebRequest.Create(publicIpUrl);
                    resp = (HttpWebResponse)req.GetResponse();
                    StreamReader strmReader = new StreamReader(resp.GetResponseStream());
                    string publicIp = strmReader.ReadToEnd().Trim();
                    resp.Close();

                    string requestUrl = "http://api.hostip.info/country.php?ip=" + publicIp;
                    // get the IP's country
                    req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                    resp = (HttpWebResponse)req.GetResponse();
                    strmReader = new StreamReader(resp.GetResponseStream());
                    ipCountryISO = strmReader.ReadToEnd().Trim();

                    // check the country's default culture
                    CacheManager cm = new CacheManager(null, session);
                    MagmaLightWeb.NaplampaService.Country[] countries = ServiceManager.NaplampaService.ListCountries();

                    MagmaLightWeb.NaplampaService.Country ipCountry = countries.FirstOrDefault(c => c.ISO == ipCountryISO);

                    if (ipCountry != null) cultureName = ipCountry.DefaultCultureName;
                }
                catch { }
                finally
                {
                    if (req != null)
                    {
                        req = null;
                    }

                    if (resp != null)
                    {
                        resp.Close();
                        resp = null;
                    }
                }
            }

            if (String.IsNullOrEmpty(cultureName) && (request != null) && (request.UserLanguages != null) && (request.UserLanguages.Length > 0) && (request.UserLanguages[0] != null))
            {
                cultureName = request.UserLanguages[0];
                if (cultureName.IndexOf(';') > 0) cultureName = cultureName.Substring(0, cultureName.IndexOf(';'));
            }

            if (String.IsNullOrEmpty(cultureName))
            {
                cultureName = Thread.CurrentThread.CurrentCulture.Name;
            }

            if ("en-US,en-GB,en-IE,hu-HU,de-DE,de-AT,de-CH,de-LU,fr-FR,fr-BE,fr-CH,it-IT,it-CH,es-ES".IndexOf(cultureName, StringComparison.InvariantCultureIgnoreCase) == -1) cultureName = "en-GB";



            if (Thread.CurrentThread.CurrentCulture.Name != cultureName)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            }


            if (response.Cookies["SelectedCulture"] == null)
            {
                HttpCookie cookie = new HttpCookie("SelectedCulture");
                response.Cookies.Add(cookie);
            }

            if (request.Cookies["SelectedCulture"].Value != cultureName)
            {
                response.Cookies["SelectedCulture"].Expires = DateTime.Now.AddDays(21);
            }
            response.Cookies["SelectedCulture"].Value = cultureName;
            response.Cookies.Set(response.Cookies["SelectedCulture"]);


            if ((session != null) && (((string)session["SelectedCulture"]) != cultureName))
            {
                // remove culture specific entries
                CacheManager cm = new CacheManager(null, session);
                cm.CultureChanged();

                session["SelectedCulture"] = cultureName;
            }

            return cultureName;
        }
    }
}