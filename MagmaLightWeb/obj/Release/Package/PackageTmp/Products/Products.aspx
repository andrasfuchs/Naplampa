<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" meta:resourceKey="page"
    AutoEventWireup="true" Culture="auto:hu-HU" UICulture="auto" CodeBehind="Products.aspx.cs"
    Inherits="MagmaLightWeb.Products.Products" %>
<%@ OutputCache Duration="7200" VaryByParam="Language;Code" VaryByCustom="Culture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftBox" runat="server">
  <div class="productcard" id="productCard" runat="server">
   	<div class="productname"><asp:Label ID="lblProductName" runat="server" /></div>
        <!--pénzfisszafizetés KEZD-->

      <div class="moneyback">
        <asp:Label ID="lblGuarantee" meta:resourceKey="lblGuarantee" runat="server" />
        <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
        <div style="clear:both;"></div>

        <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
      </div>
        <!--pénzfisszafizetés VEGE-->
        <!--ár KEZD-->
        <div class="prices">
            <span>
                <asp:Label ID="lblMSRPPrice" meta:resourceKey="lblMSRPPrice" runat="server" />
                <asp:Label ID="lblMSRPPriceText" runat="server" />            
            </span>
            <p>
                <asp:Label ID="lblProductPrice" meta:resourceKey="lblProductPrice" runat="server" ForeColor="#CC0000" />
                <asp:Label ID="lblProductPriceText" runat="server" ForeColor="#CC0000" />
            </p>
            <asp:Label ID="lblSaleRemaining" meta:resourceKey="lblSaleRemaining" runat="server" ForeColor="#CC0000" />
        </div>
        <!--ár VEGE-->        
  </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="RightBox" runat="server">
   <div class="description">
    	<div class="top"></div>
        <div class="center">
        <h1><asp:Label ID="lblProperties" meta:resourceKey="lblProperties" runat="server" /></h1>
        <asp:Label ID="lblProductDescription" runat="server" />
        </div>
        <div class="foot">
        <%--<a href="../Order/Order_step1.aspx"><asp:Label ID="lblOrder" runat="server" meta:resourceKey="lnkOrder"/></a>--%>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MiddleHeader" runat="server">
    <div class="ordernow">
        <a href="../Order/Order_step1.aspx"><asp:Label ID="lblOrder" runat="server" meta:resourceKey="lnkOrder"/></a>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
			<!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
            <div class="insideframe">
            <!--oldalsó termékajánló NEM VÉLETLEN VAN ELÖBB MINT A FEJLÉC NE VÁLTOZTASD MEG!!! KEZD-->
            <div class="product_recommendation">
           	  <div class="header"><asp:Label ID="lblOtherProducts" meta:resourceKey="lblOtherProducts" runat="server" /></div>


    <asp:Panel ID="pnlProducts" runat="server">
        <div class="center">
        	<asp:HyperLink ID="lnkProduct1" runat="server" CssClass="product">
            <asp:Image ID="imgProduct1" runat="server" />
            <span class="data_bg">
            <h1><asp:Label ID="lblProductName1" runat="server" /></h1>
            <h2><asp:Label ID="lblProductOneLiner1" runat="server" /></h2>

            <font><asp:Label ID="lblProductDetails1" runat="server" meta:resourceKey="lblDetails"/></font>
            </span>
            <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            <span style="clear:both; display:block; height:0px"></span>
            <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            </asp:HyperLink>
        </div>
    <!--1 product_console keret VEGE-->    
    <!--2 product_console keret KEZD-->
        <div class="center">
        	<asp:HyperLink ID="lnkProduct2" runat="server" CssClass="product">
            <asp:Image ID="imgProduct2" runat="server" />
            <span class="data_bg">

            <h1><asp:Label ID="lblProductName2" runat="server" /></h1>
            <h2><asp:Label ID="lblProductOneLiner2" runat="server" /></h2>
            <font><asp:Label ID="lblProductDetails2" runat="server" meta:resourceKey="lblDetails"/></font>
            </span>
            <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->
            <span style="clear:both; display:block; height:0px"></span>
            <!--SZÜKSÉGES ROSSZ NE TÖRÖLD KI-->

            </asp:HyperLink>
        </div>
    <!--2 product_console keret VEGE-->
    </asp:Panel>
    </div>

             <!--oldalsó termékajánló NEM VÉLETLEN VAN ELÖBB MINT A FEJLÉC NE VÁLTOZTASD MEG!!! VEGE-->
            <!--oldalfejlec KEZD-->

            <div class="header"><asp:Label ID="lblDetailedDescription" meta:resourceKey="lblDetailedDescription" runat="server" /></div>
            <!--oldalfejlec VEGE-->
            <!--innentol jon a szöveges tartalom KEZD-->
            <p><strong><asp:Label ID="lblFullSpectrumHeader" meta:resourceKey="lblFullSpectrumHeader" runat="server" /></strong></p>
            <asp:Label ID="lblFullSpectrumText" meta:resourceKey="lblFullSpectrumText" runat="server" />
            
            <p><asp:HyperLink ID="lnkLinks1" meta:resourceKey="lnkLinks" runat="server" NavigateUrl="../links/links.aspx" /></p>
            <p>&nbsp;</p>

            <asp:Panel ID="pnlIonizer" runat="server">
            <p><strong><asp:Label ID="lblIonizerHeader" meta:resourceKey="lblIonizerHeader" runat="server" /></strong></p>
            <asp:Label ID="lblIonizerText" meta:resourceKey="lblIonizerText" runat="server" />

            <p><asp:HyperLink ID="lnkLinks2" meta:resourceKey="lnkLinks" runat="server" NavigateUrl="../links/links.aspx" /></p>
            <p>&nbsp;</p>
            </asp:Panel>

            <p><strong><asp:Label ID="lblHealthHeader" meta:resourceKey="lblHealthHeader" runat="server" /></strong></p>
            <asp:Label ID="lblHealthText" meta:resourceKey="lblHealthText" runat="server" />

            <p><asp:HyperLink ID="lnkLinks3" meta:resourceKey="lnkLinks" runat="server" NavigateUrl="../links/links.aspx" /></p>
            <p>&nbsp;</p>

            <p><strong><asp:Label ID="lblDepressionHeader" meta:resourceKey="lblDepressionHeader" runat="server" /></strong></p>
            <asp:Label ID="lblDepressionText" meta:resourceKey="lblDepressionText" runat="server" />

            <p><asp:HyperLink ID="lnkLinks4" meta:resourceKey="lnkLinks" runat="server" NavigateUrl="../links/links.aspx" /></p>
            <p>&nbsp;</p>

            <p><strong><asp:Label ID="lblEconomyHeader" meta:resourceKey="lblEconomyHeader" runat="server" /></strong></p>
            <asp:Label ID="lblEconomyText" meta:resourceKey="lblEconomyText" runat="server" />

            <p><asp:HyperLink ID="lnkLinks5" meta:resourceKey="lnkLinks" runat="server" NavigateUrl="../links/links.aspx" /></p>
            <p>&nbsp;</p>
            <!--innentol jon a szöveges tartalom VEGE-->
            
            <!--SZUKSEGES ROSSZ NE TOROLD KEZD-->
            <div style="clear:both;"></div>
            <!--SZUKSEGES ROSSZ NE TOROLD VEGE-->
            </div>
            <!--BELSO OLDALAKON HASZNÁLATOS KERET VEGE-->
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="MiddleFooter" runat="server">
    <div style="clear:both;"></div>
    <div class="ordernow">
        <a href="../Order/Order_step1.aspx"><asp:Label ID="Label1" runat="server" meta:resourceKey="lnkOrder"/></a>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="endOfPage" runat="server">
</asp:Content>
