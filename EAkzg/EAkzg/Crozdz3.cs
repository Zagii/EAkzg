﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;
using Wordy = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace EAkzg
{
    class Crozdz3:CrozdzialUtils
    {
         string[,] spis = new string[,] {{"3 OPIS ROZWIĄZANIA IT","r3abc","spis1"},//0
                             {"3.1 Koncepcja rozwiązania","r3a1","spis1-1"},//1
                             {"3.2 Statyczna Architektura","r3a2","spis1-1"},//2
                              {"3.2.2 Opis roli systemu","r3a3b2","spis1-1-1"},
                           //  {"3.3 Architektura Danych","r3a3","spis1-1"},
                           //   {"3.2.1 Architektura Danych - ogólnie","r3a3b2","spis1-1-1"},
                            //  {"3.2.2 Opis modyfikowanych encji LDM","r3a3b2","spis1-1-1"},
                             {"3.3 Dynamiczna Architektura","r3a3","spis1-1"},
                               {"3.3.1 Biznesowe przypadki użycia","r3a3b1","spis1-1-1"},
                               {"3.3.2 Diagramy sekwencji","r3a3b2","spis1-1-1"}                               
                                };//4

         string[,] spis2 = new string[,] {{"4 OPIS ROZWIĄZANIA NT","r4abc","spis1"},//0
                             {"4.1 Koncepcja rozwiązania","r4a1","spis1-1"},//1
                             {"4.2 Statyczna Architektura","r4a2","spis1-1"},//2
                               {"4.2.1 Opis roli systemu","r4a3b2","spis1-1-1"},
                        //     {"4.3 Architektura Danych","r3a3","spis1-1"},
                         //     {"4.3.1 Architektura Danych - ogólnie","r3a3b2","spis1-1-1"},
                         //     {"4.3.2 Opis modyfikowanych encji LDM","r3a3b2","spis1-1-1"},
                             {"4.3 Dynamiczna Architektura","r4a3","spis1-1"},
                               {"4.3.1 Biznesowe przypadki użycia","r4a3b1","spis1-1-1"},
                                {"4.3.2 Diagramy sekwencji","r4a3b2","spis1-1-1"}
                                                               };//4

         string[,] spisEN = new string[,] {{"3 IT SOLUTION DESIGN","r3abc","spis1"},//0
                             {"3.1 Solution concept","r3a1","spis1-1"},//1
                             {"3.2 Static Architecture","r3a2","spis1-1"},//2
                             {"3.3.2 System change - abstract","r3a3b2","spis1-1-1"},
                       //      {"3.3 Architektura Danych","r3a3","spis1-1"},
                       //       {"3.2.1 Architektura Danych - ogólnie","r3a3b2","spis1-1-1"},
                      //        {"3.2.2 Opis modyfikowanych encji LDM","r3a3b2","spis1-1-1"},
                              {"3.3 Dynamic Architecture","r3a3","spis1-1"},
                               {"3.3.1 Business Use Case","r3a3b1","spis1-1-1"},
                                {"3.3.2 Sequence diagrams","r3a3b2","spis1-1-1"}
                                };//4

         string[,] spis2EN = new string[,] {{"4 NT SOLUTION DESIGN","r4abc","spis1"},//0
                             {"4.1 Solution concept","r4a1","spis1-1"},//1
                             {"4.2 Static Architecture","r4a2","spis1-1"},//2
                             {"4.3.2 System change - abstract","r4a3b2","spis1-1-1"},
                       //       {"4.3 Architektura Danych","r3a3","spis1-1"},
                      //        {"4.3.1 Architektura Danych - ogólnie","r3a3b2","spis1-1-1"},
                       //       {"4.3.2 Opis modyfikowanych encji LDM","r3a3b2","spis1-1-1"},
                              {"4.3 Dynamic Architecture","r4a3","spis1-1"},
                               {"4.3.1 Business Use Case","r4a3b1","spis1-1-1"},
                                {"4.3.2 Sequence diagrams","r4a3b2","spis1-1-1"}
                                };//4

           Package projekt;
            Package systemsPckg;
            Package koncepcjaPckg;
            Package usecasePckg;
            Package sekwencjePckg;
            Package wkladyPckg;
            EA.Repository rep;
            String NrRozdzialu;
            Word word;
            Statystyki okno;
            CModel modelProjektu;
            int Obszar;
            bool jezykPolski;
        public Crozdz3(EA.Repository r, EA.Package p, Package dzialPckg, String sciezkaZrodlo,String sciezkaDocelowa,String nrRozdzialu,Word W,Statystyki o,bool jezykPl)
                : base(sciezkaZrodlo,sciezkaDocelowa)
            {
                jezykPolski = jezykPl;
                word = W;
                rep = r;
                projekt = p;
                okno = o;
                okno.Log(Statystyki.LogMsgType.Info, "Lokalizacja pakietów");
                systemsPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Architektura Statyczna");
                koncepcjaPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Koncepcja");
                usecasePckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Przypadki Użycia");
                sekwencjePckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Diagramy Sekwencji");
                wkladyPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Wkłady Systemowe");
                NrRozdzialu=nrRozdzialu;
                okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            }
        public Crozdz3(CModel modelProj,int obszar, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu, Word W, Statystyki o, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            modelProjektu = modelProj;
            Obszar = obszar;
            jezykPolski = jezykPl;
            word = W;
            
            okno = o;
           
            
            NrRozdzialu = nrRozdzialu;
     
        }
            private String dajTytulRozdz(String h, ref int nrRozdz)
            {
                String w = "";
                if (NrRozdzialu == "3")
                {
                    w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);
                    if (jezykPolski)
                    {
                        word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                    }
                    else
                    {
                        word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                    }
                }
                else 
                {
                    w = dajNaglowek(h, spis2[nrRozdz, (int)poziom.ID], spis2[nrRozdz, (int)poziom.TRESC]);
                    if (jezykPolski)
                    {
                        word.wstawParagraf(spis2[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                    }
                    else
                    { word.wstawParagraf(spis2EN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h)); }

                }
                nrRozdz++;
                return w;
            }
        
            private String dajKoncepcje(Package k, ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
                String w = "";
                w += "<div class=\"img\">";
                w += dajTytulRozdz("2", ref nrRozdz);
             
                //word.wstawParagraf(modelProjektu.KoncepcjaElem[Obszar].Notes, 0);
                word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, modelProjektu.KoncepcjaElem[Obszar]);
                word.wstawZalacznikRTF(modelProjektu.KoncepcjaElem[Obszar]);

               /*
                * nowy generator 
                * foreach (Element e in k.Elements)
                {
                    if (e.Name == "Koncepcja")
                    {
                        word.wstawZalacznikRTF(e);
                        w += parsujImg( e.Notes, e.Name,word);

                    }
                }
                w += "</div>";*/
                okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
                return w;
            }
            public String dajSpisTresci()
            {
                if(NrRozdzialu=="3")
                return base.dajSpisTresci(spis);
                else
                    return base.dajSpisTresci(spis2);
            }

            private string dajArchitektureRek(ref int nrRozdz, Package pack)
            {
                string wynik = "";
                if (pack.Diagrams.Count > 0 || pack.Packages.Count > 0)
                {
                  //  wynik += "<div class=\"img\">";
                   // wynik += dajTytulRozdz("2", ref nrRozdz);
                    int i = 1;
                    // nowy generator if (arch.Diagrams.Count > 0)


                    foreach (Diagram diag in /* nowy generator arch.Diagrams*/ pack.Diagrams)
                    {
                        if (diag.Stereotype == CmodelKonfigurator.ukryjDiagramStr) continue;
                        wynik += "<div class=\"img\">";
                        wynik += "<h3>3.2." + i + " " + diag.Name + "</h3>";

                        Diagram d = diag;
                        String plik = EAUtils.zapiszDiagramJakoObraz(modelProjektu, ref d, dajSciezkeDocelowa());

                        wynik += "<img src='" + plik + "'>";
                        word.wstawParagraf(NrRozdzialu + ".2." + i + ". " + diag.Name, 3);
                        word.wstawObrazek(dajSciezkeDocelowa() + plik, i + ". " + diag.Name);
                        word.wstawParagraf(nrRozdz + ".2." + i + ". " + diag.Name, word.stylPodpis);

                        wynik += "<div class=\"desc\"> " + i + ". " + diag.Name + "</div>";


                        i++;
                        wynik += "</div>";
                        modelProjektu.Repozytorium.CloseDiagram(diag.DiagramID);
                        okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: " + (i - 1).ToString() + "/" + /* nowy generator arch.Diagrams.Count*/ pack.Diagrams.Count + " [ok]\n");

                    }
                }
              
                foreach (Package p in pack.Packages)
                {
                  wynik += dajArchitektureRek(ref nrRozdz, p);
                }
            return wynik;
            }
            private String dajArchitekture(/*Package arch,*/ ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport architektury statycznej");
                String wynik = "";
                if (modelProjektu.ArchStatPckg[Obszar].Diagrams.Count > 0||modelProjektu.ArchStatPckg[Obszar].Packages.Count>0)
                {
                    wynik += "<div class=\"img\">";
                    wynik += dajTytulRozdz("2", ref nrRozdz);
                    dajArchitektureRek(ref nrRozdz, modelProjektu.ArchStatPckg[Obszar]);
                    wynik += "</div>";
                }
                else
                {
                    nrRozdz++;//po to by tytuł przeskoczył dalej
                    okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: 0/" + /* nowy generator arch.Diagrams.Count*/ modelProjektu.ArchStatPckg[Obszar].Diagrams.Count + " [ok]\n");

                }
                
                return wynik;
            }
            private String dajOpisRoliSystemu(Package systemsPckg, ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport listy systemów");
                String wynik = "";
                if (modelProjektu.WkladyPckg[Obszar].Packages.Count > 0)
                {
                    wynik += "<div class=\"img\">";
                    wynik += dajTytulRozdz("3", ref nrRozdz);
                    Wordy.Table tab;
                    if (jezykPolski)
                    {
                    tab = word.wstawTabele("", new string[] { "Lp", "Nazwa systemu", "Opis roli systemu w projekcie" });//,"Dostawca" });
                    }
                    else 
                    {
                    tab = word.wstawTabele("", new string[] { "No", "System name", "System changes - abstract" });//,"Vendor" });
                    }
                    tab.Columns[1].SetWidth(30f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[2].SetWidth(90f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[3].SetWidth(380f, Wordy.WdRulerStyle.wdAdjustNone);
                    //tab.Columns[4].SetWidth(65f, Wordy.WdRulerStyle.wdAdjustNone);
                    wynik += "<table><tr><th>Lp</th><th>Nazwa systemu</th><th>Opis roli systemu w projekcie</th></tr>";
                    int j = 1;
                    foreach (Package p in systemsPckg.Packages)
                    {

                        EA.Element systEl = EAUtils.dajComponentSystemZpakietu(modelProjektu.Repozytorium, p);
                        String dostawca = EAUtils.dajDostawceSystemu(modelProjektu.Repozytorium, systEl);

                        word.wstawWierszDoTabeli("", tab, j + 1, new string[] { j.ToString(), p.Name, p.Notes });//, dostawca });
                        wynik += "<tr><td>" + j + "</td><td>" + p.Name + "</td><td>" + p.Notes + " </td></tr>";
                        j++;
                    }
                    wynik += "</table>";
                    wynik += "</div>";
                }
                else {
                    nrRozdz++; //by przeskoczyc do nast rodzialu
                }
                okno.Log(Statystyki.LogMsgType.WynikOK," systemów: "+systemsPckg.Packages.Count+ " [ok]\n");
                return wynik;
            }

            private String dajUseCaseRek(ref int nrRozdz,ref int i, Package pack)
            {
                String wynik = "";
                foreach (Diagram diag in /*nowy generator uc.Diagrams*/pack.Diagrams)
                {
                    if (diag.Stereotype == CmodelKonfigurator.ukryjDiagramStr) continue;
                    wynik += "<div class=\"img\">";
                    wynik += "<h3>3.3.1." + i + " " + diag.Name + "</h3>";

                    Diagram d = diag;
                    String plik = EAUtils.zapiszDiagramJakoObraz(modelProjektu, ref d, dajSciezkeDocelowa());
                    wynik += "<img src='" + plik + "'>";
                    word.wstawParagraf(NrRozdzialu + ".3.1." + i + ". " + diag.Name, 4);
                    word.wstawObrazek(dajSciezkeDocelowa() + plik);
                    word.wstawParagraf(NrRozdzialu + ".3.1." + i + ". " + diag.Name, word.stylPodpis);


                    wynik += "<div class=\"desc\"> " + i + ". " + diag.Notes + "</div>";


                    i++;
                    wynik += "</div>";
                    modelProjektu.Repozytorium.CloseDiagram(diag.DiagramID);
                }
                foreach (Package p in pack.Packages)
                {
                    wynik+=dajUseCaseRek(ref nrRozdz, ref i,  p);
                }
                return wynik;
            }

            private String dajUseCase(/*Package uc,*/ ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport diagramów UC");
                String wynik = "";
                if (modelProjektu.PrzypadkiPckg[Obszar].Diagrams.Count > 0|| modelProjektu.PrzypadkiPckg[Obszar].Packages.Count>0)
                {
                    wynik += "<div class=\"img\">";
                 
                    wynik += dajTytulRozdz("3", ref nrRozdz);
                    int i = 1;

                    wynik += dajUseCaseRek(ref nrRozdz, ref i, modelProjektu.PrzypadkiPckg[Obszar]);

                   okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: " + (i - 1).ToString() + "/" + /* uc.Diagrams.Count*/ modelProjektu.PrzypadkiPckg[Obszar].Diagrams.Count + " [ok]\n");  
              
                }
                else {
                  //  nrRozdz++;//przeskakujemy o rozdzial
                    nrRozdz++;//przeskakujemy o podrozdzial
                }
                wynik += "</div>";
                okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: 0/" + /* uc.Diagrams.Count*/ modelProjektu.PrzypadkiPckg[Obszar].Diagrams.Count + " [ok]\n");  
                return wynik;
            }
            private String dajSekwencjeRek(ref int nrRozdz, ref int i, Package pack)
            {
                String wynik = "";
                foreach (Diagram diag in /*uc.Diagrams*/ pack.Diagrams)
                {
                    if (diag.Stereotype == CmodelKonfigurator.ukryjDiagramStr) continue;
                    wynik += "<div class=\"img\">";
                    wynik += "<h3>3.3.2." + i + " " + diag.Name + "</h3>";

                    Diagram d = diag;
                    String plik = EAUtils.zapiszDiagramJakoObraz(modelProjektu, ref d, dajSciezkeDocelowa());
                    wynik += "<img src='" + plik + "'>";
                    word.wstawParagraf(NrRozdzialu + ".3.2." + i + ". " + diag.Name, 4);
                    if (diag.Notes.Length > 0) // dodanie opisu diagramu sekwencji z pola notes
                    {
                        word.wstawParagraf(diag.Notes, 0);
                        word.wstawParagraf("", 0);
                    }
                    word.wstawObrazek(dajSciezkeDocelowa() + plik);
                    word.wstawParagraf(nrRozdz + ".3.2." + i + ". " + diag.Name, word.stylPodpis);
                    wynik += "<div class=\"desc\">Sekw " + i + ". " + diag.Notes + "</div>";

                    Wordy.Table tab;
                    if (jezykPolski)
                    {
                        tab = word.wstawTabele("", new string[] { "Lp", "Systemy", "Krok", "Wejście/Wyjście", "Opis Kroku" });
                    }
                    else
                    {
                        tab = word.wstawTabele("", new string[] { "No", "Systems", "Step", "Input/Output", "Step describtion" });
                    }
                    tab.Columns[1].SetWidth(25f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[2].SetWidth(85f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[3].SetWidth(110f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[4].SetWidth(110f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[5].SetWidth(120f, Wordy.WdRulerStyle.wdAdjustNone);
                    //tab.Columns[6].SetWidth(140f, Wordy.WdRulerStyle.wdAdjustNone);

                    wynik += "<table><tr><th>Lp</th><th>Systemy</th><th>Krok</th><th>Wejście/Wyjście</th><th>Opis kroku</th></tr>";
                    int j = 1;

                    foreach (DiagramLink obj in diag.DiagramLinks)
                    {
                        if (obj.IsHidden) continue;


                        Connector con = modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID);  //rep.GetConnectorByID(obj.ConnectorID);
                        if (con.Type != "Sequence") continue;

                        Element elP = modelProjektu.Repozytorium.GetElementByID(con.ClientID);
                        Element elK = modelProjektu.Repozytorium.GetElementByID(con.SupplierID);

                        {
                            ///Zmiany 
                            string PDATA2 = modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).TransitionGuard;
                            string StyleEx = modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).StyleEx;
                            //bool isReturn = modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).Properties.Item("IsReturn").Value;

                            //okno.Log(Statystyki.LogMsgType.Info, "DUMP# " + modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).TransitionGuard + "\n");
                            //okno.Log(Statystyki.LogMsgType.Info, "DUMP# " + modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).StyleEx + "\n");
                            //okno.Log(Statystyki.LogMsgType.Info, modelProjektu.Repozytorium.GetConnectorByID(obj.ConnectorID).Properties.Item("IsReturn").Value);

                            string pattern = @"retval=([^;]+);";
                            string returnValue = "";
                            string paramValue = "";
                            int parSet = 0;

                            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                            MatchCollection matches = rgx.Matches(PDATA2);
                            if (matches.Count == 1)
                            {
                                returnValue = "OUT[" + matches[0].Groups[1].Value + "]";
                            }

                            //okno.Log(Statystyki.LogMsgType.Info, "DUMP# returnValue = " + returnValue + "\n");
                            //okno.Log(Statystyki.LogMsgType.Info, "DUMP# PDATA2 = " + PDATA2 + "\n");
                            pattern = @"paramsDlg=([^;]+);";
                            rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                            matches = rgx.Matches(PDATA2);
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
                            if (returnValue.Length > 0 && paramValue.Length > 0) { paramValue = paramValue + "\n" + returnValue; }
                            else if (returnValue.Length > 0) { paramValue = returnValue; }

                            //retval=test2;params=;paramsDlg=promo_id=current promo_id;;1;paramvalues=test1;;

                            wynik += "<tr><td>" + j + "</td><td>" + elP.Name + "->" + elK.Name + "</td><td>" + con.Name + "</td><td>" + "</td><td>" + con.Notes + "</td></tr>";
                            wynik += "</td></tr>";


                            int xx = con.Name.IndexOf("(");
                            string tmp = con.Name;
                            if (xx > 0)
                                tmp = con.Name.Substring(0, xx);
                            word.wstawWierszDoTabeli("", tab, j + 1, new string[] { con.SequenceNo.ToString(), elP.Name + "->" + elK.Name, tmp, paramValue, con.Notes });
                            j++;

                        }
                    }
                    tab.SortAscending();
                    wynik += "</table>";

                    i++;
                    modelProjektu.Repozytorium.CloseDiagram(diag.DiagramID);
                   
                }
                wynik += "</div>";
                foreach (Package p in pack.Packages)
                {
                    wynik += dajSekwencjeRek(ref nrRozdz, ref i,p);
                }
                return wynik;
            }

            private String dajSekwencje(/*Package uc,*/ ref int nrRozdz)
            {
                okno.Log(Statystyki.LogMsgType.Info, "Eksport diagramów Sekwencji");
                String wynik = "";
                // if (uc.Diagrams.Count > 0)
                if (modelProjektu.DiagrSekwPckg[Obszar].Diagrams.Count > 0||modelProjektu.DiagrSekwPckg[Obszar].Packages.Count>0)
                {
                    wynik += "<div class=\"img\">";
                    wynik += dajTytulRozdz("3", ref nrRozdz);

                    int i = 1;

                    dajSekwencjeRek(ref nrRozdz, ref i, modelProjektu.DiagrSekwPckg[Obszar]);


                    okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: " + (i - 1).ToString() + "/" + modelProjektu.DiagrSekwPckg[Obszar].Diagrams.Count + " [ok]\n");  
               
                }
                else
                {
                    nrRozdz++; //przeskakujemy o rozdział
                }
                wynik += "</div>";
                okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: 0/" + modelProjektu.DiagrSekwPckg[Obszar].Diagrams.Count + " [ok]\n");  
               
                return wynik;
            }
         
            private string dajArchitektureDanych(ref int nrRozdz)
            {
                String wynik = "";
                okno.Log(Statystyki.LogMsgType.Info, "Eksport architektury danych");
            
                if (modelProjektu.ArchitekturaDanychIDMPckg[Obszar].Diagrams.Count > 0)
                {
                    wynik += "<div class=\"img\">";
                    wynik += dajTytulRozdz("2", ref nrRozdz);
                    int i = 1;
                    // nowy generator if (arch.Diagrams.Count > 0)

                    wynik += dajTytulRozdz("3", ref nrRozdz);
                    foreach (Diagram diag in /* nowy generator arch.Diagrams*/ modelProjektu.ArchitekturaDanychIDMPckg[Obszar].Diagrams)
                    {
                        if (diag.Stereotype == CmodelKonfigurator.ukryjDiagramStr) continue;
                        wynik += "<div class=\"img\">";
                        wynik += "<h3>3.2." + i + " " + diag.Name + "</h3>";

                        Diagram d = diag;
                        String plik = EAUtils.zapiszDiagramJakoObraz(modelProjektu, ref d, dajSciezkeDocelowa());

                        wynik += "<img src='" + plik + "'>";
                        word.wstawParagraf(NrRozdzialu + ".3.1." + i + ". " + diag.Name, 4);
                        word.wstawObrazek(dajSciezkeDocelowa() + plik, i + ". " + diag.Name);
                        word.wstawParagraf(nrRozdz + ".3.1." + i + ". " + diag.Name, word.stylPodpis);

                        wynik += "<div class=\"desc\"> " + i + ". " + diag.Name + "</div>";


                        i++;
                        wynik += "</div>";
                        modelProjektu.Repozytorium.CloseDiagram(diag.DiagramID);
                        okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: " + (i - 1).ToString() + "/" + /* nowy generator arch.Diagrams.Count*/ modelProjektu.ArchitekturaDanychIDMPckg[Obszar].Diagrams.Count + " [ok]\n");

                    }
                }
                else
                {
                    nrRozdz++;//po to by tytuł przeskoczył dalej
                    okno.Log(Statystyki.LogMsgType.WynikOK, " diagramów eksportowanych: 0/" + /* nowy generator arch.Diagrams.Count*/ modelProjektu.ArchitekturaDanychIDMPckg[Obszar].Diagrams.Count + " [ok]\n");

                }
                wynik += "</div>";
                if (modelProjektu.ArchitekturaDanychLDMPckg[Obszar].Diagrams.Count > 0)
                {
                    wynik += dajTytulRozdz("3", ref nrRozdz);
                    Wordy.Table tab;
                    if (jezykPolski)
                    {
                        tab = word.wstawTabele("", new string[] { "Lp", "Nazwa encji", "Opis zmian w encji LDM" });
                    }
                    else
                    {
                        tab = word.wstawTabele("", new string[] { "No", "Name", "Description" });
                    }
                    tab.Columns[1].SetWidth(50f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[2].SetWidth(150f, Wordy.WdRulerStyle.wdAdjustNone);
                    tab.Columns[3].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);

                    foreach (Diagram diag in /* nowy generator arch.Diagrams*/ modelProjektu.ArchitekturaDanychLDMPckg[Obszar].Diagrams)
                    {

                        String sql = "select o.object_id from t_object o,t_diagramobjects do, t_diagram d where " +
                             "o.stereotype in ('changed','new') and o.object_type='Class' and " +
                             "do.object_id=o.object_id and d.diagram_id=" + diag.DiagramID +
                             " and d.diagram_id=do.diagram_id";
                        int i = 1;
                        foreach (EA.Element klasa in modelProjektu.Repozytorium.GetElementSet(sql, 2))
                        {
                            word.wstawWierszDoTabeli("", tab, i + 1, new string[] { i.ToString(), diag.Name + ":" + klasa.Name, klasa.Notes });
                            i++;
                        }

                    }
                }
                return wynik;
             }
            public String dajRozdzial()
            {
                int nrRozdz = 0;
                String w = "<div id=\"Rozdzial"+NrRozdzialu+"\">";
                Stopwatch st = new Stopwatch();
                st.Start();

                w += dajTytulRozdz("1", ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " tytuł- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                w += dajKoncepcje(koncepcjaPckg, ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " koncepcja- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                w += dajArchitekture(/*systemsPckg,*/ ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " ArchStat- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                w += dajOpisRoliSystemu(modelProjektu.WkladyPckg[Obszar], ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " Opis ról- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();

            //  w += dajArchitektureDanych(ref nrRozdz);
            // st.Restart();
                w += dajTytulRozdz("2", ref nrRozdz);
                w += dajUseCase(/*usecasePckg,*/ ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " UC- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Restart();
                w += dajSekwencje(/*sekwencjePckg, */ ref nrRozdz);
                okno.Log(Statystyki.LogMsgType.Info, "R " + NrRozdzialu + " SQ- " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");
                st.Stop();
               
                w += "</div>";
                return w;
            }
    }
}
