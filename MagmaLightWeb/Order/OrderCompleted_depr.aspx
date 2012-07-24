<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" meta:resourceKey="page"
    AutoEventWireup="true" CodeBehind="OrderCompleted_depr.aspx.cs" Inherits="MagmaLightWeb.Order.OrderCompleted_depr"
    Culture="auto:hu-HU" UICulture="auto" %>

<%@ Register Src="../Common/ProductPanel.ascx" TagName="ProductPanel" TagPrefix="uc1" %>
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
            <asp:Label ID="lblOrder" meta:resourceKey="lblOrder" runat="server" />
        </div>
        <p>
            <asp:Label ID="lblThankYou" meta:resourceKey="lblThankYou" runat="server" /></p>
        <p>
            <asp:Label ID="lblAutoConfirm" meta:resourceKey="lblAutoConfirm" runat="server" /></p>
        <p style="color: Red">
            <asp:Label ID="lblPayPalError" meta:resourceKey="lblPayPalError" runat="server" Visible="False"/></p>
        </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="endOfPage" runat="server">
    <input type="hidden" id="orderID" runat="server" />
    <input type="hidden" id="orderTotal" runat="server" />
    <input type="hidden" id="es002Price" runat="server" />
    <input type="hidden" id="es002Quantity" runat="server" />
    <input type="hidden" id="es004Price" runat="server" />
    <input type="hidden" id="es004Quantity" runat="server" />
    <input type="hidden" id="es015Price" runat="server" />
    <input type="hidden" id="es015Quantity" runat="server" />

    <!-- Google Code for Bulb Purchase v3 Conversion Page -->
    <script type="text/javascript">
    <!--
        var orderTotal = document.all['ctl00$endOfPage$orderTotal'].value;
        var google_conversion_id = 1049591349;
        var google_conversion_language = "en";
        var google_conversion_format = "3";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "agA-COH0rQEQtfy99AM";
        var google_conversion_value = 0;
        if (orderTotal) {
            google_conversion_value = orderTotal;
        }
    //-->
    </script>
    <script type="text/javascript" src="http://www.googleadservices.com/pagead/conversion.js">
    </script>

    <noscript>
    <div style="display:inline;">
    <img height="1" width="1" style="border-style:none;" alt="" src="http://www.googleadservices.com/pagead/conversion/1049591349/?value=15000&amp;label=agA-COH0rQEQtfy99AM&amp;guid=ON&amp;script=0"/>
    </div>
    </noscript>    

    <script type="text/javascript">
        //
        // This is the function that does all the work.
        // It calculates a "total" based on form choices,
        // manipulates the format of the data and finally 
        // sends the data to Google Analytics.
        //

        var orderID = document.all['ctl00$endOfPage$orderID'].value;
        var orderTotal = document.all['ctl00$endOfPage$orderTotal'].value;
        var es002Price = document.all['ctl00$endOfPage$es002Price'].value;
        var es002Quantity = document.all['ctl00$endOfPage$es002Quantity'].value;
        var es004Price = document.all['ctl00$endOfPage$es004Price'].value;
        var es004Quantity = document.all['ctl00$endOfPage$es004Quantity'].value;
        var es015Price = document.all['ctl00$endOfPage$es015Price'].value;
        var es015Quantity = document.all['ctl00$endOfPage$es015Quantity'].value;
        //
        // Finally, format the data by concatenating the form selections
        // and send the data to GA.
        //
        pageTracker._addTrans(
                                    orderID,                       // Order ID
                                    "",                            // Affiliation
                                    orderTotal,                       // Total
                                    "0.00",                        // Tax
                                    "0.00",                        // Shipping
                                    "",                  // City
                                    "",                     // State
                                    ""                          // Country
                                  );

        if (es002Quantity > 0) {
            pageTracker._addItem(
                                        orderID,        // Order ID
                                        "ES002",        // SKU
                                        "ES002",        // Product Name
                                        "",             // Category
                                        es002Price,     // Price
                                        es002Quantity   // Quantity
                                    );
        }
        //
        if (es004Quantity > 0) {
            pageTracker._addItem(
                                        orderID,        // Order ID
                                        "ES004",        // SKU
                                        "ES004",        // Product Name
                                        "",             // Category
                                        es004Price,     // Price
                                        es004Quantity   // Quantity
                                    );
        }
        //
        if (es015Quantity > 0) {
            pageTracker._addItem(
                                        orderID,        // Order ID
                                        "ES015",        // SKU
                                        "ES015",        // Product Name
                                        "",             // Category
                                        es015Price,     // Price
                                        es015Quantity   // Quantity
                                    );
        }
        // Send the transaction to GA!
        //
        pageTracker._trackTrans();

    </script>

</asp:Content>
