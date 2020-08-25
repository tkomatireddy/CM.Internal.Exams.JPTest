<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Instructions.aspx.cs" Inherits="JPTest.Instructions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Styles/css/bootstrap.min.css" />
    <script src="Styles/css/jquery-3.4.1.slim.min.js"></script>
    <script src="Styles/css/popper.min.js"></script>
    <script src="Styles/css/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Styles/css/exams.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server"></asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">JP Online Test </label>

                <label for="lblLoginName" style="color: black; font-size: large; margin-left: 20px">Login Name:</label>
                <asp:Label ID="lblLoginName" Text="Login Name" runat="server" Style="color: yellow; font-size: large;"></asp:Label>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfStartTime" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header font-weight-bold">
                        <p class="MsoTitle" align="center" style='text-align: center'>
                            <span>Journal Production Test Instructions</span>
                        </p>
                        <%--<div style='border: none; border-bottom: solid #4F81BD 1.0pt; padding: 0cm 0cm 4.0pt 0cm'>
                        </div>--%>
                    </div>
                    <div class="card-body">
                        <p>1) Read the instructions document that outlines the process with a sample Title</p>
                        <p>2) You must create the folders and files by referring to the notepad document and the PDF files available at a shared URL</p>
                        <p>3) The exam page has the options to create the Directory/Folder or File</p>
                        <p>4) Click "Start Test" button to begin the test</p>
                    </div>

                    <div class="card-footer text-muted">
                        <asp:Button ID="btnStartTest" runat="server" Text="Start Test" class="btn btn-primary" OnClick="btnStartTest_Click" />
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
