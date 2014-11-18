<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectArrivalType.aspx.cs"
    Inherits="Vancl.TMS.PDA.Web.SelectArrivalType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../style/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="margin: 0 auto;">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnStation" Text="入库到站点" Style="width: 160px" 
                        class="button" onclick="btnStation_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSortCenter" Text="入库到二级分拣" Style="width: 160px"
                        class="button" onclick="btnSortCenter_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnDistribution" Text="入库到配送商" Style="width: 160px"
                        class="button" onclick="btnDistribution_Click" />
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
