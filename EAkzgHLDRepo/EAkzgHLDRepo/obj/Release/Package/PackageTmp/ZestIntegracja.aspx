<%@ Page Title="HLD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZestIntegracja.aspx.cs" Inherits="EAkzgHLDRepo.ZestIntegracja" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link href="Content/datepicker.css" rel="stylesheet" type="text/css"/>
      <!-- <script src= https://code.jquery.com/jquery-1.12.4.js></script>
     <script src= https://code.jquery.com/ui/1.12.1/jquery-ui.js ></script> -->
      <script type="text/javascript">
          $(function () {
              $('.ui-datepicker').datepicker({
                  inline: true,
                  nextText: '&rarr;',
                  prevText: '&larr;',
                  showOtherMonths: true,
                  dateFormat: 'dd-mm-yy',
                  firstDay: 0,
                  dayNamesMin: ['Pn', 'Wt', 'Śr', 'Czw', 'Pt', 'Sb', 'Nd'],
                  monthNames: [ "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" ],
                  showOn: "button",
                  buttonImage: "Content/cal.png",
                  buttonImageOnly: true
              });
          });
        </script>
   

    <div class="jumbotron">
        <h2>Zestawienie nowych interfejsów w projekcie </h2>
        <p class="lead"> <asp:Label ID="nazwaProjektuLbl" runat="server" Text="Label"></asp:Label></p>
        <p><a href="Default.aspx" class="btn btn-primary btn-lg">Wróć</a>
              <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Wpisz datę w odpowiednim formacie (dd-mm-yyyy)" ControlToValidate="TextAreaDataDo" Display="Dynamic" ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="AllValidators">Błędny format daty (dd-mm-yyyy)</asp:CustomValidator>

               <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </p>
  <div class="row">
      <div class="row">
        <div class="col-md-4">
           Status interfejsu: 
        </div>
           <div class="col-md-8">
           <asp:CheckBox ID="StatusCheckBox_new" runat="server" Text="new" Checked="True"  AutoPostBack="True" />
            <asp:CheckBox ID="StatusCheckBox_change" runat="server" Text="change" Checked="True" AutoPostBack="True" />
            <asp:CheckBox ID="StatusCheckBox_reuse" runat="server" Text="reuse" Checked="True" AutoPostBack="True" />
            <asp:CheckBox ID="StatusCheckBox_remove" runat="server" Text="remove" Checked="True"  AutoPostBack="True" />
               <asp:CheckBox ID="StatusCheckBox_null" runat="server" Text="null" Checked="True" AutoPostBack="True" />
        </div>
      </div>
    <!--
         <div class="row">
                 
           <div class="col-md-4">
           Symbol projektu:
            </div>
           
   
              <div class="col-md-8">
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"  Width="500pt">
            </asp:DropDownList>
         </div>
       
     
        </div>
     -->
        <div class="row">
        <div class="col-md-6">
             <asp:Label ID="zalProjLbl" runat="server" Text="Data założenia projektu:"></asp:Label><br />
           <asp:Label ID="projOdLbl" runat="server" Text="Data założenia projektu:">Od:</asp:Label>  <asp:TextBox ID="TextAreaDataProjOd" runat="server" class="ui-datepicker"  readonly="true"></asp:TextBox><br />
           <asp:Label ID="projDoLbl" runat="server" Text="Data założenia projektu:">Do:</asp:Label>   <asp:TextBox ID="TextAreaDataProjDo" runat="server" class="ui-datepicker" readonly="true"></asp:TextBox>
          

         </div>
      <div class="col-md-6">
            <asp:Label ID="modObjLbl" runat="server" Text="Data modyfikacji obiektu:"></asp:Label><br />
           <asp:Label ID="odLbl" runat="server" Text="Data założenia projektu:">Od:</asp:Label>  <asp:TextBox ID="TextAreaDataOd" runat="server" class="ui-datepicker"  readonly="true"></asp:TextBox><br />
           <asp:Label ID="doLbl" runat="server" Text="Data założenia projektu:">Do: </asp:Label>  <asp:TextBox ID="TextAreaDataDo" runat="server" class="ui-datepicker"  readonly="true"></asp:TextBox>

       

         </div>
            </div>
       
          <div class="row">
           <div class="col-md-12">
               <asp:Button ID="Button1" runat="server" Text="Szukaj.." class="btn btn-primary btn-lg" OnClick="Button1_Click" ValidationGroup="AllValidators" />

            </div>
        </div>
     
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextAreaDataDo" Display="Dynamic" ErrorMessage="Wypełnij pole daty do" ForeColor="#FF3300" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>

    </div>
        </div>
     <div class="row">
   
         <asp:GridView ID="GridView1" runat="server" BackColor="#669999" CssClass="rounded_corners" Width="1069px" AllowSorting="True" HorizontalAlign="Center" OnDataBound="GridView_RowDataBound">
             <AlternatingRowStyle BackColor="White" />
             <EmptyDataRowStyle BackColor="Red" />
             <HeaderStyle BackColor="#336699" ForeColor="White" />
             <RowStyle BackColor="#99CCFF" />
         </asp:GridView>
         </div>
     <div class="row">
   
              <div class="col-md-12">
              <asp:Literal runat="server" ID="ltRaport" />
        </div>
    </div>

   <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="System.Data.Odbc" ConnectionString="DSN=eakzg_mysql;UID=eakzg;description=x;server=10.22.23.82;database=eakzg_schema;port=3306;"></asp:SqlDataSource>
</asp:Content>

