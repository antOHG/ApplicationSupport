using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenericForms2
{



    public partial class AvailableForms : System.Web.UI.Page
    {
        // to keep track of errors to display to the user
        private FriendlyError errors = new FriendlyError();

        // these hashsets keep track of the form names and groups that are added the the form in the code below
        private HashSet<string> referenceGroups = new HashSet<string>();
        private HashSet<string> formNames = new HashSet<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    // Get items from query string
                    string refGroup = Request.QueryString["refGroup"];
                    string formName = Request.QueryString["formName"];
                    if (refGroup == null) { refGroup = "*"; }
                    if (formName == null) { formName = "*"; }

                    int lockTimeout = Properties.Settings.Default.LockTimeoutMinutes;

                    // Get SID of current user
                    var user = (WindowsIdentity)HttpContext.Current.User.Identity;
                    string sid = user.User.ToString();

                    //Get list of available forms from the database
                    List<AvailableForm> availableForms = new List<AvailableForm>();
                    foreach (var rec in DataAccess.Instance.GetAvailableFormInstances(sid, refGroup, formName))
                    {
                        availableForms.Add(DataFactory.AvailableFormFactory(rec));
                    }

                    if (availableForms.Count == 0)
                    {
                        PlaceHolder1.Controls.Add(new Label() { Text = "You do not have any forms to review." });
                    }
                    else
                    {
                        // Create the table and populate the data
                        Table tab = new Table();

                        foreach (var a in availableForms)
                        {
                            // Add the form name and reference group to the sets for display in the dropdown
                            referenceGroups.Add(a.ReferenceGroup);
                            formNames.Add(a.FormName);

                            bool locked = (a.LockedTS != null && a.LockedTS.Value.AddMinutes(lockTimeout) > DateTime.Now);
                            System.Drawing.Color c = locked ? System.Drawing.Color.Gray : System.Drawing.Color.Black;
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell() { Text = a.ReferenceGroup, ForeColor = c });
                            tr.Cells.Add(new TableCell() { Text = a.FormName, ForeColor = c });
                            tr.Cells.Add(new TableCell() { Text = a.UserCreated, ForeColor = c });
                            tr.Cells.Add(new TableCell() { Text = (a.TSCreated == null) ? string.Empty : a.TSCreated.Value.ToString("dd/MM/yyyy HH:mm"), ForeColor = c });
                            tr.Cells.Add(new TableCell() { Text = a.RecipientReference, ForeColor = c });
                            if (!locked)
                            {
                                tr.Cells.Add(new TableCell());
                                TableCell tc = new TableCell();
                                tc.Controls.Add(new HyperLink() { Text = "Edit", NavigateUrl = "ReviewForm.aspx?FormId=" + a.CompletedFormId.ToString() });
                                tr.Cells.Add(tc);
                            }
                            else
                            {
                                tr.Cells.Add(new TableCell() { Text = (a.LockedUser == null) ? string.Empty : a.LockedUser, ForeColor = c });
                                tr.Cells.Add(new TableCell());
                            }
                            tab.Rows.Add(tr);
                        }
                        // add table to the form
                        PlaceHolder1.Controls.Add(tab);
                    }

                    //Add the items to the dropdowns
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
                catch (Exception ex)
                {
                    errors.Add("Error loading available forms page");
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
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("AvailableForms.aspx?refGroup={0}&formName={1}", RefGroupDropDown.SelectedValue, FormNameDropDown.SelectedValue));
        }
    }
}