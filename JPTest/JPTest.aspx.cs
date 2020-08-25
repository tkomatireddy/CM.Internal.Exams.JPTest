using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace JPTest
{
    public partial class JPTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["crnttime"] != null)
                {
                    hdfTimer.Value = Session["crnttime"].ToString();
                }


                //HttpCookie cookiename = Request.Cookies.Get("cookiename");

                //// Check if cookie exists in the current request.
                //if (cookiename != null)
                //{
                //    hdfTimer.Value = cookiename.Value;
                //}

                if (Request.QueryString["UserType"] != null)
                {
                    //hdfStartTime.Value = string.Empty;
                    Session["StartTime"] = "";

                    Session["UserType"] = null;
                    Session["EvaluatorId"] = null;
                    Session["LoginName"] = null;
                    Session["StartTime"] = null;

                    Session["UserType"] = Request.QueryString["UserType"];

                    if (Request.QueryString["UserType"] == "User")
                    {
                        if (Request.QueryString["UserId"] != null)
                        {
                            Session["UserId"] = Request.QueryString["UserId"];
                            Session["EvaluatorId"] = 0;
                        }
                    }
                }
                if (Request.QueryString["LoginName"] != null)
                {
                    Session["LoginName"] = Request.QueryString["LoginName"];
                }


                if (Session["UserType"] != null)
                {
                    if (Session["UserType"].ToString() == "User")
                    {
                        lnkHome.Visible = false;
                    }
                }

                if (Session["UserName"] != null)
                {
                    lblUserName.Text = Session["UserName"].ToString();
                }

                if (!this.IsPostBack)
                {
                    string mapPath = Server.MapPath("/Articals");
                    mapPath = Path.Combine(mapPath, Session["UserName"].ToString());

                    if (!Directory.Exists(mapPath))
                    {
                        Directory.CreateDirectory(mapPath);
                    }

                    DirectoryInfo rootInfo = new DirectoryInfo(mapPath);
                    this.PopulateTreeView(rootInfo, null);
                }
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
                    TreeView1.Nodes.Add(directoryNode);
                }
                else
                {
                    //If Child Node, add to Parent Node.
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
                        ToolTip = "F"
                        //NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()
                    };
                    directoryNode.ChildNodes.Add(fileNode);
                }
                PopulateTreeView(directory, directoryNode);
            }
        }
        protected void FillTreeView()
        {
            string mapPath = Server.MapPath("/Articals");
            mapPath = Path.Combine(mapPath, Session["UserName"].ToString());

            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }
        }
        protected void btnTimeOut_Click(object sender, EventArgs e)
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (Session["UserType"] != null && Session["UserType"].ToString() == "User")
            {
                JP_DAC dac = new JP_DAC();
                int res = dac.UpdateUserDetails(UserId, 49);
                if (res > 0)
                {
                    Session["UserId"] = null;
                    Session["UserType"] = null;
                    Response.Redirect("~/Login.aspx");
                }
                dac = null;
            }
        }
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (Session["UserType"] != null && Session["UserType"].ToString() == "User")
            {
                JP_DAC dac = new JP_DAC();
                int res = dac.UpdateUserDetails(UserId, 49);
                if (res > 0)
                {
                    Session["UserId"] = null;
                    Session["UserType"] = null;
                    Response.Redirect("~/Login.aspx");
                }
                dac = null;
            }
        }

        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('Please enter name.')", true);
                return;
            }
            if (rbtnCreate.SelectedIndex == 0)
            {
                string filename = txtName.Text.Trim();

                if(filename.Contains('.'))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('Please enter valid directory name.')", true);
                    return;
                }               
            }

            if (rbtnCreate.SelectedIndex == 1)
            {
                string filename = txtName.Text.Trim();

                if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('File name have invalided charactors.')", true);
                    return;
                }

                var regExp = @"^.*\.(jpg|JPG|gif|GIF|doc|DOC|pdf|PDF|txt|TXT)$";
                Regex regex = new Regex(regExp);

                if (!regex.IsMatch(filename))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('File extension is required to create a file.')", true);
                    return;
                }
            }

            if (TreeView1.SelectedNode == null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('Please select the root folder to create.')", true);
                return;
            }
            else // if (TreeView1.SelectedNode.ToolTip != "F")
            {
                string filePath = TreeView1.SelectedNode.Value;

                filePath = Path.Combine(filePath, txtName.Text.Trim());
                try
                {
                    if (rbtnCreate.SelectedValue == "D")
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                            TreeNode directoryNode = new TreeNode
                            {
                                Text = txtName.Text.Trim(),
                                Value = filePath,
                                ToolTip = "D"
                            };
                            TreeView1.SelectedNode.ChildNodes.Add(directoryNode);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('There is already a directory with same name in this location.Please change the directory name.')", true);
                            return;
                        }
                    }
                    else
                    {
                        if (!File.Exists(filePath))
                        {
                            File.Create(filePath);
                            TreeNode fileNode = new TreeNode
                            {
                                Text = txtName.Text.Trim(),
                                Value = filePath,
                                Target = "_blank",
                                ToolTip = "F"
                                //, SelectAction = TreeNodeSelectAction.None
                                //NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(filePath)).ToString()
                            };
                            TreeView1.SelectedNode.ChildNodes.Add(fileNode);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('There is already a file with same name in this location.Please change the file name.')", true);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('" + ex.Message.Replace("'", "''") + "')", true);
                    return;
                }
            }
            //else if (TreeView1.SelectedNode.ToolTip == "F")
            //{

            //}
        }
        public bool IsValidFolderName(string testName)
        {
            //Regex regex = new Regex("^.$");
            //Regex containsABadCharacter = new Regex("[" + Regex.Escape(Path.InvalidPathChars.ToString()) + "]");
            //if (containsABadCharacter.IsMatch(testName)) { return false; };

            //// other checks for UNC, drive-path format, etc

            return true;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode != null)
            {
                if (TreeView1.SelectedNode.Text == "Articals")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('You cannot delete this root folder')", true);
                    return;
                }

                if (TreeView1.SelectedNode.ToolTip == "F")
                {
                    File.Delete(TreeView1.SelectedNode.Value);
                    TreeView1.SelectedNode.Parent.ChildNodes.Remove(TreeView1.SelectedNode);
                }
                else if (TreeView1.SelectedNode.ToolTip == "D")
                {
                    if (TreeView1.SelectedNode.ChildNodes.Count == 0)
                    {
                        Directory.Delete(TreeView1.SelectedNode.Value);
                        TreeView1.SelectedNode.Parent.ChildNodes.Remove(TreeView1.SelectedNode);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "marin1", "alert('Only empty folder you can delete.')", true);
                        return;
                    }
                }
            }
        }

        protected void lnkShowPDF_Click(object sender, EventArgs e)
        {
            //string path = Server.MapPath("Journal_Production_Test_Instructions_Manual.pdf");
            //WebClient client = new WebClient();
            //Byte[] buffer = client.DownloadData(path);
            //if (buffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", buffer.Length.ToString());
            //    Response.BinaryWrite(buffer);
            //}
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('ShowPDF.aspx', '_blank','location=no,toolbar=no,menubar=no,scrollbars=yes');", true);
            //string redirect = "<script>window.open('http://www.google.com');</script>";
            //Response.Write(redirect);
        }
    }
}