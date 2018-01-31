using EAkzg;
using System;
using System.Diagnostics;
using System.ComponentModel;
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
using Spire.Doc;
using Spire.Doc.Documents;
using System.Threading.Tasks;
using System.Security.Principal;


namespace EAkzgHLDRepo
{
   
    public class CWklady
    {
        
        private string _obszar;
        private int _nr;
        private int _nrRozdz;
        private string _systemID;
        private string _System;
        private string _PakietID;
        private string _Pakiet;
        private string _Chm;
        private hld HLDdoc;
        public CWklady(hld h,int n, string obszar, int nr, int nrRozdz, string systemID, string System, String PakietID, string Pakiet, String ChM, ManualResetEvent doneEvent)
        {
            HLDdoc = h;
            _n = n; _obszar = obszar; _nr = nr; _nrRozdz = nrRozdz;
            _systemID = systemID; _System = System; _PakietID = PakietID; _Pakiet = Pakiet; _Chm = ChM;
            _doneEvent = doneEvent;
            _phMenu = new PlaceHolder();
            _phTresc = new PlaceHolder();
        }

        // Wrapper method for use with thread pool.
        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
       //    hld.Deb("thread {0} started..."+threadIndex);
            if (_System != "Fasttrack")
            {
                 HLDdoc.dajSystem(_obszar, _nr, _nrRozdz, _systemID, _System, _PakietID, _Pakiet, _Chm, _phTresc, _phMenu);

            }

        //    hld.Deb("thread {0} result calculated..." + threadIndex);
            _doneEvent.Set();
        }


        public int N { get { return _n; } }
        private int _n;

        public PlaceHolder phTresc { get { return _phTresc; } }
        private PlaceHolder _phTresc;

        public PlaceHolder phMenu { get { return _phMenu; } }
        private PlaceHolder _phMenu;

      //  public string FibOfN { get { return _fibOfN; } }
      //  private string _fibOfN;

        private ManualResetEvent _doneEvent;
    }


    public static class ReflectionHelpers
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
    public partial class hld : System.Web.UI.Page
    {
        enum styl { brak,
                    [Description("Tytul")] Tyt, 
                    [Description("Tytul_1")] Tyt_1,
                    [Description("Tytul_1_2")] Tyt_1_2,
                    [Description("Tytul_1_2_3")] Tyt_1_2_3,
                    [Description("Tytul_1_2_3_4")] Tyt_1_2_3_4,
                    [Description("txt_feature_param")] txt_feature_param,
                    [Description("txt_feature_tytul")] txt_feature_tytul,
                    [Description("txt_normal")] Norm,
                    [Description("floatMenu")] floatMenu,
        };

    //    private IMarkupConverter markupConverter;
        String schema="";
        int debugCzas = 0;
        CModel model;

        public void Deb(string s, string s1 = "")
        {
            return;
            System.Diagnostics.Debug.WriteLine(s + s1);
            Response.Write("<script>console.log('" + s + s1 + "');</script>");
        }

      //  EA.Repository repository;
        private void dajSlownik(PlaceHolder ph,string t)
        {
            wstawLabel(ph, t, styl.Tyt_1_2, "r1_1", "r1_1");
            wstawMenu(HLDmenu, "1.1 Słownik", "r1_1", styl.brak);
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Skrót/pojęcie"); dt.Columns.Add("Opis");
            string sql = "select  o.name as Pojęcie ,o.note as Opis from `"+
                schema+"`.t_object o,  `"+schema+"`.t_package p where  p.Name='Słownik' and p.package_id=o.Package_ID";
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                GridView g = new GridView();
                wstawGridView(ph, g);
                g.DataSource = dv1.ToTable();
                g.DataBind();
            }
            catch
            {
                wstawLabel(ph, "Brak", styl.Norm);
                return ;
            }
 
        }
        private void dajZalaczniki(PlaceHolder ph,string t)
        {
            wstawLabel(ph, t, styl.Tyt_1_2,"r1_3","r1_3");
            wstawMenu(HLDmenu, "1.3 Załączniki", "r1_2", styl.brak);
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Nazwa/Opis"); dt.Columns.Add("Autor"); dt.Columns.Add("Dokument");
            string sql = "select  o.name as Nazwa ,o.Author as Autor ,o.note Opis from `" +
                schema + "`.t_object o,  `" + schema + "`.t_package p where  p.Name='Załączniki' and p.package_id=o.Package_ID";
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    GridView g = new GridView();
                    wstawGridView(ph, g);
                    g.DataSource = dv1.ToTable();
                    g.DataBind();
                }
                else
                {
                  //  GridViewZalaczniki.Visible = false;
                    //lt1_3.Text = "Brak";
                    wstawLabel(ph, "Brak", styl.Norm);
                }
            }
            catch
            {
                return;
            }
        }
        private void dajZespol(PlaceHolder ph, string t)
        {
            wstawLabel(ph, t, styl.Tyt_1_2,"r1_2","r1_2");
            wstawMenu(HLDmenu, t, "r1_2", styl.brak);
        
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Rola / Obszar"); dt.Columns.Add("Imię i Nazwisko"); 
            string sql = "select  o.name as Obszar , pr.Value as 'Imię i Nazwisko' from "+
              "`"+ schema + "`.t_object o,  "+
               "`"+ schema + "`.t_package p,"+
               "`"+ schema + "`.t_objectproperties pr where  p.Name='Słownik' and p.package_id=o.Package_ID "+
                " and o.object_id=pr.Object_ID and pr.Property='Imie i Nazwisko' union all select  sys.name,pr.Value from "+
   "`"+ schema + "`.t_object o,  "+
    "`"+ schema + "`.t_package p, "+
    "`"+ schema + "`.t_connector c, "+
    "`"+ schema + "`.t_object sys ,"+
    "`" + schema + "`.t_objectproperties pr " +
    "    where  p.Name='Wkłady Systemowe' and p.package_id=o.Package_ID and "+
     "   c.start_Object_ID=o.Object_ID and sys.object_id=c.end_Object_ID and sys.object_id=pr.Object_ID "+
     "   and pr.Property='Rozwój' order by 1";
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    GridView g = new GridView();
                    wstawGridView(ph, g);
                    g.DataSource = dv1.ToTable();
                    g.DataBind();
                }
                else
                {
                   // GridViewZespol.Visible = false;
                 // lt1_4.Text = "Brak";
                    wstawLabel(ph, "Brak", styl.Norm);
                }
            }
            catch
            {
                return;
            }
        }
        private void dajPowiazania(PlaceHolder ph,string t)
        {
            wstawLabel(ph, t, styl.Tyt_1_2,"r1_3","r1_3");
            wstawMenu(HLDmenu, "1.3 Zależności projektowe", "r1_3", styl.brak);
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Projekt"); dt.Columns.Add("Termin wdrozenia"); dt.Columns.Add("Rodzaj zaleznosci");dt.Columns.Add("Opis");

            string sql = "select  o.name as Projekt,max(case when pr.property='Opis' then pr.value end) as Opis,max(case when pr.property='Krytycznosc' then pr.value end) as Krytycznosc, max(case when pr.property='Termin' then "+
" pr.value end) as Termin from "+
"`"+schema+"`.t_object o,  "+
"`"+schema+"`.t_package p, "+
"`"+schema+"`.t_objectproperties pr "+
                 "where  p.Name='Zależności' and p.package_id=o.Package_ID and pr.object_id=o.object_id and pr.Property in ('Opis','Krytycznosc','Termin') "+
                 " group by o.name ";
           
       
            
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    GridView g = new GridView();
                    wstawGridView(ph, g);
                    g.DataSource = dv1.ToTable();
                    g.DataBind();
                }
                else
                {
              
                    wstawLabel(ph, "Brak", styl.Norm);
                }
            }
            catch
            {
                return;
            }
        }
        public string dajNazweObiektu(String obiekt)
        {
            string sql = "select note from `" + schema + "`.t_object where name='"+obiekt+"'";
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                return (string)(dv.ToTable()).Rows[0][0].ToString();
            }
            catch
            {
                return "brak";
            }
        }
        static public  string Decompress(byte [] blob)
        {
            MemoryStream zipToOpen = new MemoryStream();
            String ret = "";
            zipToOpen.Write(blob, 0, blob.Length);
          //  FileStream zipToOpen=new FileStream(;
          //  theMemStream.CopyTo(zipToOpen);
            
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                       // if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                        {
                            Stream a = entry.Open();

                            using (StreamReader sr = new StreamReader(a))
                            {
                                ret = sr.ReadToEnd();
                               // Response.Write(ret);
                            }
                        }
                    }
                }

                return ret;
        }
        private string ConvertRtfToHtml(string rtfText)
        {
            var thread = new Thread(ConvertRtfInSTAThread);
            var threadData = new ConvertRtfThreadData { RtfText = rtfText };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(threadData);
            thread.Join();
            return threadData.HtmlText;
        }

        private void ConvertRtfInSTAThread(object rtf)
        {
         //   var threadData = rtf as ConvertRtfThreadData;
        //    threadData.HtmlText = markupConverter.ConvertRtfToHtml(threadData.RtfText);
        }


        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }
        private void dajKrotkiOpis(PlaceHolder ph)
        {
            wstawLabel(ph,  "2.1 Krótki opis projektu z perspektywy biznesowej", styl.Tyt_1_2,"r2_1","r2_1");
            wstawMenu(HLDmenu, "2.1 Krótki opis projektu", "r2_1", styl.brak);

            string txtIT = model.dajNotesObiektu(model.SkrotElem[0]);
            string txtNT = model.dajNotesObiektu(model.SkrotElem[1]);

            wstawLabel(ph, txtIT, styl.brak);
            wstawLinkedDocument(ph, model.SkrotElem[0].ToString());
            wstawLabel(ph, txtNT, styl.brak);
            wstawLinkedDocument(ph, model.SkrotElem[1].ToString());

                return;
      
        }
        private void dajOgraniczeniaRozwiazania(PlaceHolder ph)
        {
            wstawLabel(ph, "2.2 Ograniczenia rozwiązania ", styl.Tyt_1_2, "r2_2","r2_2");
            wstawMenu(HLDmenu, "2.2 Ograniczenia", "r2_2", styl.brak);
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Ograniczenie rozwiązania"); dt.Columns.Add("Opis"); 
            string sql = "select  o.name as 'Ograniczenie', o.note as 'Opis' from `" +
                schema + "`.t_object o,  `" + schema + "`.t_package p where  p.Name='Ograniczenia rozwiązania' and p.package_id=o.Package_ID and o.object_type!='Issue' "+
            "union all "+
            "select  o.name, o.note from `" +
                schema + "`.t_object o  where o.object_type='Issue'";

            SqlDataSource1.SelectCommand = sql;
            try
            {
              DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    GridView g = new GridView();
                    wstawGridView(ph, g);
                    g.DataSource = dv1.ToTable();
                    g.DataBind();
                }
                else
                {
              
                    wstawLabel(ph, "Brak", styl.Norm);
                }
            
            }
            catch
            {
                return;
            }

        }
        public class WymBiz
        {
            
            public WymBiz(hld h,/*byte[] n,*/string id, string note, string name)
            {
               // _n = n;
               
                _note = note;
                _name = name;
                _id = id;
                _hld = h;
            }

            // Wrapper method for use with thread pool.
            public void ThreadPoolCallback()
            {
                

                try
                {
                    _note = _note + _hld.dajHtmlLincedDocument(_id);
                }
                finally
                {
                   
                }
            }

            private hld _hld;
            private string _id;
            public byte [] N { get { return _n; } }
            private byte [] _n;

            public string dajNote { get { return _note; } }
            private string _note;
            public string dajName { get { return _name; } }
            private string _name;
            public PlaceHolder phTresc { get { return _phTresc; } }
            private PlaceHolder _phTresc;


           
        }
       
        private void dajWymaganiaBizTaskPool(DataView dv1, DataTable ndt)
        {
             int ileZadan = dv1.Table.Rows.Count;

             WymBiz[] wymArray = new WymBiz[ileZadan];

            // Configure and launch threads using ThreadPool:
            Deb("Wymagania biznesowe zadan: "+ ileZadan);
            int i=0;
             int remainingToProcess =ileZadan;

             using (var mre = new ManualResetEvent(false))
             {
            while (i < ileZadan)
            {
               
               
                                 
                    DataRow dr2=dv1.Table.Rows[i];
             //       doneEvents[j] = new ManualResetEvent(false);
                   
                    string note = "";
                    string name = "";
                    string id = "";
                    try
                    {
                        note = dr2["note"].ToString();
                        name = dr2["name"].ToString();
                        id = dr2["object_id"].ToString();
                    }
                    catch (Exception exc)
                    {
                        Deb("dajWymaganiaBiz Parser error #" + i + "=" + exc.Message);
                    }
                    WymBiz f = new WymBiz(this, id, note, name);
                    wymArray[i] = f;
                   
                    ThreadPool.QueueUserWorkItem(delegate 
                        {
                            f.ThreadPoolCallback();
                            if (Interlocked.Decrement(ref remainingToProcess) == 0)
                                mre.Set();
                          });
                           
                    
                   i++;
                }
                 mre.WaitOne();
             }
            
            
                // czeka na wszystkie
                //WaitHandle.WaitAll(doneEvents);
                
                

                /*foreach(ManualResetEvent d in doneEvents)  
                {
                   d.Dispose();
                }*/
          
        //    Deb("Przeliczone wszystkie zadania.");

            // Display the results...
            for (int ii = 0; ii < dv1.Table.Rows.Count; ii++)
            {
                WymBiz f = wymArray[ii];
                DataRow newRow = ndt.NewRow();
                newRow["Tytuł"] = f.dajName;

                newRow["Opis"] = f.dajNote;
                ndt.Rows.Add(newRow);
            }
        }
        private void dajWymaganiaBizSingle(DataView dv1, DataTable ndt)
        {
            ///////////// wiele watkow
            string[] htmlLin = new string[dv1.Table.Rows.Count];
            //  Task  <string> [] t=new Task<String> [10]();

            List<byte[]> texts = new List<byte[]>();
            foreach (DataRow dr2 in dv1.Table.Rows)
            {
                if (dr2["binContent"] == System.DBNull.Value)
                {
                    texts.Add(ASCIIEncoding.ASCII.GetBytes(""));

                }else{
                    texts.Add((byte[])dr2["binContent"]);
                }

            }

            int ind = 0;
            Parallel.ForEach(texts, i =>
            {

                htmlLin[ind++] = binToHtml(i);

            }
                );
            ///////////// wiele watkow
            int x = 0;
            foreach (DataRow dr2 in dv1.Table.Rows)
            {
                DataRow newRow = ndt.NewRow();
                string bin = (String)dr2["BinContent"].ToString();
                newRow["Tytuł"] = (String)dr2["name"].ToString();
                newRow["Opis"] = (String)dr2["note"].ToString();

                //////////// jeden watek
                //       byte [] b=( byte[] )dr2["binContent"];
                //       newRow["Opis"] += @"<BR>" + binToHtml(b);

                ////////////////jeden watek

                ///////////// wiele watkow


                newRow["Opis"] += @"<BR>" + htmlLin[x++];

                ////////////wiele watkow

                ndt.Rows.Add(newRow);
            }
        }

        private void dajWymaganiaBiz(PlaceHolder ph)
        {
            wstawLabel(ph, "2.3 Wymagania biznesowe", styl.Tyt_1_2,"r2_3","r2_3");
            wstawMenu(HLDmenu, "2.3 Wymagania biznesowe", "r2_3", styl.brak);
         //   "select Object_ID from t_object where object_type='Requirement'
                DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            dt.Columns.Add("Wymaganie biznesowe"); dt.Columns.Add("Opis"); 
            string sql = "select  o.name 'Wymaganie biznesowe', o.note 'Opis' from `" +
                schema + "`.t_object o where o.object_type='Requirement' ";

            sql = @"SELECT o.object_id,d.BinContent, o.Note, o.name FROM " +
               "`" + schema + "`.t_object o, " +
               "`" + schema + "`.t_document d  WHERE " +
              " o.Style LIKE '%MDoc=1%' and o.object_type='Requirement' and d.elementid=o.ea_guid  ";

            sql = "SELECT o.object_id , d.BinContent, o.Note, o.name FROM " +
               "`" + schema + "`.t_object o  left join `" + schema + "`.t_document d   on d.elementid=o.ea_guid " +
               "where o.object_type='Requirement'";

            SqlDataSource1.SelectCommand = sql;
            try
            {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                 //   GridView g = new GridView();
                  //  wstawGridView(HLDtresc, g);
                  //  g.DataSource = dv1.ToTable();
                  //  g.DataBind();

                    ///////////////////
                    GridView gv = new GridView();
                    wstawGridView(ph, gv);

                    DataTable ndt = new DataTable("x");
                    ndt.Columns.Add("Tytuł"); 
                    ndt.Columns.Add("Opis");


                   // dajWymaganiaBizSingle(dv1,ndt);
                  dajWymaganiaBizTaskPool(dv1,ndt);


                    gv.DataSource = ndt;
                    gv.DataBind();


                    
                }
                else
                {
              
                    wstawLabel(ph, "Brak", styl.Norm);
                }
            
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               Deb(exc.Message.ToString() + " sql=" + sql);
                return;
            }
 
        }
        private string zmienObrazki(string rtf)
        {
            
            string r = @"\{\\pict(.*)\s(.*)\}";
                r = @"{\\pict[^ ]* ([^\}]*)\}";
     //       string i = "<img src=\"data:image/emf;base64,"; //<img src=\"data:image/emf;base64,"+"$2"+"\">
        //rtf=@"\fs20 {\*\shppict{\pict\emfblip\picw25837\pich26393\picwgoal10980\pichgoal11235\picscalex82\picscaley82\sspicalign0 010000006c0000000100000001000000db020000ec0200000000000000000000ed6400001967000020454d46000001008c8802002d18000005000000000000000000000000000000900600001a04000051020000720100000000000000000000000000001a0b0900f0a60500460000002c00000020000000454d462b014001001c000000100000000210c0db01000000600000006000000046000000e0000000d4000000454d462b1e4005000c000000000000002a40000024000000180000000000803f0000000000000000000080bf00000000000000002a40000024000000180000000000803f0000000000000000000080bf000000c00000004108400002380000002c0000000210c0db0000000086000000000000 00000000000100000001000000000000000210c0db00000000000000ff0840010334000000280000000210c0db0500000000}}alasm";
            try
            {
                Regex regex = new Regex(r,RegexOptions.Singleline);
                MatchCollection matches = regex.Matches(rtf);
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        Response.Write("Index=" + capture.Index + ", value= <br>" + capture.Value);
                    }
                }
                string w = "";
                Response.Write(w + "<br>");
                //string d = "$2";
                w = Regex.Replace(rtf, r, "<img src=\"data:image/emf;base64,$1\"/>", RegexOptions.Singleline);


                Response.Write(w + "<br>");
                return w;
            }
               catch (Exception exc)
            {
                Response.Write(exc.Message.ToString()+ ", r="+r+"\n");
               
            }
           
            return "";
        }
        private void dajArchitektureStatyczna(PlaceHolder ph,string obszar,string nr)
        {
            wstawLabel(ph, nr + " Architektura Statyczna " + obszar, styl.Tyt_1_2,nr,nr);
            wstawMenu(HLDmenu, nr + " Architektura Statyczna " + obszar, nr, styl.brak);
            DataTable dt = new DataTable();
            string sql;
            sql = @"select d.diagram_id,d.name,d.notes, l.sciezka,l.plik from "+
                    "`" + schema + "`.t_package pas,"+
                    "`" + schema + "`.t_package pob,"+
                    "`" + schema + "`.t_package phl,"+
                    "`" + schema + "`.t_diagram d, eakzg_schema.eakzg_wyrzyg_log l where "+
                    "pas.Parent_ID=pob.package_id "+
                    "and pob.parent_id=phl.package_id "+
                    "and pas.name='Architektura Statyczna' " +
                    "and phl.name='HLD' "+
                    "and pob.name='" + obszar + "'" +
                    "and d.Package_ID=pas.Package_ID "+
                    "and l.objectID=d.Diagram_ID "+
                    "and l.projekt='"+schema+"'";

            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    int i=1;
                  //  string o=".2";
                 //   if(obszar!="IT")o=".3";
                    foreach (DataRowView dr in dv1)
                    {

                        int objID = (int)dr["diagram_id"];
                        string name = dr["name"].ToString();
                        string note = dr["notes"].ToString();
                        //lt3_2.Text +="3"+o+"."+i+" "+ name;
                        wstawLabel(ph, nr + "." + i + " " + name, styl.Tyt_1_2_3, "as" + objID, "as" + objID);

                        string sciezka = dr["sciezka"].ToString();
                        string plik = dr["plik"].ToString();


                        string sciezkaMini = sciezka.Substring(sciezka.IndexOf("Content"));
                        try
                        {
                            //html = File.ReadAllText(sciezka + plik);
                            //lt3_2.Text += "<br><img src='"+sciezkaMini+plik+"'><br>";
                            wstawImg(ph, sciezkaMini + plik);
                            wstawLabel(ph, note, styl.brak);
                            //lt3_2.Text+=note+"<br>";
                        }
                        catch (Exception ex)
                        {
                            //lt3_1.Text += ex.Message;
                            wstawLabel(ph, ex.Message, styl.brak);
                        }

                    }
                }
                else
                {
                    //  GridViewZalaczniki.Visible = false;
                    //lt3_2.Text = "Brak";
                    wstawLabel(ph, "Brak", styl.brak);
                }
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               Deb(exc.Message.ToString() + " sql=" + sql);
                return;
            }
        }
        private void wstawImg(PlaceHolder o, string plik)
        {
            Panel p = new Panel();
            o.Controls.Add(p);
            Image img = new Image();

            img.ImageUrl = plik;
            img.CssClass = "diagram";
                
            p.Controls.Add(img);

        }

        private void wstawMenu( PlaceHolder m, string txt, string tag, styl css )
        {
       
            wstawLink(m, txt, tag, css);
        }
        private void wstawLink(PlaceHolder o, string txt,string tag, styl css)
        {
            
            Panel p = new Panel();
            o.Controls.Add(p);
            HyperLink hl = new HyperLink();
            hl.Text = txt;
            hl.Attributes.Add("href", "#" + tag);
            if (css != styl.brak)
                hl.CssClass = css.Description();
            p.Controls.Add(hl);
         

        }
    
        private void wstawLabel(PlaceHolder o, string txt,styl css,string anch="",string tooltip="")
        {
            Panel p = new Panel();
            o.Controls.Add(p);
            Label lbl = new Label();
            lbl.Text = txt;
            if (css != styl.brak)
                lbl.CssClass = css.Description();
            
            if (anch.Length > 0)
            {
                HyperLink hl = new HyperLink();
                hl.Text = "";
                hl.Attributes.Add("name", anch);
                hl.CssClass="anchor";
                p.Controls.Add(hl);

               
            }

            if (tooltip.Length > 0)
            {
                Panel toolTip = new Panel();
                p.Controls.Add(toolTip);
                toolTip.CssClass = "tooltipKZG";

                toolTip.Controls.Add(lbl);

              //  Label lbl_t = new Label();
               // lbl_t.Text = "Kopiuj link...";

             System.Web.UI.WebControls.HyperLink link = new HyperLink();
                link.Text="Kopiuj link do tego miejsca.";
               // link.NavigateUrl = "#";
                string turl = Page.Request.Url.AbsoluteUri.ToString() + "#" + tooltip;
                link.Attributes.Add("onclick", "copyTextToClipboard('"+turl+"')");
                link.CssClass = "tooltiptextKZGKlik";
                toolTip.Controls.Add(link);

              //  lbl_t.CssClass = "tooltiptextKZGKlik";
              //  toolTip.Controls.Add(lbl_t);
            }
            else
            {
                p.Controls.Add(lbl);
            }
 
          

        }
        private void wstawGridView(PlaceHolder o, GridView g)
        {
            Panel p = new Panel();
            o.Controls.Add(p);
            g.CssClass = "gridView";
            g.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
            p.Controls.Add(g);

        }
        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (System.Web.UI.WebControls.TableCell cell in e.Row.Cells)
            {
                cell.Text = Server.HtmlDecode(cell.Text);
            }
        }
        private void dajKoncepcje(PlaceHolder ph,string obszar,string nr)
        {
            wstawLabel(ph, nr + " Koncepcja rozwiązania " + obszar, styl.Tyt_1_2, nr,nr);
            wstawMenu(HLDmenu, nr + " Koncepcja rozwiązania " + obszar, nr, styl.brak);

            int idobsz=0;
            if(obszar=="NT")idobsz=1;

           string notes= model.dajNotesObiektu(model.KoncepcjaElem[idobsz]);
           wstawLabel(ph, notes, styl.brak);
           wstawLinkedDocument(ph, model.KoncepcjaElem[idobsz].ToString());

           return;
            DataTable dt = new DataTable();
            //dt.Columns.Add("Lp");
            //  dt.Columns.Add("Nazwa/Opis"); dt.Columns.Add("Autor"); dt.Columns.Add("Dokument");
            string sql;

       /***
        *  na potrzebyt biblioteki generujacej html
        */
            sql = @"SELECT o.object_id,d.BinContent, o.Note, r.Name FROM "+
                "`" + schema + "`.t_object o, "+
                "`" + schema + "`.t_package p, "+
                "`" + schema + "`.t_package r, "+
                "`" + schema + "`.t_document d  WHERE "+
               " o.Style LIKE '%MDoc=1%' and o.object_type='Object' and d.elementid=o.ea_guid  and o.name='Koncepcja' "+
               " and o.package_id=p.package_id and p.Parent_ID=r.package_id  and r.name='"+obszar+"'";
            /*
            sql = @"select o.object_id, o.Note, r.Name, l.sciezka,l.plik FROM  " +
                "`" + schema + "`.t_object o, " +
                "`" + schema + "`.t_package p, " +
                "`" + schema + "`.t_package r, " +
                " eakzg_schema.eakzg_wyrzyg_log l WHERE " +
               "  o.object_type='Object' and l.projekt='"+schema+"' and o.name='Koncepcja' " +
               " and l.objectID=o.Object_ID and o.package_id=p.package_id and p.Parent_ID=r.package_id  and r.name='" + obszar + "'";
            */
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    foreach (DataRowView dr in dv1)
                    {

                        string html = "";

                        //lt2_1.Text += HttpUtility.HtmlDecode(dr[0].ToString());
                      
                     //   lt3_1.Text += dr[2].ToString() + "<br>";
                        /*
                         * 
                         * część do uzywania biblioteki   
                         */ 
                        byte[] bytes = (byte[])dr[1];
                        string rtf = Decompress(bytes);

                      
                       int q = 100;
                        SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml();
                        r.ImageStyle.Quality = q;
                       r.ImageStyle.IncludeImageInHtml = true;

                         html = r.ConvertString(rtf);
                      

                        /////////////////////// koniec biblioteki

                        int objID = (int)dr["object_id"];
                        string note = dr["note"].ToString();
                       
                        wstawLabel(HLDtresc, note, styl.brak);
                   //     string sciezka = dr["sciezka"].ToString();
                   //     string plik = dr["plik"].ToString();


                       
                        try
                        {
                      //      html = File.ReadAllText(sciezka + plik);
                            
                            wstawLabel(HLDtresc, html, styl.brak);
                        }
                        catch(Exception ex)
                        {
                            //lt3_1.Text += ex.Message;
                            wstawLabel(HLDtresc, ex.Message, styl.brak);
                        }
                       
                    }
                }
                else
                {
                    //  GridViewZalaczniki.Visible = false;
                    wstawLabel(HLDtresc, "Brak", styl.brak); 
                }
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               Deb(exc.Message.ToString() + " sql=" + sql);
                return;
            }

            

        }
         private void dajAspektyPozaf(PlaceHolder ph, string obszarstr,int obszar,string nr)
        {
            wstawLabel(ph, nr+" Aspekty pozafunkcjonalne " + obszarstr, styl.Tyt_1,nr,nr);
            
            
            wstawLabel(ph, nr + ".1 Wymagania dotyczące migracji danych ", styl.Tyt_1_2);

          
            string txt = model.dajNotesObiektu(model.MigracjaElem[obszar]);


            wstawLabel(ph, txt, styl.brak);
            wstawLinkedDocument(ph, model.MigracjaElem[obszar].ToString());

            return;

        
        }
        private bool dajArchTransmisyjna(PlaceHolder ph, int nr)
        {
            wstawLabel(ph, nr.ToString() + " Architektura Transmisyjna ", styl.Tyt_1, nr.ToString(), nr.ToString());
            wstawMenu(HLDmenu, nr.ToString() + " Architektura Transmisyjna ", nr.ToString(), styl.brak);



            string txt = model.dajNotesObiektu(model.ArchitekturaTransmisyjnaElem);
     

            wstawLabel(ph, txt, styl.brak);
            wstawLinkedDocument(ph, model.ArchitekturaTransmisyjnaElem.ToString());

            return true;
     
        }
        private void dajOpisRoliSystemu(PlaceHolder ph,string obszar,string rozdz)
        {
            wstawLabel(ph, rozdz + " Opis roli systemu "+obszar, styl.Tyt_1_2, rozdz,rozdz);
            wstawMenu(HLDmenu,rozdz+ " Rola systemu "+obszar, rozdz, styl.brak);

            DataTable dt = new DataTable();
            dt.Columns.Add("Lp."); dt.Columns.Add("Nazwa systemu");
            dt.Columns.Add("Opis roli systemu w projekcie"); dt.Columns.Add("Dostawca");
            string sql = "select o.name as 'Nazwa systemu',o.note as 'Opis roli systemu w projekcie' , oel.Value as 'Dostawca' from "+
                           "`" + schema + "`.t_package pas, "+
                            "`" + schema + "`.t_package pob, "+
                            "`" + schema + "`.t_package phl, "+
                            "`" + schema + "`.t_object o, "+
                            "`" + schema + "`.t_connector c, "+
                            " (select ooel.*,op.Value,op.Property from  `"+schema+"`.t_object ooel  left join "+
                            " `"+schema+"`.t_objectproperties op on "+
                            " op.Object_ID = ooel.Object_ID and op.property='Dostawca')  oel " +
                           " where pas.Parent_ID=pob.package_id "+
                           " and pob.parent_id=phl.package_id "+
                            "and pas.name='Wkłady Systemowe' "+
                            "and phl.name='HLD' "+
                            "and pob.name='"+obszar+"' "+
                            "and o.Package_ID =pas.Package_ID  "+
                            "and o.Object_Type='Package' "+
                            "and c.Start_Object_ID =o.Object_ID "+
                            "and c.End_Object_ID=oel.object_id "+
                            "order by o.name asc ";

            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    GridView g = new GridView();
                    wstawGridView(ph, g);
                    g.DataSource = dv1.ToTable();
                    g.DataBind();
                }
                else
                {

                    wstawLabel(ph, "Brak", styl.Norm);
                }
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               Deb(exc.Message.ToString() + " sql=" + sql);
                return;
            }
 
        }

          private void dajPrzypadkiUzycia(PlaceHolder ph,string obszar,string rozdz)
         {
             wstawLabel(ph, rozdz + " Przypadki użycia", styl.Tyt_1_2, rozdz,rozdz);
             wstawMenu(HLDmenu, rozdz + " Przypadki użycia", rozdz, styl.brak);
             DataTable dt = new DataTable();
             string sql;
             sql = @"select d.diagram_id,d.name,d.notes, l.sciezka,l.plik from " +
                     "`" + schema + "`.t_package pas," +
                     "`" + schema + "`.t_package pob," +
                     "`" + schema + "`.t_package phl," +
                     "`" + schema + "`.t_diagram d, eakzg_schema.eakzg_wyrzyg_log l where " +
                     "pas.Parent_ID=pob.package_id " +
                     "and pob.parent_id=phl.package_id " +
                     "and pas.name='Przypadki Użycia' " +
                     "and phl.name='HLD' " +
                     "and pob.name='" + obszar + "'" +
                     "and d.Package_ID=pas.Package_ID " +
                     "and l.objectID=d.Diagram_ID " +
                     "and l.projekt='" + schema + "'";

             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                     int i = 1;
                    //string o = rozdz+".2";
                   //  if (obszar != "IT") o =rozdz+ ".3";
                     foreach (DataRowView dr in dv1)
                     {

                         int objID = (int)dr["diagram_id"];
                         string name = dr["name"].ToString();
                         string note = dr["notes"].ToString();

                         wstawLabel(ph, rozdz + "." + i + " " + name, styl.Tyt_1_2_3, "uc" + objID, "uc" + objID);

                         string sciezka = dr["sciezka"].ToString();
                         string plik = dr["plik"].ToString();


                         string sciezkaMini = sciezka.Substring(sciezka.IndexOf("Content"));
                         try
                         {
                             //html = File.ReadAllText(sciezka + plik);
                             wstawImg(ph, sciezkaMini + plik);
                             wstawLabel(ph, note,styl.brak);
                             
                         }
                         catch (Exception ex)
                         {
                           //  lt3_3.Text += ex.Message;
                             wstawLabel(ph, ex.Message, styl.Norm);

                         }

                     }
                 }
                 else
                 {
                     //  GridViewZalaczniki.Visible = false;
                     //lt3_3.Text = "Brak";
                     wstawLabel(ph, "Brak", styl.Norm);
                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
                Deb(exc.Message.ToString() + " sql=" + sql);
                 return;
             }
          }
          private void dajDynamicznaArchitekture(PlaceHolder ph,string obszar,string nr)
         {
             wstawLabel(ph, nr + " Architektura Dynamiczna " + obszar, styl.Tyt_1_2,nr,nr);
             wstawMenu(HLDmenu, nr + " Architektura Dynamiczna " + obszar, nr, styl.brak);
             DataTable dt = new DataTable();
             string sql;
             sql = @"select d.diagram_id,d.name,d.notes, l.sciezka,l.plik from " +
                     "`" + schema + "`.t_package pas," +
                     "`" + schema + "`.t_package pob," +
                     "`" + schema + "`.t_package phl," +
                     "`" + schema + "`.t_diagram d, eakzg_schema.eakzg_wyrzyg_log l where " +
                     "pas.Parent_ID=pob.package_id " +
                     "and pob.parent_id=phl.package_id " +
                     "and pas.name='Diagramy Sekwencji' " +
                     "and phl.name='HLD' " +
                     "and pob.name='" + obszar + "'" +
                     "and d.Package_ID=pas.Package_ID " +
                     "and l.objectID=d.Diagram_ID " +
                     "and l.projekt='" + schema + "'";

             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                     int i = 1;
                     //string o = ".4";
                  //   if (obszar != "IT") o = ".4";
                     int objID = 0;
                     string name = "";
                     foreach (DataRowView dr in dv1)
                     {

                         objID = (int)dr["diagram_id"];
                        name = dr["name"].ToString();
                         string note = dr["notes"].ToString();

                         wstawLabel(ph, nr + "." + i + " " + name, styl.Tyt_1_2_3, "sq" + objID, "sq" + objID);
                         //lt3_4.Text +=  o + "." + i + " " + name;

                         string sciezka = dr["sciezka"].ToString();
                         string plik = dr["plik"].ToString();


                         string sciezkaMini = sciezka.Substring(sciezka.IndexOf("Content"));
                         try
                         {
                             
                            // wstawLabel(HLDtresc, name, styl.brak);
                             wstawImg(ph, sciezkaMini + plik);
                             wstawLabel(ph, note, styl.brak);

                             
                         }
                         catch (Exception ex)
                         {
                             wstawLabel(ph, ex.Message, styl.brak);
                             
                         }

                        // DataTable dt = new DataTable();
                        // dt.Columns.Add("Lp."); dt.Columns.Add("Nazwa systemu");
                        // dt.Columns.Add("Opis roli systemu w projekcie"); dt.Columns.Add("Dostawca");

                         sql = @"select concat(s.name,'->',e.name) as System,c.name,c.Notes, c.SeqNo,c.pdata2,c.styleEx from "+
                                "`" + schema + "`.t_connector c , "+
                                 "`" + schema + "`.t_object s, "+
                                "`" + schema + "`.t_object e " +
                               " where DiagramID="+objID+" and Connector_Type='Sequence' "+
                               " and s.Object_ID = c.Start_Object_ID "+
                               " and e.object_id= c.end_object_id " +
                               " order by c.seqno asc";



                         SqlDataSource1.SelectCommand = sql;

                         DataView dv2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                         if (dv2.Table.Rows.Count > 0)
                         {
                             GridView gv = new GridView();
                             wstawGridView(ph, gv);

                             DataTable ndt = new DataTable(name);
                             ndt.Columns.Add("Lp."); ndt.Columns.Add("System"); ndt.Columns.Add("Krok");
                             ndt.Columns.Add("Wejście/Wyjście"); ndt.Columns.Add("Opis Kroku");

                             int lp = 1;
                             foreach (DataRow dr2 in dv2.Table.Rows)
                             {
                                 DataRow newRow =  ndt.NewRow();

                                 newRow["Lp."] = lp++;
                                 newRow["System"] = ((String)dr2["System"].ToString());
                                 int xx = dr2["name"].ToString().IndexOf("(");
                                 string tmp = (String)dr2["name"].ToString();
                                 if (xx > 0)
                                     tmp =dr2["name"].ToString().Substring(0, xx);
                                 newRow["Krok"] = tmp;
                                 newRow["Wejście/Wyjście"] = ((String)dajIn(dr2["pdata2"].ToString(), dr2["styleEx"].ToString())) +" "+ dajRet((String)dr2["pdata2"].ToString());
                                 
                                 newRow["Opis Kroku"] = ((String)dr2["notes"].ToString());


                                 ndt.Rows.Add(newRow);
                             }
                             
                             gv.DataSource = ndt;
                             gv.DataBind();
                         }

                        
                     }
                 }
                 else
                 {
                     //  GridViewZalaczniki.Visible = false;
                     wstawLabel(ph, "Brak", styl.brak);
                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
                Deb(exc.Message.ToString() + " sql=" + sql);
                 return;
             }
          }
          private string dajRet(string pdata2)
          {           
              string pattern = @"retval=([^;]+);";
              string returnValue = "";
 
              Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
              MatchCollection matches = rgx.Matches(pdata2);
              if (matches.Count == 1)
              {
                  returnValue = "OUT[" + matches[0].Groups[1].Value + "]";
              }
              return returnValue;
          }

          private string dajIn(string pdata2,string StyleEx)
          {
              string pattern = @"paramsDlg=([^;]+);";
              
              string paramValue = "";
              int parSet = 0;
              Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
              MatchCollection matches = rgx.Matches(pdata2);
              if (matches.Count == 1)
              {
                  paramValue = "IN[" + matches[0].Groups[1].Value;
                  parSet = 1;
              };

              if (StyleEx.Length > 1)
              {
                  if (parSet == 0) { paramValue = "IN["; }
                  pattern = @"paramvalues=([^;]+);";
                  rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                  matches = rgx.Matches(StyleEx);
                  if (matches.Count == 1)
                  {
                      paramValue = paramValue + ", " + matches[0].Groups[1].Value + "]";
                      parSet = 0;
                  }
              }
              if (parSet == 1) { paramValue = paramValue + "]"; }
            
            

              return paramValue;
          }
          protected void GridViewSlownik_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void dajOrganizacyjne(PlaceHolder ph)
    {
        string t = @" 
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
            </ul>";
        wstawLabel(ph, t, styl.Norm);
       // wstawLabel(HLDmenu, "1 ORGANIZACYJNIE", styl.brak);
        //wstawMenu( HLDmenu, "1 ORGANIZACYJNIE", "r_1", styl.brak);
                 
        
    }

    protected void dajWskazowkiDotTestowAutom(PlaceHolder ph,int nr)
    {
        wstawLabel(ph, nr.ToString() + " Wskazówki dotyczące automatyzacji testów", styl.Tyt_1, nr.ToString(), nr.ToString());
        wstawMenu(HLDmenu, nr.ToString() + " Testy automatyzacja", nr.ToString(), styl.brak);



        string txt = model.dajNotesObiektu(model.TestyElemAutomat);


        wstawLabel(ph, txt, styl.brak);
        wstawLinkedDocument(HLDtresc, model.TestyElemAutomat.ToString());

        return;
    }
         protected void dajWskazowkiDotTestow(PlaceHolder ph,int nr)
         {
             wstawLabel(ph, nr.ToString() + " Wskazówki dotyczące testów", styl.Tyt_1, nr.ToString(), nr.ToString());
              wstawMenu(HLDmenu, nr.ToString() + " Wskazówki testów", nr.ToString(), styl.brak);



              string txt = model.dajNotesObiektu(model.TestyElem);


              wstawLabel(ph, txt, styl.brak);
              wstawLinkedDocument(ph, model.TestyElem.ToString());

              return ;

          
         }
         public static MemoryStream GenerateStreamFromString(string s)
         {
             MemoryStream stream = new MemoryStream();
             StreamWriter writer = new StreamWriter(stream);
             writer.Write(s);
             writer.Flush();
             stream.Position = 0;
             return stream;
         }
        /// <summary>
        /// wstawienie linkeddcument z bibiloteki
         /// https://www.e-iceblue.com/Tutorials/Spire.Doc/Spire.Doc-Program-Guide/How-to-Use-C-/VB.NET-to-Convert-RTF-to-HTML-via-Spire.Doc.html
        /// </summary>
        /// <param name="div"></param>
        /// <param name="obiektID"></param>
         protected void wstawLinkedDocumentSpire(PlaceHolder div, string obiektID)
         { 
            DataTable dt = new DataTable();
           
            string sql;

       /***
        *  na potrzebyt biblioteki generujacej html
        */
            sql = @"SELECT o.object_id,d.BinContent, o.Note, r.Name FROM "+
                "`" + schema + "`.t_object o, "+
                "`" + schema + "`.t_package p, "+
                "`" + schema + "`.t_package r, "+
                "`" + schema + "`.t_document d  WHERE "+
               " o.Style LIKE '%MDoc=1%' and o.object_type='Object' and d.elementid=o.ea_guid  and o.name='Koncepcja' "+
               " and o.package_id=p.package_id and p.Parent_ID=r.package_id  and r.name='IT'";
            
           
            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                    foreach (DataRowView dr in dv1)
                    {

                        string html = "";

                        byte[] bytes = (byte[])dr[1];
                        string rtf = Decompress(bytes);

                        using (Stream s = GenerateStreamFromString(rtf))
                        {

                            Document document = new Document();
                           //Section x= document.AddSection();
                           //Paragraph p=x.AddParagraph();

                            document.LoadFromFile(@"D:\Documents and Settings\kzagawa\My Documents\EAkzgZrzut\PR-3999\LinkedDoc\16138.rtf");
                            document.HtmlExportOptions.ImageEmbedded = true;
                            using (MemoryStream htmlStream = new MemoryStream())
                            {
                                document.SaveToStream(htmlStream, FileFormat.Html);
                                html = htmlStream.ToString();
                            }

                        }            
                      

                        /////////////////////// koniec biblioteki

                        int objID = (int)dr["object_id"];
                        string note = dr["note"].ToString();
                       
                        wstawLabel(HLDtresc, note, styl.brak);
                 

                       
                        try
                        {
                    
                            
                            wstawLabel(HLDtresc, html, styl.brak);
                        }
                        catch(Exception ex)
                        {
           
                            wstawLabel(HLDtresc, ex.Message, styl.brak);
                        }
                       
                    }
                }
                else
                {
         
                    wstawLabel(HLDtresc, "Brak", styl.brak); 
                }
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               Deb(exc.Message.ToString() + " sql=" + sql);
                return;
            }

        }
         protected string binToHtml(string str)
         {
             return binToHtml((byte[]) ASCIIEncoding.ASCII.GetBytes(str.ToCharArray()));
    }

         static public string binToHtml(byte[] bytes)
         {

             string html = "";
             try
             {
               //  if (bin == "System.Byte[]") return "";
                 

             //  byte[] bytes = Encoding.UTF8.GetBytes(bin);

                 string rtf = Decompress(bytes);

                 int q = 100;
                 SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml();

               ///  //specify some options
             //    r.OutputFormat = SautinSoft.RtfToHtml.eOutputFormat.HTML_5;
             //    r.Encoding = SautinSoft.RtfToHtml.eEncoding.UTF_8;
        //         string sciezkaIMG= @"Content/Modele/"+schema+@"/";
         //        string spre = @"D:\Documents and Settings\kzagawa\EAkzg_svn\linkedDoc\";
           //      System.IO.Directory.CreateDirectory(spre+sciezkaIMG);
             ///    //specify image options
      //           r.ImageStyle.ImageFolder =sciezkaIMG;            //this folder must exist
       //          r.ImageStyle.ImageSubFolder = sciezkaIMG;    //this folder will be created by the component
        //         r.ImageStyle.ImageFileName = "img";            //template name for images
         //        r.ImageStyle.IncludeImageInHtml = false;    //false - save images on HDD, true - save images inside HTML
               //  r.ImageStyle.ImagesFormat = SautinSoft.RtfToHtml.eImageFormat.Auto;
        //         r.ImageStyle.PreserveImages = true;




                 r.ImageStyle.Quality = q;

                 r.ImageStyle.IncludeImageInHtml = true;

                 html = r.ConvertString(rtf);
             }
             catch (Exception exc)
             {
                 //wstawLabel(HLDtresc, "EXC: binToHtml "+exc.Message, styl.brak);
                 return "EXC: binToHtml " + exc.Message;
             }


             return html;
         }
         public string dajHtmlLincedDocument(string obiektID)
         {
             DataTable dt = new DataTable();

             string sql;

             /***
              *  na potrzebyt biblioteki generujacej html
              */
             sql = @"SELECT o.object_id,d.BinContent, o.Note FROM " +
                 "`" + schema + "`.t_object o, " +
                 "`" + schema + "`.t_document d  WHERE " +
                " o.Style LIKE '%MDoc=1%' and d.ElementType='ModelDocument' and d.elementid=o.ea_guid  " +
                " and o.object_id=" + obiektID;
             if (obiektID == "16909")
             {
                 int b = 1 + 2;             
             }


             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                     string html = "";
                     foreach (DataRowView dr in dv1)
                     {
                     
                       //  int objID = (int)dr["object_id"];
                       
                         string str = dr["BinContent"].ToString();
                         if (str != "System.Byte[]")
                         {
                             continue;
                         }
                         byte[] s = (byte[])dr["BinContent"];


                         html += binToHtml(s);


                     }
                     return html;
                 }
                 else
                 {
                     return "";

                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + " sql=" + sql + "\n");
               //  wstawLabel(div, exc.Message.ToString() + " sql=" + sql + "\n", styl.brak);
                Deb("LinkedDoc exc: "+exc.Message.ToString() + " sql=" + sql);
                 return "LinkedDoc exc: "+exc.Message.ToString() + " sql=" + sql + "\n";
             }
         }
         protected bool wstawLinkedDocument(PlaceHolder div, string obiektID)
         {
             string html = dajHtmlLincedDocument(obiektID);
              html =Regex.Replace(html,@"(\.st\d+)\{",@"$1_"+obiektID+@"{");
              html = Regex.Replace(html, @"(class=""st\d+)""", @"$1_"+obiektID+@""" ");
                 

             if (html.Length == 0) return false;
             try
             {
                 wstawLabel(div, html, styl.brak);
                 return true;
             }
             catch (Exception ex)
             {
                 wstawLabel(div, ex.Message, styl.brak);
                 return false;
             }
             
         }

        protected void wstawLinkedObiekt(PlaceHolder div,string obiektID)
        {
              DataTable dt = new DataTable();

             string sql;
             sql = @"select o.object_id, o.Note,  l.sciezka,l.plik FROM  " +
                 "`" + schema + "`.t_object o, " +
                 " eakzg_schema.eakzg_wyrzyg_log l WHERE " +
                "  o.object_type='Object' and l.projekt='" + schema + "' and o.object_id ="+obiektID +
                " and l.objectID=o.Object_ID";

             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                     foreach (DataRowView dr in dv1)
                     {
                         int objID = (int)dr["object_id"];
                         string note = dr["note"].ToString();
                      
                         string sciezka = dr["sciezka"].ToString();
                         string plik = dr["plik"].ToString();
                         string html = "";
                         try
                         {
                             html = File.ReadAllText(sciezka + plik);
                             wstawLabel(HLDtresc, html, styl.brak);
                         }
                         catch (Exception ex)
                         {
                             wstawLabel(HLDtresc, ex.Message, styl.brak);
                         }
                     }
                 }
                 else
                 {
                    // wstawLabel(HLDtresc, "Brak", styl.brak);
                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + "Wstaw obj sql=" + sql + "\n");
                Deb(exc.Message.ToString() + " sql=" + sql);
                 return;
             }

        }
         protected void dajKoncepcjeSystemowa(PlaceHolder HLDt,PlaceHolder HLDm,string r,string pakietSys)
         {
             wstawLabel(HLDt, r + ".1 Koncepcja systemowa", styl.Tyt_1_2_3);
             

             string sql = @"select o.object_id as koncID, o.note as note from " +
                 "`" + schema + "`.t_object o where o.name='Koncepcja systemowa' and o.object_type='Object' and o.Package_ID=" + pakietSys;
           
             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                    foreach (DataRowView dr in dv1)
                    {
                        wstawLabel(HLDt, dr["note"].ToString(), styl.brak);
                    //    wstawLinkedObiekt(HLDt, dr["koncID"].ToString());
                        wstawLinkedDocument(HLDt, dr["koncID"].ToString());
                    }
                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + "dajKoncepcjeSystemowa sql=" + sql + "\n");
                Deb(exc.Message.ToString() + " sql=" + sql);
                 return;
             }
           
         }

         protected void dajWymaganiaGen(PlaceHolder HLDt, string pakietID, string sterList, bool czyBezp)
         {

             string sql = @"select  owym.object_id as F_id,owym.name as F_name,owym.note as F_note,owym.status as F_stat,GROUP_CONCAT(bizid SEPARATOR '<BR>') as R_id, GROUP_CONCAT(bizname SEPARATOR '<BR>') as R_name, GROUP_CONCAT(owym.stereotype SEPARATOR '<BR>') as stereotype from  " +
                        "`"+schema+"`.t_object oPak, "+
                        "(select  c1.Start_Object_ID, c1.Object_ID as bizID,c1.name as bizname , of.package_id, of.Object_Type, of.object_id, of.name, of.note, of.status, of.stereotype "+
                        "from "+
                       "`"+schema+"`.t_object of left join  (select c2.Start_Object_ID, oWymBiz.Object_ID,owymbiz.name  from "+
                        "`"+schema+"`.t_connector c2 , "+
                        "`"+schema+"`.t_object oWymBiz where c2.end_object_id=owymBiz.object_id   and owymbiz.object_Type='Requirement' "+
                         " ) c1 on c1.Start_Object_ID=of.object_id)  owym  where   opak.object_type='Package'  and oPak.Name='Wymagania Systemowe' "+
                        " and oPak.Package_ID="+ pakietID +"  and oWym.package_id=opak.pdata1  and owym.object_type='Feature' " +
                        " and "+sterList+
                        " group by owym.object_id, owym.name, owym.note, owym.status "+
                        " order by owym.object_id ";

             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 if (dv1.Table.Rows.Count > 0)
                 {
                     foreach (DataRowView dr in dv1)
                     {
                         wstawLabel(HLDt, dr["F_name"].ToString(), styl.txt_feature_tytul, "Fe" + dr["F_id"].ToString(), "Fe" + dr["F_id"].ToString());
                         if (!czyBezp)
                         {
                             wstawLabel(HLDt, "Status:", styl.txt_feature_param);
                             wstawLabel(HLDt, dr["F_stat"].ToString(), styl.brak);
                             wstawLabel(HLDt, "Nadrzędne wymaganie biznesowe:", styl.txt_feature_param);
                             wstawLabel(HLDt, dr["R_name"].ToString(), styl.brak);
                             wstawLabel(HLDt, "Szczegóły:", styl.txt_feature_param);
                         }
                         wstawLabel(HLDt, dr["F_note"].ToString(), styl.brak);
                        // wstawLinkedObiekt(HLDt, dr["F_id"].ToString());
                         wstawLinkedDocument(HLDt, dr["F_id"].ToString());
                     }
                 }
             }
             catch (Exception exc)
             {
                 Response.Write(exc.Message.ToString() + "dajWymaganiaGen sql=" + sql + "\n");
                Deb("dajWymaganiaGen pak,ster,bez:"+pakietID+", "+sterList+", "+czyBezp+" exc="+exc.Message.ToString() + " sql=" + sql);
                 return;
             }
         }
         protected void dajWymaganiaSystemowe(PlaceHolder HLDt, PlaceHolder HLDm, string nr, string pakietID)
         {
             wstawLabel(HLDt, nr.ToString() + ".2" + " Wymagania systemowe ", styl.Tyt_1_2_3,pakietID,pakietID);
             
             wstawLabel(HLDt, nr.ToString() + ".2.1" + " Wymagania Funkcjonalne ", styl.Tyt_1_2_3_4);
             dajWymaganiaGen(HLDt,pakietID, @" (owym.stereotype is null or owym.stereotype not in ('Infrastrukt.','Bezp.','Pojemność','Dostępność')) ", false);

             wstawLabel(HLDt, nr.ToString() + ".2.2" + " Wymagania na Infrastrukturę ", styl.Tyt_1_2_3_4);
             dajWymaganiaGen(HLDt, pakietID, @" owym.stereotype='Infrastrukt.' ", false);

             wstawLabel(HLDt, nr.ToString() + ".2.3" + " Wymagania Bezpieczeństwa ", styl.Tyt_1_2_3_4);
             dajWymaganiaGen(HLDt, pakietID, @" owym.stereotype='Bezp.' ", true);
         }
         public void dajSystem(string obszar,int nr, int nrRozdz, string systemID,string system,string pakietID,string pakiet, string chm,  PlaceHolder  HLDt, PlaceHolder  HLDm)
         {

             wstawLabel(HLDt, nr.ToString() + "." + nrRozdz.ToString() + " System " + system + @" (" + chm + @")", styl.Tyt_1_2, nr.ToString() + "." + nrRozdz.ToString(), nr.ToString() + "." + nrRozdz.ToString());
             wstawMenu(HLDm, "  "+nr.ToString() + "." + nrRozdz.ToString() +" "+ system, nr.ToString() + "." + nrRozdz.ToString(), styl.brak);

            dajKoncepcjeSystemowa(HLDt,HLDm,nr.ToString() + "." + nrRozdz.ToString(),pakietID);
           //  dajDiagrSystCentr();
            dajWymaganiaSystemowe(HLDt, HLDm, nr.ToString() + "." + nrRozdz.ToString(), pakietID);
           
            // dajWplywNaPoj();
            // dajWplywNaDost();
            // dajUdostepnInt();
            // dajWykorzystInt();
         }

       
    

        /// <summary>
        /// if (dr3["System"].ToString() != "Fasttrack")
                //                      {
                  //                        dajSystem(obszar, nr, nrRozdz++, dr3["systemID"].ToString(),
                    //                                                     dr3["System"].ToString(),
                      //                                                   dr3["PakietID"].ToString(),
                        //                                                 dr3["Pakiet"].ToString(),
                          //                                               dr3["ChM"].ToString()
                            //                                             );
                              //        }
        /// </summary>
        /// <param name="dv1"></param>
        /// <param name="obszar"></param>
        /// <param name="nr"></param>
        /// <param name="nrRozdz"></param>
         private void dajWkladyTaskPool(PlaceHolder ph,DataView dv1, string obszar, int nr, int nrRozdz)
         {



             int ileZadan = dv1.Table.Rows.Count;
        
             ManualResetEvent[] doneEvents = new ManualResetEvent[ileZadan];
             CWklady[] wyniki = new CWklady[ileZadan];


             // Configure and launch threads using ThreadPool:
            Deb("Wkłady "+obszar+" liczba wątków: "+ ileZadan);
             for (int i = 0; i < ileZadan; i++)
             {
                 doneEvents[i] = new ManualResetEvent(false);
                 CWklady f = new CWklady(this,i,obszar, nr, nrRozdz++,   dv1.Table.Rows[i]["systemID"].ToString(),
                                                                  dv1.Table.Rows[i]["System"].ToString(),
                                                                  dv1.Table.Rows[i]["PakietID"].ToString(),
                                                                  dv1.Table.Rows[i]["Pakiet"].ToString(),
                                                                  dv1.Table.Rows[i]["ChM"].ToString(), doneEvents[i]);
                 wyniki[i] = f;
                 ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
             }

             // czeka na wszystkie
             WaitHandle.WaitAll(doneEvents);
            Deb("Przeliczone wszystkie zadania.");

             // Display the results...
             for (int i = 0; i < ileZadan; i++)
             {
                 CWklady f = wyniki[i];
                 HLDmenu.Controls.Add(f.phMenu);
                 ph.Controls.Add(f.phTresc);
                 //Console.WriteLine("WymBiz({0}) = {1}", f.N, f.FibOfN);
             }
         }
        
        
                         
         protected void dajWklady(PlaceHolder ph,string obszar,int nr)
         {
             wstawLabel(ph, nr.ToString() + " Zmiany w systemach "+obszar, styl.Tyt_1,nr.ToString());
           //  wstawMenu(HLDmenu, nr.ToString() + " Zmiany w systemach " + obszar, nr.ToString() , styl.brak);
             int nrRozdz=1;
             String sql;
             ////// przed wkladami IT damy rozdział ze zmianami fasttrack
             int idFT = -1; 
             if (obszar == "IT")
             {

                  sql = "select distinct s.object_id as systemID from "+
                        "`" + schema + "`.t_object f, "+
                        "`" + schema + "`.t_object s, "+
                        "`" + schema + "`.t_connector c where f.object_type='Feature' " +
                                 "and s.object_type='Component' and ((c.start_object_id=f.object_id and c.end_object_id=s.object_id) or " +
                                 "(c.start_object_id=s.object_id and c.end_object_id=f.object_id) ) and s.name='Fasttrack'";
                   SqlDataSource1.SelectCommand = sql;

                         DataView dv2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                         if(dv2.Table.Rows.Count > 0)//jeśli są wymagania do FT
                         {
                             idFT = 1;
                   
                          }

                 }
                 else
                 {   //przeskocz do nast napisu

                 }

            

                    sql = "  select oel.object_id as systemID,o.name as 'System',o.name as 'Pakiet',o.PDATA1 'PakietID',  op.Value 'ChM' from "+
                         "`" + schema + "`.t_package pas, "+
                         "`" + schema + "`.t_package pob, "+
                         "`" + schema + "`.t_package phl, "+
                         "`" + schema + "`.t_object o, "+
                         "`" + schema + "`.t_connector c, "+
                          "`" + schema + "`.t_object oel,  "+
                         "`" + schema + "`.t_objectproperties op  " +
                           "  where pas.Parent_ID=pob.package_id  "+
                           "  and pob.parent_id=phl.package_id  "+
                           " and pas.name='Wkłady Systemowe'  "+
                           " and phl.name='HLD'  "+
                           " and pob.name='"+ obszar+"' "+
                           " and o.Package_ID =pas.Package_ID   "+
                           " and o.Object_Type='Package'  "+
                           " and c.Start_Object_ID =o.Object_ID  "+
                           " and c.End_Object_ID=oel.object_id "+
                           " and op.Object_ID = oel.Object_ID  "+
                           " and op.Property='Rozwój'"; 
                   SqlDataSource1.SelectCommand = sql;

                         DataView dv3 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                          
                         if (dv3.Table.Rows.Count > 0)
                         {
                             foreach (DataRow dr3 in dv3.Table.Rows)
                             {
                                 if (dr3["System"].ToString() == "Fasttrack" && idFT>0)
                                 {
                                     dajSystem(obszar,nr, nrRozdz++, dr3["systemID"].ToString(),
                                                                  dr3["System"].ToString(),
                                                                  dr3["PakietID"].ToString(),
                                                                  dr3["Pakiet"].ToString(),
                                                                  dr3["ChM"].ToString(),ph,HLDmenu
                                                                  );
                                 }
                             }

                             Stopwatch yy=new Stopwatch();
                             yy.Start();
                              Stopwatch zz = new Stopwatch();

                              if (true)  //// czy jechac wielowątkowo
                              {
                                  //////////////task pool ///////////////
                                  zz.Restart();
                                  dajWkladyTaskPool(ph,dv3, obszar, nr, nrRozdz);
                                  swLog(yy, zz, "*** Wkłady razem: ",ph);
                                  ///////////// task pool /////////////
                              }
                              else
                              {
                                  //////////////////jeden watek /////////////////////////////////
                                  foreach (DataRow dr3 in dv3.Table.Rows)
                                  {

                                      zz.Restart();
                                      if (dr3["System"].ToString() != "Fasttrack")
                                      {
                                          dajSystem(obszar, nr, nrRozdz++, dr3["systemID"].ToString(),
                                                                         dr3["System"].ToString(),
                                                                         dr3["PakietID"].ToString(),
                                                                         dr3["Pakiet"].ToString(),
                                                                         dr3["ChM"].ToString(),
                                                                         HLDtresc, HLDmenu
                                                                         );
                                      }
                                      swLog(yy, zz, "***System " + dr3["System"].ToString(),ph);
                                  }
                                  //////////////////////// jeden watek ///////////////////////
                              }
                          }



         }
         protected string dajTag(string nazwa,string tag)
         {
            
             string sql = "select value from `" + schema + "`.t_object o, "+
                 "`" + schema + "`.t_objectproperties p where o.name='" + nazwa + "'" +
                 " and p.Property='" + tag + "' and p.Object_ID = o.Object_ID";
             SqlDataSource1.SelectCommand = sql;
             try
             {
                 DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                 return (string)(dv.ToTable()).Rows[0][0].ToString();
             }
             catch
             {
                 return "brak";
             }
            
         }
      
         protected void swLog(Stopwatch g,Stopwatch l,string t,PlaceHolder ph)
         {
             if (debugCzas==1)
             {

                 string s = "Czas " + t +" "+ l.Elapsed.ToString("mm\\:ss\\.ff") + ", Całkowity czas: " + g.Elapsed.ToString("mm\\:ss\\.ff");
                 wstawLabel(ph,s,styl.brak);
                Deb("swlog: " + s);
             }
         }
         protected void dbtnInvoke_Click(object sender, EventArgs e)
         {
            // System.Threading.Thread.Sleep(3000);
             aaaPage_Load_cz2_wymBiz();
             //lblText.Text = "Processing completed";
         }

         protected bool weryfikujScheme(string s)
         {
                DataTable dt = new DataTable();
            
            string sql;


            sql = @"select * from `"+s+"`.t_secuser  where userLogin='www' and Password='2GNb9GUcq5BP'";

          Deb("weryfikujScheme "+s);

            SqlDataSource1.SelectCommand = sql;
            try
            {
                DataView dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (dv1.Table.Rows.Count > 0)
                {
                   
                    return true;
                }
                Deb("weryfikujScheme-brak wierszy");
            }
            catch (Exception exc)
            {
                Deb("weryfikujScheme - wyjątek " + exc.Message);
                return false;
            }
            return false;
         }
         protected void Page_Load(object sender, EventArgs e)
         {
             try
             {
                Deb("PageLoad");
                 
                 schema = Request.QueryString["schema"];
                 if (schema == null) schema = "";
                 Deb("schema:" + schema);
                 string debCzasStr = Request.QueryString["dc"];
                 if (debCzasStr == null)
                 {
                     Deb("debugCzasStr=null");
                     debugCzas = 0;
                 }
                 else
                 {
                     Deb("debugCzasStr="+debCzasStr);
                     if (debCzasStr.Length > 0)
                     {
                        
                         Deb("TryParse:"+ int.TryParse(debCzasStr, out debugCzas));
                     }
                 }
                 
             }
             catch (Exception exc)
             {
                Deb("Parser: " + exc.Message);
               
             }
    
             aaaPage_Load();
         }
         protected void aaaPage_Load_cz1(ref int  r1,ref int  r1_2)
         {
             Stopwatch swGlobal = new Stopwatch();
             Stopwatch swLocal = new Stopwatch();

             swGlobal.Start();
             swLocal.Start();



             wstawLabel(HLDtresc_cz1, schema + " " + dajNazweObiektu("Projekt-Nazwa"), styl.Tyt,"tytul");
             wstawLabel(HLDtresc_cz1, "SD IT: " + dajTag("SD IT", "Imię i Nazwisko"), styl.Norm);
             wstawLabel(HLDtresc_cz1, "SD NT: " + dajTag("SD NT", "Imię i Nazwisko"), styl.Norm);
             swLog(swGlobal, swLocal, "## cz.1 - Inicjacja", HLDtresc_cz1);
             //Rozdział 1
             swLocal.Restart();
             r1 = 1;
             wstawLabel(HLDtresc_cz1, r1 + " ORGANIZACYJNE", styl.Tyt_1, "r_1");
             dajOrganizacyjne(HLDtresc_cz1);
             //todo dajHistoriaZmian(r1+(r1_2++)+"Historia zmian");



             dajSlownik(HLDtresc_cz1, r1 + "." + (r1_2++) + " Słownik użytych skrótów i pojęć");
             // dajZalaczniki(r1 + "." + (r1_2++) + "");
             dajZespol(HLDtresc_cz1,r1 + "." + (r1_2++) + " Zespół projektowy");
             dajPowiazania(HLDtresc_cz1,r1 + "." + (r1_2++) + " Powiązania z innymi projektami");
             swLog(swGlobal, swLocal, "## cz. 1 Rozdział 1 Organizacyjne", HLDtresc_cz1);
             //Rozdział 2
             swLocal.Restart();
             r1 = ++r1;
             wstawLabel(HLDtresc_cz1, r1 + " PERSPEKTYWA FUNKCJONALNA", styl.Tyt_1, "r2");
             //   wstawMenu(HLDmenu, r1 + " FUNKCJONALNIE", "r2", styl.brak);

             dajKrotkiOpis(HLDtresc_cz1);
             dajOgraniczeniaRozwiazania(HLDtresc_cz1);
         }
         protected void aaaPage_Load_cz2_wymBiz()
         {
             dajWymaganiaBiz(HLDtresc_cz2);
         }
         protected void aaaPage_Load_cz3(ref int r1, ref int r1_2)
         {
             Stopwatch swGlobal = new Stopwatch();
             Stopwatch swLocal = new Stopwatch();

             swGlobal.Start();
             swLocal.Start();


             wstawLabel(HLDtresc_cz3, r1 + " OPIS ROZWIĄZANIA IT", styl.Tyt_1, "r3");
             //  wstawMenu(HLDmenu, r1 + " OBSZAR IT", "r3", styl.brak);
             dajKoncepcje(HLDtresc_cz3,"IT", r1 + ".1");
             dajArchitektureStatyczna(HLDtresc_cz3, "IT", r1 + ".2");
             dajOpisRoliSystemu(HLDtresc_cz3, "IT", r1 + ".2.2");
             dajPrzypadkiUzycia(HLDtresc_cz3, "IT", r1 + ".2.3");
             dajDynamicznaArchitekture(HLDtresc_cz3, "IT", r1 + ".3");
             swLog(swGlobal, swLocal, "## cz. 3 Rozdział 3 Opis rozwiązania IT", HLDtresc_cz3);
             // Rozdział 4
             swLocal.Restart();
             r1 = ++r1;
             wstawLabel(HLDtresc_cz3, r1 + " OPIS ROZWIĄZANIA NT", styl.Tyt_1, "r" + r1);
             //  wstawMenu(HLDmenu, r1 + " OBSZAR NT", "r" + r1, styl.brak);
             dajKoncepcje(HLDtresc_cz3, "NT", "4.1");
             dajArchitektureStatyczna(HLDtresc_cz3, "NT", r1 + ".2");
             dajOpisRoliSystemu(HLDtresc_cz3, "NT", r1 + ".2.2");
             dajPrzypadkiUzycia(HLDtresc_cz3, "NT", r1 + ".2.3");
             dajDynamicznaArchitekture(HLDtresc_cz3, "NT", r1 + ".3");
             swLog(swGlobal, swLocal, "## cz. 3 Rozdział 4 Opis rozwiązania NT", HLDtresc_cz3);
             // Rozdział 5
             swLocal.Restart();
             r1 = ++r1;
             dajArchTransmisyjna(HLDtresc_cz3, r1);



             swLog(swGlobal, swLocal, "## cz. 3 Rozdział 5 Arch transm.", HLDtresc_cz3);
             // Rozdział 6
             swLocal.Restart();
             r1 = ++r1;
             dajWskazowkiDotTestow(HLDtresc_cz3, r1);
             swLog(swGlobal, swLocal, "## cz. 3 Rozdział 6 Wskaz do testów", HLDtresc_cz3);
             // Rozdział 7
             swLocal.Restart();
             r1 = ++r1;
             dajWskazowkiDotTestowAutom(HLDtresc_cz3, r1);
            
         }
         protected void aaaPage_Load_cz4_wklady(ref int r1, ref int r1_2)
         {
             Stopwatch swGlobal = new Stopwatch();
             Stopwatch swLocal = new Stopwatch();

             swGlobal.Start();
             swLocal.Start();


             dajWklady(HLDtresc_cz4,"IT", r1);

             swLog(swGlobal, swLocal, "## cz. 4 Rozdział 7 Wkłady IT", HLDtresc_cz4);
             // Rozdział 8
             swLocal.Restart();
             r1 = ++r1;
             wstawLabel(HLDtresc_cz4, r1 + " Zmiany w systemach NT", styl.Tyt_1);
             dajWklady(HLDtresc_cz4,"NT", r1);
             swLog(swGlobal, swLocal, "## cz.4 Rozdział 8 Wklady NT", HLDtresc_cz4);
             
         }
         protected void aaaPage_Load_cz5(ref int r1, ref int r1_2)
         {
             dajAspektyPozaf(HLDtresc_cz5,"IT", 0, r1.ToString());
             wstawMenu(HLDmenu, r1 + " Aspekty pozafunkcjonalne ", r1.ToString(), styl.brak);
             r1 = ++r1;
             dajAspektyPozaf(HLDtresc_cz5,"NT", 1, r1.ToString());

         }
        protected void aaaPage_Load()
        {
           if (!weryfikujScheme(schema))
            {
                wstawLabel(HLDtresc, "Błędny model:" + schema, styl.Tyt);
                return;
            }

            Stopwatch swGlobal = new Stopwatch();
            Stopwatch swLocal = new Stopwatch();

            swGlobal.Start();
            swLocal.Start();


            int r1 = 0;
            int r1_2 = 1;
            model = new CModel(schema, SqlDataSource1);

            aaaPage_Load_cz1(ref r1,ref r1_2);
            swLog(swGlobal, swLocal, @"Przetwarzanie cz. 1/5",HLDtresc_cz1);
            swLocal.Restart();
              r1 = ++r1 ;

              aaaPage_Load_cz2_wymBiz();
              aaaPage_Load_cz3(ref r1, ref r1_2);
              swLog(swGlobal, swLocal, @"Przetwarzanie cz. 3/5",HLDtresc_cz3);
              swLocal.Restart();

              r1 = ++r1;

              aaaPage_Load_cz4_wklady(ref r1, ref r1_2);
              // Rozdział 9
              swLocal.Restart();
              r1 = ++r1;

              aaaPage_Load_cz5(ref r1, ref r1_2);
              swLog(swGlobal, swLocal, @"Przetwarzanie cz. 5/5 - koniec",HLDtresc_cz5);
                    
        }
       
        protected void UpdatePanel1_Load(object sender, EventArgs e)
        {
         //   Label progressMessageLabel = updProgress.FindControl("updTxt") as Label;
           // if (progressMessageLabel != null)
        //    {
              //  progressMessageLabel.Text += schema ;
              
          //  }

        //    UpdateTimer.Enabled = false;
       //     aaaPage_Load();
            aaaPage_Load_cz2_wymBiz();
        }

        protected void Button_cz2_Click(object sender, EventArgs e)
        {
         //   HLDtresc_cz2.Controls.Clear();
         //  UpdateTimer_cz2.Enabled= false;
         //   aaaPage_Load_cz2_wymBiz();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
         //   panelMenu.Attributes.Add("style", "display:block;");

        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
          //  panelMenu.Attributes.Add("style", "display:none;");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int r1 = 4;
            int r1_2 = 1;
            aaaPage_Load_cz4_wklady(ref r1, ref r1_2);
        }

       
    }
}