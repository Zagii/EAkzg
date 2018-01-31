using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using EA;
using Wordy = Microsoft.Office.Interop.Word;

namespace EAkzg
{
        class Crozdz2 : CrozdzialUtils
        {
            string[,] spis = new string[,] {{"2 PERSPEKTYWA FUNKCJONALNA","r2abc","spis1"},//0
                             {"2.1 Krótki opis projektu z perspektywy biznesowej","r2a1","spis1-1"},//1
                             //{"2.1.1 Perspektywa IT","r2a1b1","spis1-1-1"},//1
                             //{"2.1.2 Perspektywa NT","r2a1b2","spis1-1-1"},//1
                              {"2.2 Ograniczenia rozwiązania","r2a2","spis1-1"},//2
                             {"2.3 Wymagania biznesowe","r2a3","spis1-1"},//2
                             {"2.4 Wymagania architektoniczne","r2a4","spis1-1"},//3
                              {"2.5 Kwestie otwarte","r2a5","spis1-1"}//3
                                };//4
            string[,] spisEN = new string[,] {{"2 FUNCTIONAL PERSPECTIVE","r2abc","spis1"},//0
                             {"2.1 Short description of the project - business view","r2a1","spis1-1"},//1
                             //{"2.1.1 IT perspective","r2a1b1","spis1-1-1"},//1
                             //{"2.1.2 NT perspective","r2a1b2","spis1-1-1"},//1
                              {"2.2 Solution constraints","r2a2","spis1-1"},//2
                             {"2.3 Business requirements","r2a3","spis1-1"},//2
                             {"2.4 Architection requirements","r2a4","spis1-1"},//3
                              {"2.5 Open issues","r2a5","spis1-1"}//3
                                };//4
            Package projekt;
            Package wymaganiaPckg;
            Package koncepcjaITPckg;
            Package koncepcjaNTPckg;
            EA.Repository rep;
            Word word;
            Statystyki okno;
            bool CzyPokazywacTrescWymagan;
            CModel modelProjektu;
            bool jezykPolski;
            public Crozdz2(EA.Repository r, EA.Package p, Package wymPckg,Package konPckg, String sciezkaZrodlo,String sciezkaDocelowa,Word W,Statystyki o,bool jezykPl,bool czyTresc)
                : base(sciezkaZrodlo,sciezkaDocelowa)
            {
                jezykPolski = jezykPl;
                word = W;
                rep = r;
                projekt = p;
                wymaganiaPckg = wymPckg;
                okno = o;
                okno.Log(Statystyki.LogMsgType.Info, "Lokalizacja pakietów");
                koncepcjaITPckg = EAUtils.dajPakietSciezkiP(ref projekt, "IT", "Koncepcja");
                koncepcjaNTPckg = EAUtils.dajPakietSciezkiP(ref projekt, "NT", "Koncepcja");
                okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
                CzyPokazywacTrescWymagan = czyTresc;
            }
            public Crozdz2(CModel modelProj, String sciezkaZrodlo, String sciezkaDocelowa, Word W, Statystyki o, bool jezykPl, bool czyTresc)
                : base(sciezkaZrodlo, sciezkaDocelowa)
            {
                jezykPolski = jezykPl;
                word = W;
                modelProjektu = modelProj;
                okno = o;
                CzyPokazywacTrescWymagan = czyTresc;
            }
            public String dajSpisTresci()
            {
                return base.dajSpisTresci(spis);
            }
          
            // nowy generator private String dajKoncepcje(Package k,ref int nrRozdz,bool duplikat)
            private String dajKoncepcje(int obszar, ref int nrRozdz,bool duplikat)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji skróconej");
                String w = "";
                w += "<div class=\"img\">";
                if (!duplikat) w += dajTytulRozdz("2", ref nrRozdz);
      
              //  word.wstawParagraf(modelProjektu.SkrotElem[obszar].Notes,0);
                word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, modelProjektu.SkrotElem[obszar]);
                word.wstawZalacznikRTF(modelProjektu.SkrotElem[obszar]);
                if (!duplikat) word.wstawParagraf("", 0);

                w += "</div>";
                okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
                return w;
            }
            private string dajOgraniczeniaRozwiazania(ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport ograniczeń rozwiązania");
                String w = "";
                w += "<div class=\"img\">";
                w += dajTytulRozdz("2", ref nrRozdz);
    
              if(modelProjektu.OgraniczeniaPckg.Elements.Count>0 || modelProjektu.ListaIssue.Count>0)
                {
                    Wordy.Table tab;
                    if (jezykPolski)
                    {
                        tab = word.wstawTabele("", new string[] { "Lp", "Ograniczenie rozwiązania", "Opis" });
                    }
                    else
                    {
                        tab = word.wstawTabele("", new string[] { "No", "Constraint", "Descritpion" });
                    }
                        tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
                    w += "<table><tr><th>Lp</th><th>Ograniczenie rozwiązania</th><th>Opis</th></tr>";
                    int i = 1;
                    foreach (Element e in modelProjektu.OgraniczeniaPckg.Elements)
                    {
                        if (e.Type == "Issue") continue;//omin issue bo potem osobno wszystkie issue ida do tabelki

                        w += "<tr";

                        w += "><td>" + i + "</td><td>" + e.Name + "</td><td>";
                        string opis = "";

                        opis = e.Notes; ////<---- zmiana względem mojego modelu zamiast tagi notatka
                        ////koniec zmiany
                        w += opis + "</td></tr>";
                        word.wstawWierszDoTabeli("", tab, i + 1, new string[] { i.ToString(), e.Name, opis });
                        i++;
                    }
                    foreach (Element e in modelProjektu.ListaIssue)
                    {
                        string opis = e.Notes;
                        word.wstawWierszDoTabeli("", tab, i + 1, new string[] { i.ToString(), e.Name, opis });
                        i++;
                    }
                }
                else 
                {
                    word.wstawParagraf("Brak", 0);
                }
                w += "</div>";
                okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + modelProjektu.OgraniczeniaPckg.Elements.Count + " [ok]\n");
                return w;
            }

           

            private string dajWymaganiaPakietu(Package d, ref int i,ref int ii,ref  Wordy.Table tab,String pakTxt,string[] typyWymagan)
            {
              //  return ""; /////////////////////////<<<<<<<<<<-------------------------- zeby szybciej sie generowalo
            
                String wynik="";
                pakTxt += d.Name;
       
                foreach(Package dd in d.Packages)
                {
                    wynik += dajWymaganiaPakietu(dd,ref i,ref  ii,ref tab,pakTxt+" -> ",typyWymagan);
                }
                foreach (Element e in d.Elements)
                {
                    if(e.Type!="Requirement")continue;
                    Element ee = e;
                    bool czyRobic =  modelProjektu.dodajWymaganieDoListy(ref ee);
                  
                    if (!czyRobic)
                    {
                     
                        okno.Log(Statystyki.LogMsgType.Error, "Błędny stereotyp wymagania: " + e.Name + " stereotyp: " + e.Stereotype);
                        continue;
                    }
                   
                    ///{"Typ","Pakiet", "Wymaganie","Status"}
                    word.wstawWierszDoTabeli("Rozdzial1b", tab, i + 1, new string[] { e.Stereotype,pakTxt, e.Name, e.Status });
                    
                    i++;
                
                    ii++;
                }
          
                return wynik;
            }
            private string dajWymaganiaPakietuArch(Package d, ref int i, ref int ii, String pakTxt, string[] typyWymagan)
            {
                //  return ""; /////////////////////////<<<<<<<<<<-------------------------- zeby szybciej sie generowalo
                String wynik = "";
                pakTxt += d.Name;
          
                foreach (Package dd in d.Packages)
                {
                    wynik += dajWymaganiaPakietuArch(dd, ref i, ref  ii, pakTxt + " -> ", typyWymagan);
                }
               
                foreach (Element e in d.Elements)
                {
                    if (e.Type != "Requirement") continue;
                    bool czyRobic = false;
                    foreach (string typWymagania in typyWymagan)
                    {
                        if (e.Stereotype == typWymagania)
                        {
                            czyRobic = true;
                            break;
                        }
                    }
                    if (!czyRobic) continue;

                 

                    Wordy.Table tab = null; 
                        tab= word.wstawTabele("", new string[] { e.Name, "", "", ""/*, "Treść", "Status","Właściciel"*/ });
                        tab.Columns[1].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[2].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[3].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[4].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                      
      
                    ///{"Typ","Pakiet", "Wymaganie","Status"}
                    int index = 1;
                    Wordy.WdColor kolor1 = Wordy.WdColor.wdColorBlack;
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 0;
                    index++;
                    if (jezykPolski)
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "", "Właściciel", "Typ", "Status" }, false);
                    }
                    else
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "", "Owner", "Type", "Status" }, false);
                    }
                        tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 1;
                    index++;
                    word.wstawWierszDoTabeli("", tab, i + index, new string[] { " ", e.Author, e.Stereotype, e.Status });
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 0;
                    index++;
                    /*  !!! to komentujemy bo nie mamy w modelu tresci wytycznych
                    word.wstawWierszDoTabeli("", tab, i + index, new string[] { "Treść Wytycznej", "","","" });
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                  //  tab.Rows[index].Cells[2].SetWidth(350f, Wordy.WdRulerStyle.wdAdjustNone);
                  //  tab.Rows[index].Cells[2].Range.Text = "To jest wytyczna architektoniczna";
                    
                    index++;
                     *  !!!! koniec */
                    if (jezykPolski)
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "Treść Wymagania", e.Notes });
                    }
                    else {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "Requrement", e.Notes });
                    
                    }
                        tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                    tab.Rows[index].Cells[2].SetWidth(350f, Wordy.WdRulerStyle.wdAdjustNone);
                   // tab.Rows[index].Cells[2].Range.Text = "To jest wymaganie architektoniczne";
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    word.wstawParagraf("", 0);
                    index++;
                    i++;
                    
                }
     
                return wynik;
            }

            private void wypiszWymagania(ref int wiersz, bool PokazTresc)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport wymagań");
                String wynik = "";
                wynik += "<div class=\"img\">";
                wynik += dajTytulRozdz("2", ref wiersz);
              
                int i = 2;
                if(PokazTresc)
                {
                    foreach (Element e in modelProjektu.WymaganiaBiznesoweLista)
                    {
                        Wordy.Table tab = null;
                        if (jezykPolski)
                        {
                            tab = word.wstawTabele("Rozdzial1b", new string[] { "Autor",/* "Pakiet",*/ "Wymaganie", "Status"/*, "Treść", "Status","Właściciel"*/ });
                        }
                        else
                        {
                            tab = word.wstawTabele("Rozdzial1b", new string[] { "Author", /*"Package",*/ "Requirement", "Status"/*, "Treść", "Status","Właściciel"*/ });

                        }
                        tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[2].SetWidth(370f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[3].SetWidth(80f, Wordy.WdRulerStyle.wdAdjustNone);
             
                        word.wstawWierszDoTabeli("Rozdzial1b", tab, i++, new string[] { e.Author,/* pakTxt,*/ e.Name, e.Status });

                        if (e.Notes.Length > 0)
                        {
                            word.wstawParagraf(" ", 5);
                            word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, e);
                        }
                        if (e.GetLinkedDocument().Length > 0)
                        {
                            word.wstawParagraf(" ", 5);
                            word.wstawZalacznikRTF(e);
                        }
                        word.wstawParagraf("", 5);
                    }
                    Wordy.Table tabI = null;
                    if (jezykPolski)
                    {
                        tabI = word.wstawTabele("Rozdzial1b", new string[] { "Autor",/* "Pakiet",*/ "Wymaganie", "Status"/*, "Treść", "Status","Właściciel"*/ });
                    }
                    else
                    {
                        tabI = word.wstawTabele("Rozdzial1b", new string[] { "Author", /*"Package",*/ "Requirement", "Status"/*, "Treść", "Status","Właściciel"*/ });

                    }
                    i = 2;
                    tabI.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                    tabI.Columns[2].SetWidth(370f, Wordy.WdRulerStyle.wdAdjustNone);
                    tabI.Columns[3].SetWidth(80f, Wordy.WdRulerStyle.wdAdjustNone);
                    foreach (Element e in modelProjektu.WymaganiaInfrastrukturaLista)
                    {
                        word.wstawWierszDoTabeli("Rozdzial1b", tabI, i++, new string[] { e.Author,/* pakTxt,*/ e.Name, e.Status });
                    }
                   // tab.SortAscending();
                }else
                {
                    Wordy.Table tab = null;
                        if (jezykPolski)
                    {
                        tab = word.wstawTabele("Rozdzial1b", new string[] { "Autor",/* "Pakiet",*/ "Wymaganie", "Status"/*, "Treść", "Status","Właściciel"*/ });
                    }
                    else
                    {
                        tab = word.wstawTabele("Rozdzial1b", new string[] { "Author", /*"Package",*/ "Requirement", "Status"/*, "Treść", "Status","Właściciel"*/ });

                    }

                    tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                 //   tab.Columns[2].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[2].SetWidth(370f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[3].SetWidth(80f, Wordy.WdRulerStyle.wdAdjustNone);

                   
                 //   int ii = 1;
                    foreach (Element e in modelProjektu.WymaganiaBiznesoweLista)
                    {
                        word.wstawWierszDoTabeli("Rozdzial1b", tab, i ++, new string[] { e.Author,/* pakTxt,*/ e.Name, e.Status });
                    }
            
                foreach (Element e in modelProjektu.WymaganiaInfrastrukturaLista)
                {
                    word.wstawWierszDoTabeli("Rozdzial1b", tab, i ++, new string[] { e.Author,/* pakTxt,*/ e.Name, e.Status });
                }
                tab.SortAscending();

                }
                okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: \n Biznesowych: " + modelProjektu.WymaganiaBiznesoweLista.Count +
                   "\n Infrastruktura: " + modelProjektu.WymaganiaInfrastrukturaLista.Count + " [ok]\n");
            }

            private void wypiszWymaganiaArch( ref int wiersz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport wymagań architektonicznych");
               
               dajTytulRozdz("2", ref wiersz);

                int i = 1;
             //   int ii = 1;
       //       dajWymaganiaPakietuArch(d, ref i, ref ii, "", new string[] { "Arch." });
               

               if(modelProjektu.WymaganiaArchitektoniczneLista.Count<=0)
                {
                    word.wstawParagraf("Brak", 0);
                }
               foreach (Element e in modelProjektu.WymaganiaArchitektoniczneLista)
               {
                      Wordy.Table tab = null; 
                        tab= word.wstawTabele("", new string[] { e.Name, "", "", ""/*, "Treść", "Status","Właściciel"*/ });
                        tab.Columns[1].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[2].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[3].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                        tab.Columns[4].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
                      
      
                    ///{"Typ","Pakiet", "Wymaganie","Status"}
                    int index = 1;
                    Wordy.WdColor kolor1 = Wordy.WdColor.wdColorBlack;
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 0;
                    index++;
                    if (jezykPolski)
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "", "Właściciel", "Typ", "Status" }, false);
                    }
                    else
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "", "Owner", "Type", "Status" }, false);
                    }
                        tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 1;
                    index++;
                    word.wstawWierszDoTabeli("", tab, i + index, new string[] { " ", e.Author, e.Stereotype, e.Status });
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[3].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[3].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[4].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[4].Range.Font.Bold = 0;
                    index++;
                   
                    if (jezykPolski)
                    {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "Treść Wymagania", e.Notes });
                    }
                    else {
                        word.wstawWierszDoTabeli("", tab, i + index, new string[] { "Requrement", e.Notes });
                    
                    }
                        tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                    tab.Rows[index].Cells[2].Merge(tab.Rows[index].Cells[3]);
                    tab.Rows[index].Cells[2].SetWidth(400f, Wordy.WdRulerStyle.wdAdjustNone);
                   
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    word.wstawParagraf("", 0);
                    index++;
                    i++;
                    
                
               }
                okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " +modelProjektu.WymaganiaArchitektoniczneLista.Count + " [ok]\n");
               
            }
            private string dajWymagania(/*Package d,*/ ref int wiersz)
            {
               // return "";
                okno.Log(Statystyki.LogMsgType.Info, "Eksport wymagań");
                String wynik = "";
                wynik += "<div class=\"img\">";
               wynik += dajTytulRozdz("2", ref wiersz);
               Wordy.Table tab;
               if (jezykPolski)
               {
                   tab = word.wstawTabele("Rozdzial1b", new string[] { "Typ", "Pakiet", "Wymaganie", "Status"/*, "Treść", "Status","Właściciel"*/ });
               }
               else
               {
                   tab = word.wstawTabele("Rozdzial1b", new string[] { "Type", "Package", "Requirement", "Status"/*, "Treść", "Status","Właściciel"*/ });
            
               }
                    
                    tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
               tab.Columns[2].SetWidth(100f, Wordy.WdRulerStyle.wdAdjustNone);
               tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
               tab.Columns[4].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
   
                wynik += "<table><tr><th>Lp</th><th>Pakiet</th><th>Wymaganie</th><th>Opis</th><th>Status</th><th>Właściciel</th></tr>";
                int i = 1;
                int ii = 1;
              /* nowy generator  wynik += dajWymaganiaPakietu(d, ref i, ref ii, ref tab, "", new string[] { "Biznesowe", "Procesowe", "Functional", "MUST", "NICE-TO-HAVE", "Business" });
               * */
           //    dajWymaganiaPakietu(modelProjektu.WymaganiaPckg, ref i, ref ii, ref tab, "", CmodelKonfigurator.stereotypyWymaganBiznesowych);
            //    dajWymaganiaPakietu(modelProjektu.CPPckg, ref i, ref ii, ref tab, "", CmodelKonfigurator.stereotypyWymaganBiznesowych);

                tab.SortAscending();
                wynik += "</table></div>";
                okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + (i - 1).ToString() + " [ok]\n");
                
                return wynik;
            }
            private string dajWymaganiaArch(Package d, ref int wiersz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport wymagań architektonicznych");
                String wynik = "";
                wynik += "<div class=\"img\">";
                wynik += dajTytulRozdz("2", ref wiersz);
              
                wynik += "<table><tr><th>Lp</th><th>Pakiet</th><th>Wymaganie</th><th>Opis</th><th>Status</th><th>Właściciel</th></tr>";
                int i = 1;
                int ii = 1;
                wynik += dajWymaganiaPakietuArch(d, ref i, ref ii,  "", new string[] { "Arch." });
               
                wynik += "</table></div>";
                if (i == 1)
                {
                    word.wstawParagraf("Brak", 0);
                }
                okno.Log(Statystyki.LogMsgType.WynikOK, " elementów: " + (i - 1).ToString() + " [ok]\n");
                return wynik;
            }
         
            private String dajTytulRozdz(String h, ref int nrRozdz)
            {
                String w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);

                if (jezykPolski)
                {
                    word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
                else
                {
                    word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
                nrRozdz++;
                return w;
            }
            public String dajRozdzial()
            {
                int nrRozdz = 0;
                String w = "<div id=\"Rozdzial2\">";
                Stopwatch st = new Stopwatch();
                st.Start();
                
                w += dajTytulRozdz("1", ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R2 tytuł- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                dajKoncepcje(CModel.IT, ref nrRozdz, false);
                okno.Log(Statystyki.LogMsgType.Info, "R2 koncepcja IT- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                
                dajKoncepcje(CModel.NT, ref nrRozdz, true);
                okno.Log(Statystyki.LogMsgType.Info, "R2 koncepcja NT- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                
                 w += dajOgraniczeniaRozwiazania(ref nrRozdz);
                 okno.Log(Statystyki.LogMsgType.Info, "R2 ograniczenia- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                 st.Restart();
                
                 wypiszWymagania(ref nrRozdz,CzyPokazywacTrescWymagan);
                 okno.Log(Statystyki.LogMsgType.Info, "R2 wymagania- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                 st.Restart();
                
                 wypiszWymaganiaArch(ref nrRozdz);
                 okno.Log(Statystyki.LogMsgType.Info, "R2 wymagania arch- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                 st.Stop();
                
                
                w += "</div>";
                return w;
            }

        }
    
}
