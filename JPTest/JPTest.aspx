<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JPTest.aspx.cs" Inherits="JPTest.JPTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Styles/css/bootstrap.min.css" />
    <script src="Styles/css/jquery-3.4.1.slim.min.js"></script>
    <script src="Styles/css/popper.min.js"></script>
    <script src="Styles/css/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Styles/css/exams.css" />
    <script src="Styles/js/exams.js"></script>
    <script src="Styles/js/jquery.cookie.js"></script>
    <style>
        .spaced input[type="radio"] {
            margin-right: 5px; /* Or any other value */
            margin-left: 10px;
        }
    </style>
    <script type="text/javascript">
        function countdown() {
            time = document.getElementById("hdfTimer").value;
            if (time > 0) {
                document.getElementById("hdfTimer").value = time - 1;
                time = time - 1

                if (time == 300) {
                    alert('You have 5 minuts left..!');
                }


                var minutes = Math.floor(time / 60);
                var seconds = time - minutes * 60;
                document.getElementById("lblTimer").innerHTML = minutes + " : " + seconds

                JPTest.CurrentTime.SetSession(time);
                setTimeout("countdown()", 1000);              
            }
            if (time == 0) {
                alert('Your exam is timed-out and hence, submitted for evaluation. Thank you, we will get back to you!');
                document.getElementById("<%=btnTimeOut.ClientID %>").click();
            }
        }
        setTimeout("countdown()", 1000);

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server">
            <Services>
                <asp:ServiceReference Path="~/CurrentTime.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">JP Online Test:</label>
                <label for="lblUserName" style="margin-left: 20px; color: yellow">User Name:</label>
                <asp:Label ID="lblUserName" Text="User Name" runat="server" Style="font-size: large;"></asp:Label>
                <asp:LinkButton ID="lblLogOut" Text="Logout" runat="server" Style="color: white; font-size: large; float: right; margin-left: 20px;" OnClick="lblLogOut_Click"></asp:LinkButton>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfTimer" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <asp:RadioButtonList ID="rbtnCreate" runat="server" RepeatDirection="Horizontal" CssClass="spaced">
                                <asp:ListItem Text="Directory" Value="D" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="File" Value="F"></asp:ListItem>
                            </asp:RadioButtonList>
                            <div class="form-inline col-md-8" style="margin-left: 20px;">
                                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none"></asp:Label>
                                <label for="txtName">Name:</label>
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" CssClass="btn btn-info" style="margin-left: 20px;" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="btn btn-danger" style="margin-left: 20px;" />
                                
                                <asp:LinkButton ID="lnkShowPDF" runat="server" Text="Show Instructions" style="margin-left: 20px;" OnClick="lnkShowPDF_Click" > </asp:LinkButton>                          
                                <asp:HyperLink runat="server" NavigateUrl="http://202.177.173.182/JPTest/JP/Journal%20Details.txt" Target="_blank" style="margin-left: 20px;">Journal Details</asp:HyperLink>
                                <asp:HyperLink runat="server" NavigateUrl="http://202.177.173.182/JPTest/JP/Archive/" Target="_blank" style="margin-left: 20px;">Source Aritcles</asp:HyperLink>
                            </div>

                            <div class="col-md-2">
                                <asp:LinkButton ID="lnkHome" Style="float: right;" runat="server" Text="Admin Home" PostBackUrl="~/Models/AdminHome.aspx" Visible="false" />

                                 <asp:Button ID="btnTimeOut" runat="server" Text="Time Out" style="display:none" CssClass="btn btn-warning" OnClick="btnTimeOut_Click" />
                                <asp:Button ID="btnFinish" runat="server" Text="Finished" OnClientClick="return confirm('Are you sure finished the test?');" CssClass="btn btn-warning" OnClick="btnFinish_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="col-md-offset-2 col-md-8 col-sm-12">
                            <div class="tree-spaced margin-top">
                                <div>
                                    Please note the following: 
                                    <br />
                                    1. After you complete creating the directory/folder structure, please click “Finished” button to submit your exam
                                    <br />
                                    2. In case, exam times-out your results would be automatically submitted </div>
                                <div class="row">
                                    <div class="col-xs-6">
                                        <asp:TreeView ID="TreeView1" runat="server" ImageSet="XPFileExplorer" NodeIndent="15">
                                            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                                                NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                            <ParentNodeStyle Font-Bold="False" />
                                            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                                                VerticalPadding="0px" />
                                        </asp:TreeView>
                                    </div>
                                    <div class="col-xs-6">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

