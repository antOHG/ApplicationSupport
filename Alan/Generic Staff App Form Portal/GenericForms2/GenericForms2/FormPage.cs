using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{
    public class FormPage : System.Web.UI.Page
    {
        public FormPage(GenericForm frm) : base()
        {
            foreach(GenericForm.FormField fld in frm.FormFields)
            {
                if (fld.DataType != GenericForm.FieldDataType.Type_divider)
                {
                    Controls.Add(new Label() { Text = fld.Description });
                }
            }
        }
    }
}