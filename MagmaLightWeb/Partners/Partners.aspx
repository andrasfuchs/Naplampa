<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Partners.aspx.cs" Inherits="MagmaLightWeb.Partners.Partners"
    Culture="auto:hu-HU" UICulture="auto" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>

<%@ OutputCache Duration="86400" VaryByParam="Language" VaryByCustom="Culture" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Image ID="imgLogoVisa" runat="server" AlternateText="VISA" ImageUrl="~/images/Logos_VISA.png" />
                    </td>
                    <td align="center">
                        <asp:Image ID="imgMasterCard" runat="server" AlternateText="MasterCard" ImageUrl="~/images/Logos_MasterCard.png" />
                    </td>
                    <td align="center">
                        <asp:Image ID="imgPayPal" runat="server" AlternateText="PayPal" ImageUrl="~/images/Logos_PayPal.png" />
                    </td>
                    <td align="center">
                        <asp:Image ID="imgMagyarPosta" runat="server" AlternateText="MagyarPosta" ImageUrl="~/images/Logos_MagyarPosta.png" />
                    </td>
                </tr>
            </table>
            <div class="form_container_header">
                <asp:Label ID="lblCredentials" meta:resourceKey="lblCredentials" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblUsername" meta:resourceKey="lblUsername" runat="server" /></div>
            <div class="form_cell">
                <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" Width="260px" CssClass="input" /></div>
            <div class="form_cell">
                <asp:Label ID="lblPassword" meta:resourceKey="lblPassword" runat="server" /></div>
            <div class="form_cell">
                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" Width="260px" CssClass="input"
                    TextMode="Password" /></div>
            <div class="form_accept">
                <asp:Button ID="btnForgotPassword" meta:resourceKey="btnForgotPassword" runat="server"
                    Width="200px" OnClick="btnForgotPassword_Click" CssClass="cancel" />
                <asp:Button ID="btnLogin" meta:resourceKey="btnLogin" runat="server" Width="200px"
                    OnClick="btnLogin_Click" CssClass="accept" />
            </div>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
