using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace SSO
{
    public partial class SSODefault : System.Web.UI.Page
    {
        public bool Error { get; set; }
        public string ErrorDescription { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
           Response.Clear();
            //AT enter Variable for calling sp which refers to AD in IIS there is a variable that stores your user name

           string conn_str =
               ConfigurationManager.ConnectionStrings["DW"].ConnectionString;

           //string username = Environment.UserName;
           string username = "WIBBLE";

           Response.Write("The username = ");
           Response.Write(username + "</BR>");

           SqlConnection conn = new SqlConnection(conn_str);

           conn.Open();
           SqlCommand thisCommand = conn.CreateCommand();
           thisCommand.CommandText = "SELECT TOP 1 USER_EMAIL_ADDRESS FROM ACTIVE_DIRECTORY_USER_DETAIL_VIEW WHERE USER_NAME = '" + username + "'";

           SqlDataReader thisReader = thisCommand.ExecuteReader();

           while (thisReader.Read())
           {
               Response.Write("The username = ");
               Response.Write(thisReader["USER_EMAIL_ADDRESS"] + "</BR>");
               //Response.Write(GenerateForm(thisReader["USER_EMAIL_ADDRESS"].ToString())); //replace $User ID$ part with Login User ID. 
           }

           thisReader.Close();

           string sp = "GET_AD_EMAIL_ADDRESS";

           SqlCommand thisCommand_2 = new SqlCommand(sp, conn);
           thisCommand_2.CommandType = CommandType.StoredProcedure;

           SqlParameter theUserName = thisCommand_2.Parameters.Add("@USER_NAME", SqlDbType.VarChar, 255);

           theUserName.Value = username;

           SqlDataReader TheReader_2 = thisCommand_2.ExecuteReader();

           while (TheReader_2.Read())
           {
               //string nextID = TheReader_2["OrderID"].ToString();
               //string nextSubtotal = TheReader_2["Subtotal"].ToString();
               //orderlist += nextID + '\t' + nextSubtotal + '\n';

               string EmailAddress = TheReader_2["USER_EMAIL_ADDRESS"].ToString() ;
              
               Response.Write(GenerateForm(EmailAddress)); //replace $User ID$ part with Login User ID. 
               //Response.Write(EmailAddress);

           }






           thisReader.Close();
           conn.Close();


           Response.Write(username);



           //Response.Write(GenerateForm("athompson@onehousinggroup.co.uk")); //replace $User ID$ part with Login User ID. 
        }

        public string GenerateForm(string userId)
        {
            StringBuilder sbForm = new StringBuilder();
            //get base url and all other URLs
            string acct = ConfigurationManager.AppSettings.Get("acct");
            string ssoURL = ConfigurationManager.AppSettings.Get("baseURL");
            string ouId = ConfigurationManager.AppSettings.Get("ouId");
            string logoutURL = ConfigurationManager.AppSettings.Get("logoutURL");
            string timeoutURL = ConfigurationManager.AppSettings.Get("timeoutURL");
            string errorURL = ConfigurationManager.AppSettings.Get("errorURL");
            string destURL = ConfigurationManager.AppSettings.Get("destURL");

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
                sbForm.AppendFormat("<form method=\"POST\" action=\"{0}\">", ssoURL).AppendLine();
                sbForm.AppendFormat("<input type=\"hidden\" name=\"key\" value=\"{0}\"/>", encryptedToken).AppendLine();
                sbForm.AppendFormat("<input type=\"hidden\" name=\"ouid\" value=\"{0}\"/>", ouId);
                sbForm.AppendLine("</form>").AppendLine("</body>").AppendLine("</html>");
            }

            return sbForm.ToString();

        }
    }

}