using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSurvey3.Web
{
    public partial class Surveys : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            SurveyEntitiesContainer context = new SurveyEntitiesContainer();
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            List<SurveyUser> su = context.SurveyUsers.Where(u => u.user_name == user).ToList();

            if (su.Count==0)
            {
                Page.Response.Redirect("/AccessDenied.aspx?" + user);
            }
        }


    }
}