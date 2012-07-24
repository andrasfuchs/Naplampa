<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order_step1.aspx.cs" Inherits="MagmaLightWeb.Order.Order1"
    MasterPageFile="~/MasterPage.master" meta:resourceKey="page" Culture="auto:hu-HU"
    UICulture="auto" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header"><asp:Label ID="lblOrder" meta:resourceKey="lblOrder" runat="server" /></div>
        <!--oldalfejlec VEGE-->
        <!--innentol jon a megrendelési űrlap KEZD-->
        <div class="form_frame">
            <div class="form_row">
                <asp:Label ID="lblHolidays" meta:resourceKey="lblHolidays" runat="server" ForeColor="Red" Font-Bold="true" />
            </div>
            <div class="form_row">
                <asp:Label ID="lblQuantityDiscountInfo" meta:resourceKey="lblQuantityDiscountInfo"
                    runat="server" ForeColor="Red" />
            </div>
            <div class="form_separator">
                &nbsp;</div>

            <asp:Panel ID="pnlProducts" runat="server">
                <div class="form_product_thumbnail_cell">
                            <asp:Image ID="imgProduct1" runat="server" Width="120px" Height="40px" />
                </div>
                <div class="form_product_cell">                            
                            <asp:Label ID="lblProduct1" runat="server" />
                </div>
                <div class="form_product_small_cell">
                
                            <asp:Button ID="btnProd1Minus" runat="server" Text="-" OnClick="btnProd1Minus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                            <asp:TextBox ID="txtProduct1" runat="server" MaxLength="3" Width="31px" Wrap="False"
                                AutoPostBack="true">0</asp:TextBox>
                            <asp:Button ID="btnProd1Plus" runat="server" Text="+" OnClick="btnProd1Plus_Click"
                                CausesValidation="false" CssClass="pmbutton" />

                </div>
                <div class="form_separator_minor">&nbsp;</div>
                <div class="form_product_thumbnail_cell">
                            <asp:Image ID="imgProduct2" runat="server" Width="120px" Height="40px" />
                </div>
                <div class="form_product_cell">                            
                            <asp:Label ID="lblProduct2" runat="server" />
                            <br />
                            <asp:Label ID="lblProduct4" runat="server" />
                </div>
                <div class="form_product_small_cell">

                            <asp:Button ID="btnProd2Minus" runat="server" Text="-" OnClick="btnProd2Minus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                            <asp:TextBox ID="txtProduct2" runat="server" MaxLength="3" Width="31px" Wrap="False"
                                AutoPostBack="true">0</asp:TextBox>
                            <asp:Button ID="btnProd2Plus" runat="server" Text="+" OnClick="btnProd2Plus_Click"
                                CausesValidation="false" CssClass="pmbutton" />

                            <br />

                            <asp:Button ID="btnProd4Minus" runat="server" Text="-" OnClick="btnProd4Minus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                            <asp:TextBox ID="txtProduct4" runat="server" MaxLength="3" Width="31px" Wrap="False"
                                AutoPostBack="true">0</asp:TextBox>
                            <asp:Button ID="btnProd4Plus" runat="server" Text="+" OnClick="btnProd4Plus_Click"
                                CausesValidation="false" CssClass="pmbutton" />

                </div>
                <div class="form_separator_minor">&nbsp;</div>
                <div class="form_product_thumbnail_cell">

                            <asp:Image ID="imgProduct3" runat="server" Width="120px" Height="40px" />
                </div>
                <div class="form_product_cell">                            
                            <asp:Label ID="lblProduct3" runat="server" />
                            <br />
                            <asp:Label ID="lblProduct5" runat="server" />
                </div>
                <div class="form_product_small_cell">

                            <asp:Button ID="btnProd3Minus" runat="server" Text="-" OnClick="btnProd3Minus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                            <asp:TextBox ID="txtProduct3" runat="server" MaxLength="3" Width="31px" Wrap="False"
                                AutoPostBack="true">0</asp:TextBox>
                            <asp:Button ID="btnProd3Plus" runat="server" Text="+" OnClick="btnProd3Plus_Click"
                                CausesValidation="false" CssClass="pmbutton" />

                            <br />

                            <asp:Button ID="btnProd5Minus" runat="server" Text="-" OnClick="btnProd5Minus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                            <asp:TextBox ID="txtProduct5" runat="server" MaxLength="3" Width="31px" Wrap="False"
                                AutoPostBack="true">0</asp:TextBox>
                            <asp:Button ID="btnProd5Plus" runat="server" Text="+" OnClick="btnProd5Plus_Click"
                                CausesValidation="false" CssClass="pmbutton" />
                </div>
            </asp:Panel>
            <br />
            <div class="form_separator">
                &nbsp;</div>
            <br />
            <div class="form_cell">
                <asp:Label ID="lblPackageDestination" meta:resourceKey="lblPackageDestination" runat="server" />
            </div>
            <div class="form_cell">
                <asp:DropDownList ID="ddlCountry" runat="server" Width="200px" DataValueField="CountryId"
                    DataTextField="Name" AutoPostBack="true" CssClass="dropdown"/>
            </div>
            <div class="form_cell">
                <asp:Label ID="lblPackageWeight" meta:resourceKey="lblPackageWeight" runat="server" />
            </div>
            <div class="form_cell_value">
                <asp:Label ID="lblPackageWeightText" meta:resourceKey="lblPackageWeightText" runat="server" />
                &nbsp;
            </div>
            <div class="form_cell">
                <asp:Label ID="lblCurrency" meta:resourceKey="lblCurrency" runat="server" />
            </div>
            <div class="form_cell">
                <asp:DropDownList ID="ddlCurrency" runat="server" Width="200px" DataValueField="CurrencyId"
                    DataTextField="Name" AutoPostBack="true" CssClass="dropdown"/>
            </div>
            <div class="form_cell">
                <asp:Label ID="lblPaymentMethod" meta:resourceKey="lblPaymentMethod" runat="server" />
            </div>
            <div class="form_big_cell">
                <asp:RadioButtonList ID="rblPayment" runat="server" AutoPostBack="true" Width="350px" CellSpacing="0">
                    <asp:ListItem Value="1" meta:resourceKey="lblPaymentMethodBankTransfer" Selected="true" />
                    <asp:ListItem Value="2" meta:resourceKey="lblPaymentMethodPayPal" />
                    <asp:ListItem Value="3" meta:resourceKey="lblPaymentMethodDebitCard" />
                    <asp:ListItem Value="4" meta:resourceKey="lblPaymentMethodPost" />
                </asp:RadioButtonList>
            </div>
            <div class="form_cell">
                <asp:Label ID="lblCoupon" meta:resourceKey="lblCoupon" runat="server" />
            </div>
            <div class="form_cell">
                <asp:TextBox ID="txtCoupon" runat="server" MaxLength="30" Width="260px" CssClass="input" AutoPostBack="true"/>
            </div>
            <div class="form_separator">
                &nbsp;</div>
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
                    Font-Size="Larger" />
            </div>

            <div class="form_accept">
                <asp:Button ID="btnContinue" meta:resourceKey="btnContinue" runat="server" Width="200px"
                    OnClick="btnContinue_Click" CssClass="accept" />
            </div>
            <!--div nyujto KEZD-->
            <div style="clear: both;">
            </div>
            <!--div nyujto VEGE-->
        </div>
        <!--megrendelési űrlap VEGE-->
        <!--SZUKSEGES ROSSZ NE TOROLD KEZD-->
        <div style="clear: both;">
        </div>
        <!--SZUKSEGES ROSSZ NE TOROLD VEGE-->
    </div>
</asp:Content>
