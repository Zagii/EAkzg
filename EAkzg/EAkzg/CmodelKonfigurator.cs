using EA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      /* lista statusów wymagań biznesowych ktorych nie prezentujemy w HLD ==> wymagania anulowane */
       public static string[] statusyBRqAnulowane = { "15-Odrzucone", "Anulowane przez BO", "Anulowane przez IT" };
 
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
       public const string HistoriaPakStr = "Historia zmian";
       public const string ProjektNazwaElemString = "Projekt-Nazwa";
       public const string ArchitekturaTransmisyjnaElemString = "Architektura Transmisyjna";
       public const string PMTipsElemString = "PM - wskazówki";
       public const string TestyElemString = "Testy - wskazówki";
       public const string TestyAutomatElemString = "Testy - automatyzacja";
       public const string TestyWydajnoscElemString = "Testy - wydajnościowe";

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


       Package RootPckg;

      // String SDit;
     //  String SDnt;
       String NazwaProjektuPelna;

       public Package HLDPckg;
       public Package CPPckg;
       public Package StatystykiPckg;
       public Package AktorzyPckg;
       public Package DefinicjePckg;
       public Package OgraniczeniaPckg;
       public Package SlownikPckg;
       public Package ZaleznosciPckg;
       public Package ZalacznikiPckg;
       public Package HistoriaPckg;
       public Element ProjektNazwaElem;
       public Element ArchitekturaTransmisyjnaElem;
       public Element PMTipsElem;
       public Element TestyElem;
       public Element TestyElemAutomat;
       public Element TestyElemWydajnosc;

       public Package[] ObszarPckg = new Package[2];
       public Package[] ArchStatPckg = new Package[2];
       public Package[] DiagrSekwPckg = new Package[2];
       public Package[] KoncepcjaPckg = new Package[2];
       public Element[] KoncepcjaElem = new Element[2];
       public Element[] MigracjaElem = new Element[2];
       public Element[] SkrotElem = new Element[2];
       public Package[] PrzypadkiPckg = new Package[2];
       public Package[] WkladyPckg = new Package[2];
       public Package[] ArchitekturaDanychPckg = new Package[2];
       public Package[] ArchitekturaDanychLDMPckg = new Package[2];
       public Package[] ArchitekturaDanychIDMPckg = new Package[2];

       public String[] SDNazwaStr = new String[2];

       public Package BiznesowePrzypadkiPckg;
       public Package WymaganiaPckg;

      public List <Element> WymaganiaBiznesoweLista=new List<Element>();
      public  List <Element> WymaganiaArchitektoniczneLista=new List<Element>();
      public  List <Element> WymaganiaInfrastrukturaLista=new List<Element>();

       /// <summary>
       /// Lista kwestii otwartych w projekcie
       /// </summary>
      public List<Element> ListaIssue = new List<Element>();

      public EA.Project projektInterfejs;
      public EA.Repository Repozytorium;

       public CModel(ref Repository Repo)
       {
           Repozytorium = Repo;
           projektInterfejs = Repo.GetProjectInterface();
          RootPckg=EAUtils.dajModelPR(ref Repozytorium);
           odczytajNaprawModel(ref  RootPckg);
       }
       public void szukajIssueSQL()
       {
           foreach (EA.Element element in Repozytorium.GetElementSet("select Object_ID " +
            "from t_object where object_type='Issue'", 2))
           {
               ListaIssue.Add(element);
           }
       }

       public void szukajIssue(Package p)
       {
           foreach (Package pp in p.Packages)
           {
               szukajIssue(pp);
           }
           foreach (Element e in p.Elements)
           {
               if (e.Type == "Issue")
               {
                   ListaIssue.Add(e);
               }
           }
       }
       /// <summary>
       /// rekurencyjnie wyszukuje wymagań w pakiecie d
       /// </summary>
       /// <param name="d">pakiet startowy</param>
       public void wyszukajWymaganiaPakietu(Package d)
       {
           foreach (Package dd in d.Packages)
           {
               wyszukajWymaganiaPakietu(dd);
           }
           foreach (Element e in d.Elements)
           {
               if (e.Type != "Requirement") continue;
               Element ee = e;
               dodajWymaganieDoListy(ref ee);
           }
       }
       public void wyszukajWymaganiaSQL()
       {
           foreach (EA.Element element in Repozytorium.GetElementSet("select Object_ID " +
                     "from t_object where object_type='Requirement'", 2))
           {
               Element ee =element;
               dodajWymaganieDoListy(ref ee);
           }
       }
       /// <summary>
       /// Dodaje wymaganie do odpowiedniej listy wymagań
       /// </summary>
       /// <param name="rq"></param>
       public bool dodajWymaganieDoListy(ref Element rq)
       {
               
                if(CmodelKonfigurator.czyZawiera( rq.Stereotype, CmodelKonfigurator.stereotypyWymaganArchitektonicznych))
                {
                       WymaganiaArchitektoniczneLista.Add(rq);
                        return true;   
                }
                if (CmodelKonfigurator.czyZawiera(rq.Stereotype, CmodelKonfigurator.stereotypyWymaganInfrastruktura))
                {
                    WymaganiaInfrastrukturaLista.Add(rq);
                    return true;  
                }
           ////uznajemy, że każde wymaganie które nie jest architektoniczne ani na infrastrukture jest biznesowe
           // bo każdy oznacza to requriementy jak chce i robi sie burdel
                if (!CmodelKonfigurator.czyZawiera(rq.Status, CmodelKonfigurator.statusyBRqAnulowane))
                {
                    WymaganiaBiznesoweLista.Add(rq);
                    return true;
                }
               return false;
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
           return RootPckg.Name;
       }
       public void odswiezModel()
       {
           odczytajNaprawModel(ref RootPckg);
       }
       
       public void odczytajNaprawModel(ref Package r)
       {
           RootPckg = r;
           CPPckg = EAUtils.utworzPakietGdyBrak(ref RootPckg, CmodelKonfigurator.CPpakStr, "");
           HLDPckg = EAUtils.utworzPakietGdyBrak(ref RootPckg, CmodelKonfigurator.HLDpakStr, "");
           StatystykiPckg=EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.StatystykiPakStr, "");
           AktorzyPckg = EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.AktorzyPakStr, "");
           DefinicjePckg = EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.DefinicjePakStr, "");
                   OgraniczeniaPckg = EAUtils.utworzPakietGdyBrak(ref DefinicjePckg, CmodelKonfigurator.OgraniczeniaPakStr, "");
                   HistoriaPckg = EAUtils.utworzPakietGdyBrak(ref DefinicjePckg, CmodelKonfigurator.HistoriaPakStr, "");
                   SlownikPckg = EAUtils.utworzPakietGdyBrak(ref DefinicjePckg, CmodelKonfigurator.SlownikPakStr, "");
                   ZaleznosciPckg = EAUtils.utworzPakietGdyBrak(ref DefinicjePckg, CmodelKonfigurator.ZaleznosciPakStr, "");
                   ZalacznikiPckg = EAUtils.utworzPakietGdyBrak(ref DefinicjePckg, CmodelKonfigurator.ZalacznikiPakStr, "");
                   ProjektNazwaElem = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.ProjektNazwaElemString);
                    if (ProjektNazwaElem.Notes == "")
                    {
                        ProjektNazwaElem.Notes = "Tu wpisz pełną nazwę projektu.";
                        ProjektNazwaElem.Update();
                    }

                    ArchitekturaTransmisyjnaElem = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.ArchitekturaTransmisyjnaElemString);
                    PMTipsElem = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.PMTipsElemString);
                    if (PMTipsElem.Notes == "")
                    {
                        PMTipsElem.Notes = "Brak wskazówek dla Project Managera.";
                        PMTipsElem.Update();
                    }
                    TestyElem = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.TestyElemString);
                    if (TestyElem.Notes == "")
                    {
                        TestyElem.Notes = "Brak wskazówek do przeprowadzania testów.";
                        TestyElem.Update();
                    }
                    TestyElemAutomat = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.TestyAutomatElemString);
                    if (TestyElemAutomat.Notes == "")
                    {
                        TestyElemAutomat.Notes = "Brak informacji o automatyzacji testów.";
                        TestyElemAutomat.Update();
                    }
                    TestyElemWydajnosc = EAUtils.dajElementLubGoZrob(ref DefinicjePckg, CmodelKonfigurator.TestyWydajnoscElemString);
                    if (TestyElemWydajnosc.Notes == "")
                    {
                        TestyElemWydajnosc.Notes = "Brak informacji o testach wydajnościowych.";
                        TestyElemWydajnosc.Update();
                    }
                    for (int i = 0; i < 2; i++)//(obszarEnum[])Enum.GetValues(typeof(int)))
                    {
                        ObszarPckg[i] = EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.obszarPakStr[i], "");
                        ArchStatPckg[i] = EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i], CmodelKonfigurator.ArchStatPakStr, "");
                        DiagrSekwPckg[i] = EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i], CmodelKonfigurator.DiagSekwPakStr, "");
                        KoncepcjaPckg[i] = EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i], CmodelKonfigurator.KoncepcjaPakStr, "");
                        KoncepcjaElem[i] = EAUtils.dajElementLubGoZrob(ref KoncepcjaPckg[i], CmodelKonfigurator.KoncepcjaElemStr);
                        MigracjaElem[i] = EAUtils.dajElementLubGoZrob(ref KoncepcjaPckg[i], CmodelKonfigurator.MigracjaElemStr);
                        SkrotElem[i] = EAUtils.dajElementLubGoZrob(ref KoncepcjaPckg[i], CmodelKonfigurator.SkrotElemStr);
                        PrzypadkiPckg[i] = EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i], CmodelKonfigurator.PrzypadkiTechnUzyciaPakStr, "");
                        WkladyPckg[i] = EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i], CmodelKonfigurator.WkladyPakStr, "");
                        ArchitekturaDanychPckg[i]=EAUtils.utworzPakietGdyBrak(ref ObszarPckg[i],CmodelKonfigurator.ArchitekturaDanychPakStr,"");
                        ArchitekturaDanychLDMPckg[i] = EAUtils.utworzPakietGdyBrak(ref ArchitekturaDanychPckg[i], CmodelKonfigurator.ArchitekturaDanychLDMPakStr, "");
                        ArchitekturaDanychIDMPckg[i] = EAUtils.utworzPakietGdyBrak(ref ArchitekturaDanychPckg[i], CmodelKonfigurator.ArchitekturaDanychIDMPakStr, "");
                        

                    }
             BiznesowePrzypadkiPckg = EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.PrzypadkiBizPakStr, "");
             WymaganiaPckg = EAUtils.utworzPakietGdyBrak(ref HLDPckg, CmodelKonfigurator.WymaganiaPakStr, "");

            SDNazwaStr[CModel.IT]= EAUtils.dajAutoraProjektu(ref RootPckg, "SD IT");
            SDNazwaStr[CModel.NT] = EAUtils.dajAutoraProjektu(ref RootPckg, "SD NT");
             NazwaProjektuPelna = EAUtils.dajNazweProjektu(ref HLDPckg);

           /*
             szukajIssue(HLDPckg);
             wyszukajWymaganiaPakietu(WymaganiaPckg);
             wyszukajWymaganiaPakietu(CPPckg);
            * */
             szukajIssueSQL();
             wyszukajWymaganiaSQL();
       }

 
   }
}
