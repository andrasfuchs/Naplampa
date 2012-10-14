<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="MagmaLightWeb.Feedback.Feedback"
    Culture="auto:hu-HU" UICulture="auto" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>
<%@ Register src="../Common/ProductPanel.ascx" tagname="ProductPanel" tagprefix="uc1" %>
<%@ OutputCache Duration="86400" VaryByParam="Language" VaryByCustom="Culture" %>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftBox" runat="server">
    <div class="defaultpic">
        &nbsp;</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightBox" runat="server">
    <uc1:ProductPanel ID="ProductPanel1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">

            <div class="form_row">
                <asp:Label ID="lblSummary" meta:resourceKey="lblSummary" runat="server" /></div>

            <div class="form_container_header">
                <asp:Label ID="lblCurrentStatistics" meta:resourceKey="lblCurrentStatistics" runat="server" /></div>

            <div class="form_cell">
                <asp:Label ID="lblServiceSatisfaction" meta:resourceKey="lblServiceSatisfaction" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblServiceSatisfactionText" meta:resourceKey="lblServiceSatisfactionText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblProductSatisfaction" meta:resourceKey="lblProductSatisfaction" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblProductSatisfactionText" meta:resourceKey="lblProductSatisfactionText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblSpeedSatisfaction" meta:resourceKey="lblSpeedSatisfaction" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblSpeedSatisfactionText" meta:resourceKey="lblSpeedSatisfactionText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblWebSiteSatisfaction" meta:resourceKey="lblWebSiteSatisfaction" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblWebSiteSatisfactionText" meta:resourceKey="lblWebSiteSatisfactionText" runat="server" /></div>

            <div class="form_container_header">
                <asp:Label ID="lblQuotes" meta:resourceKey="lblQuotes" runat="server" /></div>

            <div class="form_row">
                <asp:Label ID="lblQuote1" meta:resourceKey="lblQuote1" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuote2" meta:resourceKey="lblQuote2" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuote3" meta:resourceKey="lblQuote3" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuote4" meta:resourceKey="lblQuote4" runat="server" /></div>
        </div>
        <div style="clear: both;">
        </div>        
    </div>
</asp:Content>
