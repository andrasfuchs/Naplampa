using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagmaLightWeb.Common;
using MagmaLightWeb.NaplampaService;
using System.Threading;
using System.Globalization;
using System.Text;

namespace MagmaLightWeb.Admin
{
    public partial class ShowSurvey : System.Web.UI.Page
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
            lblError.Visible = true;
            lblAlready.Visible = false;
            lblExpired.Visible = false;
            lblThankYou.Visible = false;

            string confirmationCode = Context.Request.QueryString["ConfirmationCode"];
            string surveyIdStr = Context.Request.QueryString["SurveyID"];

            int surveyId = -1;

            if (surveyIdStr == null) return;
            if (!Int32.TryParse(surveyIdStr, out surveyId)) return;


            MagmaLightWeb.NaplampaService.Order order = (MagmaLightWeb.NaplampaService.Order)Session["Order"];
            string email = order.Partner.ContactPerson.Email;

            int errorCode = ServiceManager.NaplampaService.ConfirmEmail(email, confirmationCode);
            Survey survey = ServiceManager.NaplampaService.GetSurvey(surveyId);
            Session["Survey"] = survey;

            if ((errorCode == 1) || (order == null)) return;

            if ((survey == null) || (survey.Expires > DateTime.Now))
            {
                lblError.Visible = false;
                lblAlready.Visible = false;
                lblExpired.Visible = true;
                return;
            }

            if (survey.SurveyResults.FirstOrDefault<SurveyResult>(sr => sr.Order.OrderId == order.OrderId) != null)
            {
                lblError.Visible = false;
                lblAlready.Visible = true;
                lblExpired.Visible = false;
                return;
            }

            lblHeader.Text = Resources.Surveys.ResourceManager.GetString(survey.Name);
            lblError.Visible = false;
            lblAlready.Visible = false;
            lblExpired.Visible = false;
            pnlQuestions.Visible = true;

            int k = 1;
            for (int i = 1; i < 10; i++)
            {
                Panel panel = (Panel)pnlQuestions.FindControl("pnlRadio" + k.ToString());
                Label lblQuestion = (Label)pnlQuestions.FindControl("lblRadio" + k.ToString() + "Question");
                RadioButtonList rbList = (RadioButtonList)pnlQuestions.FindControl("rblRadio" + k.ToString());

                if (panel == null) continue;
                if (rbList.Items.Count > 0) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_RDB";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    panel.Visible = true;
                    lblQuestion.Text = question;
                    for (int j = 1; j < 10; j++)
                    {
                        string option = Resources.Surveys.ResourceManager.GetString(rootKey + "_OPT" + j.ToString());
                        if (!String.IsNullOrEmpty(option)) 
                        {
                            rbList.Items.Add(option);
                        }
                    }
                    k++;
                }
                else
                {
                    panel.Visible = false;
                }
            }

            k = 1;
            for (int i = 1; i < 10; i++)
            {
                Panel panel = (Panel)pnlQuestions.FindControl("pnlChk" + k.ToString());
                Label lblQuestion = (Label)pnlQuestions.FindControl("lblChk" + k.ToString() + "Question");
                CheckBoxList cbList = (CheckBoxList)pnlQuestions.FindControl("cblChk" + k.ToString());

                if (panel == null) continue;
                if (cbList.Items.Count > 0) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_CHK";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    panel.Visible = true;
                    lblQuestion.Text = question;
                    for (int j = 1; j < 10; j++)
                    {
                        string option = Resources.Surveys.ResourceManager.GetString(rootKey + "_OPT" + j.ToString());
                        if (!String.IsNullOrEmpty(option))
                        {
                            cbList.Items.Add(option);
                        }
                    }
                    k++;
                }
                else
                {
                    panel.Visible = false;
                }
            }

            k = 1;
            for (int i = 1; i < 10; i++)
            {
                Panel panel = (Panel)pnlQuestions.FindControl("pnlTxt" + k.ToString());
                Label lblQuestion = (Label)pnlQuestions.FindControl("lblTxt" + k.ToString() + "Question");
                TextBox txt = (TextBox)pnlQuestions.FindControl("txtTxt" + k.ToString());

                if (panel == null) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_TXT";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    panel.Visible = true;
                    lblQuestion.Text = question;
                    k++;
                }
                else
                {
                    panel.Visible = false;
                }
            }

        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            string surveyIdStr = Context.Request.QueryString["SurveyID"];

            int surveyId = -1;

            if (surveyIdStr == null) return;
            if (!Int32.TryParse(surveyIdStr, out surveyId)) return;

            MagmaLightWeb.NaplampaService.Order order = (MagmaLightWeb.NaplampaService.Order)Session["Order"];

            StringBuilder questions = new StringBuilder(); 
            
            // index zero is not used, value -1 means 'not used', value 0 means 'not answered'
            int[] resultValue = new int[10];
            string[] resultText = new string[10];
            int[] resultFlags = new int[10];

            for (int i = 0; i < 10; i++)
            {
                resultValue[i] = -1;
                resultText[i] = null;
                resultFlags[i] = -1;
            }

            Survey survey = (Survey)Session["Survey"];


            int k = 1;
            for (int i = 1; i < 10; i++)
            {
                RadioButtonList rbList = (RadioButtonList)pnlQuestions.FindControl("rblRadio" + k.ToString());

                if (rbList == null) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_RDB";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    questions.Append("Int"+k.ToString()+", ");
                    questions.AppendLine(question);
                    int j = 1;
                    foreach (ListItem item in rbList.Items)
                    {
                        if (resultValue[k] == -1) resultValue[k] = 0;
                        if (item.Selected) resultValue[k] = j;
                        questions.Append(item.Text);
                        questions.Append('|');
                        j++;
                    }
                    questions.AppendLine();
                    k++;
                }
            }

            k = 1;
            for (int i = 1; i < 10; i++)
            {
                CheckBoxList cbList = (CheckBoxList)pnlQuestions.FindControl("cblChk" + k.ToString());

                if (cbList == null) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_CHK";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    questions.Append("Flags"+k.ToString()+", ");
                    questions.AppendLine(question);
                    int j = 1;
                    foreach (ListItem item in cbList.Items)
                    {
                        if (resultFlags[k] == -1) resultFlags[k] = 0;
                        if (item.Selected) resultFlags[k] |= j;
                        questions.Append(item.Text);
                        questions.Append('|');
                        j *= 2;
                    }
                    questions.AppendLine();
                    k++;
                }
            }


            k = 1;
            for (int i = 1; i < 10; i++)
            {
                TextBox txt = (TextBox)pnlQuestions.FindControl("txtTxt" + k.ToString());

                if (txt == null) continue;

                string rootKey = survey.Name + "_Q" + i.ToString() + "_TXT";
                string question = Resources.Surveys.ResourceManager.GetString(rootKey);

                if (!String.IsNullOrEmpty(question))
                {
                    questions.Append("Text"+k.ToString()+", ");
                    questions.AppendLine(question);
                    if (!String.IsNullOrEmpty(txt.Text)) resultText[k] = txt.Text;
                    k++;
                }
            }


            //

            ServiceManager.NaplampaService.SaveSurveyResult(surveyId, order.OrderId, Thread.CurrentThread.CurrentCulture.Name, questions.ToString(), 
                resultValue[1], resultValue[2], resultValue[3], resultValue[4], resultValue[5], resultValue[6], resultValue[7], 
                resultText[1], resultText[2], resultText[3], resultText[4], resultText[5], resultText[6], resultText[7],
                resultFlags[1], resultFlags[2], resultFlags[3], resultFlags[4], resultFlags[5], resultFlags[6], resultFlags[7]);
            

            pnlQuestions.Visible = false;
            lblThankYou.Visible = true;

            if ((survey.Name == "SURVEY_AFTERORDER") && ((resultValue[4] == 4) || (resultValue[4] == 5)))
            {
                Response.Redirect("~/Recommend/Recommend.aspx?OrderID=" + order.OrderId.ToString());
            }
        }
    }
}
