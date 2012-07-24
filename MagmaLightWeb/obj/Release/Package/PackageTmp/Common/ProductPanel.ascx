<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductPanel.ascx.cs"
    Inherits="MagmaLightWeb.Common.ProductPanel" %>


<!--1 product_console keret KEZD-->
<asp:Panel ID="pnlProducts" runat="server">
    <div class="console">
        <div class="top">
        </div>
        <div class="center">
            <asp:HyperLink ID="lnkProduct1" runat="server" CssClass="product">
                <asp:Image ID="imgProduct1" runat="server" />
                <span class="data_bg">
                    <h1>
                        <asp:Label ID="lblProductName1" runat="server" /></h1>
                    <h2>
                        <asp:Label ID="lblProductOneLiner1" runat="server" /></h2>
                    <font>
                        <asp:Label ID="lblProductDetails1" runat="server" meta:resourceKey="lblDetails" /></font>
                </span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
                <span style="clear: both; display: block; height: 0px"></span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            </asp:HyperLink>
        </div>
        <div class="foot">
        </div>
    </div>
    <!--1 product_console keret VEGE-->
    <!--2 product_console keret KEZD-->
    <div class="console">
        <div class="top">
        </div>
        <div class="center">
            <asp:HyperLink ID="lnkProduct2" runat="server" CssClass="product">
                <asp:Image ID="imgProduct2" runat="server" />
                <span class="data_bg">
                    <h1>
                        <asp:Label ID="lblProductName2" runat="server" /></h1>
                    <h2>
                        <asp:Label ID="lblProductOneLiner2" runat="server" /></h2>
                    <font>
                        <asp:Label ID="lblProductDetails2" runat="server" meta:resourceKey="lblDetails" /></font>
                </span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
                <span style="clear: both; display: block; height: 0px"></span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            </asp:HyperLink>
        </div>
        <div class="foot">
        </div>
    </div>
    <!--2 product_console keret VEGE-->
    <!--3 product_console keret KEZD-->
    <div class="console">
        <div class="top">
        </div>
        <div class="center">
            <asp:HyperLink ID="lnkProduct3" runat="server" CssClass="product">
                <asp:Image ID="imgProduct3" runat="server" />
                <span class="data_bg">
                    <h1>
                        <asp:Label ID="lblProductName3" runat="server" /></h1>
                    <h2>
                        <asp:Label ID="lblProductOneLiner3" runat="server" /></h2>
                    <font>
                        <asp:Label ID="lblProductDetails3" runat="server" meta:resourceKey="lblDetails" /></font>
                </span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
                <span style="clear: both; display: block; height: 0px"></span>
                <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            </asp:HyperLink>
        </div>
        <div class="foot">
        </div>
    </div>
    <!--3 product_console keret VEGE-->
</asp:Panel>
