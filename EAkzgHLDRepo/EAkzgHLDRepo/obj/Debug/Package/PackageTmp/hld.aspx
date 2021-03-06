﻿<%@ Page Title="Widok HLD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="hld.aspx.cs" Inherits="EAkzgHLDRepo.hld" EnableViewState="False" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Repozytorium dokumentów HLD</h1>

        <p class="lead">Podgląd informacji o HLD</p>
        <p>
            <a href="default.aspx" class="btn btn-primary btn-lg">Wróć</a>
        </p>
    </div>
    <div>

       
        <div>
            <div class="floatMenu">
               
                <asp:Panel runat="server" ID="panelMenu">
                    
                    <div class="container change" onclick="menuFunction(this)">
                      
                        <div class="col-md-4">

                      <div class="bar1"></div>
                      <div class="bar2"></div>
                      <div class="bar3"></div>
                            </div>
                        <div class="txtSpisTresci">Spis treści</div>
                    </div>
                    
                    <div class="floatPodMenu" id="menu">
                    <asp:PlaceHolder runat="server" ID="HLDmenu" ></asp:PlaceHolder>
                        </div>
                </asp:Panel>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/Content/logo.png" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:PlaceHolder runat="server" ID="HLDtresc_cz1"></asp:PlaceHolder>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
             <!--       <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                        <ProgressTemplate>
                            <div class="row">
                                <div class="center">
                                    <asp:Label ID="updTxt" Text="Wczytuję model ... " runat="server" CssClass="Tytul_1_2" />
                                </div>
                            </div>
                            <div class="row">

                                <img alt="progress" src="Content/kostki.gif" class="diagram" />

                            </div>



                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    -->

                <asp:PlaceHolder runat="server" ID="HLDtresc_cz2"></asp:PlaceHolder>

               
             
             <!--   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                         <asp:PlaceHolder runat="server" ID="HLDtresc_cz2_tmp"></asp:PlaceHolder>
                        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel> -->
            </div>
        </div>
       

        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder runat="server" ID="HLDtresc_cz3"></asp:PlaceHolder>
            </div>
        </div>
         <div class="row">
            <div class="col-md-12">
             <!--   <asp:UpdateProgress ID="UpdateProgress_cz2" AssociatedUpdatePanelID="UpdatePanel_cz2" runat="server">
                    <ProgressTemplate>

                        <div class="row">

                            <img alt="progress" src="Content/kostki.gif" class="diagram" />

                        </div>



                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel_cz2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder runat="server" ID="HLDtresc_cz4_tmp"></asp:PlaceHolder>



                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="UpdatePanel1_Load" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                -->
                  <asp:PlaceHolder runat="server" ID="HLDtresc_cz4"></asp:PlaceHolder>
            </div>
        </div>
         
       
        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder runat="server" ID="HLDtresc_cz5"></asp:PlaceHolder>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder runat="server" ID="HLDtresc"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    </div>


    <asp:Literal runat="server" ID="ltProjekt" />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Dsn=eakzg_mysql;description=x;server=10.22.23.82;uid=eakzg;pwd=a;database=eakzg_schema;port=3306" ProviderName="System.Data.Odbc"></asp:SqlDataSource>

</asp:Content>

