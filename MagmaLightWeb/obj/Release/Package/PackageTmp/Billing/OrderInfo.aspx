<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" meta:resourceKey="page"
    AutoEventWireup="true" Inherits="MagmaLightWeb.Billing.OrderInfo" Culture="auto:hu-HU"
    UICulture="auto" CodeBehind="OrderInfo.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblOrderInfo" meta:resourceKey="lblOrderInfo" runat="server" /></div>
        <div class="form_frame">
            <div class="form_row">
                <asp:Label ID="lblError" meta:resourceKey="lblError" runat="server" ForeColor="Red"
                    Visible="false" /></div>
            <asp:Panel runat="server" ID="pnlDetails">
                <div class="form_container_header">
                    <asp:Label ID="lblBuyer" meta:resourceKey="lblBuyer" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblOrderId" meta:resourceKey="lblOrderId" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblOrderIdValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblStatus" meta:resourceKey="lblStatus" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblStatusText" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblTitle" meta:resourceKey="lblTitle" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblTitleValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblFirstName" meta:resourceKey="lblFirstName" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblFirstNameValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblLastName" meta:resourceKey="lblLastName" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblLastNameValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblPaymentMethod" meta:resourceKey="lblPaymentMethod" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblPaymentMethodValue" runat="server" /></div>
                <div class="form_container_header">
                    <asp:Label ID="lblDeliveryAddress" meta:resourceKey="lblDeliveryAddress" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblAddressName" meta:resourceKey="lblAddressName" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblAddressNameValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblAddressLine" meta:resourceKey="lblAddressLine" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblAddressLineValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblTown" meta:resourceKey="lblTown" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblTownValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblPostalCode" meta:resourceKey="lblPostalCode" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblPostalCodeValue" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblCountry" meta:resourceKey="lblCountry" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblCountryValue" runat="server" /></div>
                <div class="form_container_header">
                    <asp:Label ID="lblProducts" meta:resourceKey="lblProducts" runat="server" /></div>
                    
                <asp:Panel ID="pnlProducts" runat="server">
                    <div class="form_product_thumbnail_cell">
                        <asp:Image ID="imgProduct1" runat="server" /></div>
                    <div class="form_product_cell">
                        <asp:Label ID="lblProduct1" runat="server" /></div>
                    <div class="form_product_small_cell">
                        <asp:TextBox ID="txtProduct1" runat="server" MaxLength="3" Width="31px" Wrap="False"
                            ReadOnly="true">0</asp:TextBox></div>
                    <div class="form_product_thumbnail_cell">
                        <asp:Image ID="imgProduct2" runat="server" /></div>
                    <div class="form_product_cell">
                        <asp:Label ID="lblProduct2" runat="server" />
                        <br />
                        <asp:Label ID="lblProduct4" runat="server" /></div>
                    <div class="form_product_small_cell">
                        <asp:TextBox ID="txtProduct2" runat="server" MaxLength="3" Width="31px" Wrap="False"
                            ReadOnly="true">0</asp:TextBox>
                        <br />
                        <asp:TextBox ID="txtProduct4" runat="server" MaxLength="3" Width="31px" Wrap="False"
                            ReadOnly="true">0</asp:TextBox></div>
                    <div class="form_product_thumbnail_cell">
                        <asp:Image ID="imgProduct3" runat="server" /></div>
                    <div class="form_product_cell">
                        <asp:Label ID="lblProduct3" runat="server" />
                        <br />
                        <asp:Label ID="lblProduct5" runat="server" /></div>
                    <div class="form_product_small_cell">
                        <asp:TextBox ID="txtProduct3" runat="server" MaxLength="3" Width="31px" Wrap="False"
                            ReadOnly="true">0</asp:TextBox>
                            <br />
                        <asp:TextBox ID="txtProduct5" runat="server" MaxLength="3" Width="31px" Wrap="False"
                            ReadOnly="true">0</asp:TextBox>
                            </div>
                </asp:Panel>
                        
                <div class="form_container_header">
                    <asp:Label ID="lblCosts" meta:resourceKey="lblCosts" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblProductsSum" meta:resourceKey="lblProductsSum" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblProductsSumText" runat="server" />
                </div>                    
                <div class="form_cell">
                    <asp:Label ID="lblTransactionCost" meta:resourceKey="lblTransactionCost" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblTransactionCostText" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblPostCost" meta:resourceKey="lblPostCost" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblPostCostText" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblPackageCost" meta:resourceKey="lblPackageCost" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblPackageCostText" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblQuantityDiscount" meta:resourceKey="lblQuantityDiscount" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblQuantityDiscountText" runat="server" ForeColor="Red" />
                </div>
                <div class="form_cell">
                    <asp:Label ID="lblCouponDiscount" meta:resourceKey="lblCouponDiscount" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblCouponDiscountText" runat="server" ForeColor="Red" />
                </div>                
                <div class="form_cell">
                    <asp:Label ID="lblTotalCosts" meta:resourceKey="lblTotalCosts" runat="server" />
                </div>
                <div class="form_cell_value">
                    <asp:Label ID="lblTotalCostsText" runat="server" ForeColor="Green" Font-Bold="true"
                        Font-Size="Larger" /></div>
<%--
                <div class="form_accept">
                    <asp:Button runat="server" ID="btnInvoice" meta:resourceKey="btnInvoice" OnClick="btnInvoice_OnClick"
                        CssClass="accept" />
                </div>
                <div class="form_row">
                    <asp:Label ID="lblInvoiceInfo" meta:resourceKey="lblInvoiceInfo" runat="server" ForeColor="Red" />
                </div>
--%>                
            </asp:Panel>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
