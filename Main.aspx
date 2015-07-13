<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
    <div class="body">
        <h2 style="text-align:center;"><asp:Localize ID="Wel" runat="server" Text="<%$ Resources:lang,WelRes %>"></asp:Localize></h2>
        <h3><asp:Localize ID="Intr" runat="server" Text="<%$ Resources:lang, IntrRes %>"></asp:Localize></h3>
    <ol class="round">
        <li class="one">
            <h5><asp:Localize ID="MainG" runat="server" Text="<%$ Resources:lang, MainGRes %>"></asp:Localize></h5>
            <p><asp:Localize ID="br" runat="server" Text="<%$ Resources:lang, brRes %>"></asp:Localize></p>
        </li>
        <li class="two">
            <h5><asp:Localize ID="QueryEG" runat="server" Text="<%$ Resources:lang, QueryEGRes %>"></asp:Localize></h5>
            <p><asp:Localize ID="sen1" runat="server" Text="<%$ Resources:lang, sen1Res %>"></asp:Localize></p>
            <p><asp:Localize ID="sen2" runat="server" Text="<%$ Resources:lang, sen2Res %>"></asp:Localize></p>
        </li>
        <li class="three">
            <h5><asp:Localize ID="QueryBG" runat="server" Text="<%$ Resources:lang, QueryBGRes %>"></asp:Localize></h5>
            <p><asp:Localize ID="sen3" runat="server" Text="<%$ Resources:lang, sen3Res %>"></asp:Localize></p>
            <p><asp:Localize ID="sen4" runat="server" Text="<%$ Resources:lang, sen4Res %>"></asp:Localize></p>
        </li>
    </ol>
        </div>
</asp:Content>
