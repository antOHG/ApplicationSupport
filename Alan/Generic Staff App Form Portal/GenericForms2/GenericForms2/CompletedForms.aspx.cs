using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{
    public partial class CompletedForms : System.Web.UI.Page
    {
        // to keep track of errors to display to the user
        private FriendlyError errors = new FriendlyError();

        // these hashsets keep track of the form names and groups that are added the the form in the code below
        private HashSet<string> referenceGroups = new HashSet<string>();
        private HashSet<string> formNames = new HashSet<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
                try
                {
                    // Get items from query string
                    string refGroup = Request.QueryString["refGroup"];
                    string formName = Request.QueryString["formName"];
                    if (refGroup == null) { refGroup = "*"; }
                    if (formName == null) { formName = "*"; }

                    // Get SID of current user
                    var user = (WindowsIdentity)HttpContext.Current.User.Identity;
                    string sid = user.User.ToString();

                    //Get list of completed forms from the database
                    List<CompletedForm> completedForms = new List<CompletedForm>();
                    foreach (var rec in DataAccess.Instance.GetCompletedFormInstances(sid, refGroup, formName))
                    {
                        completedForms.Add(DataFactory.CompletedFormFactory(rec));
                    }

                    if (completedForms.Count == 0)
                    {
                        PlaceHolder1.Controls.Add(new Label() { Text = "You do not have any forms to see." });
                    }
                    else
                    {
                        // Create the table and populate the data
                        Table tab = new Table();

                        foreach (var c in completedForms)
                        {
                            var colour = c.PdfAvailble ? System.Drawing.Color.Black : System.Drawing.Color.Gray;

                            // Add the form name and reference group to the sets for display in the dropdown
                            referenceGroups.Add(c.ReferenceGroup);
                            formNames.Add(c.FormName);

                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell() { Text = c.ReferenceGroup, ForeColor = colour });
                            tr.Cells.Add(new TableCell() { Text = c.FormName, ForeColor = colour });
                            tr.Cells.Add(new TableCell() { Text = c.UserCreated, ForeColor = colour });
                            tr.Cells.Add(new TableCell() { Text = (c.TSCreated == null) ? string.Empty : c.TSCreated.Value.ToString("dd/MM/yyyy HH:mm"), ForeColor = colour });
                            tr.Cells.Add(new TableCell() { Text = c.RecipientReference, ForeColor = colour });
                            //tr.Cells.Add(new TableCell() { Text = c.RecipientAddress, ForeColor = colour });
                            tr.Cells.Add(new TableCell() { Text = c.UserCreated, ForeColor = colour });
                            if (c.PdfAvailble)
                            {
                                // Include button to view PDF only if the PDF is available
                                TableCell tc = new TableCell();
                                // set the button's ID to be the form instance ID as this will be used by the button click event
                                var b = new Button() { ID = c.FormInstanceId.ToString() };
                                b.Click += Button_Click;
                                tc.Controls.Add(b);
                                tr.Cells.Add(tc);
                            }
                            else
                            {
                                tr.Cells.Add(new TableCell() { Text = "X", ForeColor = System.Drawing.Color.Red });
                            }
                            tab.Rows.Add(tr);
                        }

                        //Add the table to the placeholder
                        PlaceHolder1.Controls.Add(tab);

                        //Add the items to the dropdowns, but not if it's a postback (otherwise there'd be duplicates)
                        if (!IsPostBack)
                        {
                            FormNameDropDown.Items.Add(new ListItem("All", "*"));
                            foreach (string s in formNames.ToArray())
                            {
                                FormNameDropDown.Items.Add(new ListItem(s, s));
                            }
                            FormNameDropDown.SelectedIndex = 0;

                            RefGroupDropDown.Items.Add(new ListItem("All", "*"));
                            foreach (string s in referenceGroups.ToArray())
                            {
                                RefGroupDropDown.Items.Add(new ListItem(s, s));
                            }
                            RefGroupDropDown.SelectedIndex = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add("Error loading completed forms page");
                    Tracing.HandleError(ex, Tracing.TracingEventType.AvailableForms);

                }
                finally
                {
                    if (errors.ErrorsExist)
                    {
                        Response.Clear();
                        Response.Write(FriendlyError.ErrorPost(errors.ErrorMessage, "Error.aspx"));
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            //}
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            int fID = -1;
            try
            {
                // display the pdf in the box clicked by the user
                // embed tag is employed using a URL to get the PDF as Internet Explorer does not support object/base 64 representation that Chrome does
                // the button's ID contains the form instance ID
                fID = Convert.ToInt32(((Button)sender).ID);
               

                string s = "<iframe src=\"GetPdf.aspx?fId=" + fID.ToString() + "\" name=\"iframe_a\" style=\"height:900px; width: 700px\"></iframe>";
                //string s = "<embed type=\"Application/pdf\" width=\"700px\" height=\"900px\" src=GetPdf.aspx?fId=" + fID.ToString() + " />";
                PlaceHolder2.Controls.Add(new Literal() { Text = s });

                //if (Request.Browser.Browser == "InternetExplorer")
                //{
                //    s = "<embed type=\"Application/pdf\" width=\"700px\" height=\"900px\" src=GetPdf.aspx?fId=" + fID.ToString() + " />";
                //}
                //else
                //{
                //    s = string.Format("<object data=\"data:application/pdf;base64,{0}\" type=\"application/pdf\" width=\"700px\" height=\"900px\" />", Convert.ToBase64String(DataAccess.Instance.GetPdf(fID)));
                //}

            }
            catch (Exception ex)
            {
                errors.Add("Error displaying PDF for FormId=" + fID.ToString());
                Tracing.HandleError(ex, Tracing.TracingEventType.AvailableForms);
            }
            finally
            {
                // if there are any errors, redirect to the error page.
                if (errors.ErrorsExist)
                {
                    Response.Clear();
                    Response.Write(FriendlyError.ErrorPost(errors.ErrorMessage, "Error.aspx"));
                    Response.End();
                }
            }
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("CompletedForms.aspx?refGroup={0}&formName={1}", RefGroupDropDown.SelectedValue, FormNameDropDown.SelectedValue));
        }

    }
}