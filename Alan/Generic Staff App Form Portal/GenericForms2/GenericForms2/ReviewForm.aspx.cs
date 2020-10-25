using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{
    public partial class ReviewForm : System.Web.UI.Page
    {
        private GenericForm frm;
        private FriendlyError errors = new FriendlyError();
        string referer;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                referer = Request.UrlReferrer.PathAndQuery;

                string formid_char = Request.QueryString["FormId"];
                int formid;
                if (int.TryParse(formid_char, out formid))
                {
                    //Check permissions
                    var user = (WindowsIdentity)HttpContext.Current.User.Identity;
                    string sid = user.User.ToString();
                    if (!DataAccess.Instance.IsPermitted(formid, sid))
                    {
                        Response.Redirect("AccessDenied.aspx");
                    }

                    //Permission granted...
                    frm = new GenericForm(formid);

                    foreach (GenericForm.FormField fld in frm.FormFields)
                    {
                        if (fld.DisplayOnWebForm)
                        {
                            var tr = new TableRow();

                            if (fld.DataType != GenericForm.FieldDataType.Type_divider && fld.DataType != GenericForm.FieldDataType.Type_label)
                            {

                                tr.Cells.Add(new TableCell() { Width = 450, Text = fld.Description });
                                tr.Cells.Add(new TableCell() { Width = 450 });
                                if (fld.DataType == GenericForm.FieldDataType.Type_char || fld.DataType == GenericForm.FieldDataType.Type_lookup)
                                {
                                    int rows = (fld.Value_char.Length / 60) + 1;
                                    if (rows == 0) { rows = 1; }
                                    var tb = new TextBox()
                                    {
                                        Text = fld.Value_char,
                                        ReadOnly = !fld.CanBeModifiedOnWebForm,
                                        Width = 500,
                                        //Height = 20 * rows,
                                        Rows = rows,
                                        MaxLength = fld.MaxLen,
                                        ID = fld.FormFieldInstanceId.ToString(),
                                        TextMode = TextBoxMode.MultiLine,
                                        Wrap = true
                                    };

                                    tb.TextChanged += TextBox_changed;
                                    tr.Cells[1].Controls.Add(tb);
                                }
                                if (fld.DataType == GenericForm.FieldDataType.Type_image && fld.Value_binary != null)
                                {
                                    string fileExt = Utility.GetFileExtension(fld.Value_char, false);
                                    if (fileExt == string.Empty) { fileExt = "jpg"; }
                                    tr.Cells[1].Text = "<img runat=\"server\" id=\"image\" height=250 width=400 src =\"data:image/jpg;base64," + Convert.ToBase64String(fld.Value_binary) + "\" />";
                                }
                            }
                            if (fld.DataType == GenericForm.FieldDataType.Type_label)
                            {
                                tr.Cells.Add(new TableCell() { Height = 20, Width = 450 });
                                tr.Cells.Add(new TableCell() { Height = 20, Width = 450, Text = fld.Description });
                            }
                            if (fld.DataType == GenericForm.FieldDataType.Type_divider)
                            {
                                tr.Cells.Add(new TableCell() { Height = 10, Width = 450 });
                                tr.Cells.Add(new TableCell() { Height = 10, Width = 450 });
                                tr.BackColor = System.Drawing.Color.LightGray;
                            }
                            QATable.Rows.Add(tr);
                        }
                    }
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
            catch (System.Exception ex)
            {
                errors.Add("An error occurred loading the page. [" + Request.Url + "]");
                Tracing.HandleError(ex, Tracing.TracingEventType.AvailableForms);
            }
            finally
            {
                if (errors.ErrorsExist)
                {
                    Response.Clear();
                    Response.Write(FriendlyError.ErrorPost(errors.ErrorMessage, "Error.aspx"));
                    Response.End();
                }
            }
        }

        private void TextBox_changed(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            frm.ChangedFields.Add(Convert.ToInt32(tb.ID), tb.Text);
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    frm.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    errors.Add("Error saving changes to form");
                    throw;
                }
                try
                {
                    string msg = frm.CompleteForm(HttpContext.Current.User.Identity.Name, Properties.Settings.Default.LockTimeoutMinutes);
                    if (msg != string.Empty)
                    {
                        errors.Add(msg);
                        throw new Exception(msg);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add("Error marking form as complete");
                    throw;
                }
                try
                {
                    if (!frm.MarkReadyToBlob())
                    {
                        errors.Add("Could not generate PDF as no template exists for this form");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add("Error marking document ready to blob!");
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                Tracing.HandleError(ex, Tracing.TracingEventType.AvailableForms);
            }
            finally
            {
                if (errors.ErrorsExist)
                {
                    //redirect to errors page
                    Response.Clear();
                    Response.Write(FriendlyError.ErrorPost(errors.ErrorMessage, "Error.aspx"));
                    Response.End();
                }
                else
                {
                    //string path = (referer == null) ? "AvailableForms.aspx" : referer;
                    Response.Redirect("AvailableForms.aspx");
                }
            }
        }
    }
}