<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="JPTest.AdminHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
    <%--<style type="text/css">
        .container {
            padding-right: 0px;
            padding-left: 0px;
            margin-right: auto;
            margin-left: auto;
        }
    </style>--%>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server"></asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">JP Online Test </label>

                <label for="lblLoginName" style="color: black; font-size: large; margin-left: 20px">Login Name:</label>
                <asp:Label ID="lblLoginName" Text="Login Name" runat="server" Style="color: yellow; font-size: large;"></asp:Label>

                <asp:LinkButton ID="lblLogOut" Text="Logout" runat="server" Style="color: white; font-size: large; float: right; margin-left: 20px;" OnClick="lblLogOut_Click"></asp:LinkButton>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfStartTime" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-2" style="margin-left: 20px;">
                                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <label for="txtFromDate">From:</label>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="100px" onFocus="this.blur();" AutoCompleteType="Disabled"></asp:TextBox>

                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy" />
                                <label for="txtToDate">To:</label>
                                <asp:TextBox ID="txtToDate" runat="server" Width="100px" onFocus="this.blur();" AutoCompleteType="Disabled"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy" />
                                <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-info btn-md" OnClick="btnSearch_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return confirm('Are you sure want to close?');" CssClass="btn btn-warning" OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gvUserTestDtls" runat="server" CssClass="table table-striped table-hover text-center valign-middle"
                                    AutoGenerateColumns="false" OnRowDataBound="gvUserTestDtls_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserId" HeaderText="UserId" />
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="TestDate" HeaderText="Test Date" />
                                        <asp:BoundField DataField="TestStatus" HeaderText="Test Status" />
                                        <asp:BoundField DataField="TimeTaken" HeaderText="Time Taken" />
                                        <asp:BoundField DataField="EvaluationDate" HeaderText="Evaluation Date" />
                                        <asp:BoundField DataField="EvaluatorStatus" HeaderText="Evaluator Status" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Total_Marks" HeaderText="Total Marks" />
                                        <asp:BoundField DataField="Obtained_Marks" HeaderText="Obtained Marks" />
                                        <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMFSLoad" runat="server" OnClick="lnkGoToQuesctions_Click" CommandName='<%# Eval("UserName")%>' CommandArgument='<%# Eval("UserId")%>' Text="Go to Test Details" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Button ID="btnTemp" runat="server" Text="" Style="display: none" />

        <ajaxToolkit:ModalPopupExtender ID="mpeUserFolders" BehaviorID="bmpeUserFolders" runat="server" TargetControlID="btnTemp"
            PopupControlID="pnlUserFolders" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlUserFolders" runat="server" CssClass="table-responsive" BackColor="White" Style="padding: 5px; min-height: 400px"
            Width="600px" Height="400px">

            <table style="width: 100%;" class="topBanner">
                <tr>
                    <td width="70%" align="left">
                        <asp:Label ID="lblcommentsheader" runat="server"> User Test Details</asp:Label></td>
                    <td align="center" width="10%"></td>
                    <td width="10%">
                        <asp:Button ID="Button1" Text="Close [X]" runat="server" CssClass="btn btn-primary btn-md" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="left" colspan="3">
                        <label for="ddlMarks">Marks</label>
                        <%-- <asp:TextBox ID="txtMarks" runat="server" maxlength="2" TextMode="Number" Width="50px" ></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlMarks" runat="server"></asp:DropDownList>
                        <asp:Button ID="btnSaveMarks" runat="server" Text="Save" OnClick="btnSaveMarks_Click" />

                    </td>
                </tr>
            </table>
            <div>
            </div>
            <div class="text-center">
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="XPFileExplorer" NodeIndent="15">
                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="1px"
                        NodeSpacing="0px" VerticalPadding="1px"></NodeStyle>
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                </asp:TreeView>
            </div>

        </asp:Panel>

    </form>
</body>
</html>
