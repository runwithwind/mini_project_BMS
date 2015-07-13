<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Query_book.aspx.cs" Inherits="Return" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
        <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
    <div class="body">
        <h2 style="text-align:center;"><asp:Localize ID="QueryBTitle" runat="server" Text="<%$ Resources:lang, QueryBTitleRes %>"></asp:Localize></h2>
        <div>
            <asp:Localize ID="Tip2" runat="server" Text="<%$ Resources:lang, Tip2Res %>"></asp:Localize>
            <asp:TextBox ID="TextBox1" runat="server" Height="32px"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-default" Text="<%$ Resources:lang,SearchRes %>" Font-Size="Small" Font-Underline="False" Height="32px" Width="65px" OnClick="Button1_Click"/>

        </div>
        <div>
            <asp:GridView ID="GridView1" runat="server" DataKeyNames="BID" OnRowDeleting="GridView1_RowDeleting" >
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Delete" HeaderText="ReturnBook" ShowHeader="True" Text="Return" />
                </Columns>
                <HeaderStyle Wrap="True" />
            </asp:GridView>
            <asp:GridView ID="GridView2" runat="server" DataKeyNames="BID" OnRowDeleting="GridView2_RowDeleting">
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Delete" HeaderText="BorrowBook" ShowHeader="True" Text="Borrow" />
                </Columns>

            </asp:GridView>
            <br />

        </div>
    </div>
</asp:Content>

