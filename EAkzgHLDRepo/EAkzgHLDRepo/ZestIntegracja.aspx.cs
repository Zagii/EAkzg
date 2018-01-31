using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Diagnostics;

namespace EAkzgHLDRepo
{
    public partial class ZestIntegracja : System.Web.UI.Page
    {
        String schema = "";

        public string walidujDate(string txt,DateTime def)
        {
     /*       DateTime d;
            if (!DateTime.TryParse(txt, out d))
            {
                d = def;
            }
            return d.ToString("MM-dd-yyyy");
            */
            return txt;
        }
        protected void RaportujZmianyInterfejsow()
        {
            string projekt = "";
            if (DropDownList1.SelectedIndex > 0)
            {
                projekt = " and p.code='" + DropDownList1.SelectedValue.ToString() + "' ";
            }
         //   SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnStr5.1"].ConnectionString;
            Stopwatch scaly = Stopwatch.StartNew();
            string statusy = "(";
            
            
            statusy+=" inter.Stereotype in ( 'fikcja',";
            int tmpdl = statusy.Length;
            if (StatusCheckBox_new.Checked) statusy += "'" + StatusCheckBox_new.Text + "'" + ", ";
            if (StatusCheckBox_change.Checked) statusy += "'" + StatusCheckBox_change.Text + "'" + ", ";
            if (StatusCheckBox_reuse.Checked) statusy += "'" + StatusCheckBox_reuse.Text + "'" + ", ";
            if (StatusCheckBox_remove.Checked) statusy += "'" + StatusCheckBox_remove.Text + "'" + ", ";
            if (statusy.Length == tmpdl) //zaden status nie zaznaczony
            {
                statusy = "(";
                if (StatusCheckBox_null.Checked) // ale wyswietla nule
                {
                    statusy += " inter.stereotype is null ";
                }
                else // nic nie zaznaczone czyli kazdy status dobry
                {
                    statusy += "true";
                }

            }
            else //zaznaczony jakis status
            {
                statusy = statusy.Substring(0, statusy.Length - 2);
                statusy += ")";
                if (StatusCheckBox_null.Checked)
                {
                    statusy += " or inter.stereotype is null ";
                }
            
               
            }
        
          //  if (StatusCheckBox_inne.Checked) statusy += " or inter.Stereotype not in ('new', 'change', 'reuse', 'remove')";
            
    
            statusy += ")";
            
            String dod=walidujDate(TextAreaDataOd.Text,DateTime.Now.AddMonths(-1));
            TextAreaDataOd.Text = dod;
            string ddo=walidujDate(TextAreaDataDo.Text,DateTime.Now);
            TextAreaDataDo.Text=ddo;
              String dodp=walidujDate(TextAreaDataProjOd.Text,DateTime.Now.AddMonths(-1));
            TextAreaDataProjOd.Text=dodp;
            string ddop=walidujDate(TextAreaDataProjDo.Text,DateTime.Now);
            TextAreaDataProjDo.Text = ddop;

            if (projekt.Length > 0)
            {
                dod=dodp="01-01-1900";
                ddo = ddop = "01-01-3000";
            }
            string datyMod=" between STR_TO_DATE('"+dod+"', '%d-%m-%Y') and STR_TO_DATE('"+ddo+"', '%d-%m-%Y')";
            string datyCre = " between STR_TO_DATE('" + dodp + "', '%d-%m-%Y') and STR_TO_DATE('" + ddop + "', '%d-%m-%Y')";

          

            string sql = "";
            // SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
            sql = @"select code as Symbol, name as Nazwa, created_date as DataUtworzenia
                from sdpd.project p ,information_schema.SCHEMATA s
                  where (p.code like 'PR-%' or p.code like 'EU-%') "+ projekt+
                 " and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%') "+
                " and lower(p.code)=lower(s.schema_name) and p.created_date " + datyCre;
            SqlDataSource1.SelectCommand = sql;

       //     SqlDataSource ds1 = new SqlDataSource();
      //      ds1.ConnectionString = SqlDataSource1.ConnectionString;
           

            DataView dv = (DataView) SqlDataSource1.Select(new DataSourceSelectArguments());
         //   Response.Write("caly init="+scaly.ElapsedMilliseconds + "<br>");
            DataTable dt= new DataTable();
            dt.Columns.Add("Symbol"); dt.Columns.Add("Nazwa interfejsu"); dt.Columns.Add("Typ zmian"); dt.Columns.Add("System");
            dt.Columns.Add("Relacja"); //dt.Columns.Add("Data utworzenia"); dt.Columns.Add("Data zmiany");
            dt.Columns.Add("ChM"); dt.Columns.Add("IP_SIT"); dt.Columns.Add("IP_PRE"); dt.Columns.Add("IP_PROD");
            foreach(DataRowView rowView in dv)
            {
                DataRow r=rowView.Row;
               // String [] r= {"pr-3427"};
                Stopwatch s1 = Stopwatch.StartNew();
                Stopwatch s2 = Stopwatch.StartNew();
          

            sql = @" select a.symbol, a.intName `Nazwa interfejsu`, a.stereotype `Typ zmian`,  sysSt.name `System`, a.connector_type, "+
                //a.createdDate,a.modifiedDate,
                " prop.Chm, prop.IP_SIT, prop.IP_PRE, prop.IP_PROD  from " +
     "`"+r[0]+"`.t_object sysSt "+
 ", ( select s1.Object_ID,max(s1.name) SysName, "+
 "max(case when isit.Property ='IP_SIT' and isit.Property is not null then isit.Value end) IP_SIT,"+
 "max(case when isit.Property ='IP_PRE' and isit.Property is not null then isit.Value end) IP_PRE,"+
 "max(case when isit.Property ='IP_PROD' and isit.Property is not null then isit.Value end) IP_PROD,"+
 "max(case when isit.Property ='Rozwój' and isit.Property is not null then isit.Value end) ChM from "+
    "`"+r[0]+"`.t_object s1 left join "+
 "`"+r[0]+"`.t_objectproperties isit on s1.object_id=isit.object_id " +
 "where s1.Object_Type='Component' group by  s1.Object_ID)prop, ( select "+
   "'" + r[0] + "' as Symbol, inter.name intName, inter.Stereotype,c.connector_type,c.End_Object_ID,c.Start_Object_ID, inter.CreatedDate,inter.ModifiedDate from " +
 "`"+r[0]+"`.t_object inter,"+
 "`"+r[0]+"`.t_connector c, "+
 "`"+r[0]+"`.t_package pakPR, "+
"`"+r[0]+"`.t_package pakObsz,"+
"`"+r[0]+"`.t_package pakHLD,"+
 "`"+r[0]+"`.t_package pakArchSt,"+
"`"+r[0]+"`.t_diagram diagrArchStat,"+
 "`" + r[0] + "`.t_diagramobjects diagrObjects" +
 " where  "+statusy+
 " and pakPR.Parent_ID=0 and "+
"  pakHLD.parent_ID=pakPR.Package_ID and pakHLD.Name='HLD' and "+
 " pakObsz.parent_id=pakHLD.package_id and pakObsz.Name in ('IT','NT') and "+
 " pakArchSt.parent_id=pakObsz.package_id and pakArchSt.name='Architektura Statyczna' and "+
 " diagrArchStat.Package_ID=pakArchSt.package_id and "+
 " diagrObjects.Diagram_ID = diagrArchStat.Diagram_ID and "+
 " diagrObjects.Object_ID = inter.Object_ID " +
  " and inter.Object_type='Interface'  and (inter.modifiedDate "+datyMod+
  " or inter.createdDate "+datyCre+")  and c.Connector_Type in ('Realisation','Usage') and (c.End_Object_ID=inter.Object_ID or c.Start_Object_ID=inter.Object_ID) "+
   ") a  where sysSt.Object_ID=a.start_object_id   and sysSt.object_type='Component'  and prop.object_id=a.start_object_id order by 2 asc, 5 asc;";

          

            if (r[0].ToString() == "PR-3427")
            {
                int x = 0;
            }
            try
            {
                SqlDataSource1.SelectCommand = sql;
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                s2.Stop();
                Stopwatch s3 = Stopwatch.StartNew();
               foreach (DataRowView drv in dv1)
               {
                    dt.Rows.Add(drv.Row.ItemArray);
                }
               s3.Stop();
               s1.Stop();

              
               if (s1.ElapsedMilliseconds > 1000)
               {
                   Response.Write(r[0]+" ->"+s2.ElapsedMilliseconds + "ms + " + s3.ElapsedMilliseconds + "ms = " + s1.ElapsedMilliseconds + "<br>");
                   Response.Write("Długi czas dla sql= " + sql + "<br>");
               }
            }
            catch (Exception exc)
            {
                Response.Write("Błąd: " + exc.Message + "<br>");
                Response.Write("sql: " + sql + "<br>");
              //  Response.Write("sql2: " + sql2 + "<br>");
            }

            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
          //  Response.Write("caly koniec=" + scaly.ElapsedMilliseconds + "<br>");
        }
       
        protected void Raportuj()
        {
            string statusy = "";
            if (StatusCheckBox_new.Checked) statusy += StatusCheckBox_new.Text + ", ";
            if (StatusCheckBox_change.Checked) statusy += StatusCheckBox_change.Text + ", ";
            if (StatusCheckBox_reuse.Checked) statusy += StatusCheckBox_reuse.Text + ", ";
            if (StatusCheckBox_remove.Checked) statusy += StatusCheckBox_remove.Text + ", ";
           // if (StatusCheckBox_inne.Checked) statusy += StatusCheckBox_inne.Text + ", ";
            if (statusy.Length > 0) statusy = statusy.Substring(0, statusy.Length - 2);
            string sql = "";
            string sql2 = "";
            try
            {
                String con = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
                using (OdbcConnection connection = new OdbcConnection(con))
                {
                    connection.Open();
                    sql = @"select code, name,created_date, SHORT_DESC 
                                        from sdpd.project 
                                        where (code like 'PR-%' or code like 'EU-%') 
                                            and CREATED_DATE  between STR_TO_DATE('12-10-2016', '%d-%m-%Y') and STR_TO_DATE('12-11-2016', '%d-%m-%Y');";

                  

                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ltRaport.Text += dr[0]+", "+dr[1]+", "+dr[2]+"<br>";
                           

                            try
                            {
                  /*              sql2 = "select o.note from `" + dr[0].ToString() + "`.t_object o where o.Name='Projekt-Nazwa';";
                                //   Response.Write("sql2: " + sql2 + "<br>");
                                using (OdbcCommand command2 = new OdbcCommand(sql2, connection))
                                using (OdbcDataReader dr2 = command2.ExecuteReader())
                                {
                                    while (dr2.Read())
                                    {
                                //        ltRaport.Text += " href=hld.aspx?schema=" + dr[0].ToString() + ">";
                               //         ltRaport.Text += dr[0].ToString() + " " + dr2[0].ToString();
                                 //       ltRaport.Text += @"</a></p>";
                                        break;
                                    }

                                    dr2.Close();
                                }*/
                            }
                            catch (Exception exc)
                            {
                             //   ltListaProjektow.Text += "a/>";
                           //     ltListaProjektow.Text += dr[0].ToString() + " exc:" + exc.Message;
                            //    ltListaProjektow.Text += @"</p>";
                            }

                        }
                        dr.Close();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write("Błąd: " + ex.Message + "<br>");
                Response.Write("sql: " + sql + "<br>");
                Response.Write("sql2: " + sql2 + "<br>");
            }    
 
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //bo datepicker ma taki format daty domyslny
            TextAreaDataDo.Text = DateTime.Now.ToString("dd-MM-yyyy");
            TextAreaDataOd.Text = DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy");
            TextAreaDataProjDo.Text = DateTime.Now.ToString("dd-MM-yyyy");
            TextAreaDataProjOd.Text = DateTime.Now.AddMonths(-3).ToString("dd-MM-yyyy");
          //  RaportujZmianyInterfejsow();
        }
        private void ustawGUI()
        {
           string sql = @"select code as Symbol,p.name,  concat(code,' ',p.name) opis
                from sdpd.project p ,information_schema.SCHEMATA s
                  where (p.code like 'PR-%' or p.code like 'EU-%') " +
                   " and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%')" +
                " and lower(p.code)=lower(s.schema_name) order by 1 desc ";

            SqlDataSource1.SelectCommand = sql;
            DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            DropDownList1.DataSource = dv1.ToTable();
            DropDownList1.DataTextField = "opis";
            DropDownList1.DataValueField = "Symbol";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0,"--Wszystkie projekty--");
            if (schema!=null)
            {
                if (schema.Length > 0)
                {
                    try
                    {
                        DropDownList1.SelectedIndex = DropDownList1.Items.IndexOf(DropDownList1.Items.FindByValue(schema));
                    }
                    catch
                    {
                        DropDownList1.SelectedIndex = 0;
                        schema = "";
                    }
                }
                else
                {
                    DropDownList1.SelectedIndex = 0;
                }
            }
            widokFiltrGUI();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            schema = "";
            schema = Request.QueryString["proj"];
            Label1.Text = "";

            if (!IsPostBack)
            {
                ustawGUI();
            }

            RaportujZmianyInterfejsow();

        }
       


        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RaportujZmianyInterfejsow();
            }
            else
            {
               // Label1.Text = "Błędne kryteria wyszukania";
            }
        }

        protected void TextAreaDataProjOd_TextChanged(object sender, EventArgs e)
        {
           // TextAreaDataProjOd.Text = walidujDate(TextAreaDataProjOd.Text, DateTime.Now.AddMonths(-1));
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
            args.IsValid = true;
            Label1.Text = "";
            return;
            try
            {
                DateTime.ParseExact(TextAreaDataDo.Text, "dd-MM-yyyy",  System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }    
            catch
            {
                Label1.Text = "DataDo, ";
                args.IsValid = false;
            }
            try
            {
                DateTime.ParseExact(TextAreaDataOd.Text, "dd-MM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                
            }
            catch
            {
                Label1.Text += "DataOd, ";
                args.IsValid = false;
            }
            try
            {
                DateTime.ParseExact(TextAreaDataProjDo.Text, "dd-MM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
               
             }
            catch
            {
                Label1.Text += "DataProjektuDo, ";
                args.IsValid = false;
            } 
            try
            {
               DateTime.ParseExact(TextAreaDataProjOd.Text, "dd-MM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
               
            }
            catch
            {
                Label1.Text += "DataProjektuOd, ";
                args.IsValid = false;
            } 

           
            
            
        }

       

        protected void GridView_RowDataBound(object sender, EventArgs ee)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Text == "&nbsp;")
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }
        protected void widokFiltrGUI()
        {
            if (DropDownList1.SelectedIndex > 0)
            {
                TextAreaDataProjOd.Visible = false;
                TextAreaDataProjDo.Visible = false;
                TextAreaDataOd.Visible = false;
                TextAreaDataDo.Visible = false;
                modObjLbl.Visible = false;
                zalProjLbl.Visible = false;
                nazwaProjektuLbl.Text = DropDownList1.SelectedItem.ToString();
                Button1.Visible = false;
                projDoLbl.Visible = false;
                projOdLbl.Visible = false;
                odLbl.Visible = false;
                doLbl.Visible = false;
            }
            else
            {
                TextAreaDataProjOd.Visible = true;
                TextAreaDataProjDo.Visible = true;
                TextAreaDataOd.Visible = true;
                TextAreaDataDo.Visible = true;
                modObjLbl.Visible = true;
                zalProjLbl.Visible = true;
                nazwaProjektuLbl.Text = DropDownList1.SelectedItem.ToString();
                Button1.Visible = true;
                projDoLbl.Visible = true;
                projOdLbl.Visible = true;
                odLbl.Visible = true;
                doLbl.Visible = true;
            }
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            widokFiltrGUI();
        }
    }
}