<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmEmail.aspx.cs" Inherits="MagmaLightWeb.Admin.ConfirmEmail"
    Culture="auto:hu-HU" UICulture="auto" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">
            <div class="form_row">
                <asp:Label ID="lblError" meta:resourceKey="lblError" runat="server" ForeColor="Red"
                    Visible="false" />
                <asp:Label ID="lblAlready" meta:resourceKey="lblAlready" runat="server" Visible="false" />
                <asp:Label ID="lblSuccess" meta:resourceKey="lblSuccess" runat="server" />
            </div>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
