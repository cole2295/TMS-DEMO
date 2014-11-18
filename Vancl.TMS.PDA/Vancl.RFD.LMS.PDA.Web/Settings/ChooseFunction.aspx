<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseFunction.aspx.cs"
    Inherits="Vancl.TMS.PDA.Web.ChooseFunction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>功能选择</title>
    <link href="../style/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="margin: 0 auto;">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btn_InBound" Text="分拣入库" style="width:160px" 
                        class="button" onclick="btn_InBound_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" Visible="false" ID="btn_TransInBound" Text="转站入库" style="width:160px" class="button" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" Visible="false" ID="btn_OutBound" Text="扫描出库" style="width:160px" class="button" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
