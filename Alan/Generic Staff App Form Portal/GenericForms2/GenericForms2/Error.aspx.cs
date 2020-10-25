using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errmsg = Request.Form["ErrMsg"];
            if (errmsg != null)
            {
                //Encode string
                Literal1.Text = HttpUtility.HtmlEncode(errmsg);
                string user = HttpContext.Current.User.Identity.Name;

                // Write error message to the log
                Tracing.WriteLine("Errors reported to user " + user + ": " + errmsg);
            }
        }
    }
}