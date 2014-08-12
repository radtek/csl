<%@ page language="C#" autoeventwireup="true" inherits="_Default, App_Web_zt0k4v0w" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>素材上传专用页面</title>
    <link href="table.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td colspan="2" class="table_title">
                    材料上传专用页面
                </td>
            </tr>
            <tr>
                <td align="right" class="table_label_col">
                    请选择材料文件：
                </td>
                <td class="table_value_col">
                    <asp:FileUpload ID="FileUpLoad1" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <center>
                        <asp:Button ID="btnFileUpload" runat="server" OnClick="btnFileUpload_Click" Text="文件上传"
                            CssClass="buttonCss" /></center>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <center>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label></center>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <center>
                        <asp:Button ID="Button1" runat="server" OnClick="back" Text="返回" CssClass="buttonCss" /></center>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
