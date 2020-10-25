using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{
    public partial class GetPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // page to return a pdf in binary
            string formid_char = Request.QueryString["fID"];
            int fID;
            if (int.TryParse(formid_char, out fID))
            {
                byte[] b = DataAccess.Instance.GetPdf(fID);

                if (b != null)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline;filename=\"FileName.pdf\"");
                    Response.BinaryWrite(DataAccess.Instance.GetPdf(fID));

                    Response.Flush();

                    Response.End();
                }
            }
        }
    }
}