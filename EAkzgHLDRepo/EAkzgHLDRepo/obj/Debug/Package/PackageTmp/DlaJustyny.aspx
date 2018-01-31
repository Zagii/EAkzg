<%@ Page Title="Widok ludzików" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DlaJustyny.aspx.cs" Inherits="EAkzgHLDRepo.DlaJustyny" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Tajny podgląd dla Justyny</h1>
        <p class="lead">Widok ludzików by wszystko wiedzieć</p>
        <p><a href="default.aspx" class="btn btn-primary btn-lg">Wróć</a>
                   
           
                <asp:Label ID="Info" runat="server" Text="Label" Visible="False" BorderStyle="Outset" CssClass="alert-info" Font-Overline="False" Font-Underline="True" Width="984px"></asp:Label>
        </p>
    </div>
    <div class="row">
       <div class="col-md-12">
           
           <asp:Label ID="Label0" runat="server" Text="Wybierz listę PR'ek" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
              <asp:ListBox ID="ListBox1" runat="server" Width="449px"></asp:ListBox>
           
               <asp:Button ID="Button2" runat="server" Height="34px" Text="Dodaj do listy" OnClick="Button2_Click" Width="203px" />
               <asp:TextBox ID="TextBox1" runat="server" Height="27px" Width="251px"></asp:TextBox>
           
           
        </div>
        <div class="col-md-10">
           
           
                <asp:Button ID="Button1" runat="server" Text="Usuń zaznaczony z listy" OnClick="Button1_Click" />
           
        </div>
        <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="6000" OnTick="Timer1_Tick"></asp:Timer>
      </div>
         
           
   

            <asp:Label ID="Label5" runat="server" Text="Ludziki w projektach" Font-Names="TeleGrotesk Headline" Font-Size="Larger" ForeColor="Black" Font-Bold="True"></asp:Label>
            <asp:GridView ID="GridViewZespolLudzikow" runat="server" BackColor="#669999" CssClass="table table-bordered table-striped" Width="1069px"  HorizontalAlign="Center" ViewStateMode="Enabled" AutoGenerateColumns="False"  >
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="Black" />
             <RowStyle BackColor="#99CCFF" />
                 <Columns>
                    <asp:BoundField DataField="Projekt" HeaderText="Projekt" ItemStyle-Width="30" />
                    <asp:BoundField DataField="System" HeaderText="System" ItemStyle-Width="150" />
                    <asp:BoundField DataField="Ludzik" HeaderText="Ludzik" ItemStyle-Width="150" />
                </Columns>
               
         </asp:GridView>
        </div>
        </div>
    </div>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Dsn=eakzg_mysql;description=x;server=10.22.23.82;uid=eakzg;pwd=a;database=eakzg_schema;port=3306" ProviderName="System.Data.Odbc"></asp:SqlDataSource>
</asp:Content>
