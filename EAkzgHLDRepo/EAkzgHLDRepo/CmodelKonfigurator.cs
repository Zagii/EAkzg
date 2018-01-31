
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace EAkzg
{
  public static class CmodelKonfigurator
    {
    
       
      /// <summary>
      ///  czy dany string zawiera sie w tabeli stringów
      /// </summary>
      /// <param name="s">string do sprawdzenia</param>
      /// <param name="tab">tablica do przeszukania</param>
      /// <returns></returns>
 
      public static bool czyZawiera(String s, String[] tab)
      {          
          foreach (String str in tab)
          {
              if (str == s)
                  return true;
          }
          return false;
      }

      public static string[] symboleNazwProjektow = { "PR", "EU" };

      // public static string[] statusyWymaganBiznesowych = { "Nowy", "OK", "Uzgodnione", "Z uwagami", "Uwzględnione uwagi", "Anulowane przez BO", "Anulowane przez IT" };
      // public static string[] statusyWymaganSystemowych = { "Nowy", "OK", "Uzgodnione", "Z uwagami", "Uwzględnione uwagi", "Anulowane przez BO", "Anulowane przez IT" };

       public static string[] statusyFeatureGotowe = {"Zaakceptowane", "Uzgodnione", "Anulowane przez BO", "Anulowane przez IT" };

       /* lista statusów akceptowalnych dla wymagań biznesowych przy przejściu do analizy systemowej */ 
       public static string[] statusyBRqGotowe = { "03-Zweryfikowane", "04-Wykonalne", "05-Niewykonalne", "14-Zamknięte", "15-Odrzucone","Uzgodnione","Zaakceptowane" };
 
       /* tabela z komentarzami dodawanymi przez generator */
       public static Hashtable worning = new Hashtable() 
       {
            { "SRQanalysis", "Wymaganie systemowe w trakcie analizy" },
            { "BRQanalysis", "Wymaganie biznesowe w trakcie analizy" },
            { "BRQ_SRQanalysis", "Wymaganie biznesowe jest w roboczym statusie"},
       };
       //po zmianach 01.12.2015 wszystkie stereotypy ktore nie są Architektoniczne lub Infrastrukturowe przypadaja na wymagania biznesowe
    //   public static string[] stereotypyWymaganBiznesowych = { "1-Concept","Biznesowe", "Procesowe", "Functional", "MUST", "NICE-TO-HAVE", "Business","1-Funkcjon.","2-Niefunkc.","7-ProcesyIT" };
       public static string[] stereotypyWymaganArchitektonicznych = { "Arch." };
       public static string[] stereotypyWymaganInfrastruktura = { "Infrastruktura" };


       public static string[] stereotypyFeatureSystemowychFunkcjonalne = { "Funkcjonalne","Functional" };
      //po zmianach 01.12.2015 wszystkie stereotypy, które nie są wymienione poniżej traktowane są jako funkcjonalne;
       public static string[] stereotypyFeatureSystemowychInfrastrukturalne = { "Infrastrukt." };
       public static string[] stereotypyFeatureSystemowychBezpieczeństwa = { "Bezp." };
       public static string[] stereotypyFeatureSystemowychPojemnosc = { "Pojemność" };
       public static string[] stereotypyFeatureSystemowychDostepnosc = { "Dostępność" };

       public static string ukryjDiagramStr = "ukryj";

       public const string CPpakStr = "Koncepcja Biznesowa";
       public const string HLDpakStr = "HLD";
       public const string StatystykiPakStr = "Statystyki";
       public const string AktorzyPakStr = "Aktorzy";
       public const string DefinicjePakStr = "Definicje";
       public const string OgraniczeniaPakStr = "Ograniczenia rozwiązania";
       public const string SlownikPakStr = "Słownik";
       public const string ZaleznosciPakStr = "Zależności";
       public const string ZalacznikiPakStr = "Załączniki";
       public const string ProjektNazwaElemString = "Projekt-Nazwa";
       public const string ArchitekturaTransmisyjnaElemString = "Architektura Transmisyjna";
       public const string TestyElemString = "Testy - wskazówki";
       public const string TestyAutomatElemString = "Testy - automatyzacja";
       public const string PMwskazowkiElemString = "PM - wskazówki";

       public static string [] obszarPakStr = {"IT","NT"};
      
       public const string ArchStatPakStr = "Architektura Statyczna";
       public const string ArchitekturaDanychPakStr = "Architektura Danych";
       public const string ArchitekturaDanychLDMPakStr = "LDM";
       public const string ArchitekturaDanychIDMPakStr = "IDM";

       public const string DiagSekwPakStr = "Diagramy Sekwencji";
       public const string KoncepcjaPakStr = "Koncepcja";
       public const string KoncepcjaElemStr = "Koncepcja";
       public const string MigracjaElemStr = "Migracja";
       public const string SkrotElemStr = "Skrot";
       public const string PrzypadkiTechnUzyciaPakStr = "Przypadki Użycia";
       public const string WkladyPakStr = "Wkłady Systemowe";
       public const string PrzypadkiBizPakStr = "Przypadki Użycia";
       public const string WymaganiaPakStr = "Wymagania";

       public const string SlownikNazwiskoTagValue = "Imię i Nazwisko";

       public const string TypLinkuPakietSystemowyComponent = "Abstraction"; //"Realisation";

       public static int nrPliku = 0;
       public static string prefixPlik = "EAkzgDiagram";

    }
   public class CModel
   {
       //public enum obszarEnum{IT=0,NT=1};
       public const int IT=0;
       public const int NT=1;

       public const int WymaganiaBiz = 0;
       public const int WymaganiaInf = 1;
       public const int WymaganiaArch = 2;

       string Schema;
 

      // String SDit;
     //  String SDnt;
       public String NazwaProjektuPelna;
       public int RootPckg;
       public int HLDPckg;
       public int CPPckg;
       public int StatystykiPckg;
       public int AktorzyPckg;
       public int DefinicjePckg;
       public int OgraniczeniaPckg;
       public int HistoriaZmianPckg;
       public int SlownikPckg;
       public int ZaleznosciPckg;
       public int ZalacznikiPckg;
       public int ProjektNazwaElem;
       public int ArchitekturaTransmisyjnaElem;
       public int TestyElem;
       public int TestyElemAutomat;
       public int PMwskazowkiElem;

       public int[] ObszarPckg = new int[2];
       public int[] ArchStatPckg = new int[2];
       public int[] DiagrSekwPckg = new int[2];
       public int[] KoncepcjaPckg = new int[2];
       public int[] KoncepcjaElem = new int[2];
       public int[] MigracjaElem = new int[2];
       public int[] SkrotElem = new int[2];
       public int[] PrzypadkiPckg = new int[2];
       public int[] WkladyPckg = new int[2];
       public int[] ArchitekturaDanychPckg = new int[2];
       public int[] ArchitekturaDanychLDMPckg = new int[2];
       public int[] ArchitekturaDanychIDMPckg = new int[2];

       public String[] SDNazwaStr = new String[2];

       public int BiznesowePrzypadkiPckg;
       public int WymaganiaPckg;

       public List<int> WymaganiaBiznesoweLista = new List<int>();
       public List<int> WymaganiaArchitektoniczneLista = new List<int>();
       public List<int> WymaganiaInfrastrukturaLista = new List<int>();

       /// <summary>
       /// Lista kwestii otwartych w projekcie
       /// </summary>
       public List<int> ListaIssue = new List<int>();

       SqlDataSource SqlDataSource1;

       public CModel(string schema,SqlDataSource ds)
       {
           SqlDataSource1 = ds;
           Schema = schema;
            odczytajNaprawModel(Schema);
       }
       public void szukajIssueSQL()
       {
           
       }

       public void szukajIssue(int p)
       {
          
       }
       /// <summary>
       /// rekurencyjnie wyszukuje wymagań w pakiecie d
       /// </summary>
       /// <param name="d">pakiet startowy</param>
       public void wyszukajWymaganiaPakietu(int d)
       {
          
       }
       public void wyszukajWymaganiaSQL()
       {
        
       }
       /// <summary>
       /// Dodaje wymaganie do odpowiedniej listy wymagań
       /// </summary>
       /// <param name="rq"></param>
       public bool dodajWymaganieDoListy(int rq)
       {
           return true;
       }
       public String dajAutoraProjektu(int ktory)
       {
           return SDNazwaStr[ktory];
           
       }
       public String dajPelnaNazweProjektu()
       {
           return NazwaProjektuPelna;
       }
       public String dajNazweModelu()
       {
           return "";// RootPckg.Name;
       }
       public void odswiezModel()
       {
           odczytajNaprawModel(Schema);
       }

       public int dajObiekt(String nazwa, string typ)
       {
         
           string sql = "select object_id from `" + Schema + "`.t_object where name='" + nazwa + "' and object_type='" + typ + "';";
           SqlDataSource1.SelectCommand = sql;
           try
           {
               DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
               return (int)(dv.ToTable()).Rows[0][0];
           }
           catch
           {
               return -1;
           }
       }
       public int dajObiekt(String nazwa, string typ,int rodzic)
       {
            string sql = "select object_id from `" + Schema + "`.t_object where name='" + nazwa + "' and object_type='" + typ + "' and package_id="+rodzic+";";
           SqlDataSource1.SelectCommand = sql;
           try
           {
               DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
               return (int)(dv.ToTable()).Rows[0][0];
           }
           catch
           {
               return -1;
           }
       }
       public int dajPakiet(String nazwa,  int rodzic)
       {
           string sql = "select package_id from `" + Schema + "`.t_package where name='" + nazwa + "' and Parent_ID=" + rodzic;
           SqlDataSource1.SelectCommand = sql;
           try
           {
               DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
               return (int)(dv.ToTable()).Rows[0][0];
           }
           catch
           {
               return -1;
           }
       }
       public string dajNazweObiektu(int obiektID)
       {
           string sql = "select name from `" + Schema + "`.t_object where object_id=" + obiektID;
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
       public string dajNotesObiektu(int obiektID)
       {
           string sql = "select note from `" + Schema + "`.t_object where object_id=" + obiektID;
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
       public void odczytajNaprawModel(string schema)
       {
                   RootPckg = dajPakiet(schema,0);
                   HLDPckg = dajPakiet(CmodelKonfigurator.HLDpakStr, RootPckg);
                   CPPckg = dajPakiet(CmodelKonfigurator.CPpakStr, RootPckg);
                   
                   DefinicjePckg = dajPakiet( CmodelKonfigurator.DefinicjePakStr, HLDPckg);
                   OgraniczeniaPckg = dajPakiet(CmodelKonfigurator.OgraniczeniaPakStr, DefinicjePckg);
                   SlownikPckg = dajPakiet(CmodelKonfigurator.SlownikPakStr, DefinicjePckg);
                   ZaleznosciPckg = dajPakiet(CmodelKonfigurator.ZaleznosciPakStr, DefinicjePckg);
                   ZalacznikiPckg = dajPakiet(CmodelKonfigurator.ZalacznikiPakStr, DefinicjePckg);
                   
                   ProjektNazwaElem = dajObiekt(CmodelKonfigurator.ProjektNazwaElemString, "Object", DefinicjePckg);
                   ArchitekturaTransmisyjnaElem = dajObiekt(CmodelKonfigurator.ArchitekturaTransmisyjnaElemString, "Object", DefinicjePckg);
                   TestyElem = dajObiekt(CmodelKonfigurator.TestyElemString, "Object", DefinicjePckg);
                   TestyElemAutomat = dajObiekt(CmodelKonfigurator.TestyAutomatElemString, "Object", DefinicjePckg);
                   PMwskazowkiElem = dajObiekt(CmodelKonfigurator.PMwskazowkiElemString, "Object", DefinicjePckg);
                   
           
                    for (int i = 0; i < 2; i++)//(obszarEnum[])Enum.GetValues(typeof(int)))
                    {
                        ObszarPckg[i] = dajPakiet(CmodelKonfigurator.obszarPakStr[i], HLDPckg);
                        ArchStatPckg[i] = dajPakiet(CmodelKonfigurator.ArchStatPakStr,  ObszarPckg[i]);
                        DiagrSekwPckg[i] = dajPakiet(CmodelKonfigurator.DiagSekwPakStr,  ObszarPckg[i]);
                        KoncepcjaPckg[i] = dajPakiet(CmodelKonfigurator.KoncepcjaPakStr,  ObszarPckg[i]);
                        KoncepcjaElem[i] = dajObiekt(CmodelKonfigurator.KoncepcjaElemStr, "Object", KoncepcjaPckg[i]);
                        MigracjaElem[i] = dajObiekt(CmodelKonfigurator.MigracjaElemStr, "Object", KoncepcjaPckg[i]);
                        SkrotElem[i] = dajObiekt(CmodelKonfigurator.SkrotElemStr, "Object", KoncepcjaPckg[i]);
                        PrzypadkiPckg[i] = dajPakiet(CmodelKonfigurator.PrzypadkiTechnUzyciaPakStr,  ObszarPckg[i]);
                        WkladyPckg[i] = dajPakiet(CmodelKonfigurator.WkladyPakStr,  ObszarPckg[i]);
                        ArchitekturaDanychPckg[i] = dajPakiet(CmodelKonfigurator.ArchitekturaDanychPakStr,  ObszarPckg[i]);
                        ArchitekturaDanychLDMPckg[i] = dajPakiet(CmodelKonfigurator.ArchitekturaDanychLDMPakStr, ArchitekturaDanychPckg[i]);
                        ArchitekturaDanychIDMPckg[i] = dajPakiet(CmodelKonfigurator.ArchitekturaDanychIDMPakStr, ArchitekturaDanychPckg[i]);
                        

                    }
                    BiznesowePrzypadkiPckg = dajPakiet(CmodelKonfigurator.PrzypadkiBizPakStr, HLDPckg);
                    WymaganiaPckg = dajPakiet(CmodelKonfigurator.WymaganiaPakStr, HLDPckg);

   //         SDNazwaStr[CModel.IT]= EAUtils.dajAutoraProjektu(ref RootPckg, "SD IT");
   //         SDNazwaStr[CModel.NT] = EAUtils.dajAutoraProjektu(ref RootPckg, "SD NT");
            //        NazwaProjektuPelna = dajNotesObiektu(ProjektNazwaElem);

          
             szukajIssueSQL();
             wyszukajWymaganiaSQL();
       }

 
   }
}
