<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleInbound.aspx.cs"
    Inherits="Vancl.TMS.PDA.Web.SimpleInbound" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var codeTxtBox;
        function doSubmit() {
            if (event.keyCode == 13) {
                if (codeTxtBox.value.length > 14) {
                    codeTxtBox.value = "";
                    codeTxtBox.focus();
                    alert("单号错误");
                    document.all.form1.onsubmit = function() { return false; };
                }
                else {
                    document.all.form1.onsubmit = function() { return true; };
                }
            }
        }

        window.onload = function() {
            if (document.getElementById("td_msg").innerText.indexOf("成功") == -1 && document.getElementById("td_msg").innerText != "") {
                alert(document.getElementById("td_msg").innerText);
            }
            codeTxtBox = document.getElementById("<%= txtCode.ClientID %>");
            codeTxtBox.value = "";
            codeTxtBox.focus();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <div>
        <table style="margin: 0 auto;">
            <tr>
                <td align="right" style="font-weight: bold; color: #203f6a; font-size: 12px">
                    目的地:
                </td>
                <td align="center" style="color: red; font-size: 12px">
                    <asp:Literal runat="server" ID="ltArrivalStation" Text=""></asp:Literal>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight: bold; color: #203f6a; font-size: 12px">
                    单号类型:
                </td>
                <td align="center" style="color: red; font-size: 12px">
                    <asp:DropDownList runat="server" ID="ddlFormType">
                        <asp:ListItem Text="运单号" Value="0"></asp:ListItem>
                        <asp:ListItem Text="订单号" Value="1"></asp:ListItem>
                        <asp:ListItem Text="配送单" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight: bold; color: #203f6a; font-size: 12px">
                    入库数量:
                </td>
                <td align="center" style="font-weight: bold; color: red; font-size: 12px">
                    <asp:Literal runat="server" ID="ltInboundCount" Text="0"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th align="right" style="font-weight: bold; color: #203f6a; font-size: 12px">
                    提示信息:
                </th>
                <td align="center" style="color: red; font-size: 12px" id="td_msg">
                    <asp:Literal runat="server" ID="ltMsg"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="font-weight: bold; color: #203f6a; font-size: 13px">
                    <hr style="color: Gray" />
                    <asp:TextBox Width="120px" runat="server" ID="txtCode" onKeyDown="doSubmit()"></asp:TextBox><hr
                        style="color: Gray" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <a href="#" style="font-size: 12px; padding-right: 5px" runat="server" id="a_switchArrival">
                        【目的切换】</a><a href="../Settings/ChooseCity.aspx" style="font-size: 12px;">【城市切换】</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <a href="../Settings/SelectArrivalType.aspx" style="font-size: 12px; padding-right: 5px">【类型切换】</a><a
                        href="../Settings/ChooseFunction.aspx" style="font-size: 12px;">【功能切换】</a>
                    <asp:Button runat="server" ID="btnSubmit" Style="display: none" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
