<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Search.aspx.vb" Inherits="Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>The Locator- Search Engine</title>
</head>
<body>
    <form id="Search" runat="server">
        <div>
            &nbsp;
            <asp:TextBox ID="TextBoxSearch" runat="server" Height="24px" Style="left: 6.2cm;
                position: absolute; top: 2.9cm" ToolTip="Enter Text to be searched" Width="472px"></asp:TextBox><asp:Button ID="ButtonSearch" runat="server" Height="24px" Style="left: 11cm; position: absolute;
                top: 3.8cm" Text="Search" Width="128px" PostBackUrl="~/Search.aspx" />
            <asp:Label ID="LabelLocator" runat="server" Height="32px" Style="font-weight: bolder;
                font-size: 1cm; left: 328px; visibility: visible; vertical-align: text-top; text-transform: uppercase;
                color: #0033cc; direction: ltr; font-style: normal; font-family: 'Comic Sans MS';
                letter-spacing: normal; position: absolute; top: 16px; text-align: center; font-variant: normal"
                Text="Locator" Width="312px"></asp:Label>
            <asp:HyperLink ID="HyperLinkAboutus" runat="server" Height="1px" Style="left: 640px;
                color: blue; position: absolute; top: 80px; text-align: center; text-decoration: underline"
                Width="80px" Visible="False">About Us</asp:HyperLink>
        </div>
        <br />
        &nbsp;&nbsp;&nbsp;<div style="text-align: left">
            <table style="left: 16px; width: 920px; position: absolute; top: 176px; background-color: deepskyblue;">
                <tr>
                    <td style="width: 114px">
                    </td>
                    <td style="width: 1px" align="right"">
                <asp:Label ID="LabelPages" runat="server" Height="24px" Style="font-weight: lighter; left: 320px; visibility: visible; vertical-align: text-top; 
                    color: black; direction: ltr; font-style: normal; font-family: Arial;
                    letter-spacing: normal; top: 264px; text-align: right; font-variant: normal; position: static;"
                    Text="Pages:  " Width="30px"></asp:Label></td>
                    <td style="width: 121px">
                <asp:DropDownList ID="DropDownListPages" runat="server" Height="1px" Style="
                    left: 408px; visibility: visible; vertical-align: text-top; text-transform: uppercase;
                    color: black; direction: ltr; font-style: normal; font-family: Arial;
                    letter-spacing: normal; top: 296px; text-align: center; font-variant: normal"
                    Width="72px" AutoPostBack="True"></asp:DropDownList></td>
                </tr>
            </table>
        </div>
        &nbsp;&nbsp;
    </form>
</body>
</html>
