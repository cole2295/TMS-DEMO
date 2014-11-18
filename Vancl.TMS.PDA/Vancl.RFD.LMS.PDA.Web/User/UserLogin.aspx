<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="Vancl.TMS.PDA.Web.UserLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分拣PDA系统-登录</title>
    <link href="../style/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function checkUserInput() {
            document.getElementById("divError").style.display = "none";
            var uName = document.getElementById("txtUserName").value;
            var uPwd = document.getElementById("txtUserPwd").value;

            if (uName == "" || uPwd == "") {
                alert("用户名或密码不能为空!");
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="margin:0 auto;">
            <tr>
                <td style="font-weight:bold;color:#203f6a;" align="center">
                    分拣PDA系统
                </td>
            </tr>
            <tr>
                <td align="center">
                    <input type="text" runat="server" id="txtUserName" style="width:80%" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <input type="password" runat="server" id="txtUserPwd" style="width:80%"  />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <input type="submit" runat="server" id="btnLogin" value="登录" style="width:80%" class="button" onclick="return checkUserInput();" />
                </td>
            </tr>
            <tr>
                <td align="center" style="color:red; font-size:12px">
                    <div style="display:none" id="divError" runat="server"></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
