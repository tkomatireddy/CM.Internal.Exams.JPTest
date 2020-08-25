using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JPTest;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Text;
using System.IO;
using System.Globalization;

namespace JPTest
{
    public partial class AdminHome : System.Web.UI.Page
    {
        //public static string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["UserType"] != null)
                {
                    hdfStartTime.Value = string.Empty;
                    Session["StartTime"] = "";
                    Session["UserName"] = null;
                    Session["UserType"] = null;
                    Session["EvaluatorId"] = null;
                    Session["LoginName"] = null;

                    Session["UserType"] = Request.QueryString["UserType"];

                    if (Request.QueryString["UserType"] == "Evaluator")
                    {
                        if (Request.QueryString["UserId"] != null)
                        {
                            Session["EvaluatorId"] = Request.QueryString["UserId"];
                        }
                    }
                    if (Request.QueryString["LoginName"] != null)
                    {
                        Session["LoginName"] = Request.QueryString["LoginName"];
                    }
                }

                if (Session["LoginName"] != null)
                {
                    lblLoginName.Text = Session["LoginName"].ToString();
                }
                //Session["StartTime"] = DateTime.Now.ToShortTimeString();
                txtFromDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                GetUserTestDetails();
            }
        }
        private void GetUserTestDetails()
        {
            int ModuleId = 49;

            DateTime StartDate = DateTime.Now.Date;
            DateTime EndDate = DateTime.Now.Date;

            //DateTime.TryParseExact(txtFromDate.Text, sysFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate);
            //DateTime.TryParseExact(txtToDate.Text, sysFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);

            StartDate = Convert.ToDateTime(txtFromDate.Text);
            EndDate = Convert.ToDateTime(txtToDate.Text);
            gvUserTestDtls.DataSource = null;
            JP_DAC dac = new JP_DAC();
            {
                DataTable dt = dac.Get_OtherUserTestDetails(ModuleId, StartDate, EndDate);
                gvUserTestDtls.DataSource = dt;

                if(dt != null && dt.Rows.Count>0)
                {
                    int Total_Marks = 0;
                    int.TryParse(dt.Rows[0]["Total_Marks"].ToString(), out Total_Marks);

                    if(Total_Marks != ddlMarks.Items.Count)
                    {
                        for(int i=0;i<= Total_Marks; i++)
                        {
                            ddlMarks.Items.Add(i.ToString());
                        }
                        ddlMarks.DataBind();
                    }
                }
            }
            gvUserTestDtls.DataBind();
        }
        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetUserTestDetails();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }

        protected void gvUserTestDtls_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void lnkGoToQuesctions_Click(object sender, EventArgs e)
        {
            LinkButton lnkGoToQuesctions = (LinkButton)sender;

            GridViewRow row = (GridViewRow)lnkGoToQuesctions.NamingContainer;
            if (row != null)
            {
                int usermarks = 0;
               string obtMarks= row.Cells[9].Text;

                obtMarks = obtMarks.Substring(0, obtMarks.IndexOf('.') > 0 ? obtMarks.IndexOf('.') : obtMarks.Length);

                //int.TryParse(obtMarks, out usermarks);

                ddlMarks.SelectedIndex = ddlMarks.Items.IndexOf(ddlMarks.Items.FindByText(obtMarks.ToString()));
            }

                long UserId = 0;
            long.TryParse(lnkGoToQuesctions.CommandArgument, out UserId);

            if (UserId > 0)
            {
                Session["UserId"] = UserId;
                Session["UserName"] = lnkGoToQuesctions.CommandName;

                string mapPath = Server.MapPath("/Articals");
                mapPath = Path.Combine(mapPath, Session["UserName"].ToString());

                TreeView1.Nodes.Clear();
                DirectoryInfo rootInfo = new DirectoryInfo(mapPath);
                this.PopulateTreeView(rootInfo, null);
                mpeUserFolders.Show();
            }
        }

        private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
        {
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                TreeNode directoryNode = new TreeNode
                {
                    Text = directory.Name,
                    Value = directory.FullName,
                    ToolTip = "D"
                };

                if (treeNode == null)
                {
                    //If Root Node, add to TreeView.
                    directoryNode.Expand();
                    TreeView1.Nodes.Add(directoryNode);
                }
                else
                {
                    //If Child Node, add to Parent Node.
                    //directoryNode.Expand();
                    treeNode.ChildNodes.Add(directoryNode);
                }

                //Get all files in the Directory.
                foreach (FileInfo file in directory.GetFiles())
                {
                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = file.Name,
                        Value = file.FullName,
                        Target = "_blank",
                        ToolTip = "F",
                        SelectAction =  TreeNodeSelectAction.None

                    //, NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()
                };
                    directoryNode.ChildNodes.Add(fileNode);
                }
                PopulateTreeView(directory, directoryNode);
            }
        }
        protected void btnSaveMarks_Click(object sender, EventArgs e)
        {
            //if (txtMarks.Text.Trim().Length == 0)
            //{
            //    mpeUserFolders.Show();
            //}
            //else
            {
                int ModuleId = 49;
                int marks = 0;
                int.TryParse(ddlMarks.SelectedValue, out marks);

                long UserId = 0;
                long.TryParse(Session["UserId"].ToString(), out UserId);

                if (Session["UserType"] != null && Session["UserType"].ToString() == "Evaluator")
                {
                    long EvaluatorId = 0;
                    long.TryParse(Session["EvaluatorId"].ToString(), out EvaluatorId);

                    JP_DAC dac = new JP_DAC();
                    int res = dac.UpdateEvaluatorDetails(UserId, EvaluatorId, ModuleId, marks);
                    dac = null;

                    if(res>0)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('Your evaluation for this user is saved')", true);
                    }
                    Session["UserId"] = null;
                    //gvUserTestDtls.SelectedIndex = -1;
                    GetUserTestDetails();
                }
                if (Session["UserType"] != null && Session["UserType"].ToString() == "Administrator")
                {
                    Session["UserId"] = null;
                    //Response.Redirect("AdminHome.aspx");
                }
            }
        }
    }
}