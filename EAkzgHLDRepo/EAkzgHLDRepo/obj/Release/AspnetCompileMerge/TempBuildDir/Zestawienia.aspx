<%@ Page Title="HLD - Zestawienia" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Zestawienia.aspx.cs" Inherits="EAkzgHLDRepo.Zestawienia" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>SPR - Podgląd dedykowanych zestawień</h2>
        <p class="lead">Lista dostępnych statystyk opartych o bazę Cloud Enterprice Architect</p>
        <asp:Button ID="Button3" runat="server" Text="Zestawienia projektów pod względem zmian integracyjnych." CssClass="btn btn-primary btn-lg"  PostBackUrl="~/ZestIntegracja.aspx"/>
        
    </div>

    <div class="row">
        <div class="col-md-12">
            
           
            
        </div>

    </div>
   
    
 
</asp:Content>
