<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPreferences.aspx.cs"
    Inherits="MagmaLightWeb.Admin.UserPreferences" Culture="auto:hu-HU" UICulture="auto"
    MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">
            <div class="form_row">
                <asp:Label ID="lblError" meta:resourceKey="lblError" runat="server" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblEmailNotConfirmed" meta:resourceKey="lblEmailNotConfirmed" runat="server" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblSuccess" meta:resourceKey="lblSuccess" runat="server" Visible="false" />
            </div>
            <asp:Panel ID="pnlPreferences" runat="server">
                <div class="form_cell" id="div1" runat="server">
                    <asp:Label ID="lblTitle" meta:resourceKey="lblTitle" runat="server" /></div>
                <div class="form_cell" id="div2" runat="server">
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="30" Width="260px" CssClass="input" /></div>            
                <div class="form_cell" id="divLastNameLabel" runat="server">
                    <asp:Label ID="lblLastName" meta:resourceKey="lblLastName" runat="server" /></div>
                <div class="form_cell" id="divLastName" runat="server">
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" Width="260px" CssClass="input" /></div>
                <div class="form_cell" id="divFirstNameLabel" runat="server">
                    <asp:Label ID="lblFirstName" meta:resourceKey="lblFirstName" runat="server" /></div>
                <div class="form_cell" id="divFirstName" runat="server">
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" Width="260px" CssClass="input" /></div>
                <div class="form_cell" id="divEmailLabel" runat="server">
                    <asp:Label ID="lblEmail" meta:resourceKey="lblEmail" runat="server" /></div>
                <div class="form_cell_value" id="divEmail" runat="server">
                    <asp:Label ID="lblEmailText" meta:resourceKey="lblEmailText" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblPhone" meta:resourceKey="lblPhone" runat="server" /></div>
                <div class="form_cell">
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" Width="260px" CssClass="input" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblFax" meta:resourceKey="lblFax" runat="server" /></div>
                <div class="form_cell">
                    <asp:TextBox ID="txtFax" runat="server" MaxLength="50" Width="260px" CssClass="input" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblLanguage" meta:resourceKey="lblLanguage" runat="server" /></div>
                <div class="form_cell">
                    <asp:DropDownList ID="ddlLanguages" runat="server" Width="200px" DataValueField="Key"
                    DataTextField="Value" CssClass="dropdown"/></div>
                <div class="form_row">
                    <asp:CheckBox ID="chkNewsletter" runat="server" Checked="True" meta:resourceKey="chkNewsletter"
                        CssClass="input" />
                </div>
                <div class="form_accept">
                    <asp:Button ID="btnSave" meta:resourceKey="btnSave" runat="server" Width="200px"
                        OnClick="btnSave_Click" CssClass="accept" />
                </div>
            </asp:Panel>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
