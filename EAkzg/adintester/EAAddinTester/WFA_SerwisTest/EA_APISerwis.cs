using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using EA;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.IO;
using System.Threading;
using MarkupConverter;
//using System.Windows.Forms;


namespace EAkzg_WindowsService
{
    class EA_APISerwis
    {
        IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter();
        internal static EA.Repository eaRepository;
        static String schema = "";
        static MySql.Data.MySqlClient.MySqlConnection  conn;

        static EA.App ap = null;
       
        static EA.Project projektInterfejs;
        const  string  myConnectionString="server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";

        string sciezkaDomyslna = @"u:\EAkzg_Cloud_Diagram\";
        int debug = 1;
        int coileTimer = 5; //min

        public int getInterwal()
        {
            return coileTimer;
        }
        public string getSciezka()
        {
            return sciezkaDomyslna;
        }

        private static EA.App getOpenedApp()
        {
            try
            {
               ap=(EA.App)Marshal.GetActiveObject("EA.App");
                
               return ap;
            }
            catch (COMException)
            {
                ap = new EA.App();
                return ap;
            }
        }

        public  EA_APISerwis()
        {
          
            DB_Connect();
            
            eaRepository = getOpenedApp().Repository;
            

           
        }
        public int DB_Connect()
        {
            try
            {
                if (conn!=null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        return 0;
                    }
                }
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                   //     MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                      //  MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                conn.Dispose();
            }
            return 0;
        }
        public int EA_Close()
        {
            try
            {
                
               // eaRepository.CloseFile();
             
                //ap = null;
            }
            catch
            { }
            schema = "";
            return 0;
        }
        public int EA_Connect(String s)
        {
             String connStr = s + ";Connect=Cloud=protocol:http,address:10.22.23.82,port:1804;Data Source=" + s + ";DSN=" + s + ";Integrated Security=false;Persist Security Info=True;uid=www;pwd=www;User ID=www;Password=www;lazyload=false";
         //   String connStr = "Connect=Provider=MSDASQL5.1;port=1804;Password=www;Persist Security Info=True;User ID=www;Data Source=" + s + ";Initial Catalog=" + s;
         //   String connStr = "Dsn=" + s + ";uid=www;pwd=www;description=x;server={10.22.23.82};database=" + s + ";port=3306";
            EA_Close();
            try
            {
                eaRepository = getOpenedApp().Repository;
                

                bool w = eaRepository.OpenFile2(connStr,"www","www");

                if (!w)
                {
                    w = eaRepository.OpenFile2(connStr, "admin", "password");
                }

                projektInterfejs = eaRepository.GetProjectInterface();
                if (w)
                {
                    schema = s;
                    if (IsProjectOpen(eaRepository))
                        return 0;
                    else
                        return -2;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
               log("EA_Connect exc " + s+": "+ex.Message,"Exc - EA_Connect");
                return -3;
            }
        }
        public int dzialajDlaProjektuDiagramy(string pr, string sql)
        {
           log(" Projekt " + pr ,"Info");
           int ile = 0;
           using(   MySql.Data.MySqlClient.MySqlConnection conn2 = new MySql.Data.MySqlClient.MySqlConnection())
           using(MySqlCommand cmd2 = new MySqlCommand())
           using (MySqlCommand cmd = new MySqlCommand())
           {
               
               try
               {
                   cmd.Connection = conn;
                   cmd.CommandText = sql;
                   string sciezkaDef = sciezkaDomyslna+ pr + @"\Diagramy\";
                   string sciezkaDb;

                   using (MySqlDataReader reader = cmd.ExecuteReader())
                   {
                       if (reader.HasRows)
                       {
                           if (EA_Connect(pr) == 0)
                           {
                               conn2.ConnectionString = "server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";
                               conn2.Open();

                               cmd2.Connection = conn2;

                               while (reader.Read())
                               {
                                   string diagramID = reader["diagram_ID"].ToString();
                                   string datWyrzyg = reader["dataWyrzygu"].ToString();
                                   string sciezka = "";//reader["sciezka"].ToString();
                                   string plik = reader["plik"].ToString();
                                   string diagGuid = reader["ea_guid"].ToString();
                                   if (sciezka == String.Empty) sciezka = sciezkaDef;
                                   
                                   try
                                   {


                                       System.IO.Directory.CreateDirectory(sciezka);
                                       sciezkaDb = sciezka.Replace(@"\", @"\\");


                                       if (projektInterfejs.PutDiagramImageToFile(diagGuid, sciezka + diagramID + ".png", 1))
                                       {
                                       }
                                       else 
                                       {
                                       }



                                       string updt = " INSERT INTO eakzg_schema.eakzg_wyrzyg_log (id,projekt,objectID,objectGUID,dataWyrzygu,sciezka,plik) " +
                                       "values (null,'" + pr + "','" + diagramID + "','" + diagGuid + "', " + @"now(), '" + sciezkaDb + "','" + diagramID + ".png') on duplicate key update " +
                                       @" dataWyrzygu=now(),sciezka='" + sciezkaDb + "',plik='" + diagramID + ".png'";

                                       cmd2.CommandText = updt;
                                       int numRowsUpdated = cmd2.ExecuteNonQuery();

                                       ile++;
                                   }
                                   catch (Exception ex)
                                   {
                                       log("Blad IO msg: " + ex.Message, "Exc - dzialajDlaProjektu " + " TargetSite: " + ex.TargetSite.ToString());
                                   }
                               }
                           }
                       }
                   }
               }
               catch (MySqlException ex)
               {
                   log("Blad " + ex.Number + " msg: " + ex.Message, "Exc - dzialajDlaProjektu " + " TargetSite: " + ex.TargetSite.ToString());

               }
         //      log("Projekt " + pr +"### Wygenerowano: " + ile + " diagramów", "Info");
             
           }
           return ile;
        }
        public int dzialajDlaProjektuLinkedDoc(string pr, string sql)
        {
            log("Projekt " + pr , "Info");
            int ile = 0;
            using (MySql.Data.MySqlClient.MySqlConnection conn2 = new MySql.Data.MySqlClient.MySqlConnection())
            using (MySqlCommand cmd2 = new MySqlCommand())
            using (MySqlCommand cmd = new MySqlCommand())
            {

                try
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    string sciezkaDef = sciezkaDomyslna + pr + @"\LinkedDoc\";
                    string sciezkaDb;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (EA_Connect(pr) == 0)
                            {
                                conn2.ConnectionString = "server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";
                                conn2.Open();

                                cmd2.Connection = conn2;

                                while (reader.Read())
                                {
                                    string objectID = reader["object_ID"].ToString();
                                    string datWyrzyg = reader["dataWyrzygu"].ToString();
                                    string sciezka = reader["sciezka"].ToString();
                                    string plik = reader["plik"].ToString();
                                    string objectGuid = reader["ea_guid"].ToString();
                                    if (sciezka == String.Empty) sciezka = sciezkaDef;

                                    try
                                    {


                                        System.IO.Directory.CreateDirectory(sciezka);
                                        sciezkaDb = sciezka.Replace(@"\", @"\\");



                                        //projektInterfejs.PutDiagramImageToFile(objectGuid, sciezka + objectID + ".png", 1);

        /*                                sql = " SELECT o.object_id,d.BinContent, o.Note FROM " +
               "`" + schema + "`.t_object o " +
               ", `" + schema + "`.t_document d " +
           " WHERE o.Style LIKE '%MDoc=1%' and o.object_type='Object' and d.elementid=o.ea_guid  and o.name='Skrot';";
                                        */

                                       

                                        EA.Element e = eaRepository.GetElementByGuid(objectGuid);
                                        string rtf = e.GetLinkedDocument();
                                        string html = ConvertRtfToHtml(rtf);

                                        System.IO.StreamWriter file = new System.IO.StreamWriter(sciezka + objectID + ".html");
                                        file.Write(html);
                                        file.Close();
                                        System.IO.StreamWriter filertf = new System.IO.StreamWriter(sciezka + objectID + ".rtf");
                                        filertf.Write(rtf);
                                        filertf.Close();


                                        string updt = " INSERT INTO eakzg_schema.eakzg_wyrzyg_log (id,projekt,objectID,objectGUID,dataWyrzygu,sciezka,plik) " +
                                        "values (null,'" + pr + "','" + objectID + "','" + objectGuid + "', " + @"now(), '" + sciezkaDb + "','" + objectID + ".html') on duplicate key update " +
                                        @" dataWyrzygu=now(),sciezka='" + sciezkaDb + "',plik='" + objectID + ".html'";

                                        cmd2.CommandText = updt;
                                        int numRowsUpdated = cmd2.ExecuteNonQuery();

                                        ile++;
                                    }
                                    catch (Exception ex)
                                    {
                                        log("Blad IO msg: " + ex.Message, "Exc - dzialajDlaProjektu " + " TargetSite: " + ex.TargetSite.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    log("Blad " + ex.Number + " msg: " + ex.Message, "Exc - dzialajDlaProjektu " + " TargetSite: " + ex.TargetSite.ToString());

                }
                log("Projekt " + pr + "### Wygenerowano: " + ile + " diagramów", "Info");

            }
            return ile;
        }
        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }
        private void ConvertRtfInSTAThread(object rtf)
        {
            var threadData = rtf as ConvertRtfThreadData;
            try
            {
                threadData.HtmlText = markupConverter.ConvertRtfToHtml(threadData.RtfText);
            }
            catch (Exception ex)
            {
                threadData.HtmlText = "EXC: ConvertRtfInSTAThread; "+ex.Message+" \n" + threadData.RtfText;
            }
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
        public string AddCommasIfRequired(string path)
        {
            return (path.Contains(" ")) ? "\"" + path + "\"" : path;
        }
        public void log(string msg, string typ)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connDebug = new MySql.Data.MySqlClient.MySqlConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string insert = "";
                    try
                    {

                        connDebug.ConnectionString = myConnectionString;
                        connDebug.Open();
                        AddCommasIfRequired(msg);
                        AddCommasIfRequired(typ);
                        insert = "INSERT INTO eakzg_schema.eakzg_wyrzyg_debug(data,msg,typ) VALUES ( " +
                                    @" now(), '" + msg + "','" + typ + "')";

                        cmd.Connection = connDebug;
                        cmd.CommandText = insert;
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Blad log msg: " + ex.Message+ " msq: "+msg+ " typ:"+typ);

                    }
                    finally
                    {
                        connDebug.Close();
                        connDebug.Dispose();
                        cmd.Dispose();
                    }
                }
            }
        }
        public int getConfig()
        {
           String sql="";
           DB_Connect();
           using (MySqlCommand cmd = new MySqlCommand())
           {
               try
               {
                   cmd.Connection = conn;

                   //odczytuje konfiguracje
                   sql = @"select priorytet, coile, sciezkaDef, debug  from  eakzg_schema.eakzg_konfig order by priorytet limit 1";
                   cmd.CommandText = sql;
                  // cmd.ExecuteNonQuery();
                   using (MySqlDataReader reader = cmd.ExecuteReader())
                   {
                       while (reader.Read())
                       {
                           coileTimer = (int)reader["coile"];
                           sciezkaDomyslna = reader["sciezkaDef"].ToString();
                           debug = (int)reader["debug"];

                       }
                   }
               }
               catch (Exception ex)
               {
                   log("sql=" + sql + " exc: " + ex.Message, "Blad - getConfig " + " TargetSite: " + ex.TargetSite.ToString());
                   return -1;
               }
           
           }
           return 0;
        }
        public int dzialajDlaWszystkich(WFA_SerwisTest.Form1 f,BackgroundWorker bw)
        {
            String sql;
           DB_Connect();
           using (MySqlCommand cmd = new MySqlCommand())
           {
               try
               {

                   cmd.Connection = conn;

                   //odczytuje konfiguracje
                   getConfig();
                    List<string> lista = new List<string>();
                   int n = 0;
                   //lista wszystkich diagramow z data utworzenia i modyfikacji
                   // zlaczona z lista logow ze zrzutu - brak zrzutu lub stara data
                   sql = @"select code as Symbol, name as Nazwa,  created_date as DataUtworzenia
                from sdpd.project p ,information_schema.SCHEMATA s
                  where (p.code like 'PR-%' or p.code like 'EU-%') 
                 and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%') 
                and lower(p.code) not in (select projekt from eakzg_schema.eakzg_czarnalista)         
                and lower(p.code)=lower(s.schema_name) order by code asc";
                
                   cmd.CommandText = sql;
                  
                  /////////// przygotowanie zapytan do pozniejszego przebiegu
                   using (MySqlDataReader reader = cmd.ExecuteReader())
                   {

                       while (reader.Read())
                       {
                           string schema = reader[0].ToString();



                           // sprawdz czy w danym projekcie trzeba robic zrzut
                           String sql2 = "select a.id, a.projekt,a.objectID, a.objectGUID, a.dataWyrzygu,a.sciezka,a.plik,d.ea_guid, d.Diagram_ID,d.CreatedDate,d.ModifiedDate,d.Name from " +
                           "`" + schema + "`.t_diagram d left join ( select *  from eakzg_schema.eakzg_wyrzyg_log  where projekt='" + schema + "' ) a on d.Diagram_ID=a.objectID , eakzg_schema.eakzg_slo_typydiagramow sloTyp, " +
                           "`" + schema + "`.t_secuser su "+
                           "where (a.dataWyrzygu is null or a.dataWyrzygu<d.ModifiedDate) and d.Diagram_Type = sloTyp.typ "+
                           " and su.userlogin='www' ";//and su.Password='2GNb9GUcq5BP'";

                           //Console.WriteLine(sql2);
                           // Zrzuc diagramy dla pr=schema lista diagramow w sql2
                           lista.Add(schema);
                           n++;
                           lista.Add(sql2);
                           n++;
                       

                       ///////////////// linked document

                  /*         sql2= "select a.id, a.projekt,a.objectID, a.objectGUID, a.dataWyrzygu,a.sciezka,a.plik,o.ea_guid, o.object_ID,o.CreatedDate,o.ModifiedDate,o.Name from "+
                               "`"+schema+"`.t_secuser su, "+
                               "`"+schema+"`.t_object o left join ( select *  from eakzg_schema.eakzg_wyrzyg_log  where projekt="+
                               "'"+schema+"' ) a on o.object_ID=a.objectID where (a.dataWyrzygu is null or a.dataWyrzygu<o.ModifiedDate)  and o.Style LIKE '%MDoc=1%' "+
                               " and su.userlogin='www' and su.Password='2GNb9GUcq5BP'";
                          lista.Add(sql2);
                           n++;
                */
                         /////////////////////////////////
                         }

                   }
             
                   int i = 0;
                   while (i < n)
                   {
                       if (bw.CancellationPending)
                       {
                           // e.Cancel = true;
                           bw.ReportProgress(0);
                           throw new Exception("Anulowanie operacji");
                       }
                       string schema = lista[i++];
                       string sql2 = lista[i++];
                       f.logStart(DateTime.Now.ToString()+" Start "+schema);
                       int ile=dzialajDlaProjektuDiagramy(schema, sql2);
                       if ( ile< 0)
                           log(schema + " sql= " + sql2, "Blad - dzialajDlaWszystkichDiagramy");
                       f.logKoniec("                     "+DateTime.Now.ToString()+" Koniec " + schema + " # Wygenerowano diagramów:" + ile);
                       string[] s = new string[2];
                       s[0]=schema + " - " + 100 * i / n + "%";
                       s[1] = null;// schema + "- wygenerowano " + ile + " diagramów.";

                       logujDBp(schema, ile);

                       bw.ReportProgress(100 * i / n,s );

                       /////////////// linked doc
                       /*string sql3 = lista[i++];
                       ile = dzialajDlaProjektuLinkedDoc(schema, sql3);
                       if (ile < 0)
                           log(schema + " sql= " + sql3, "Blad - dzialajDlaWszystkichLinkedDoc");

                    
                       s[0] = schema + " - " + 100 * i / n + "%";
                       s[1] = schema + "- wygenerowano " + ile + " linkedDoc.";

                       bw.ReportProgress(100 * i / n, s);
                       */
                   }

               }
               catch (MySqlException ex)
               {
                   log("Blad " + ex.Number + " msg: " + ex.Message, "Exc -dzialajDlaWszystkich " + " TargetSite: " + ex.TargetSite.ToString());

               }
            
           }
           EA_Disconnect();
            return 0;
        }
        protected void logujDBp(string schema,int ile)
        {
            using (MySql.Data.MySqlClient.MySqlConnection conn2 = new MySql.Data.MySqlClient.MySqlConnection())
            using (MySqlCommand cmd2 = new MySqlCommand())
            {
                try
                {
                    conn2.ConnectionString = "server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";
                    conn2.Open();

                    cmd2.Connection = conn2;
                    string updt = "INSERT INTO eakzg_schema.eakzg_wyrzyg_logP (projekt,ileAll,initRzyg ,ostChk ,ileOstChk ) "+
                                                              " values ('"+schema+"', "+ile+@",  now(),  now() ,  0 ) on duplicate key update "+
                                                  @" ostChk=now(),ileOstChk="+ile +",ileAll=ileAll+"+ile;

                    cmd2.CommandText = updt;
                    int numRowsUpdated = cmd2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log("Blad logowania msg: " + ex.Message, "Exc - logujDBp " + " TargetSite: " + ex.TargetSite.ToString());
                }
            }
        }
      
      /*  public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.
            return "a string";
        }*/
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;
               
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void EA_Disconnect()
        {
            EA_Close();
            conn.Close();
            conn.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
