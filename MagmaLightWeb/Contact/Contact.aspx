<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MagmaLightWeb.Contact.Contact" 
    Culture="auto:hu-HU" UICulture="auto" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>
<%@ Register src="../Common/ProductPanel.ascx" tagname="ProductPanel" tagprefix="uc1" %>


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
            <div class="form_container_header">
                <asp:Label ID="lblQA" meta:resourceKey="lblQA" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuestion1" meta:resourceKey="lblQuestion1" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer1" meta:resourceKey="lblAnswer1" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuestion2" meta:resourceKey="lblQuestion2" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer2" meta:resourceKey="lblAnswer2" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuestion3" meta:resourceKey="lblQuestion3" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer3" meta:resourceKey="lblAnswer3" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuestion4" meta:resourceKey="lblQuestion4" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer4" meta:resourceKey="lblAnswer4" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblQuestion5" meta:resourceKey="lblQuestion5" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer5" meta:resourceKey="lblAnswer5" runat="server" /></div>
<%--        <div class="form_row">
                <asp:Label ID="lblQuestion6" meta:resourceKey="lblQuestion6" runat="server" /></div>
            <div class="form_row_answer">
                <asp:Label ID="lblAnswer6" meta:resourceKey="lblAnswer6" runat="server" /></div>
--%>
            <div class="form_container_header">
                <asp:Label ID="lblSendMessage" meta:resourceKey="lblSendMessage" runat="server" /></div>
            <div class="form_row">
                <asp:Label ID="lblThankYou" meta:resourceKey="lblThankYou" runat="server" Visible="false"/></div>
            <asp:Panel ID="pnlSendMessage" runat="server">
            <div class="form_cell">
                <asp:Label ID="lblSenderEmail" meta:resourceKey="lblSenderEmail" runat="server" /></div>
            <div class="form_cell">
                <asp:TextBox ID="txtSenderEmail" runat="server" MaxLength="30" Width="260px" CssClass="input" /></div>
            <div class="form_cell">
                <asp:Label ID="lblMessage" meta:resourceKey="lblMessage" runat="server" /></div>
            <div class="form_big_cell">
                <asp:TextBox ID="txtMessage" runat="server" Height="172px" MaxLength="500" TextMode="MultiLine"
                        Width="255px" CssClass="textinput"/></div>
                <div class="form_accept">
                    <asp:Button ID="btnSend" meta:resourceKey="btnSend" runat="server" Width="200px"
                        OnClick="btnSend_Click" CssClass="accept" />
                </div>                                
            </asp:Panel>
            <div class="form_container_header">
                <asp:Label ID="lblCountryContactPerson" meta:resourceKey="lblCountryContactPerson" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblContactName" meta:resourceKey="lblContactName" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblContactNameText" meta:resourceKey="lblContactNameText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblContactEmail" meta:resourceKey="lblContactEmail" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblContactEmailText" meta:resourceKey="lblContactEmailText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblContactPhone" meta:resourceKey="lblContactPhone" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblContactPhoneText" meta:resourceKey="lblContactPhoneText" runat="server" /></div>
            <div class="form_cell">
                <asp:Label ID="lblContactFax" meta:resourceKey="lblContactFax" runat="server" /></div>
            <div class="form_cell_value">
                <asp:Label ID="lblContactFaxText" meta:resourceKey="lblContactFaxText" runat="server" /></div>                
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
