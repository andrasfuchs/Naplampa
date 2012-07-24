<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order_step2.aspx.cs" Inherits="MagmaLightWeb.Order.Order2"
    MasterPageFile="~/MasterPage.master" meta:resourceKey="page" Culture="auto:hu-HU"
    UICulture="auto" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblOrder" meta:resourceKey="lblOrder" runat="server" /></div>
        <!--oldalfejlec VEGE-->
        <!--innentol jon a megrendelési űrlap KEZD-->
        <asp:Panel ID="pnlOrderStep2" runat="server">
            <div class="form_frame">
                <div class="form_cell" ID="divLastNameLabel" runat="server">
                    <asp:Label ID="lblLastName" meta:resourceKey="lblLastName" runat="server" /></div>
                <div class="form_cell" ID="divLastName" runat="server">
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" Width="260px" CssClass="input" /></div>
                <div class="form_help" ID="divLastName_Error" runat="server" visible="false">
                    <asp:Label ID="lblLastName_Error" meta:resourceKey="lblLastName_Error" runat="server"/></div>
                <div class="form_cell" ID="divFirstNameLabel" runat="server">
                    <asp:Label ID="lblFirstName" meta:resourceKey="lblFirstName" runat="server" /></div>
                <div class="form_cell" ID="divFirstName" runat="server">
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" Width="260px" CssClass="input"/></div>
                <div class="form_help" ID="divFirstName_Error" runat="server" visible="false">
                    <asp:Label ID="lblFirstName_Error" meta:resourceKey="lblFirstName_Error" runat="server"/></div>
                <div class="form_cell" ID="divEmailLabel" runat="server">
                    <asp:Label ID="lblEmail" meta:resourceKey="lblEmail" runat="server" /></div>
                <div class="form_cell" ID="divEmail" runat="server">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="260px" CssClass="input"/></div>
                <div class="form_help" ID="divEmail_Error_RequiredField" runat="server" visible="false">
                    <asp:Label ID="lblEmail_Error_RequiredField" meta:resourceKey="lblEmail_Error_RequiredField" runat="server"/></div>
                <div class="form_help" ID="divEmail_Error_RegularExpression" runat="server" visible="false">
                    <asp:Label ID="lblEmail_Error_RegularExpression" meta:resourceKey="lblEmail_Error_RegularExpression" runat="server"/></div>
                <div class="form_cell">
                    <asp:Label ID="lblPhone" meta:resourceKey="lblPhone" runat="server" /></div>
                <div class="form_cell">
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" Width="260px" CssClass="input"/></div>
                <div class="form_container_header">
                    <asp:Label ID="lblAddress" meta:resourceKey="lblAddress" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblCountry" meta:resourceKey="lblCountry" runat="server" /></div>
                <div class="form_cell">
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="260px" DataValueField="CountryId"
                        DataTextField="Name" />
                </div>
                <div class="form_cell" ID="divTownLabel" runat="server">
                    <asp:Label ID="lblTown" meta:resourceKey="lblTown" runat="server" /></div>
                <div class="form_cell" ID="divTown" runat="server">
                    <asp:TextBox ID="txtTown" runat="server" MaxLength="50" Width="260px" CssClass="input"/></div>
                <div class="form_help" ID="divTown_Error" runat="server" visible="false">
                    <asp:Label ID="lblTown_Error" meta:resourceKey="lblTown_Error" runat="server"/></div>
                <div class="form_cell" ID="divPostalCodeLabel" runat="server">
                    <asp:Label ID="lblPostalCode" meta:resourceKey="lblPostalCode" runat="server" /></div>
                <div class="form_cell" ID="divPostalCode" runat="server">
                    <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" Width="260px" CssClass="input"/></div>
                <div class="form_help" ID="divPostalCode_Error_RequiredField" runat="server" visible="false">
                    <asp:Label ID="lblPostalCode_Error_RequiredField" meta:resourceKey="lblPostalCode_Error_RequiredField" runat="server"/></div>                    
                <div class="form_help" ID="divPostalCode_Error_RegularExpression" runat="server" visible="false">
                    <asp:Label ID="lblPostalCode_Error_RegularExpression" meta:resourceKey="lblPostalCode_Error_RegularExpression" runat="server"/></div>                    
                <div class="form_cell" ID="divAddressLineLabel" runat="server">                
                    <asp:Label ID="lblAddressLine" meta:resourceKey="lblAddressLine" runat="server" /></div>
                <div class="form_big_cell" ID="divAddressLine" runat="server">
                    <asp:TextBox ID="txtAddressLine" runat="server" Height="44px" MaxLength="80" TextMode="MultiLine"
                        Width="255px" CssClass="textinput"/></div>
                <div class="form_help" ID="divAddressLine_Error" runat="server" visible="false">
                    <asp:Label ID="lblAddressLine_Error" meta:resourceKey="lblAddressLine_Error" runat="server"/></div>                        
                <div class="form_container_header">
                    <asp:Label ID="lblInvoice" meta:resourceKey="lblInvoice" runat="server" /></div>
                <div class="form_cell">
                    <asp:Label ID="lblInvoiceName" meta:resourceKey="lblInvoiceName" runat="server" /></div>
                <div class="form_cell">
                    <asp:TextBox ID="txtInvoiceName" meta:resourceKey="txtInvoiceName" runat="server"
                        MaxLength="80" TextMode="SingleLine" Width="260px" CssClass="input"/></div>
                <div class="form_cell">
                    <asp:Label ID="lblInvoiceCountry" meta:resourceKey="lblInvoiceCountry" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:DropDownList ID="ddlInvoiceCountry" runat="server" Width="260px" DataValueField="CountryId"
                        DataTextField="Name" />
                </div>
                <div class="form_cell" ID="divInvoiceTownLabel" runat="server">
                    <asp:Label ID="lblInvoiceTown" meta:resourceKey="lblInvoiceTown" runat="server" />
                </div>
                <div class="form_cell" ID="divInvoiceTown" runat="server">
                    <asp:TextBox ID="txtInvoiceTown" runat="server" MaxLength="50" Width="260px" CssClass="input"/>
                </div>
                <div class="form_help" ID="divInvoiceTown_Error" runat="server" visible="false">
                    <asp:Label ID="lblInvoiceTown_Error" meta:resourceKey="lblInvoiceTown_Error" runat="server"/></div>                                        
                <div class="form_cell" ID="divInvoicePostalCodeLabel" runat="server">
                    <asp:Label ID="lblInvoicePostalCode" meta:resourceKey="lblInvoicePostalCode" runat="server" />
                </div>
                <div class="form_cell" ID="divInvoicePostalCode" runat="server">
                    <asp:TextBox ID="txtInvoicePostalCode" runat="server" MaxLength="10" Width="260px" CssClass="input"/>
                </div>
                <div class="form_help" ID="divInvoicePostalCode_Error_RegularExpression" runat="server" visible="false">
                    <asp:Label ID="lblInvoicePostalCode_Error_RegularExpression" meta:resourceKey="lblInvoicePostalCode_Error_RegularExpression" runat="server"/></div>
                <div class="form_cell" ID="divInvoiceAddressLineLabel" runat="server">
                    <asp:Label ID="lblInvoiceAddressLine" meta:resourceKey="lblInvoiceAddressLine"
                        runat="server" />
                </div>
                <div class="form_big_cell" ID="divInvoiceAddressLine" runat="server">
                    <asp:TextBox ID="txtInvoiceAddressLine" runat="server" Height="44px" MaxLength="80"
                        TextMode="MultiLine" Width="255px" CssClass="textinput" />
                </div>
                <div class="form_help" ID="divInvoiceAddressLine_Error" runat="server" visible="false">
                    <asp:Label ID="lblInvoiceAddressLine_Error" meta:resourceKey="lblInvoiceAddressLine_Error" runat="server"/></div>                                        
                <div class="form_separator">
                    &nbsp;</div>
                <div class="form_cell">
                    <asp:Label ID="lblReferer" meta:resourceKey="lblReferer" runat="server" />
                </div>
                <div class="form_cell">
                    <asp:TextBox ID="txtReferer" runat="server" MaxLength="50" Width="260px" CssClass="input"/>
                </div>
                <div class="form_row">
                    <asp:CheckBox ID="chkNewsletter" runat="server" Checked="True" meta:resourceKey="chkNewsletter" CssClass="input"/>
                </div>
                <div class="form_accept">
                    <asp:Button ID="btnOrder" meta:resourceKey="btnOrder" runat="server" Width="200px"
                        OnClick="btnOrder_Click" CssClass="accept" />
                </div>
                <!--div nyujto KEZD-->
                <div style="clear: both;">
                </div>
                <!--div nyujto VEGE-->
            </div>
            <!--megrendelési űrlap VEGE-->
        </asp:Panel>
        <!--SZUKSEGES ROSSZ NE TOROLD KEZD-->
        <div style="clear: both;">
        </div>
        <!--SZUKSEGES ROSSZ NE TOROLD VEGE-->
    </div>
</asp:Content>
