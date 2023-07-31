<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>The Locator- Search Engine</title>
</head>
<body>
    <form id="Default" runat="server">
    <div>
        &nbsp; &nbsp;
        <asp:TextBox ID="TextBoxSearch" runat="server" style="left: 6.6cm; position: absolute; top: 4.9cm" Height="24px" Width="472px" ToolTip="Enter Text to be searched"></asp:TextBox>
        <asp:Button ID="ButtonSearch" runat="server" Text="Search" Width="128px" style="left: 11.2cm; position: absolute; top: 5.9cm" />
        <asp:Label ID="LabelLocator" runat="server" Style="font-weight: bolder; font-size: 1cm;
            left: 328px; visibility: visible; vertical-align: text-top; text-transform: uppercase;
            color: #0033cc; direction: ltr; font-style: normal; font-family: 'Comic Sans MS';
            letter-spacing: normal; position: absolute; top: 48px; text-align: center; font-variant: normal"
            Text="LOCATOR" Width="312px"></asp:Label>
        <asp:HyperLink ID="HyperLinkAboutus" runat="server" style="left: 640px; color: blue; position: absolute; top: 160px; text-align: center; text-decoration: underline" Width="80px" Visible="False">About Us</asp:HyperLink>
        &nbsp;
    </div>
    </form>
</body>
</html>
