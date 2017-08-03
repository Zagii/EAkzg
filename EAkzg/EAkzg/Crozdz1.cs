using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;
using Wordy=Microsoft.Office.Interop.Word;
using System.Diagnostics;

public class Part //: IEquatable<Part>
{
    public string cDate { get; set; }
    public string vHLD { get; set; }
    public string Notes { get; set; }
    public string Author { get; set; }
    public string pHLD { get; set; }
}

namespace EAkzg
{
    class Crozdz1 : CrozdzialUtils
    {
        string[,] spis =new string[,] {{"1 ORGANIZACYJNE","r1abc","spis1"},//0
                             //{"1.1 Zawartość, cel i przeznaczenie dokumentu","r1a1","spis1-1"},//1 // usunięcie rozdziału "Zawartość, cel i przeznaczenie dokumentu"
                             {"1.1 Wskazówki dla Project Managera","r1a1","spis1-1"},//1
                             {"1.2 Historia zmian","r1a2","spis1-1"},//2
                             {"1.3 Słownik użytych skrótów i pojęć","r1a3","spis1-1"},//2
                             {"1.4 Załączniki, powiązane dokumenty","r1a4","spis1-1"},//3
                             {"1.5 Zespół projektowy","r1a5","spis1-1"},
                             {"1.6 Powiązania z innymi projektami","r1a6","spis1-1"}
        };//4
        string[,] spisEN = new string[,] {{"1 INTRODUCTION","r1abc","spis1"},//0
                             //{"1.1 Terms of reference","r1a1","spis1-1"},//1 // usunięcie rozdziału "Zawartość, cel i przeznaczenie dokumentu"
                             {"1.1 Tips for Project Manager","r1a1","spis1-1"},//1
                             {"1.2 Document change history","r1a2","spis1-1"},//2
                             {"1.3 Dictionary","r1a3","spis1-1"},//3
                             {"1.4 Includes","r1a4","spis1-1"},//4
                             {"1.5 Project team","r1a5","spis1-1"},
                             {"1.6 Relation with other projects","r1a6","spis1-1"}
        };//4
        CModel modelProjektu;

        Package projekt;
        Package definicjePckg;
        Package ITPckg;
        Package NTPckg;
        EA.Repository rep;
        Word word;
        Statystyki okno;
        bool jezykPolski;
        public Crozdz1(CModel modelProj, String sciezkaZrodlo, String sciezkaDocelowa, Word W, Statystyki o, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            word = W;
            modelProjektu = modelProj;
           
            okno = o;
            
            jezykPolski = jezykPl;

        }
        public Crozdz1( EA.Repository r,EA.Package p, String sciezkaZrodlo,String sciezkaDocelowa,Word W,Statystyki o,bool jezykPl) : base(sciezkaZrodlo,sciezkaDocelowa)
        {
            word = W;
            rep = r;
            projekt = p;
            okno = o;
            okno.Log(Statystyki.LogMsgType.Info, "Lokalizacja pakietów");
            definicjePckg = EAUtils.dajPakietSciezkiP(ref projekt, "Definicje");
            ITPckg = EAUtils.dajPakietSciezkiP(ref projekt, "IT","Wkłady Systemowe");
            NTPckg = EAUtils.dajPakietSciezkiP(ref projekt, "NT", "Wkłady Systemowe");
            okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            jezykPolski = jezykPl;
            
        }
        public String dajSpisTresci()
        {
           return  base.dajSpisTresci(spis);
        }
        private String dajStatycznyTekst(ref int nrRozdz)
        {
            okno.Log(Statystyki.LogMsgType.Info, "Statyczny tekst");
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("2", ref nrRozdz,"Rozdzial1a",false);
         
            w += doklejPlik("rozdzial1.kzg");
       
            w += "</div>";
            okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            return w;
        }
        /** już nie używane  nowy generator */
        private string dajPMWskazowki(ref int nrRozdz)
        {
            okno.Log(Statystyki.LogMsgType.Info, "Dodanie wskazówek dla Project Managera");
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("2", ref nrRozdz, "Rozdzial1a");

            word.wstawParagraf(modelProjektu.PMTipsElem.Notes, 0);
            word.wstawZalacznikRTF(modelProjektu.PMTipsElem);

            w += "</div>";
            okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            return w;
         }
        private string dajSlownikStare(Package d, ref int wiersz)
        {
            okno.Log(Statystyki.LogMsgType.Info, "Eksport słownika");
            String wynik = "";
            wynik += "<div class=\"img\">";
            wynik+=dajTytulRozdz("2", ref wiersz,"Rozdzial1b");
            Wordy.Table tab;
            if (jezykPolski)
            {
                tab = word.wstawTabele("", new string[] { "Lp", "Skrót/pojęcie", "Opis" });
            }
            else {
                tab = word.wstawTabele("", new string[] { "No", "Term", "Description" });
            }
            tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
            wynik+="<table><tr><th>Lp</th><th>Skrót/pojęcie</th><th>Opis</th></tr>";
            int i=1;
            foreach(Element e in d.Elements)
            {
                wynik += "<tr";
                if (i % 2 == 0)
                {
                    wynik+=" class=\"parz\"";
                }
                wynik += "><td>" + i + "</td><td>" + e.Name + "</td><td>";
                string opis = "";
                ///// zmiana
               /* foreach (TaggedValue t in e.TaggedValues)
                {
                    if (t.Name == "Opis") 
                        opis= t.Value;
                }*/
                opis = e.Notes; ////<---- zmiana względem mojego modelu zamiast tagi notatka
                ////koniec zmiany
                wynik += opis+"</td></tr>";
                word.wstawWierszDoTabeli("", tab, i + 1, new string[] { i.ToString(), e.Name, opis });
                i++;
            }
         //   word.wstawKoniecTabeli("Rozdzial1b");
            wynik += "</table></div>";
            okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: "+d.Elements.Count+" [ok]\n");
            return wynik;
        }

        private string dajSlownik( ref int wiersz)
        {
            okno.Log(Statystyki.LogMsgType.Info, "Eksport słownika");
            String wynik = "";
            wynik += "<div class=\"img\">";
            wynik += dajTytulRozdz("2", ref wiersz, "Rozdzial1b");
            Wordy.Table tab;
            if (jezykPolski)
            {
                tab = word.wstawTabele("", new string[] { "Lp", "Skrót/pojęcie", "Opis" });
            }
            else
            {
                tab = word.wstawTabele("", new string[] { "No", "Term", "Description" });
            }
            tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
            wynik += "<table><tr><th>Lp</th><th>Skrót/pojęcie</th><th>Opis</th></tr>";
            int i = 1;
            foreach (Element e in modelProjektu.SlownikPckg.Elements)
            {
                wynik += "<tr";
                if (i % 2 == 0)
                {
                    wynik += " class=\"parz\"";
                }
                wynik += "><td>" + i + "</td><td>" + e.Name + "</td><td>";
                string opis = "";
               
                opis = e.Notes; 
               
                wynik += opis + "</td></tr>";
                word.wstawWierszDoTabeli("", tab, i + 1, new string[] { i.ToString(), e.Name, opis });
                i++;
            }
            wynik += "</table></div>";
            okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + modelProjektu.SlownikPckg.Elements.Count + " [ok]\n");
            return wynik;
        }
      
        private string dajZalaczniki(/*Package d,*/ref int wiersz)
        {
            okno.Log(Statystyki.LogMsgType.Info, "Eksport załączników");
            String wynik = "";
            wynik += "<div class=\"img\">";
            wynik += dajTytulRozdz("2", ref wiersz,"Rozdzial1b");
           
           /* nowy generator
            * if(d.Elements.Count>0) */
            if(modelProjektu.ZalacznikiPckg.Elements.Count>0)
            {
                Wordy.Table tab;
                if (jezykPolski)
                {
                    tab = word.wstawTabele("Rozdzial1b", new string[] { "Lp", "Nazwa/Opis", "Autor", "Dokument" });
                }
                else 
                {
                    tab = word.wstawTabele("Rozdzial1b", new string[] { "No", "Name/Descr", "Author", "Document" });
                }
                tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                tab.Columns[2].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                tab.Columns[3].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                tab.Columns[4].SetWidth(250f, Wordy.WdRulerStyle.wdAdjustNone);
                wynik += "<table><tr><th>Lp</th><th>Nazwa/Opis</th><th>Autor</th><th>Dokument</th></tr>";
                int i = 1;
                /* 
                 * nowy generator
                 * */
                //foreach (Element e in d.Elements)
                foreach(Element e in modelProjektu.ZalacznikiPckg.Elements)
                {
                    wynik += "<tr";
                         if (i % 2 == 0)
                    {
                        wynik+=" class=\"parz\"";
                    }
                     wynik+=" ><td>" + i + "</td><td>" + e.Name + "</td><td>"+e.Author+"</td><td>";
                     String p = "";
                    
                    p = e.Notes;
                   
                    wynik +=p+ "</td></tr>";
                    word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { i.ToString(), e.Name, e.Author, p });
                    i++;
                }
                
                
                wynik += "</table>";
            }
            else
            {
                wynik += "<P>Brak</P>";
                if (jezykPolski)
                {
                    word.wstawParagraf("Brak", 0);
                }
                else {
                    word.wstawParagraf("None", 0);
                }
            }
            /** nowy generator
             * */
          

            okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + modelProjektu.ZalacznikiPckg.Elements.Count + " [ok]\n");
            wynik+="</div>";
            return wynik;
        }

        /* nowy generator */
    private String   dajZespol(/*Package sl,Package aITPckg,Package aNTPckg,*/ ref int nrRozdz)
    {
        okno.Log(Statystyki.LogMsgType.Info, "Eksport zespołu projektowego");
        String w="";
        w += "<div class=\"img\">";
        w += dajTytulRozdz("2", ref nrRozdz,"Rozdzial1b");
        w += "<table><tr><th>Lp</th><th>Rola / Obszar </th><th>Imię i Nazwisko</th><th>Status</th></tr>";
        Wordy.Table tab;
        if (jezykPolski)
        {
            tab = word.wstawTabele("Rozdzial1b", new string[] { "Lp", "Rola / Obszar", "Imię i Nazwisko" });
        }
        else
        {
            tab = word.wstawTabele("Rozdzial1b", new string[] { "No", "Role", "Name" });
        
        }
        tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
        
        int i = 1;
        foreach (Element e in modelProjektu.SlownikPckg.Elements /* nowy generator sl.Elements*/)
        {
            String imie = "";
           
            foreach (TaggedValue t in e.TaggedValues)
            {
                /* nowy generator
                if (t.Name == "Imię i Nazwisko")
                 * */
                if (t.Name == CmodelKonfigurator.SlownikNazwiskoTagValue)
                {
                    imie = t.Value;
                }
              
            }
            if (imie != "")
            {
                w += "<tr";
                 if (i % 2 == 0)
                {
                    w+=" class=\"parz\"";
                }
                w+="><td>" + i + "</td><td>" + e.Name + "</td><td>" + imie + "</td><td>  </td></tr>";
                word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { i.ToString(), e.Name, imie });
                i++;
            }
           
            
        }

       w+= dajZespolzPakietu(ref i /* nowy generator ,aITPckg*/, CModel.IT,ref tab);
       w+= dajZespolzPakietu(ref i /*nowy generaotr ,aNTPckg*/,CModel.NT,ref tab);


       okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + i + " [ok]\n");  
        
        w += "</table></div>";

        return w;
    }
    private String dajZespolzPakietu(ref int i /* nowy generator,Package aPckg*/,int obszar,ref  Wordy.Table tab)
    {
        String w = "";
        foreach (Package sysPak in /* nowy generator aPckg.Packages*/ modelProjektu.WkladyPckg[obszar].Packages)
        {
            Package pTmp = sysPak;
            String  nazwisko = EAUtils.dajNazwiskoTA(modelProjektu.Repozytorium /*rep*/, sysPak);
            
            
            word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { i.ToString(), sysPak.Name,nazwisko  });
            i++;
        }
            
        return w;
    }
    private string dajZaleznosci(/* nowy generator Package d,*/ ref int wiersz)
    {
        okno.Log(Statystyki.LogMsgType.Info, "Eksport zespołu projektowego");
        String wynik = "";
        wynik += "<div class=\"img\">";
        wynik += dajTytulRozdz("2", ref wiersz,"Rozdzial1b");
        /* nowy generator if(d.Elements.Count>0) */
        if(modelProjektu.ZaleznosciPckg.Elements.Count>0)
            {
        wynik += "<table><tr><th>Lp</th><th>Projekt</th><th>Termin wdrozenia</th><th>Rodzaj zaleznosci</th><th>Opis</th></tr>";
        Wordy.Table tab;
        if (jezykPolski)
        {
            tab = word.wstawTabele("Rozdzial1b", new string[] { "Lp", "Projekt", "Termin wdrozenia", "Rodzaj zaleznosci", "Opis" });
        }
        else
        {
            tab = word.wstawTabele("Rozdzial1b", new string[] { "No", "Project", "Release date", "Dependency type", "Description" });
        }
                 tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[2].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[3].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[4].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
        tab.Columns[5].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
         int i = 1;
        foreach (Element e in /* nowy generator d.Elements*/ modelProjektu.ZaleznosciPckg.Elements)
        {
            wynik += "<tr";
            if (i % 2 == 0)
            {
                wynik += " class=\"parz\"";
            }
            wynik += "><td>" + i + "</td><td>" + e.Name + "</td><td>";
            String opis = "";
            String termin = "";
            String rodzaj = "";
            foreach (TaggedValue t in e.TaggedValues)
            {
                if (t.Name == "Opis") opis += t.Value;
                if (t.Name == "Krytycznosc") rodzaj += t.Value;
                if (t.Name == "Termin") termin += t.Value;
            }
            wynik += termin + "</td><td>" + rodzaj + "</td><td>" + opis;
            wynik += "</td></tr>";
            word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { i.ToString(), e.Name, termin, rodzaj, opis });
            i++;
        }
    
        wynik += "</table>";
         }
         else
         {
             wynik += "Brak";
             word.wstawParagraf("Brak", 0);
         }
        wynik+="</div>";
        okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + /* nowy generator d.Elements.Count*/ modelProjektu.ZaleznosciPckg.Elements.Count + " [ok]\n");
        return wynik;
    }

    private string dajHistorie(/*Package d,*/ref int wiersz)
    {
        okno.Log(Statystyki.LogMsgType.Info, "Eksport historii zmian");
        String wynik = "";
        wynik += "<div class=\"img\">";
        wynik += dajTytulRozdz("2", ref wiersz, "Rozdzial1b");

        /* nowy generator
         * if(d.Elements.Count>0) */
        if (modelProjektu.HistoriaPckg.Elements.Count > 0)
        {
            Wordy.Table tab;
            if (jezykPolski)
            {
                tab = word.wstawTabele("Rozdzial1b", new string[] { "Lp", "Data zmiany", "Wersja HLD", "Opis zmiany", "Autor", "Część HLD" });
            }
            else                
            {
                tab = word.wstawTabele("Rozdzial1b", new string[] { "No", "Revision Date", "HLD Version", "Revision Description", "Author", "HLD part" });
            }
            tab.Columns[1].SetWidth(25f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[2].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[3].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[4].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[5].SetWidth(80f, Wordy.WdRulerStyle.wdAdjustNone);
            tab.Columns[6].SetWidth(70f, Wordy.WdRulerStyle.wdAdjustNone);
            wynik += "<table><tr><th>Lp</th><th>Data zmiany</th><th>Wersja HLD</th><th>Opis zmiany</th><th>Autor</th><th>Część HLD</th></tr>";
            int i = 0;
            /* 
             * nowy generator
             * */
            List<Part> sortList = new List<Part>();
            String evHLD = "";
            String epHLD = "";
            String ecDate = "";
           
            foreach (Element elem in modelProjektu.HistoriaPckg.Elements) {
                evHLD = "";
                epHLD = "";
                ecDate = "";
                foreach (TaggedValue t in elem.TaggedValues)
                {
                    if (t.Name == "Wersja HLD") { evHLD = t.Value; }
                    else if (t.Name == "Część HLD") { epHLD = t.Value; }
                    else if (t.Name == "Date") { ecDate = t.Value; }
                }
                sortList.Add(
                    new Part() { 
                        vHLD = evHLD,
                        pHLD = epHLD,
                        cDate = ecDate,
                        Author = elem.Author,
                        Notes = elem.Notes
                    } 
                );
            }
            
            i = 1;
            
            foreach (Part ep in sortList.OrderBy(a=>a.vHLD))
            {
                wynik += "<tr";
                if (i % 2 == 0)
                {
                    wynik += " class=\"parz\"";
                }
                wynik += " ><td>" + i + "</td><td>" + ep.cDate + "</td><td>" + ep.vHLD + "</td><td>" + ep.Notes + "</td><td>" + ep.Author + "</td><td>" + ep.pHLD + "</td></tr>";
                word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { i.ToString(), ep.cDate, ep.vHLD, ep.Notes, ep.Author, ep.pHLD });
                i++;
            }
            
            wynik += "</table>";
        }
        else
        {
            wynik += "<P>Brak</P>";
            if (jezykPolski)
            {
                word.wstawParagraf("Brak", 0);
            }
            else
            {
                word.wstawParagraf("None", 0);
            }
        }
        /** nowy generator
         * */


        okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + modelProjektu.HistoriaPckg.Elements.Count + " [ok]\n");
        wynik += "</div>";
        return wynik;
    }

        private String dajTytulRozdz(String h, ref int nrRozdz,String znacznik,bool druk=true)
        {
            String w= dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);

            if (druk)
            {
                if (jezykPolski)
                {
                    word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
                else
                {
                    word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
            }
                //wstawTytulRozdzialu(znacznik, spis[nrRozdz, (int)poziom.TRESC],h );
            nrRozdz++;
            return w;
        }
        private String dajTytulRozdz1(String h, ref int nrRozdz, String znacznik)
        {
            String w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);
            if (jezykPolski)
            {
                word.dodajRozdzialNaKoncu(spis[nrRozdz, (int)poziom.TRESC], h);
            }
            else
            {
                word.dodajRozdzialNaKoncu(spisEN[nrRozdz, (int)poziom.TRESC], h);
            }
            nrRozdz++;
            return w;
        }
        public String dajRozdzial()
        {
        int nrRozdz = 0;
            String w = "<div id=\"Rozdzial1\">";
            Stopwatch st = new Stopwatch();
            st.Start();
            
            w += dajTytulRozdz("1",ref nrRozdz,"Rozdzial1",false);
            okno.Log(Statystyki.LogMsgType.Info, "R1 tytuł- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            // usunięcie rozdziału "Zawartość, cel i przeznaczenie dokumentu"
            //w += dajStatycznyTekst(ref nrRozdz);
            //okno.Log(Statystyki.LogMsgType.Info, "R1 txt- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            //st.Restart();
            w += dajPMWskazowki(ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 PM wskazówki- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += dajHistorie(/*sl,*/ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 historia- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += dajSlownik(/*sl,*/ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 słownik- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += dajZalaczniki(/*papiery,*/ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 załączniki- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += dajZespol(/*sl, ITPckg,NTPckg,*/ ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 zespół- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += dajZaleznosci(/*zal,*/ ref nrRozdz);
            okno.Log(Statystyki.LogMsgType.Info, "R1 zależności- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
            st.Restart();
            w += "</div>";

            return w;
        }
       
    }
}
