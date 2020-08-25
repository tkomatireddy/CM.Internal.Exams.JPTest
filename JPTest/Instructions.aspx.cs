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
using System.Web.UI.WebControls;
using System.Globalization;

namespace JPTest
{
    public partial class Instructions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //{
            //    Session["UserName"] = "abc@abc.com";
            //    Session["UserType"] = "User";
            //    Session["crnttime"] = "1080";
            //    Session["UserId"] = "1";
            //    Session["LoginName"] = "abc@abc.com";
            //}


            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    lblLoginName.Text = Session["UserName"].ToString();
                }
            }

            
        }

        protected void btnStartTest_Click(object sender, EventArgs e)
        {
            string mapPath = Server.MapPath("/Articals");
            mapPath = Path.Combine(mapPath, Session["UserName"].ToString());

            if(!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
                Directory.CreateDirectory(Path.Combine(mapPath, "Archive"));
            }

            Response.Redirect(@"JPTest.aspx");
        }
    }
}