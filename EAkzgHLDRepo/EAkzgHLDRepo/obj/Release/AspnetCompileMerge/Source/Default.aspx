<%@ Page Title="SPR Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EAkzgHLDRepo._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
   <!--
    <div class="jumbotron">
        <h1>Repozytorium dokumentów HLD</h1>
        <p class="lead">Na tej witrynie znajdziesz wybrane detale dotyczące realizacji dokumentów HLD.</p>
        <p class="small" >Dane prezentowane poniżej wygenerowane zostały na podstawie repozytorium projektowego Sparx Enterpirce Architect oraz Dashboardu projektów realizowanych przez SPR 
            <a href="http://architect-new/SDPD" target="_blank">http://architect-new/SDPD/</a>
        </p>
    </div>
    -->
    <div class="row">
        <div class="col-md-6">
            <h2>Lista projektów</h2>
            <p>           
              Projekty wdrażajace nowe integracje międzysystemowe
            </p>
       
            
        </div>
       <!-- <div class="col-md-6">
             <div class="row">
            <p>
               Wyświetlane projektu w statusie
            </p>
                                  </div>
            <div class="row">
                  <asp:Literal runat="server" ID="ltCheckBoxStatus" />
                <asp:CheckBoxList ID="CheckBoxListStatus" runat="server" AutoPostBack="True" RepeatColumns="4" RepeatDirection="Horizontal" ToolTip="Filtr fazy projektu" CellPadding="2" CellSpacing="2" OnSelectedIndexChanged="CheckBoxListStatus_SelectedIndexChanged"></asp:CheckBoxList>
          
                </div>
       </div>
           --> 
       
         <div class="row">
   <div class="col-md-12">
         <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" Width="1069px" HorizontalAlign="Center" OnDataBound="GridView1_DataBound" OnRowDataBound="GridView1_RowDataBound" AllowSorting="True" OnSorting="GridView1_Sorting">
             <AlternatingRowStyle BackColor="White" HorizontalAlign="Left" />
             <EditRowStyle Wrap="True" HorizontalAlign="Left" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" />
             <RowStyle HorizontalAlign="Left" />
           
         </asp:GridView>
         </div>
        </div>
        <div class="col-md-12">
       <asp:Literal runat="server" ID="ltListaProjektow" />
            </div>
    </div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Dsn=eakzg_mysql;description=x;server=10.22.23.82;uid=eakzg;pwd=a;database=eakzg_schema;port=3306" ProviderName="System.Data.Odbc"></asp:SqlDataSource>
</asp:Content>

