<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" meta:resourceKey="page"
    AutoEventWireup="true" CodeBehind="Links.aspx.cs" Inherits="MagmaLightWeb.Links.Links"
    Culture="auto:hu-HU" UICulture="auto" %>
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
            <div class="form_container_header">
                <asp:Label ID="lblDepression" meta:resourceKey="lblDepression" runat="server" /></div>
            <ul>
                <li>
                    <asp:HyperLink ID="lnkDep1" meta:resourceKey="lnkDep1" runat="server" NavigateUrl="http://www.normanrosenthal.com/articles.shtml" /><br />
                    <asp:Label ID="lblDepLink1" meta:resourceKey="lblDepLink1" runat="server" />
                </li>
                <li>
                    <asp:HyperLink ID="lnkDep2" meta:resourceKey="lnkDep2" runat="server" NavigateUrl="http://psychologytoday.com/articles/pto-20020801-000003.html" /><br />
                    <asp:Label ID="lblDepLink2" meta:resourceKey="lblDepLink2" runat="server" />
                </li>
                <li>
                    <asp:HyperLink ID="lnkDep3" meta:resourceKey="lnkDep3" runat="server" NavigateUrl="http://www.apollolight.com/pdf_files/Beginning%20to%20see%20the...pdf" /><br />
                </li>
                <li>
                    <asp:HyperLink ID="lnkDep4" meta:resourceKey="lnkDep4" runat="server" NavigateUrl="http://www.apollolight.com/pdf_files/Direct%20brain%20serotonin%20validates%20SAD.pdf" /><br />
                </li>
                <li>
                    <asp:HyperLink ID="lnkDep5" meta:resourceKey="lnkDep5" runat="server" NavigateUrl="http://www.apollolight.com/pdf_files/BiologicalClockArticleRevised.pdf" /><br />
                </li>
                <li>
                    <asp:HyperLink ID="lnkDep6" meta:resourceKey="lnkDep6" runat="server" NavigateUrl="http://www.talkaboutsleep.com/circadian-rhythm-disorders/circadian-rhythm-sleep-disorders/index.htm" /><br />
                </li>
            </ul>
            <div class="form_container_header">
                <asp:Label ID="lblIons" meta:resourceKey="lblIons" runat="server" /></div>
            <ul>
                <li>
                    <asp:HyperLink ID="lnkIon1" meta:resourceKey="lnkIon1" runat="server" NavigateUrl="http://www.apollolight.com/pdf_files/Wake%20Therapy%20Helps%20Treat%20Depression.pdf" /><br />
                </li>
                <li>
                    <asp:HyperLink ID="lnkIon2" meta:resourceKey="lnkIon2" runat="server" NavigateUrl="http://xxx.uni-augsburg.de/pdf/physics/9709010" /><br />
                </li>
                <li>
                    <asp:HyperLink ID="lnkIon3" meta:resourceKey="lnkIon3" runat="server" NavigateUrl="http://www.bright.net/~comtech/negative_ions.html" /><br />
                    <asp:Label ID="lblIonLink3" meta:resourceKey="lblIonLink3" runat="server" /></li>
                <li>
                    <asp:HyperLink ID="lnkIon4" meta:resourceKey="lnkIon4" runat="server" NavigateUrl="http://mypage.direct.ca/g/gcramer/asthma.html" />
                </li>
                <li>
                    <asp:HyperLink ID="lnkIon5" meta:resourceKey="lnkIon5" runat="server" NavigateUrl="http://www.static-sol.com/library/articles/air%20ion%20effects.htm" />
                </li>
                <li>
                    <asp:HyperLink ID="lnkIon6" meta:resourceKey="lnkIon6" runat="server" NavigateUrl="http://www.ars.usda.gov/is/AR/archive/mar00/salm0300.htm" />
                </li>
                <li>
                    <asp:HyperLink ID="lnkIon7" meta:resourceKey="lnkIon7" runat="server" NavigateUrl="http://www.cryonet.org/cgi-bin/dsp.cgi?msg=12330" />
                </li>
            </ul>
            <a href="~/CV/" visible="false" runat="server">&nbsp;</a>
        </div>
    </div>
</asp:Content>
