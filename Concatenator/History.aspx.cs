using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Concatenator
{
    public partial class History : Page
    {
        public string loggedInUser = string.Empty;
        User user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            loggedInUser = HttpContext.Current.User.Identity.Name;
#if DEBUG
            loggedInUser = "ohg\\bkhuddus";
#endif
            user = DbHelper.CheckUser(loggedInUser.ToLower());

            if (string.IsNullOrEmpty(loggedInUser) || user.ID == 0)
            {
                Response.Redirect("NotAuthorise.html");
            }
            loggedInUser = user.FullName;
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            string query = "Select * from Logs l inner join Users u on u.ID=l.UserID order by l.CreatedDate desc";
            Grid.DataSource = DbHelper.GetDataSet(query);
            Grid.DataBind();
        }
        protected void Grid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            Grid.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void Grid_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Grid.EditItemIndex = e.Item.ItemIndex;
            BindData();
        }
        protected void Grid_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            Grid.EditItemIndex = -1;
            BindData();
        }
    }
}