<%@ Page Title="Widok HLD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="EAkzgHLDRepo.test" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Repozytorium dokumentów HLD</h1>
       
        <p class="lead">Podgląd informacji o HLD</p>
        <p><a href="default.aspx" class="btn btn-primary btn-lg">Wróć</a>
        </p>
    </div>
  <div>
      
        <asp:UpdateProgress ID="updProgress"  AssociatedUpdatePanelID="UpdatePanel1"  runat="server" >
            <ProgressTemplate>   
                      <div class="row">
                          <div class="center">
                          <asp:Label id="updTxt" Text="Wczytuję model ... " runat="server"  CssClass="Tytul_1_2" />
                              </div>
                      </div>
                      <div class="row" >
                         
                            <img  alt="progress" src="Content/kostki.gif" class="diagram" />
                              
                      </div>
                    
            
                      
            </ProgressTemplate>
        </asp:UpdateProgress>
       
            
    </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                 <div class="row">
            <div class="col-md-12">
                 <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/Content/logo.png" />
             </div>
                      <div class="row">
            <div class="col-md-12">
                   <asp:PlaceHolder runat="server" ID="HLDtresc"></asp:PlaceHolder>
              </div>
          </div>
         </div>
                </ContentTemplate>
              <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="UpdateTimer" EventName="Tick" />
        </Triggers>
        </asp:UpdatePanel>     
            
          <div >
    <div class="floatMenu">
        <h4>Spis treści</h4>
       
        <asp:PlaceHolder runat="server" ID="HLDmenu" ></asp:PlaceHolder>
       
    </div>
   
    
              </div>
     
  
            
             <asp:Timer ID="UpdateTimer" runat="server" Interval="50" ontick="UpdatePanel1_Load" Enabled="true" />
        <asp:Literal runat="server" ID="ltProjekt"  />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Dsn=eakzg_mysql;description=x;server=10.22.23.82;uid=eakzg;pwd=a;database=eakzg_schema;port=3306" ProviderName="System.Data.Odbc"></asp:SqlDataSource>
     
</asp:Content>

