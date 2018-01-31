using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Net;


namespace EAkzgHLDRepo
{
    public partial class _Default : Page
    {
        
        private void listaProjektow()
        {
            string status=" p.project_phase_id in ( -1,";
            int dl = status.Length;
            for (int i = 0; i < CheckBoxListStatus.Items.Count; i++)
            {
                if (CheckBoxListStatus.Items[i].Selected)
                {
                    status += CheckBoxListStatus.Items[i].Value.ToString() + ",";
                }
            }
            status = status.Substring(0, status.Length - 1);
            status += ")";

            status = " p.project_phase_id in (22,23,24,25,31,35) ";

        //  SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnStr5.1"].ConnectionString;
              string sql = "";
            // SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
              sql = @"select code as Symbol, name as Nazwa,  DATE_FORMAT(created_date,'%d-%m-%Y') as DataUtworzenia, p.id, solution_designer_id 
                from sdpd.project p ,information_schema.SCHEMATA s
                  where (p.code like 'PR-%' or p.code like 'EU-%') " +
                   " and "+status+" and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%')"+
                " and lower(p.code)=lower(s.schema_name) ";
            sql="select code as Symbol, p.name as Nazwa,  DATE_FORMAT(created_date,'%d-%m-%Y') as DataUtworzenia, p.id, solution_designer_id ,"+
                "f.NAME faza,a.NAME alert,t.NAME typ, u.first_name,u.last_name, p.INVOLVMENT,p.TOMORROW,p.NETWORK_SOLUTION,p.TRANSMISSION_ARCHITECTURE,"+
                "p.SHORT_DESC,p.STATUS_COMMENT,p.GO2FS_DECISION_DATE,p.SD2PROJ_DATE,p.SCHEDULE_DECLARE_DATE,p.SCHEDULE_FINAL_DATE,"+
                "p.BUSSINES_REQ_DECLARE_DATE,p.BUSSINES_REQ_FINAL_DATE,p.HLD_PHASE2_DECLARE_DATE,p.HLD_PHASE2_FINAL_DATE,"+
                "p.IA_FINAL_DATE "+
                                "from sdpd.project p ,information_schema.SCHEMATA s,"+
                                "sdpd.project_alert a,"+
                                "sdpd.project_type t,"+
                                "sdpd.project_phase f,"+
                                "sdpd.solution_designer sd,"+
                                "sdpd.user u "+
                                 " where (p.code like 'PR-%' or p.code like 'EU-%') "+
                                 "  and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%') "+
                                 "and lower(p.code)=lower(s.schema_name) "+
                                 "and a.id=p.PROJECT_ALERT_ID and t.ID = p.PROJECT_TYPE_ID and f.id=p.PROJECT_PHASE_ID " +
                                 "and p.SOLUTION_DESIGNER_ID=sd.ID and sd.user_id=u.id"+ " and "+status+ " order by 1 desc;" ;
            
            SqlDataSource1.SelectCommand = sql;

            DataTable dt = new DataTable();

            dt.Columns.Add("Kod"); dt.Columns.Add("Nazwa"); dt.Columns.Add("Solution Designer"); dt.Columns.Add("Nowe"); dt.Columns.Add("Zmieniane"); dt.Columns.Add("Usuwane"); dt.Columns.Add("Reużyte");
            DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
          
            foreach (DataRowView drv in dv1)
            {

                sql = "select ifnull(sum(case when o.stereotype='new' then 1 else 0 end),0) nowe, ifnull(sum(case when o.stereotype='change' then 1 else 0 end),0) zmieniane," +
                " ifnull(sum(case when o.stereotype='remove' then 1 else 0 end),0) usuwane, ifnull(sum(case when o.stereotype='reuse' then 1 else 0 end),0) uzywane " +
                                " from  `"+drv.Row["Symbol"]+"`.t_object o  where     o.Object_Type='Interface'";
                SqlDataSource1.SelectCommand = sql;

                try
                {
                    DataView dv2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);

                    string s = @"<a class=""tooltipKZG""   href='#'>" + drv.Row.ItemArray[1].ToString() + " ";
                    string n = @"<a  href=ZestIntegracja.aspx?proj=" + drv.Row.ItemArray[0].ToString() + ">" + drv.Row.ItemArray[0].ToString() + "</a>";
                    
                    string h = @"<a class=""btn btn-primary btn-lg"" color:black;align:left;"" href=hld.aspx?schema=" + drv.Row.ItemArray[0].ToString() + ">HLD</a>";

                    string tmor = drv.Row["tomorrow"].ToString() == "1" ? "TAK" : "NIE";
                    string netw = drv.Row["network_solution"].ToString() == "1" ? "TAK" : "NIE";
                    string trans = drv.Row["transmission_architecture"].ToString() == "1" ? "TAK" : "NIE";
                    string opis = drv.Row["short_desc"].ToString();
                    if (opis.Length > 250) { opis = opis.Substring(0, 150) + "..."; }
                    string stat = drv.Row["STATUS_COMMENT"].ToString();
                    if (stat.Length > 250) { stat = stat.Substring(0, 150) + "..."; }
                    string sp = @"<span class=""tooltiptextKZG"">" +
                        "<table class=\"tooltipTableKZG\">"+
                       // "<tr><td>KOD</td><td>" + drv.Row["Symbol"] + "</td></tr>" +
                        "<tr><td>Faza</td><td>" + drv.Row["faza"] + "</td></tr>" +
                        //"<tr><td>Alert</td><td>" + drv.Row["alert"] + "</td></tr>" +
                        "<tr><td>Typ</td><td>" + drv.Row["typ"] + "</td></tr>" +
                        //"<tr><td>Solution Designer</td><td>" + drv.Row["first_name"] + " " + drv.Row["last_name"] + "</td></tr>" +
                        //"<tr><td>Zaangażowanie - " + drv.Row["INVOLVMENT"] + "%</td><td>T-Morrow - " + tmor + "</td></tr>" +
                        //"<tr><td>Rozwiązanie sieciowe - " + netw + "</td><td>Architektura statyczna - " + trans + "</td></tr>" +
                        "<tr><td>Krótki opis</td><td>" + opis + "</td></tr>" +
                        //"<tr><td>Status</td><td>" + stat + "</td></tr>" +
                        //"<tr><td>Decyzja GO2FS</td><td>" + drv.Row["GO2FS_DECISION_DATE"] + "</td></tr>" +
                        //"<tr><td>Przypisanie SD</td><td>" + drv.Row["SD2PROJ_DATE"] + "</td></tr>" +
                        //"<tr><td>Faza</td><td> Data deklarowana / Data rzeczywista </td></tr>" +
                        //"<tr><td>Harmonogram</td><td>" + drv.Row["SCHEDULE_DECLARE_DATE"] + " / " + drv.Row["SCHEDULE_FINAL_DATE"] + "</td></tr>" +
                        //"<tr><td>Gotowość Biznesowa</td><td>" + drv.Row["SCHEDULE_DECLARE_DATE"] + " / " + drv.Row["SCHEDULE_FINAL_DATE"] + "</td></tr>" +
                        //"<tr><td>Część ogólna HLD</td><td>" + drv.Row["BUSSINES_REQ_DECLARE_DATE"] + " / " + drv.Row["BUSSINES_REQ_FINAL_DATE"] + "</td></tr>" +
                        //"<tr><td>Część ogólna HLD & systemowa</td><td>" + drv.Row["HLD_PHASE2_DECLARE_DATE"] + " / " + drv.Row["HLD_PHASE2_FINAL_DATE"] + "</td></tr>" +
                        //"<tr><td>Interface Agreement</td><td>" + drv.Row["IA_FINAL_DATE"] + "</td></tr>" +
                        "</table></span></a>";
                    //drv.Row.ItemArray[1] = s;
                    DataRow dr = dt.Rows.Add(n, s + sp, drv.Row["first_name"] + " " + drv.Row["last_name"], 
                     "<div class=\"tooltipKZG\">"+   dv2.Table.Rows[0]["nowe"]+"<span class=\"tooltiptextKZG\"> Nowych interfejsów </span></div>",
                      "<div class=\"tooltipKZG\">" + dv2.Table.Rows[0]["zmieniane"] + "<span class=\"tooltiptextKZG\"> Zmienianych interfejsów </span></div>",
                      "<div class=\"tooltipKZG\">" + dv2.Table.Rows[0]["usuwane"] + "<span class=\"tooltiptextKZG\"> Usuwanych interfejsów </span></div>",
                      "<div class=\"tooltipKZG\">" + dv2.Table.Rows[0]["uzywane"] + "<span class=\"tooltiptextKZG\"> Użytych interfejsów </span></div>"
                      /*,h*/);

                }
                catch (Exception exc)
                {

                //    Response.Write("Błąd: " + exc.Message + "<br>");
                  //  Response.Write("sql: " + sql + "<br>");

                }
            }
            GridView1.DataSource = dt;
            
            GridView1.DataBind();
            
        }

        protected void przygotujFiltry()
        {
            string sql = @"select id, name from sdpd.project_phase where id in (22,23,24,25,31,35) order by name asc;";

            SqlDataSource1.SelectCommand = sql;
            DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
           
              foreach (DataRowView drv in dv1)
              {
                  CheckBoxListStatus.Items.Add(new ListItem(drv.Row["name"].ToString(), drv.Row["id"].ToString()));
               if (new int[]{22,23,24,25,31,35}.Contains(Int32.Parse(drv.Row["id"].ToString())))
                {
                   CheckBoxListStatus.Items.FindByValue(drv.Row["id"].ToString()).Selected= true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              //  przygotujFiltry();
               
            }
            listaProjektow();

            return; 
            string sql="";
            string sql2="";
            try
            {
                String con = ConfigurationManager.ConnectionStrings["MySQLConnStr5.1"].ConnectionString;
                using (OdbcConnection connection = new OdbcConnection(con))
                {
                    connection.Open();
                    sql = @"select schema_name from information_schema.SCHEMATA where ( SCHEMA_NAME like 'eu-%' or SCHEMA_NAME like 'pr-%')
                                    order by SCHEMA_NAME asc;";
             
                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ltListaProjektow.Text +="<p>";
                             ltListaProjektow.Text +=@"<a class=""btn btn-default";

                             try
                             {
                                 sql2 = "select o.note from `" + dr[0].ToString() + "`.t_object o where o.Name='Projekt-Nazwa';";
                                 //   Response.Write("sql2: " + sql2 + "<br>");
                                 using (OdbcCommand command2 = new OdbcCommand(sql2, connection))
                                 using (OdbcDataReader dr2 = command2.ExecuteReader())
                                 {
                                     while (dr2.Read())
                                     {
                                         ltListaProjektow.Text += " href=hld.aspx?schema=" + dr[0].ToString() + ">";
                                         ltListaProjektow.Text += dr[0].ToString() + " " + dr2[0].ToString();
                                         ltListaProjektow.Text += @"</a></p>";
                                         break;
                                     }

                                     dr2.Close();
                                 }
                             }
                             catch (Exception exc)
                             {
                                 ltListaProjektow.Text += "a/>";
                                 ltListaProjektow.Text += dr[0].ToString() + " exc:" + exc.Message;
                                 ltListaProjektow.Text += @"</p>";
                             }
                            
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write("Błąd: " + ex.Message+"<br>");
                Response.Write("sql: " + sql + "<br>");
                Response.Write("sql2: " + sql2 + "<br>");
            }    
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
         
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string decodedText = HttpUtility.HtmlDecode(e.Row.Cells[i].Text);
                    e.Row.Cells[i].Text = decodedText;
                }
            }
        }

        protected void CheckBoxListStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            listaProjektow();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = GridView1.DataSource as DataTable;

           if (dataTable != null)
           {
               string SortDir = string.Empty;
               if (e.SortDirection == SortDirection.Ascending)
               {
                   SortDir = "Desc";
               }
               else
               {                   
                   SortDir = "Asc";
               }
              DataView dataView = new DataView(dataTable);
               if(e.SortExpression!="Kod")  dataView.Sort = e.SortExpression + " " + SortDir + ", " + "Kod Desc";
               else dataView.Sort = e.SortExpression + " " + SortDir;

              GridView1.DataSource = dataView;
              GridView1.DataBind();
           }
        }

       
    }
}