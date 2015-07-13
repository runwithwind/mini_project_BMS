<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Query_employee.aspx.cs" Inherits="Query_employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
            <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
    <div class="body">
        <h2 style="text-align:center;"><asp:Localize ID="QueryE" runat="server" Text="<%$ Resources:lang, QueryETitleRes %>"></asp:Localize></h2>
        <div>
            <asp:Localize ID="Tip1" runat="server" Text="<%$ Resources:lang, Tip1Res %>"></asp:Localize>
            <asp:TextBox ID="TextBox1" runat="server" Height="32px"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-default" Text="<%$ Resources:lang,SearchRes %>" Font-Size="Small" Font-Underline="False" Height="32px" Width="65px" OnClick="Button1_Click"/>

        </div>
        <div>
            <!--add DataKeyNames BID-->
            <asp:GridView ID="GridView1" runat="server" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="BID">
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Delete" HeaderText="ReturnBook" ShowHeader="True" Text="Return" />
                </Columns>
                <HeaderStyle Wrap="True" />
                <RowStyle Wrap="True" />
            </asp:GridView>
            <br />

        </div>
    </div>
</asp:Content>

