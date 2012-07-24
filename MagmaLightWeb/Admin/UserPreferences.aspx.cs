using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;

namespace MagmaLightWeb.Admin
{
    public partial class UserPreferences : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            CultureInitializer.InitializeCulture(this.Request, this.Response, this.Session);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string personIdStr = Context.Request.QueryString["PersonId"];
            string confirmationCode = Context.Request.QueryString["ConfirmationCode"];

            if (!this.IsPostBack)
            {
                lblError.Visible = true;
                lblEmailNotConfirmed.Visible = false;
                pnlPreferences.Visible = false;
                lblSuccess.Visible = false;
            }

            int personId = -1;

            if (personIdStr == null) return;
            if (!Int32.TryParse(personIdStr, out personId)) return;


            ViewState["PersonId"] = personId;

            Person person = ServiceManager.NaplampaService.GetPersonById(personId);

            if (!this.IsPostBack)
            {
                KeyValuePair<string, string>[] supportedLanguages = Resources.Languages.ResourceManager.ToArray("LANG_").ToArray();
                for (int i=0; i<supportedLanguages.Length; i++)
                {
                    supportedLanguages[i] = new KeyValuePair<string, string>(supportedLanguages[i].Key.Replace('_', '-'), supportedLanguages[i].Value);
                }

                ddlLanguages.DataSource = supportedLanguages;
                ddlLanguages.DataBind();
            }


            if ((person == null) || (person.EmailConfirmationCode != confirmationCode)) return;

            lblEmailNotConfirmed.Text = String.Format(lblEmailNotConfirmed.Text, person.Email);
            if (!person.EmailConfirmed.HasValue)
            {
                lblError.Visible = false;
                lblEmailNotConfirmed.Visible = true;
                ServiceManager.NaplampaService.SendEmail("CONFIRMEMAIL", person.CultureName, person.Email, new object[] { person.FirstName, person.Email, person.EmailConfirmationCode, person.PersonId });
                return;
            }

            lblEmailText.Text = person.Email;
            txtTitle.Text = person.Title;
            txtFirstName.Text = person.FirstName;
            txtLastName.Text = person.LastName;
            txtPhone.Text = person.Base.Phone;
            txtFax.Text = person.Base.Fax;
            ddlLanguages.SelectedValue = person.CultureName;
            chkNewsletter.Checked = person.Newsletter;

            if (!this.IsPostBack)
            {
                lblError.Visible = false;
                lblEmailNotConfirmed.Visible = false;
                pnlPreferences.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int personId = (int)ViewState["PersonId"];

            int errorCode = ServiceManager.NaplampaService.UpdateUserPreferences(personId, txtTitle.Text, txtFirstName.Text, txtLastName.Text, null, txtPhone.Text, txtFax.Text, chkNewsletter.Checked, ddlLanguages.SelectedValue);

            if (errorCode == 0)
            {
                pnlPreferences.Visible = false;
                lblSuccess.Visible = true;
            }
            else
            {
                lblError.Visible = true;
            }
        }
    }
}
