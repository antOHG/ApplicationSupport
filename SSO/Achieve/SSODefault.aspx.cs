using System;
using System.Configuration;
using System.Text;
using System.Web.UI;

namespace SSO
{
    public partial class SSODefault : System.Web.UI.Page
    {
        public bool Error { get; set; }
        public string ErrorDescription { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();

            string env = "LIVE";

            string username = Page.User.Identity.Name;
            if (username.Contains("\\"))  //this is to take care of the \ in the OHG email address issue
            {
                username = username.Substring(username.IndexOf("\\") + 1);
            }

            if (Request.QueryString["env"] != null)
            {
                env = Request.QueryString["env"];

            }

            string mail = SSOApp.LDAP.GetEmail(username);

            if (mail == null)
            {
                Response.Write("No email address found for " + username + " Access Denied!");
            }
            else
            {
                Response.Write("Launching Achieve! " + env + " for " + username + "...." + "Please wait.");

                Response.Write(GenerateForm(mail, env));
            }
        }

        public string GenerateForm(string userId, string Environment)
        {
            StringBuilder sbForm = new StringBuilder();
            //get base url and all other URLs
            string acct = ConfigurationManager.AppSettings.Get("acct");
            string ouId = ConfigurationManager.AppSettings.Get("ouId");
            string ssoURL;
            string logoutURL;
            string timeoutURL;
            string errorURL;
            //string destURL;
            string destURL = Request.QueryString["link"];

            ssoURL = ConfigurationManager.AppSettings.Get("baseURL");
            logoutURL = ConfigurationManager.AppSettings.Get("logoutURL");
            timeoutURL = ConfigurationManager.AppSettings.Get("timeoutURL");
            errorURL = ConfigurationManager.AppSettings.Get("errorURL");
            destURL = ConfigurationManager.AppSettings.Get("destURL");

            if (Environment == "LIVE")
            {
                ssoURL = ConfigurationManager.AppSettings.Get("baseURL");
                logoutURL = ConfigurationManager.AppSettings.Get("logoutURL");
                timeoutURL = ConfigurationManager.AppSettings.Get("timeoutURL");
                errorURL = ConfigurationManager.AppSettings.Get("errorURL");
                destURL = ConfigurationManager.AppSettings.Get("destURL");
            }

            if (Environment == "TEST")
            {
                ssoURL = ConfigurationManager.AppSettings.Get("TESTbaseURL");
                logoutURL = ConfigurationManager.AppSettings.Get("TESTlogoutURL");
                timeoutURL = ConfigurationManager.AppSettings.Get("TESTtimeoutURL");
                errorURL = ConfigurationManager.AppSettings.Get("TESTerrorURL");
                destURL = ConfigurationManager.AppSettings.Get("TESTdestURL");
            }

            if (Environment == "UAT")
            {
                ssoURL = ConfigurationManager.AppSettings.Get("UATbaseURL");
                logoutURL = ConfigurationManager.AppSettings.Get("UATlogoutURL");
                timeoutURL = ConfigurationManager.AppSettings.Get("UATtimeoutURL");
                errorURL = ConfigurationManager.AppSettings.Get("UATerrorURL");
                destURL = ConfigurationManager.AppSettings.Get("UATdestURL");
            }




            //get the encrypted token

            string encryptedToken = WCyberu.GetSecurityToken(acct, userId, string.Empty, logoutURL, timeoutURL, errorURL, destURL);
            if (Error)
            {
                if (!string.IsNullOrEmpty(errorURL))
                {

                    sbForm.AppendLine("<html>").AppendFormat("<body onload=\"window.location.href='{0}';\">", errorURL);
                    sbForm.AppendLine("</body>").AppendLine("</html>");
                }
                else
                {
                    sbForm.AppendFormat("<html><body><p>{0}</p></body></html>", ErrorDescription);
                }
            }
            else
            {
                sbForm.AppendLine("<html>").AppendLine("<body onload=\"document.forms[0].submit();\">");
                sbForm.AppendFormat("<form method=\"POST\" action=\"{0}\">", ssoURL + destURL).AppendLine();
                sbForm.AppendFormat("<input type=\"hidden\" name=\"key\" value=\"{0}\"/>", encryptedToken).AppendLine();
                sbForm.AppendFormat("<input type=\"hidden\" name=\"ouid\" value=\"{0}\"/>", ouId);
                sbForm.AppendLine("</form>").AppendLine("</body>").AppendLine("</html>");
            }

            return sbForm.ToString();

        }
    }
}