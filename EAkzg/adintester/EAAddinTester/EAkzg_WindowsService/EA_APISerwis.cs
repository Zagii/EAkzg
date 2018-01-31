using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using EA;
using System.Data;
using MySql.Data.MySqlClient;
//using System.Windows.Forms;


namespace EAkzg_WindowsService
{
    class EA_APISerwis
    {
        internal static EA.Repository eaRepository;
        static String schema = "";
        MySql.Data.MySqlClient.MySqlConnection conn;
       
        EA.Project projektInterfejs;
        const  string  myConnectionString="server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";

        string sciezkaDomyslna = @"u:\EAkzg_Cloud_Diagram\";
        int debug = 1;
        int coileTimer = 5; //min

        public int getInterwal()
        {
            return coileTimer;
        }

        private static EA.Repository getOpenedModel()
        {
            try
            {
                EA.App ap=(EA.App)Marshal.GetActiveObject("EA.App");
                
               return ap.Repository;
            }
            catch (COMException)
            {
                
                return new EA.Repository();
            }
        }

        public  EA_APISerwis()
        {
          
            DB_Connect();
            
            eaRepository = getOpenedModel();
            

           
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
            }
            return 0;
        }
        public int EA_Close()
        {
            try
            {
                if (schema != "")
                {
                    eaRepository.CloseFile();
                }
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
                bool w = eaRepository.OpenFile2(connStr,"www","www");
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
        public int dzialajDlaProjektu(string pr, string sql)
        {
            log("Projekt " + pr,"Info");
            MySql.Data.MySqlClient.MySqlConnection conn2 = new MySql.Data.MySqlClient.MySqlConnection();
            MySqlCommand cmd2 = new MySqlCommand();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;
            int ile = 0;
         //   MySqlDataReader reader2;
            try
            {

                cmd.Connection = conn;
                cmd.CommandText = sql;

                
                string sciezkaDef = sciezkaDomyslna+pr+@"\";
                string sciezkaDb;
                reader = cmd.ExecuteReader();
                
               
                if (reader.HasRows)
                {
                    if (EA_Connect(pr) == 0)
                    {
                        conn2.ConnectionString = "server=10.22.23.82;uid=eakzg;database=eakzg_schema;Pwd=a;port=3306";
                        conn2.Open();

                        cmd2.Connection = conn2;
                       
                        while (reader.Read())
                        {
                            string diagramID = reader["Diagram_ID"].ToString();
                            string datWyrzyg = reader["dataWyrzygu"].ToString();
                            string sciezka = reader["sciezka"].ToString();
                            string plik = reader["plik"].ToString();
                            string diagGuid = reader["ea_guid"].ToString();
                            if (sciezka == String.Empty) sciezka = sciezkaDef;

                            try
                            {


                                System.IO.Directory.CreateDirectory(sciezka);
                                sciezkaDb = sciezka.Replace(@"\", @"\\");


                                projektInterfejs.PutDiagramImageToFile(diagGuid, sciezka + diagramID + ".png", 1);
                            }
                            catch (Exception ex)
                            {
                                log("Blad IO msg: " + ex.Message, "Exc - dzialajDlaProjektu");
                                continue;
                            }

                            string updt = " INSERT INTO eakzg_schema.eakzg_wyrzyg_log (id,projekt,diagramID,diagramGUID,dataWyrzygu,sciezka,plik) " +
                            "values (null,'" + pr + "','" + diagramID + "','" + diagGuid + "', " + @"now(), '" + sciezkaDb + "','" + diagramID + ".png') on duplicate key update " +
                            @" dataWyrzygu=now(),sciezka='" + sciezkaDb + "',plik='" + diagramID + ".png'";

                            cmd2.CommandText = updt;
                            int numRowsUpdated = cmd2.ExecuteNonQuery();

                            ile++;

                        }
                        conn2.Close();
                        EA_Close();
                        
                    }
                }
                    reader.Close();
                
            }
            catch (MySqlException ex)
            {
                log("Blad " + ex.Number + " msg: " + ex.Message,"Exc - dzialajDlaProjektu");
               
                conn2.Close();
                EA_Close();
              // reader.Close();
               // reader2.Close();
            }
           log("### Wygenerowano: " + ile + " diagramów","Info");
            return 0;
        }
        public void log(string msg, string typ)
        {
            try
            {
                MySql.Data.MySqlClient.MySqlConnection connDebug;
                connDebug = new MySql.Data.MySqlClient.MySqlConnection();
                connDebug.ConnectionString = myConnectionString;
                connDebug.Open();
                string insert ="INSERT INTO eakzg_schema.eakzg_wyrzyg_debug(data,msg,typ) VALUES ( "+
                            @" now(), '"+msg+"','"+typ+"')";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connDebug;
                cmd.CommandText = insert;
                cmd.ExecuteNonQuery();
                connDebug.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad log msg: " + ex.Message);
            }
        }
        public int dzialajDlaWszystkich()
        {
            String sql;
           DB_Connect();
           MySqlCommand cmd = new MySqlCommand();
           MySqlDataReader reader;

           try
           {
               
               cmd.Connection = conn;

               //odczytuje konfiguracje
               sql = @"select priorytet, coile, sciezkaDef, debug  from  eakzg_schema.eakzg_konfig order by priorytet limit 1";
               cmd.CommandText = sql;
               cmd.ExecuteNonQuery();
               reader = cmd.ExecuteReader();
               while (reader.Read())
               {
                   coileTimer = (int) reader["coile"];
                   sciezkaDomyslna = reader["sciezkaDef"].ToString();
                   debug=(int) reader["debug"];

               }
               reader.Close();
               //lista wszystkich diagramow z data utworzenia i modyfikacji
               // zlaczona z lista logow ze zrzutu - brak zrzutu lub stara data
               sql= @"select code as Symbol, name as Nazwa,  created_date as DataUtworzenia
                from sdpd.project p ,information_schema.SCHEMATA s
                  where (p.code like 'PR-%' or p.code like 'EU-%') 
                 and ( s.SCHEMA_NAME like 'eu-%' or s.SCHEMA_NAME like 'pr-%')
                and lower(p.code)=lower(s.schema_name) order by code asc";

               cmd.CommandText = sql;
               cmd.ExecuteNonQuery();
               reader = cmd.ExecuteReader();
               List<string> lista = new List<string>();
               int n = 0;
            while (reader.Read())
            {
               string schema=reader[0].ToString();
                // sprawdz czy w danym projekcie trzeba robic zrzut
                String sql2="select a.id, a.projekt,a.diagramID, a.diagramGUID, a.dataWyrzygu,a.sciezka,a.plik,d.ea_guid, d.Diagram_ID,d.CreatedDate,d.ModifiedDate,d.Name from "+
                "`" + schema + "`.t_diagram d left join  eakzg_schema.eakzg_wyrzyg_log a on d.Diagram_ID=a.diagramID , eakzg_schema.eakzg_slo_typydiagramow sloTyp "+
                "where (a.dataWyrzygu is null or a.dataWyrzygu<d.ModifiedDate) and d.Diagram_Type = sloTyp.typ";
                
                //Console.WriteLine(sql2);
                // Zrzuc diagramy dla pr=schema lista diagramow w sql2
                lista.Add(schema);
                n++;
                lista.Add(sql2);
                n++;
            }
            reader.Close();
            int i = 0;
            while(i<n)
            {
                string schema = lista[i++];
                string sql2 = lista[i++];
                if (dzialajDlaProjektu(schema, sql2) < 0)
                    log( schema + " sql=" + sql2,"Blad - dzialajDlaWszystkich");
            }
           }
           catch (MySqlException ex)
           {
              log("Blad " + ex.Number + " msg: " + ex.Message,"Exc -dzialajDlaWszystkich");
           }

            return 0;
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
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
