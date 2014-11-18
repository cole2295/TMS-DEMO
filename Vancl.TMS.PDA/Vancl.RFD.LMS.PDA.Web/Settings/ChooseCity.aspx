<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseCity.aspx.cs" Inherits="Vancl.TMS.PDA.Web.ChooseCity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../style/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="margin: 0 auto;">
    <div>
        <table style="margin: 0 auto;">
            <tr>
                <td>
                    <asp:DropDownList runat="server" Style="width: 160px" ID="ddl_City">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btn_Next" Text="下一步>>" Style="width: 120px" 
                        class="button" onclick="btn_Next_Click" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnPrev" runat="server" Text="<<上一步" OnClick="btnPrev_Click" Style="width: 120px"
                        class="button" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
