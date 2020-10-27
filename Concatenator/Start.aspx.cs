using System;
using System.Web;

namespace Concatenator
{
    public partial class Start : System.Web.UI.Page
    {
        public string loggedInUser = string.Empty;
        User user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            loggedInUser = HttpContext.Current.User.Identity.Name;
            DbHelper.InsertLog("Identity: " + loggedInUser);

#if DEBUG
            loggedInUser = "ohg\\bkhuddus";
#endif
            user = DbHelper.CheckUser(loggedInUser.ToLower());

            DbHelper.InsertLog("UserName: " + user.UserName);

            if (string.IsNullOrEmpty(loggedInUser) || user.ID == 0)
            {
                Response.Redirect("NotAuthorise.html");
            }
            loggedInUser = user.FullName;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            var result = DbHelper.RunLoad(user);
            Label1.Visible = true;
            Label1.Text = result;
        }
    }
}
