using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;
using Wordy = Microsoft.Office.Interop.Word;
using System.Diagnostics;

namespace EAkzg
{
    class Crozdz4: CrozdzialUtils
    {
        string[,] spis =new string[,] {{" ZMIANY W SYSTEMACH ","r","spis1"},//0
                            {" Realizacja ścieżką ","r","spis1-1"},
                             {" System ","r","spis1-1"},//1
                             {".1 Koncepcja systemowa","r","spis1-1"},//2
                             {".2 Diagram systemo-centryczny ","r","spis1-1"},//3
                              {".3 Wymagania systemowe","r","spis1-1"},//2
                             {".4 Udostępniane interfejsy (Realization)","r","spis1-1"},
                             {".5 Wykorzystywane interfejsy (Use)","r","spis1-1"}
        };//4
        string[,] spisEN = new string[,] {{" SYSTEM CHANGES ","r","spis1"},//0
                            {" Path realisation ","r","spis1-1"},
                             {" System ","r","spis1-1"},//1
                             {".1 System concept","r","spis1-1"},//2
                             {".2 System-centric diagram ","r","spis1-1"},//3
                              {".3 System requirements","r","spis1-1"},//2
                             {".4 Realized interfaces","r","spis1-1"},
                             {".5 Used interfaces","r","spis1-1"}
        };//4
        String NrRozdzialu;
        String numer;
        Package pakietPckg;
        Repository Repo;
        Package wkladyPckg;
        Word word;
        Statystyki okno;
        CModel modelProjektu;
        int Obszar;
        bool jezykPolski;
        public Crozdz4(EA.Repository r, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu,Word W,Statystyki o,bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski = jezykPl;
            okno = o;
            word = W;
            NrRozdzialu = nrRozdzialu;
            Repo = r;
            Package model = EAUtils.dajModelPR(ref Repo);//.Models.GetAt(0);
            pakietPckg = EAUtils.utworzPakietGdyBrak(ref model, NrRozdzialu, "");
            wkladyPckg = EAUtils.utworzPakietGdyBrak(ref pakietPckg, "Wkłady Systemowe", "");
            numer = "";
            if (NrRozdzialu == "IT")
            {
                numer += "5";
            }
            else
            { numer += "6"; }
        }
        public Crozdz4(CModel modelProj,int obszar, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu, Word W, Statystyki o, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski = jezykPl;
            okno = o;
            word = W;
            NrRozdzialu = nrRozdzialu;
            modelProjektu = modelProj;
            Obszar = obszar;
           // Repo = r;
           // Package model = EAUtils.dajModelPR(ref Repo);//.Models.GetAt(0);
          //  pakietPckg = EAUtils.utworzPakietGdyBrak(ref model, NrRozdzialu, "");
         //   wkladyPckg = EAUtils.utworzPakietGdyBrak(ref pakietPckg, "Wkłady Systemowe", "");
            numer = "";
            if (CModel.IT==Obszar)
            {
                numer += "7";
            }
            else
            { numer += "8"; }
        }

        public String dajSpisTresci()
        {
          
            String w = base.dajLinijkeSpisuTresci(spis[0,  (int)poziom.ID]+numer,spis[0,  (int)poziom.CSS],numer+spis[0,  (int)poziom.TRESC]+NrRozdzialu);
            int i = 1;
            foreach (Package p in /* kzg nowy model wkladyPckg.Packages*/ modelProjektu.WkladyPckg[Obszar].Packages)
            {
                w += base.dajLinijkeSpisuTresci(spis[1, (int)poziom.ID] + numer+"-"+i, spis[1, (int)poziom.CSS], numer + "."+i+" System " +p.Name+"("+")");
                i++;
            }
            return w;
            // return base.dajSpisTresci(spis);
        }
        private String dajTytulRozdz(String h, ref int nrRozdz, String sufix="",String ID="",String prefix="")
        {
            String w = "";
            if(ID=="")ID=spis[nrRozdz, (int)poziom.ID];

            w += dajNaglowek(h, ID, numer+prefix+spis[nrRozdz, (int)poziom.TRESC]+sufix);

            if (jezykPolski)
            {
                word.wstawParagraf(numer + prefix + spis[nrRozdz, (int)poziom.TRESC] + sufix, Int16.Parse(h));
            }
            else
            {
                word.wstawParagraf(numer + prefix + spisEN[nrRozdz, (int)poziom.TRESC] + sufix, Int16.Parse(h));
            }
            nrRozdz++;
            return w;
        }
        private String dajKoncepcje(Package k, ref int nrRozdz,int lp)
        {
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("3", ref nrRozdz,"","r"+numer+"-"+lp+"-1","."+lp);


            foreach (Element e in k.Elements)
            {
                if (e.Name == "Koncepcja Systemowa")
                {
                     
                   // word.wstawParagraf(e.Notes, 0);
                    word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium,e);
                    word.wstawZalacznikRTF(e);

       

                }
            }
            w += "</div>\n";
            return w;
        }
        private String dajWymagania(Package pakiet, ref int nrRozdz, int lp)
        {
          
            String w = "<div class=\"img\">";

            w += dajTytulRozdz("3", ref nrRozdz, "", "r" + numer + "-" + lp + "-2", "." + lp);
            if (jezykPolski)
            {
                w += dajWymaganiaSyst_nowyModel(pakiet, ref nrRozdz, lp, "1", " Wymagania funkcjonalne.");
                if (pakiet.Name != "Fasttrack")
                {
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychInfrastrukturalne, "2", " Wymagania na Infrastrukturę.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychBezpieczeństwa, "3", " Wymagania bezpieczeństwa.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychPojemnosc, "4", " Wpływ na pojemność systemu.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychDostepnosc, "5", " Wpływ na dostępność systemu.");
                }
            }
            else
            {
                w += dajWymaganiaSyst_nowyModel(pakiet, ref nrRozdz, lp, "1", " Functional requirements.");
                if (pakiet.Name != "Fasttrack")
                {
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychInfrastrukturalne, "2", " Infrastructural requirements.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychBezpieczeństwa, "3", " Security requirements.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychPojemnosc, "4", " Capacity Management.");
                    w += dajWymaganiaXXX_nowyModel(pakiet, ref nrRozdz, lp, CmodelKonfigurator.stereotypyFeatureSystemowychDostepnosc, "5", " Avaiability Management.");
                }
            }
            w += "\n</div>\n";
            return w;
        }
     /*   private String dajWymaganiaXXXStaraPrezentacja(Package wymPckg,String warunekIF,String warunekIF2)
        {
            String w = "";
            Wordy.Table tab = word.wstawTabele("Rozdzial1b", new string[] { "Nazwa", "Opis realizacji", "Nadrzędne wygmaganie biznesowe" });
            tab.Columns[1].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            w += "<table><tr><th>Nazwa</th><th>Opis realizacji</th><th>Nadrzędne wygmaganie biznesowe</th></tr>\n";
            int i = 1;
            foreach (Element elem in wymPckg.Elements)
            {
                String typ = "konfiguracja/development";
                String nadrzedne = "";
                if (elem.Stereotype != warunekIF && elem.Stereotype != warunekIF2) continue;
                foreach (Connector con in elem.Connectors)
                {

                    if (con.Type == "Realisation")
                    {
                        Element e2 = Repo.GetElementByID(con.SupplierID);
                        if (e2.Type == "Requirement")
                        {
                            nadrzedne += e2.Name + "<br>\n";
                        }
                    }
                }
                w += "<tr><td>" + elem.Name + "</td><td>" + elem.Notes + "</td><td>" + nadrzedne + "</td></tr>\n";
                word.wstawWierszDoTabeli("", tab, i + 1, new string[] { elem.Name, elem.Notes, nadrzedne });
                i++;
            }
            return w;
        }*/
        private String dajWymaganiaXXXNowaPrezentacja3(Package wymPckg, String [] stereotypy,bool systemowe=false)
        {
            String w = "";

            w += "<table><tr><th>Nazwa</th><th>Opis realizacji</th><th>Nadrzędne wygmaganie biznesowe</th></tr>\n";
            Stopwatch st = new Stopwatch();
            st.Start();
            foreach (Element elem in wymPckg.Elements)
            {
              st.Restart();
                //  String typ = "konfiguracja/development";
                String nadrzedne = "";
                String stat = "";
                bool analiza = false;
             //   Element e2 = null;
               /// bo ficzery systemowe to pozostałe
                if (systemowe)
                {
                    if (CmodelKonfigurator.czyZawiera(elem.Stereotype, stereotypy))
                        continue;
                }
                else
                {
                    if (!CmodelKonfigurator.czyZawiera(elem.Stereotype, stereotypy))
                        continue;
                }
                okno.Log(Statystyki.LogMsgType.Info, "--Wymaganie: " + elem.Name + "\n ");
                okno.Log(Statystyki.LogMsgType.cd, "----- czy=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                String sql = "select ore.object_id from t_object ore,t_object ofi, t_connector c where ofi.object_id="+elem.ElementID +" and " +
                       "((c.start_object_id=ofi.object_id and c.end_object_id=ore.object_id) or " +
                       "(c.start_object_id=ore.object_id and c.end_object_id=ofi.object_id)) and " +
                       " ore.object_type='Requirement' and c.connector_type='Realisation'";
                int ile_req = 0;
                foreach (Element e2 in modelProjektu.Repozytorium.GetElementSet(sql, 2))
                {
                    ile_req++;
                    nadrzedne += e2.Name + "\n";
                    stat += e2.Status + "\n";

                    if (!CmodelKonfigurator.czyZawiera(e2.Status, CmodelKonfigurator.statusyBRqGotowe))
                    {
                        analiza = true;
                    }
                }
                okno.Log(Statystyki.LogMsgType.cd, ", req=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                /* przejscie na sql
                foreach (Connector con in elem.Connectors)
                {

                    if (con.Type == "Realisation")
                    {
                        e2 = modelProjektu.Repozytorium.GetElementByID(con.SupplierID);
                   
                        if (e2.Type == "Requirement")
                        {
                            nadrzedne += e2.Name + "\n";
                            stat += e2.Status + "\n";
                           
                            if (!CmodelKonfigurator.czyZawiera(e2.Status, CmodelKonfigurator.statusyBRqGotowe))
                            {
                                analiza = true;
                            }
                        }
                    }
                }
                 * */
                Wordy.Paragraph paragr = word.wstawParagraf(elem.Name, 0);
                okno.Log(Statystyki.LogMsgType.cd, ", name=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                paragr = paragr.Previous();
                paragr.Range.Bold = 1;
                paragr.Range.Underline = Wordy.WdUnderline.wdUnderlineThick;
                paragr = word.wstawParagraf("Status: " + elem.Status, 0);
                paragr = paragr.Previous();
                Wordy.Range r = paragr.Range;
                r.End = r.Start + 7;
                r.Bold = 1;

                okno.Log(Statystyki.LogMsgType.cd, ", status=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));

                //if (elem.Status != "Uzgodnione" && elem.Status != "Anulowane przez IT" && elem.Status != "Anulowane przez BO")
                if(!CmodelKonfigurator.czyZawiera(elem.Status,CmodelKonfigurator.statusyFeatureGotowe))
                {
                    paragr.Range.Comments.Add(paragr.Range, CmodelKonfigurator.worning["SRQanalysis"]);
                }
                if (jezykPolski)
                {
                    paragr = word.wstawParagraf("Nadrzędne wymaganie biznesowe:", 0);
                }
                else
                { paragr = word.wstawParagraf("Parent business requirement:", 0); }
                paragr = paragr.Previous();
                paragr.Range.Bold = 1;
                okno.Log(Statystyki.LogMsgType.cd, ", nadrzedne=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));

                if (ile_req==0)
                {
                    if (jezykPolski)
                    {
                        paragr = word.wstawParagraf("Brak", 0);
                    }
                    else
                    { paragr = word.wstawParagraf("None", 0); }
                }
                else
                {
                    word.wstawParagraf(nadrzedne, 0);
                
                    paragr = paragr.Previous();
                    paragr = paragr.Previous();
                    r = paragr.Range;
                    r.End = r.Start + 29;
                    r.Select();
                    r.Bold = 1;
                    if (analiza)
                    {
                        paragr.Range.Comments.Add(paragr.Range, CmodelKonfigurator.worning["BRQ_SRQanalysis"]);
                    }
                }
                okno.Log(Statystyki.LogMsgType.cd, " nadrzTresc=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                if (jezykPolski)
                {
                    paragr = word.wstawParagraf("Szczegóły:", 0);
                }
                else
                {
                    paragr = word.wstawParagraf("Details:", 0);
                }
                paragr = paragr.Previous();
                paragr.Range.Bold = 1;

          //      word.wstawParagraf(elem.Notes, 0);
                word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, elem);
                // word.wstawWierszDoTabeli("", tab, i, new string[] { elem.Notes});
                //tab.Cell(i, 1).Merge(tab.Cell(i, 2));
                word.wstawZalacznikRTF(elem);
                okno.Log(Statystyki.LogMsgType.cd, " opis=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                if (elem.Notes == "" && elem.GetLinkedDocument() == "")
                {
                    word.wstawParagraf("Wymaganie w trakcie analizy...", 0, "Brak opisu realizacji wymagania");
                }
                word.wstawParagraf("", 0);
                okno.Log(Statystyki.LogMsgType.cd, " koniec=" + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff")+"\n");

            }
            return w;
        }
   /*     private String __dajWymaganiaXXXNowaPrezentacja2(Package wymPckg, String warunekIF, String warunekIF2)
        {
            String w = "";

            w += "<table><tr><th>Nazwa</th><th>Opis realizacji</th><th>Nadrzędne wygmaganie biznesowe</th></tr>\n";

            foreach (Element elem in wymPckg.Elements)
            {
                //  String typ = "konfiguracja/development";
                String nadrzedne = "";
                String stat = "";
                bool analiza = false;
                Element e2 = null;
                if (elem.Stereotype != warunekIF && elem.Stereotype != warunekIF2) continue;
                foreach (Connector con in elem.Connectors)
                {

                    if (con.Type == "Realisation")
                    {
                        e2 = Repo.GetElementByID(con.SupplierID);
                        if (e2.Type == "Requirement")
                        {
                            nadrzedne += e2.Name + "\n";
                            stat += e2.Status + "\n";
                            if (e2.Status != "Uzgodnione" && e2.Status != "Anulowane przez IT" && e2.Status != "Anulowane przez BO")
                            {
                                analiza = true;
                            }
                        }
                    }
                }
               Wordy.Paragraph paragr= word.wstawParagraf(elem.Name,0);
               paragr = paragr.Previous();
               paragr.Range.Bold = 1;
               paragr.Range.Underline = Wordy.WdUnderline.wdUnderlineThick;
               paragr = word.wstawParagraf("Status: " + elem.Status,0);
               paragr = paragr.Previous();
               Wordy.Range r = paragr.Range;
               r.End = r.Start + 7;
               r.Bold = 1;
               
        

                if (elem.Status != "Uzgodnione" && elem.Status != "Anulowane przez IT" && elem.Status != "Anulowane przez BO")
                {
                   paragr.Range.Comments.Add(paragr.Range, "Wymaganie w trakcie analizy");
                }
                if (jezykPolski)
                {
                    paragr = word.wstawParagraf("Nadrzędne wymaganie biznesowe:", 0);
                }
                else
                { paragr = word.wstawParagraf("Parent business requirement:", 0); }
                paragr = paragr.Previous();
                paragr.Range.Bold = 1;
                
              
                if (e2 == null)
                {
                    if (jezykPolski)
                    {
                        paragr = word.wstawParagraf("Brak", 0);
                    }
                    else
                    { paragr = word.wstawParagraf("None", 0); }
                }
                else
                {
                     word.wstawParagraf( nadrzedne,0);
                     if (jezykPolski)
                     {
                         paragr = word.wstawParagraf("Status nadrzędnego wymagania: " + stat, 0);
                     }
                     else
                     {
                         paragr = word.wstawParagraf("Status of parent business requirement: " + stat, 0);
                     }
                    paragr = paragr.Previous();
                    paragr = paragr.Previous();
                    r = paragr.Range;
                    r.End = r.Start + 29;
                    r.Select();
                    r.Bold = 1;
                    if (analiza)
                    {
                        paragr.Range.Comments.Add(paragr.Range, "Wymaganie w trakcie analizy");
                    }
                }
                if (jezykPolski)
                {
                    paragr = word.wstawParagraf("Sposób realizacji:", 0);
                }
                else
                {
                    paragr = word.wstawParagraf("Realization description:", 0);
                }
                paragr = paragr.Previous();
                paragr.Range.Bold = 1;
              
                word.wstawParagraf(elem.Notes, 0);
                // word.wstawWierszDoTabeli("", tab, i, new string[] { elem.Notes});
                //tab.Cell(i, 1).Merge(tab.Cell(i, 2));
                word.wstawZalacznikRTF(elem);
                if (elem.Notes == "" && elem.GetLinkedDocument() == "")
                {
                    word.wstawParagraf("Wymaganie w trakcie analizy...", 0, "Brak opisu realizacji wymagania");
                }
                word.wstawParagraf("", 0);
                

            }
            return w;
        }
    * */
    /*    private String dajWymaganiaXXXNowaPrezentacja(Package wymPckg, String warunekIF, String warunekIF2)
        {
            String w = "";
       //     Wordy.Table tab = word.wstawTabele("Rozdzial1b", new string[] { "Nazwa", "Opis realizacji", "Nadrzędne wygmaganie biznesowe" });
       //     tab.Columns[1].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
        //    tab.Columns[2].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
        //    tab.Columns[3].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            w += "<table><tr><th>Nazwa</th><th>Opis realizacji</th><th>Nadrzędne wygmaganie biznesowe</th></tr>\n";
           
            foreach (Element elem in wymPckg.Elements)
            {
              //  String typ = "konfiguracja/development";
                String nadrzedne = "";
                String stat = "";
                bool analiza = false;
                Element e2 = null;
                if (elem.Stereotype != warunekIF && elem.Stereotype != warunekIF2) continue;
                foreach (Connector con in elem.Connectors)
                {

                    if (con.Type == "Realisation")
                    {
                        e2 = Repo.GetElementByID(con.SupplierID);
                        if (e2.Type == "Requirement")
                        {
                            nadrzedne += e2.Name + "\n";
                            stat += e2.Status + "\n";
                            if (e2.Status != "Uzgodnione" && e2.Status != "Anulowane przez IT" && e2.Status != "Anulowane przez BO")
                            {
                                analiza = true;
                            }
                        }
                    }
                }
                Wordy.WdColor kolor1 = Wordy.WdColor.wdColorBlack;
                Wordy.WdColor kolor2 = Wordy.WdColor.wdColorGray25;
                int i = 1; 
            //    w += "<tr><td>" + elem.Name + "</td><td>" + elem.Notes + "</td><td>" + nadrzedne + "</td></tr>\n";
                     Wordy.Table tab = word.wstawTabele("", new string[] {elem.Name, elem.Status });
                     tab.Columns[1].SetWidth(350f, Wordy.WdRulerStyle.wdAdjustNone);
                     tab.Rows[i].Cells[1].Shading.BackgroundPatternColor = kolor1;
                     tab.Rows[i].Cells[2].Shading.BackgroundPatternColor = kolor1;
                    tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
                   

               
                if (elem.Status != "Uzgodnione" && elem.Status != "Anulowane przez IT" && elem.Status != "Anulowane przez BO")
                {
                    tab.Cell(i, 2).Range.Comments.Add(tab.Cell(i, 2).Range, "Wymaganie w trakcie analizy");
                }
                i++;
                word.wstawWierszDoTabeli("", tab, i, new string[] { "Nadrzędne wymaganie biznesowe", "Status nadrzędnego wymagania" });
                tab.Rows[i].Cells[1].Shading.BackgroundPatternColor = kolor2;
                tab.Rows[i].Cells[2].Shading.BackgroundPatternColor = kolor2;
                i++;
                if (e2 == null)
                {
                    word.wstawWierszDoTabeli("", tab, i, new string[] { "Brak", "Brak" });
                }
                else
                {
                    word.wstawWierszDoTabeli("", tab, i, new string[] { nadrzedne, stat });
                    if (analiza)
                    {
                        tab.Cell(i, 2).Range.Comments.Add(tab.Cell(i, 2).Range, "Wymaganie w trakcie analizy");
                    }
                }
                tab.Rows[i].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                tab.Rows[i].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                i++;
                word.wstawWierszDoTabeli("", tab, i, new string[] { "Sposób realizacji","" },false);
                tab.Cell(i, 1).Merge(tab.Cell(i, 2));
                tab.Rows[i].Cells[1].Shading.BackgroundPatternColor = kolor2;
                i++;
                word.wstawParagraf(elem.Notes, 0);
               // word.wstawWierszDoTabeli("", tab, i, new string[] { elem.Notes});
                //tab.Cell(i, 1).Merge(tab.Cell(i, 2));
                word.wstawZalacznikRTF(elem);
                if (elem.Notes == "" && elem.GetLinkedDocument() == "")
                {
                      word.wstawParagraf("Wymaganie w trakcie analizy...", 0,"Brak opisu realizacji wymagania");
                }
                word.wstawParagraf("", 0);
                i++;
             
            }
            return w;
        }*/
       /// <summary>
        /// Zrzut feature systemowych, ktore nie maja statusów obsługiwanych w innych przypadkach
        /// </summary>
        /// <param name="pakiet">Pakiet wkładu systemowego</param>
        /// <param name="nrRozdz">numer rozdziału</param>
        /// <param name="lp">LP</param>
        /// <param name="stereotypy">Lista stereotypów do filtrowania</param>
        /// <param name="kolejnosc">Numer podrozdziału</param>
        /// <param name="opis">Opis podrozdziału</param>
        /// <returns></returns>
        private String dajWymaganiaSyst_nowyModel(Package pakiet, ref int nrRozdz, int lp, String kolejnosc, String opis)
        {
            String w = "";
            Package wymPckg = EAUtils.utworzPakietGdyBrak(ref pakiet, "Wymagania Systemowe", "");

            w += "<h3>" + numer + "." + lp + ".3." + kolejnosc + opis + " </h3>\n";

            word.wstawParagraf(numer + "." + lp + ".3." + kolejnosc + opis, 4);

            String[] stereotypy = CmodelKonfigurator.stereotypyFeatureSystemowychBezpieczeństwa;
            stereotypy = stereotypy.Concat(CmodelKonfigurator.stereotypyFeatureSystemowychDostepnosc).ToArray();

            stereotypy = stereotypy.Concat(CmodelKonfigurator.stereotypyFeatureSystemowychInfrastrukturalne).ToArray();
            stereotypy = stereotypy.Concat(CmodelKonfigurator.stereotypyFeatureSystemowychPojemnosc).ToArray();

            int licznik = 0;
            foreach (Element e in wymPckg.Elements)
            {
                if (!CmodelKonfigurator.czyZawiera(e.Stereotype,stereotypy) ) 
                { licznik++; }
            }
            if (licznik == 0)
            {
                w += "Brak";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", word.stylNorm);
                }
                else
                {
                    word.wstawParagraf("None", word.stylNorm);
                }
                return w;
            }

            w += dajWymaganiaXXXNowaPrezentacja3(wymPckg, stereotypy,true);

            return w + "</table>\n";
        }

        /// <summary>
        /// Zrzut feature
        /// </summary>
        /// <param name="pakiet">Pakiet wkładu systemowego</param>
        /// <param name="nrRozdz">numer rozdziału</param>
        /// <param name="lp">LP</param>
        /// <param name="stereotypy">Lista stereotypów do filtrowania</param>
        /// <param name="kolejnosc">Numer podrozdziału</param>
        /// <param name="opis">Opis podrozdziału</param>
        /// <returns></returns>
        private String dajWymaganiaXXX_nowyModel(Package pakiet, ref int nrRozdz,int lp,String [] stereotypy,String kolejnosc,String opis)
        {
            String w = "";
            Package wymPckg = EAUtils.utworzPakietGdyBrak(ref pakiet, "Wymagania Systemowe", "");

            w += "<h3>"+numer+"."+lp+".3."+kolejnosc+opis+" </h3>\n";
           // if (jezykPolski)
           // {
                word.wstawParagraf(numer + "." + lp + ".3." + kolejnosc +opis, 4);
           /* }
            else
            {
                word.wstawParagraf(numer + "." + lp + ".3." + kolejnosc + " Requirements " + jakie, 4);
            }*/
            int licznik = 0;
            foreach (Element e in wymPckg.Elements)
            {
               // if (e.Stereotype == warunekIF || e.Stereotype == warunekIF2) licznik++;
                if(CmodelKonfigurator.czyZawiera(e.Stereotype,stereotypy))licznik++;
            }
            if (licznik==0)
            {
                w += "Brak";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", word.stylNorm);
                }
                else
                {
                    word.wstawParagraf("None", word.stylNorm);
                }
                return w;
            }
         //   w+=dajWymaganiaXXXStaraPrezentacja(wymPckg,warunekIF,warunekIF2);
            //w += dajWymaganiaXXXNowaPrezentacja(wymPckg, warunekIF, warunekIF2);
            w += dajWymaganiaXXXNowaPrezentacja3(wymPckg, stereotypy);
          
            return w+"</table>\n";
        }
        /* do usuniecia
        private String __dajWymaganiaXXX(Package pakiet, ref int nrRozdz, int lp,String warunekIF, String warunekIF2,String kolejnosc,String jakie)
        {
            String w = "";
            Package wymPckg = EAUtils.utworzPakietGdyBrak(ref pakiet, "Wymagania Systemowe", "");

            w += "<h3>"+numer+"."+lp+".3."+kolejnosc+" Wymagania "+jakie+" </h3>\n";
            if (jezykPolski)
            {
                word.wstawParagraf(numer + "." + lp + ".3." + kolejnosc + " Wymagania " + jakie, 4);
            }
            else
            {
                word.wstawParagraf(numer + "." + lp + ".3." + kolejnosc + " Requirements " + jakie, 4);
            }
            int licznik = 0;
            foreach (Element e in wymPckg.Elements)
            {
                if (e.Stereotype == warunekIF || e.Stereotype == warunekIF2) licznik++;
            }
            if (licznik==0)
            {
                w += "Brak";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", word.stylNorm);
                }
                else
                {
                    word.wstawParagraf("None", word.stylNorm);
                }
                return w;
            }
         //   w+=dajWymaganiaXXXStaraPrezentacja(wymPckg,warunekIF,warunekIF2);
            //w += dajWymaganiaXXXNowaPrezentacja(wymPckg, warunekIF, warunekIF2);
            w += dajWymaganiaXXXNowaPrezentacja2(wymPckg, warunekIF, warunekIF2);
          
            return w+"</table>\n";
        }*/
        private String dajDiagramSystemocentryczny(Package pakiet,ref int nrRozdz,int lp)
        {
            String w="";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("3", ref nrRozdz, "", "r" + numer + "-" + lp + "-3", "." + lp);
              foreach (Diagram diag in pakiet.Diagrams)
                    {
                        if (diag.Stereotype == CmodelKonfigurator.ukryjDiagramStr) continue;
                        w += "<div class=\"img\">";
                        Diagram d = diag;
                  /* kzg nowe poczatek
                     //   String plik = EAUtils.zapiszDiagramJakoObrazStare(ref Repo, ref d, dajSciezkeDocelowa());
                        
                   * */
                  String plik = EAUtils.zapiszDiagramJakoObraz(modelProjektu, ref d, dajSciezkeDocelowa());
                  //kzg nowe koniec   
                        w += "<img src='" + plik + "'>\n";
                        word.wstawObrazek(dajSciezkeDocelowa() + plik);
                        if (jezykPolski)
                        {
                            word.wstawParagraf("Diagram systemo-centryczny " + diag.Notes, word.stylPodpis);
                        }
                        else
                        {
                            word.wstawParagraf("System-centric diagram " + diag.Notes, word.stylPodpis);
                        }
                        modelProjektu.Repozytorium.CloseDiagram(diag.DiagramID);
                        w += "<div class=\"desc\">Diagram systemo-centryczny " + diag.Notes + "</div>";
              }
            return w+"</div>\n";
        }
        private String dajInterfejsyRealizacja(Package pakiet, ref int nrRozdz, int lp, String typ,String nr)
        {
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("3", ref nrRozdz, "", "r" + numer + "-" + lp + "-" + nr, "." + lp);
            Package pakietInterfejs=EAUtils.utworzPakietGdyBrak(ref pakiet,"Realizowane interfejsy","");
            int licznik=0;
             foreach (Element e1 in pakietInterfejs.Elements)
            {

                licznik += e1.Methods.Count;
                /*foreach (Method m1 in e1.Methods)
                {
                    licznik++;
                }*/
             }
            if(licznik==0)
            {
                w+="\nBrak\n<BR>";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", word.stylNorm);
                }
                else { word.wstawParagraf("None", word.stylNorm); }

                return w+"</div>";
            }
            w += "<table><tr><th>Nazwa interfejsu</th><th>Nazwa operacji</th><th>Opis</th></tr>\n";

            Wordy.Table tab;
            if (jezykPolski)
            {
                tab = word.wstawTabele("", new string[] { "Nazwa interfejsu", "Nazwa operacji", "Opis" });
            }
            else
            {
                tab = word.wstawTabele("", new string[] { "Interface Name", "Operation name", "Description" });
            }
            tab.Columns[1].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(250f, Wordy.WdRulerStyle.wdAdjustNone);
            
            int i = 1;
            foreach (Element e in pakietInterfejs.Elements)
            {
                
                foreach (Method m in e.Methods)
                {
                    w += "<tr><td>" + e.Name + "</td><td>";
                    w += m.Name + "</td><td>" + m.Notes + "</td></tr>";
                    word.wstawWierszDoTabeli("", tab, i + 1, new string[] { e.Name, m.Name,m.Notes });
                    i++;  
                }
                
            }

            return w+"</table></div>\n";
        }
        private String dajInterfejsyUsage(Package pakiet, ref int nrRozdz, int lp, String typ, String nr)
        {
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("3", ref nrRozdz, "", "r" + numer + "-" + lp + "-" + nr, "." + lp);
            Element systemElement = EAUtils.dajComponentSystemZpakietu(modelProjektu.Repozytorium, pakiet);
         /*   foreach (Connector con in pakiet.Connectors)
            {
                if (con.Type == "Realisation")
                {
                    //systemElement = Repo.GetElementByID(con.SupplierID);
                    systemElement = modelProjektu.Repozytorium.GetElementByID(con.SupplierID);
                }
            }*/
            int licznik = 0;
            if (systemElement != null)
            {
                 
                String sql="select oi.object_id from t_object oi,t_object os, t_connector c where os.object_id="+systemElement.ElementID+
                    " and  ((c.start_object_id=os.object_id and c.end_object_id=oi.object_id) or "+
                    "(c.start_object_id=oi.object_id and c.end_object_id=os.object_id)) and oi.object_type='Interface'"+
                    "and connector_type='Usage'";
                foreach (Element interfejsyUsage in modelProjektu.Repozytorium.GetElementSet(sql, 2))
                {
                    licznik += interfejsyUsage.Methods.Count;
                }

            /*    foreach (Connector c in systemElement.Connectors)
                {
                    if (c.Type != "Usage") continue;
                    Element client = modelProjektu.Repozytorium.GetElementByID(c.ClientID);//Repo.GetElementByID(c.ClientID);
                    Element interfejs = modelProjektu.Repozytorium.GetElementByID(c.SupplierID);// Repo.GetElementByID(c.SupplierID);
                    if (interfejs.Type != "Interface") continue;
                    Element realizator = null;
                    foreach (Connector cs in interfejs.Connectors)
                    {
                        if (cs.Type == "Realisation")
                        {
                            realizator = modelProjektu.Repozytorium.GetElementByID(cs.ClientID);//Repo.GetElementByID(cs.ClientID);
                        }
                    }

                    licznik += interfejs.Methods.Count;
                 //   foreach (Method m in interfejs.Methods)
                   // {
                     //   licznik++;
                    //}
                } */
            }
            if (licznik == 0)
            {
                w += "\nBrak\n<BR>";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", word.stylNorm);
                }
                else
                {
                    word.wstawParagraf("None", word.stylNorm);
                }
                return w + "</div>";
            }

            Wordy.Table tab;
            if (jezykPolski)
            {
                tab = word.wstawTabele("", new string[] { "Nazwa interfejsu", "Ralizator", "Nazwa operacji" });
            }
            else
            {
                tab = word.wstawTabele("", new string[] { "Interface name", "Realize", "Operation name" });
            }
            tab.Columns[1].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(250f, Wordy.WdRulerStyle.wdAdjustNone);

            w += "<table><tr><th>Nazwa interfejsu</th><th>Ralizator</th><th>Nazwa operacji</th></tr>\n";
            int i = 1;
            if (systemElement != null)
            {
                  
                String sql="select oi.object_id from t_object oi,t_object os, t_connector c where os.object_id="+systemElement.ElementID+
                    " and  ((c.start_object_id=os.object_id and c.end_object_id=oi.object_id) or "+
                    "(c.start_object_id=oi.object_id and c.end_object_id=os.object_id)) and oi.object_type='Interface'"+
                    "and connector_type='Usage'";
                foreach (Element interfejsyUsage in modelProjektu.Repozytorium.GetElementSet(sql, 2))
                {
                    licznik += interfejsyUsage.Methods.Count;
                
                sql="select os.object_id from t_object oi,t_object os, t_connector c where oi.object_id="+interfejsyUsage.ElementID+
                    " and ((c.start_object_id=os.object_id and c.end_object_id=oi.object_id) "+
                    " or (c.start_object_id=oi.object_id and c.end_object_id=os.object_id)) and oi.object_type='Interface'"+
                    " and connector_type='Realisation'";
                      Element realizator=modelProjektu.Repozytorium.GetElementSet(sql, 2).GetAt(0); //powinien byc tylko jeden system
                     if (realizator == null)
                    {
                        okno.Log(Statystyki.LogMsgType.Error, "Błąd w modelu, interfejs " + interfejsyUsage.Name + " nie posiada właściciela");
                        
                    }
                    else
                    {
                        foreach (Method m in interfejsyUsage.Methods)
                        {
                            w += "<tr><td>" + interfejsyUsage.Name + "</td><td>" + realizator.Name + "</td><td>" + m.Name + "</td></tr>";
                            word.wstawWierszDoTabeli("", tab, i + 1, new string[] { interfejsyUsage.Name, realizator.Name, m.Name });
                            i++;
                        }
                    }
                }
                /*
                foreach (Connector c in systemElement.Connectors)
                {
                    if (c.Type != "Usage") continue;
                    Element client = modelProjektu.Repozytorium.GetElementByID(c.ClientID); //Repo.GetElementByID(c.ClientID);
                    Element interfejs = modelProjektu.Repozytorium.GetElementByID(c.SupplierID); //Repo.GetElementByID(c.SupplierID);
                    if (interfejs.Type != "Interface") continue;
                    Element realizator=null;
                    foreach (Connector cs in interfejs.Connectors)
                    {
                        if (cs.Type == "Realisation")
                        {
                            realizator = modelProjektu.Repozytorium.GetElementByID(cs.ClientID);//Repo.GetElementByID(cs.ClientID);
                        }
                    }
                    if (realizator == null)
                    {
                        okno.Log(Statystyki.LogMsgType.Error, "Błąd w modelu, interfejs " + interfejs.Name + " nie posiada właściciela");
                        
                    }
                    else
                    {
                        foreach (Method m in interfejs.Methods)
                        {
                            w += "<tr><td>" + interfejs.Name + "</td><td>" + realizator.Name + "</td><td>" + m.Name + "</td></tr>";
                            word.wstawWierszDoTabeli("", tab, i + 1, new string[] { interfejs.Name, realizator.Name, m.Name });
                            i++;
                        }
                    }

               }*/

            }
            
            return w + "</table></div>\n";
        }
        private String dajSystem(Package pakiet,ref int nrRozdz, int lp)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            String w="<div id=\"Wklad-"+pakiet.Name+"\">";
           // String w="<div id=\"r"+numer+"-"+lp+"\">";  

            bool ft = false;
            if (pakiet.Name == "Fasttrack") ft = true;
            String nazwisko = "brak podpisu";
            if (!jezykPolski) nazwisko = "N/A";
         
            nazwisko = EAUtils.dajNazwiskoTA(/*Repo*/modelProjektu.Repozytorium, pakiet);
            w += dajTytulRozdz("2", ref nrRozdz,pakiet.Name+" ("+nazwisko+")","r"+numer+"-"+lp,"."+lp);
            okno.Log(Statystyki.LogMsgType.Info, "R 4 System tytuł- "+pakiet.Name +" "+ st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            if (ft) nrRozdz++;
            w += dajKoncepcje(pakiet, ref nrRozdz,lp);
            okno.Log(Statystyki.LogMsgType.Info, "R 4 koncepcja- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            if (!ft)
            {
                w += dajDiagramSystemocentryczny(pakiet, ref nrRozdz, lp);
                okno.Log(Statystyki.LogMsgType.Info, "R 4 diagsyst- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
            }
            else
            {
                nrRozdz++;
            }
            w += dajWymagania(pakiet, ref nrRozdz, lp);
            okno.Log(Statystyki.LogMsgType.Info, "R 4 wymagania- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            if (!ft)
            {
                w += dajInterfejsyRealizacja(pakiet, ref nrRozdz, lp, "Realisation", "1");
                okno.Log(Statystyki.LogMsgType.Info, "R 4 interf realiz- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                w += dajInterfejsyUsage(pakiet, ref nrRozdz, lp, "Usage", "2");
                okno.Log(Statystyki.LogMsgType.Info, "R 4 interf usage- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
            }
            else
            {
                nrRozdz += 2;
            }

            st.Stop();
            return w+"</div>";
        }
        public String dajRozdzial()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            int nrRozdz = 0;
            String w = "<div id=\"Rozdzial" + NrRozdzialu + "\">";
            w += dajTytulRozdz("1", ref nrRozdz,NrRozdzialu,"r"+numer);
            int lp = 1;
            ////// przed wkladami IT damy rozdział ze zmianami fasttrack
            if (Obszar == CModel.IT)
            {

                String sql="select f.object_id from t_object f,t_object s, t_connector c where f.object_type='Feature' "+
                                "and s.object_type='Component' and ((c.start_object_id=f.object_id and c.end_object_id=s.object_id) or "+
                                "(c.start_object_id=s.object_id and c.end_object_id=f.object_id) ) and s.name='Fasttrack'";

                if (modelProjektu.Repozytorium.GetElementSet(sql, 2).Count > 0) //jeśli są wymagania do FT
                {

                    foreach (Package p in modelProjektu.WkladyPckg[Obszar].Packages)
                    {
                        if (p.Name != "Fasttrack") continue;
                        okno.Log(Statystyki.LogMsgType.Info, "-- R 4 Wkład dla- " + p.Name + "\n");

                        int tmp = nrRozdz;
                        w += dajSystem(p, ref tmp, lp);
                        lp++;

                        okno.Log(Statystyki.LogMsgType.Info, "-- R 4 Wkład dla- " + p.Name + " " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                        st.Restart();

                    }

                }
                else 
                {   //przeskocz do nast napisu
                   
                }
                
            }
            nrRozdz++;
           
            if (modelProjektu.WkladyPckg[Obszar].Packages.Count == 0)
            {
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", 0);
                }
                else
                {
                    word.wstawParagraf("None", 0);
                }
            }
            st.Restart();
            okno.Log(Statystyki.LogMsgType.Info, "R 4 tytuł- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
               
            //foreach (Package p in wkladyPckg.Packages)
            foreach(Package p in modelProjektu.WkladyPckg[Obszar].Packages)
            {
                if (p.Name == "Fasttrack") continue;
                okno.Log(Statystyki.LogMsgType.Info, "-- R 4 Wkład syst- " + p.Name + "\n");
                
                int tmp = nrRozdz;
                w += dajSystem(p,ref tmp,lp);
                lp++;

                okno.Log(Statystyki.LogMsgType.Info, "-- R 4 Wkład syst- " + p.Name + " "+st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
            }
            //w += dajKoncepcje(koncepcjaPckg, ref nrRozdz);
        
            w += "</div>";
            return w;
        }
    }
}
