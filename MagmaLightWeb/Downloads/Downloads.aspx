<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Downloads.aspx.cs" Inherits="MagmaLightWeb.Downloads.Downloads"
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
            <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
            <div class="insideframe">
                <div class="form_frame">
                    <ul>
                        <li>
                            <asp:HyperLink ID="lnkDownload1" meta:resourceKey="lnkDownload1" runat="server" NavigateUrl="http://www.naplampa.hu/Catalog/NaplampaKatalogus2008web.pdf" /><br />
                            <asp:Label ID="lblDownloadLink1" meta:resourceKey="lblDownloadLink1" runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="lnkDownload2" meta:resourceKey="lnkDownload2" runat="server" NavigateUrl="http://www.naplampa.hu/Catalog/Kristalysugar_E27_EU_certificate_redist.pdf" /><br />
                            <asp:Label ID="lblDownloadLink2" meta:resourceKey="lblDownloadLink2" runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="lnkDownload3" meta:resourceKey="lnkDownload3" runat="server" NavigateUrl="http://www.naplampa.hu/Catalog/SGS-Antibacterial_Report_for_AG400.pdf" /><br />
                            <asp:Label ID="lblDownloadLink3" meta:resourceKey="lblDownloadLink3" runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="lnkDownload5" meta:resourceKey="lnkDownload5" runat="server" NavigateUrl="http://www.naplampa.hu/Catalog/SzakInfo_Naplampa_cikk.pdf" /><br />
                            <asp:Label ID="lblDownloadLink5" meta:resourceKey="lblDownloadLink5" runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="lnkDownload4" meta:resourceKey="lnkDownload4" runat="server" /><br />
                            <asp:Label ID="lblDownloadLink4" meta:resourceKey="lblDownloadLink4" runat="server" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
