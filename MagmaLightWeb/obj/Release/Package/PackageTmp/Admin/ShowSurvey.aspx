<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowSurvey.aspx.cs" Inherits="MagmaLightWeb.Admin.ShowSurvey"
    Culture="auto:hu-HU" UICulture="auto" MasterPageFile="~/MasterPage.master" meta:resourceKey="page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--BELSO OLDALAKON HASZNÁLATOS KERET KEZD-->
    <div class="insideframe">
        <!--oldalfejlec KEZD-->
        <div class="header">
            <asp:Label ID="lblHeader" meta:resourceKey="lblHeader" runat="server" /></div>
        <div class="form_frame">
            <div class="form_row">
                <asp:Label ID="lblError" meta:resourceKey="lblError" runat="server" ForeColor="Red"
                    Visible="false" />
                <asp:Label ID="lblAlready" meta:resourceKey="lblAlready" runat="server" Visible="false" />
                <asp:Label ID="lblExpired" meta:resourceKey="lblExpired" runat="server" Visible="false" />
                <asp:Label ID="lblThankYou" meta:resourceKey="lblThankYou" runat="server" Visible="false" />
            </div>
            <br />
            <asp:Panel ID="pnlQuestions" runat="server" Visible="false">
                <asp:Panel Visible="false" ID="pnlRadio1" runat="server">
                    <div class="form_row">
                        <asp:Label ID="lblRadio1Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio1" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio2" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio2Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio2" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio3" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio3Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio3" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio4" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio4Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio4" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio5" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio5Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio5" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio6" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio6Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio6" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlRadio7" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblRadio7Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:RadioButtonList ID="rblRadio7" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk1" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk1Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk1" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk2" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk2Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk2" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk3" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk3Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk3" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk4" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk4Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk4" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk5" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk5Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk5" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk6" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk6Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk6" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk7" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk7Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk7" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk8" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk8Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk8" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlChk9" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblChk9Question" runat="server" /></div>
                    <div class="form_row_answer">
                        <asp:CheckBoxList ID="cblChk9" RepeatDirection="Vertical" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt1" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt1Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt1" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt2" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt2Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt2" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt3" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt3Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt3" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt4" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt4Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt4" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt5" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt5Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt5" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt6" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt6Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt6" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt7" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt7Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt7" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt8" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt8Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt8" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <asp:Panel Visible="false" ID="pnlTxt9" runat="server">
                    <br />
                    <br />
                    <div class="form_row">
                        <asp:Label ID="lblTxt9Question" runat="server" /></div>
                    <br />
                    <div class="form_row_answer">
                        <asp:TextBox ID="txtTxt9" Width="400px" Height="100px" MaxLength="500" TextMode="MultiLine"
                            runat="server" /></div>
                </asp:Panel>
                <br />
                <br />
                <div class="form_accept">
                    <asp:Button ID="btnCompleted" meta:resourceKey="btnCompleted" runat="server" Width="200px"
                        OnClick="btnCompleted_Click" CssClass="accept" />
                </div>
            </asp:Panel>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
