<%@ Page Title="Widok HLD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="hld.aspx.cs" Inherits="EAkzgHLDRepo.hld" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Repozytorium dokumentów HLD</h1>
        <p class="lead">Podgląd informacji o HLD</p>
        <p><a href="default.aspx" class="btn btn-primary btn-lg">Wróć</a>
        </p>
    </div>
    <div class="row">
       <div class="col-md-12">
        <h3>
            <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/Content/logo.png" />
        </h3>
        </div>
        </div>
    <div class="row">
         <div class="col-md-12">
            <asp:Label ID="HLD_tytul" runat="server" Text="Label" Font-Names="TeleGrotesk Headline" Font-Size="XX-Large" ForeColor="#CC00CC" Font-Bold="True"></asp:Label>
         </div>
        </div>
     <div class="row">
         <div class="col-md-12">
            <asp:Label ID="Label1" runat="server" Text="1. Organizacyjnie" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="#CC00CC"></asp:Label>
         </div>
        </div>
      <div class="row">
         <div class="col-md-12">
            <asp:Label ID="Label2" runat="server" Text="1.1 Zawartość, cel i przeznaczenie dokumentu" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
         </div>
        </div>
     <div class="row">
         <div class="col-md-12">
             <p>
          Celem niniejszego dokumentu jest przedstawienie sposobu realizacji Wymagań Biznesowych dla projektu zawartych w dokumencie Concept Paper. Na opis sposób realizacji składają się następujące główne elementy:
                 <ol>
<li>odniesienie do wymagań biznesowych</li>
<li>zarys koncepcji rozwiązania</li>
<li>opis architektury rozwiązania wraz z dekompozycją koniecznych zmian funkcjonalnych na poszczególne systemy</li>
<li>opis koniecznych do wykonania zmian w poszczególnych systemach</li>
<li>opis zmian koniecznych z punktu widzenia Infrastruktury</li>
                     </ol>
Zawarte w dokumencie informacje będą podstawą do:
                 <ul>
<li>ustalenia kosztów oraz ostatecznych terminów wdrożenia przedsięwzięcia i tym samym podjęcia decyzji o jego realizacji,</li>
<li>dalszych prac nad projektem - projektowania spójnego rozwiązania w poszczególnych systemach</li>
                     </ul>
                 
         </div>
        </div>
     <div class="row">
         <div class="col-md-12">
            <asp:Label ID="Label3" runat="server" Text="1.2 Słownik użytych skrótów i pojęć" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
         </div>
        </div>
      <div class="row">
         <div class="col-md-12">
            <asp:GridView ID="GridViewSlownik" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
                      <br />
            <asp:Label ID="Label4" runat="server" Text="1.3 Załączniki, powiązane dokumenty" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
             <br />








      <asp:Literal runat="server" ID="lt1_3" />

             <br />
            <asp:GridView ID="GridViewZalaczniki" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
                      <br />
            <asp:Label ID="Label5" runat="server" Text="1.4 Zespół projektowy" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
                      <br />
            <asp:GridView ID="GridViewZespol" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
            <asp:Label ID="Label6" runat="server" Text="1.5 Powiązania z innymi projektami" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
                      <br />
             <asp:Literal ID="lt1_5" runat="server" />
                      <br />
            <asp:GridView ID="GridViewZaleznosci" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
            <asp:Label ID="Label7" runat="server" Text="2. Perspektywa funkcjonalna" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="#CC00CC"></asp:Label>
                      <br />
            <asp:Label ID="Label8" runat="server" Text="2.1 Krótki opis projektu z perspektywy biznesowej" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
                      <br />








      <asp:Literal runat="server" ID="lt2_1" />

                      <br />

                      <br />
            <asp:Label ID="Label9" runat="server" Text="2.2 Ograniczenia rozwiązania" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
                      <br />
             <asp:Literal ID="lt2_2" runat="server" />
                      <br />
            <asp:GridView ID="GridViewOgraniczenia" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
                      <br />
            <asp:Label ID="Label10" runat="server" Text="2.3 Wymagania biznesowe" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>

                      <br />
             <asp:Literal ID="lt2_3" runat="server" />

                      <br />
            <asp:GridView ID="GridViewWymaganiaBiz" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
              <asp:Label ID="Label11" runat="server" Text="2.4 Wymagania architektoniczne" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>

                      <br />
             <asp:Literal ID="lt2_4" runat="server" />

                      <br />
            <asp:GridView ID="GridViewWymaganiaArch" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnRowDataBound="GridViewSlownik_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>

              <div class="row">
         <div class="col-md-12">
            <asp:Label ID="Label12" runat="server" Text="3 OPIS ROZWIĄZANIA IT" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="#CC00CC"></asp:Label>
         </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                 <asp:Label ID="Label13" runat="server" Text="3.1 Koncepcja rozwiązania" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
             </div>
            <div class="col-md-12">
                  <asp:Literal runat="server" ID="lt3_1" />
             </div>
        </div>
       <div class="row">
            <div class="col-md-12">
                 <asp:Label ID="Label14" runat="server" Text="3.2 Architektura Statyczna" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
             </div>
        </div>
                <div class="row">
            <div class="col-md-12">
                 <asp:Label ID="Label16" runat="server" Text="3.2.1 Architektura Statyczna" Font-Names="TeleGrotesk Headline" Font-Size="Medium" ForeColor="Black" Font-Bold="True"></asp:Label>
             </div>
        </div>
                <div class="row">
            <div class="col-md-12">
                 <asp:Label ID="Label17" runat="server" Text="3.2.2 Architektura Statyczna" Font-Names="TeleGrotesk Headline" Font-Size="Medium" ForeColor="Black" Font-Bold="True"></asp:Label>
             </div>
        </div>
             <div class="row">
            <div class="col-md-12">
                 <asp:Label ID="Label15" runat="server" Text="3.3 Architektura Danych - ogólnie" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
             </div>
        </div>

                      <br />
                      </div>
          </div>








      <asp:Literal runat="server" ID="ltProjekt" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Dsn=eakzg_mysql;description=x;server=10.22.23.82;uid=eakzg;pwd=a;database=eakzg_schema;port=3306" ProviderName="System.Data.Odbc"></asp:SqlDataSource>


    
    </div>
</asp:Content>

