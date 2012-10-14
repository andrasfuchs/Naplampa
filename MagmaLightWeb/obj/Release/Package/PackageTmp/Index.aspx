<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" AutoEventWireup="true" Inherits="MagmaLightWeb.Index" Culture="auto:hu-HU" UICulture="auto" CodeBehind="Index.aspx.cs" %>

<%@ OutputCache Duration="86400" VaryByParam="Language" VaryByCustom="Culture" %>
<%@ Register Src="Common/ProductPanel.ascx" TagName="ProductPanel" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBox" runat="server">
    <div class="landingpic" id="divLandingPic" runat="server">
        &nbsp;</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightBox" runat="server">
    <uc1:ProductPanel ID="ProductPanel1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MiddleHeader" runat="server">
    <div class="work_frame">
        <div class="top">
        </div>
        <div class="center">
            <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
            <div class="insideframe">
                <!--oldalfejlec KEZD-->
                <div class="header">
                    <asp:Label ID="lblShortDescription" meta:resourceKey="lblShortDescription" runat="server" /></div>
                <div class="form_frame">
                    <div class="form_row">
                        <asp:Label ID="lblShortDescriptionText" meta:resourceKey="lblShortDescriptionText" runat="server" /></div>
                </div>
                <div style="clear: both;">
                </div>
            </div>
        </div>
        <div class="foot">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">
            <%--            <div class="form_container_header">
                <asp:Label ID="lblNewsHead1" meta:resourceKey="lblNewsHead1" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow1" meta:resourceKey="lblNewsRow1" runat="server" /></div>
            --%>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead2" meta:resourceKey="lblNewsHead2" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow2" meta:resourceKey="lblNewsRow2" runat="server" /></div>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead3" meta:resourceKey="lblNewsHead3" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow3" meta:resourceKey="lblNewsRow3" runat="server" /></div>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead4" meta:resourceKey="lblNewsHead4" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow4" meta:resourceKey="lblNewsRow4" runat="server" /></div>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead5" meta:resourceKey="lblNewsHead5" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow5" meta:resourceKey="lblNewsRow5" runat="server" /></div>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead6" meta:resourceKey="lblNewsHead6" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow6" meta:resourceKey="lblNewsRow6" runat="server" /></div>
            <div class="form_container_header">
                <asp:Label ID="lblNewsHead7" meta:resourceKey="lblNewsHead7" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblNewsRow7" meta:resourceKey="lblNewsRow7" runat="server" /></div>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
