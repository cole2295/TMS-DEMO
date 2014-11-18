<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebTestApp._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <a href="~/Scripts/jquery-1.4.1.min.js"></a>
    <script src="http://localhost:14914/Scripts/pages/sorting/BatchInbound.js" type="text/javascript"></script>
    <div>
        <asp:TextBox ID="t_Name" runat="server"></asp:TextBox>
        <asp:TextBox ID="t_Value" runat="server"></asp:TextBox>
        <asp:Button ID="b_submit" runat="server" onclick="b_submit_Click" Text="新增" />
    </div>
    </form>
</body>
</html>
