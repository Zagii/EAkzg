using EAkzg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
using System.Text;
using MarkupConverter;
using System.Threading;
using System.Text.RegularExpressions;

namespace EAkzgHLDRepo
{
    public partial class DlaJustyny : System.Web.UI.Page
    {
        static DataTable gvdt=new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListBox1.Items.Clear();
              //  ListBox1.Items.Add(new ListItem("pr-2107"));
                ListBox1.SelectedIndex = 0;
               // dajZespol("PR-2107");
             //   GridViewZespolLudzikow.Visible = true;  
                gvdt.Columns.AddRange(new DataColumn[3] { new DataColumn("Projekt",  typeof(string)),
                            new DataColumn("System", typeof(string)),
                            new DataColumn("Ludzik",typeof(string)) });
               // gvdt.Columns.Add("Projekt"); gvdt.Columns.Add("System"); gvdt.Columns.Add("Ludzik");
            }
         //   GridViewZespolLudzikow.DataSource = gvdt;
          //  GridViewZespolLudzikow.DataBind();
            
        }
        private void dajZespol(string schema)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Projekt"); dt.Columns.Add("System"); dt.Columns.Add("Ludzik");
            string sql = "select '"+schema+"' as Projekt ,o.name as System, pr.Value as Ludzik from " +
              "`" + schema + "`.t_object o,  " +
               "`" + schema + "`.t_package p," +
               "`" + schema + "`.t_objectproperties pr where  p.Name='Słownik' and p.package_id=o.Package_ID " +
                " and o.object_id=pr.Object_ID and pr.Property='Imie i Nazwisko' union all select '" + schema + "' as Projekt ,  sys.name,pr.Value from " +
   "`" + schema + "`.t_object o,  " +
    "`" + schema + "`.t_package p, " +
    "`" + schema + "`.t_connector c, " +
    "`" + schema + "`.t_object sys ," +
    "`" + schema + "`.t_objectproperties pr " +
    "    where  p.Name='Wkłady Systemowe' and p.package_id=o.Package_ID and " +
     "   c.start_Object_ID=o.Object_ID and sys.object_id=c.end_Object_ID and sys.object_id=pr.Object_ID " +
     "   and pr.Property='Rozwój' order by 1";
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    for (int a = 0; a < dv1.Table.Rows.Count; a++)
                    {
                        DataRow dr = gvdt.NewRow();
                        DataRow dvr = dv1.Table.Rows[a];
                      //  dr[0] = dvr[0]; dr[1] = dvr[1]; dr[2] = dvr[2];
                        dr = dvr;
                        gvdt.ImportRow(dr);

                    }
            
                }
                else
                {
                    //GridViewZespol.Visible = false;
                    // lt1_4.Text = "Brak";
                }
            }
            catch (Exception ex)
            {
                Response.Write("Błąd: " + ex.Message + "<br>");
                Response.Write("sql: " + sql + "<br>");
            
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            bool b = false;
            TextBox1.Text=TextBox1.Text.ToLower();
            if (TextBox1.Text == "") b = true;
           string sql = @"select schema_name from information_schema.SCHEMATA where schema_name='"+TextBox1.Text+
                                    "' order by SCHEMA_NAME asc;";

           SqlDataSource1.SelectCommand = sql;
           try
           {
               DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
               if (dv1.Table.Rows.Count > 0)
               {
                   
             
                   if (ListBox1.Items.Contains(new ListItem(TextBox1.Text)))
                   {
                       poka("Taki schemat już istnieje: " + TextBox1.Text + " nic nie dodaję.");
                       return;
                   }
                   ListBox1.Items.Add(TextBox1.Text);
                   poka("Dodaję: " + TextBox1.Text + " długość listy=" + ListBox1.Items.Count);
                   TextBox1.Text = "";
                   pokaListeLudzikow();
               }
               else
               {
                   b = true;
                   poka("Brak schematu o nazwie: "+TextBox1.Text);
               }
           }
           catch(Exception ex)
           {
               Response.Write("Błąd: " + ex.Message + "<br>");
               Response.Write("sql: " + sql + "<br>");
            
               return;
           }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ListBox1.SelectedIndex >= 0 && ListBox1.SelectedIndex < ListBox1.Items.Count)
            {
                poka("Usuwam pozycje "+ ListBox1.SelectedIndex+"/" +ListBox1.Items.IndexOf(ListBox1.SelectedItem) +": " + ListBox1.Items[ListBox1.SelectedIndex]);
                ListBox1.Items.RemoveAt(ListBox1.Items.IndexOf(ListBox1.SelectedItem));
                pokaListeLudzikow();
            }
        }

        void poka(string t)
        {
            Info.Text = t;
            Info.Visible = true;
            Timer1.Enabled = true;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Info.Visible = false;
            Timer1.Enabled = false;
        }


        void pokaListeLudzikow()
        {
            gvdt.Clear();
            for (int i = 0; i < ListBox1.Items.Count;i++ )
            {
                string schemat = ListBox1.Items[i].ToString();
                dajZespol(schemat);
               
            }
            GridViewZespolLudzikow.DataSource = gvdt;
            GridViewZespolLudzikow.DataBind();
            GridViewZespolLudzikow.Visible = true;
            
        }
    }
}