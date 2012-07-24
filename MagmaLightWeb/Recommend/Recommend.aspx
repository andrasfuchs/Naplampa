<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recommend.aspx.cs" Inherits="MagmaLightWeb.Recommend.Recommend"
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
                <asp:Label ID="lblThankYou" meta:resourceKey="lblThankYou" runat="server" Visible="false" />
            </div>
            <asp:Panel ID="pnlMessage" runat="server">
                <div class="form_row">
                    <asp:Label ID="lblPlease" meta:resourceKey="lblPlease" runat="server" />
                </div>
                <div class="form_separator">
                    &nbsp;</div>
                <br />
                <div class="form_cell">
                    <asp:Label ID="lblRecommendTo1" meta:resourceKey="lblRecommendTo1" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtRecommendTo1" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblRecommendTo2" meta:resourceKey="lblRecommendTo2" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtRecommendTo2" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblRecommendTo3" meta:resourceKey="lblRecommendTo3" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtRecommendTo3" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblRecommendTo4" meta:resourceKey="lblRecommendTo4" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtRecommendTo4" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblRecommendTo5" meta:resourceKey="lblRecommendTo5" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtRecommendTo5" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblMessage" meta:resourceKey="lblMessage" runat="server" />
                    <asp:RequiredFieldValidator ID="ctlRequiredFieldValidatorMessage" meta:resourceKey="ctlRequiredFieldValidatorMessage"
                        runat="server" ControlToValidate="txtMessage">*</asp:RequiredFieldValidator>
                </div>
                <div class="form_big_cell">
                    <asp:TextBox ID="txtMessage" runat="server" MaxLength="450" Width="260px" Height="100px"
                        TextMode="MultiLine" CssClass="textinput" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblSignature" meta:resourceKey="lblSignature" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtSignature" runat="server" MaxLength="70" Width="260px" CssClass="input" />
                </div>
                <div class="form_accept">
                    <asp:Button ID="btnSend" meta:resourceKey="btnSend" runat="server" Width="200px"
                        OnClick="btnSend_Click" CssClass="accept" />
                </div>
            </asp:Panel>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
