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
    class Crozdz5 : CrozdzialUtils
    {
        string[,] spis = new string[,] { { "9 INTERFEJSY IT", "r7abc", "spis1" } };

        string[,] spis2 = new string[,] {{"10 INTERFEJSY NT","r8abc","spis1"}};//0
               string[,] spisEN = new string[,] { { "9 IT INTERFACES", "r7abc", "spis1" } };

        string[,] spis2EN = new string[,] {{"10 NT INTERFACES","r8abc","spis1"}};//0
                             
     //   Package projekt;
     
    //    Package wkladyPckg;
        EA.Repository Repo;
        String NrRozdzialu;
        Word word;
        bool jezykPolski;
        CModel modelProjektu;
        int Obszar;

        public Crozdz5(CModel ModelProjektu, int obszar, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu, Word W, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            modelProjektu = ModelProjektu;
            Obszar = obszar;
            jezykPolski = jezykPl;
            word = W;
         
            NrRozdzialu = nrRozdzialu;
        }
        public Crozdz5(EA.Repository r, EA.Package p, Package dzialPckg, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu, Word W,bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski=jezykPl;
            word = W;
            Repo = r;
       //     projekt = p;
        //    wkladyPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Wkłady Systemowe");
            NrRozdzialu = nrRozdzialu;
        }
        private String dajTytulRozdz_nowyModel(String h, ref int nrRozdz)
        {
            String w = "";
            if (Obszar==CModel.IT)
            {
                w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);
                if (jezykPolski)
                {
                    word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
                else { word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h)); }
            }
            else
            {
                w = dajNaglowek(h, spis2[nrRozdz, (int)poziom.ID], spis2[nrRozdz, (int)poziom.TRESC]);
                if (jezykPolski)
                {
                    word.wstawParagraf(spis2[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }
                else { word.wstawParagraf(spis2EN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h)); }
            }
            nrRozdz++;
            return w;
        }
        private String dajTytulRozdz(String h, ref int nrRozdz)
        {
            return dajTytulRozdz_nowyModel(h, ref nrRozdz);
           
            String w = "";
            if (NrRozdzialu == "IT")
            {
                w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);
                if(jezykPolski)
                {
                word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }else{word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));}
            }
            else
            {
                w = dajNaglowek(h, spis2[nrRozdz, (int)poziom.ID], spis2[nrRozdz, (int)poziom.TRESC]);
                if(jezykPolski)
                {
                word.wstawParagraf(spis2[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
                }else{word.wstawParagraf(spis2EN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));}
            }
            nrRozdz++;
            return w;
        }

       
        public String dajSpisTresci()
        {
            if (NrRozdzialu == "IT")
                return base.dajSpisTresci(spis);
            else
                return base.dajSpisTresci(spis2);
        }

        private String dajInterfejsyRealizacjaOpis(Package pakiet, ref int nrRozdz, int lp, Package pakietSystemu)
        {
            String w = "";
            w += "<div class=\"img\">";
            
            Package pakietInterfejs = EAUtils.utworzPakietGdyBrak(ref pakiet, "Realizowane interfejsy", "");
            int licznik = 0;
            foreach (Element e1 in pakietInterfejs.Elements)
            {

                foreach (Method m1 in e1.Methods)
                {
                    licznik++;
                }
            }
            if (licznik == 0)
            {
                w += "\nSystem nie dostarcza interfejsów.\n<BR>";
                if(jezykPolski)
                {
                word.wstawParagraf("System nie dostarcza interfejsów.", word.stylNorm);
                }else{word.wstawParagraf("The system does not realize any interfaces.", word.stylNorm);}
                return w + "</div>";
            }

        

            int i = 1;
            Wordy.WdColor kolor1=Wordy.WdColor.wdColorBlack;


            foreach (Element e in pakietInterfejs.Elements)
            {
                word.wstawParagraf(nrRozdz + "." + lp + "." + i+" "+e.Name, 3);
                Wordy.Table tab = word.wstawTabele("", new string[] { e.Name, "" });
                tab.Columns[1].SetWidth(200f, Wordy.WdRulerStyle.wdAdjustNone);
                tab.Columns[2].SetWidth(300f, Wordy.WdRulerStyle.wdAdjustNone);
                /// nagłówek
                //tab.Rows[1].Cells[1].Merge(tab.Rows[1].Cells[2]);
                tab.Rows[1].Cells[1].Shading.BackgroundPatternColor = kolor1;
                tab.Rows[1].Cells[2].Shading.BackgroundPatternColor = kolor1;
                tab.Rows[1].Cells[1].Range.Font.Bold = 1;
                tab.Rows[1].Cells[1].Range.Font.Name = "Calibri";

                int index=2;
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Opis", e.Notes });
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Description", e.Notes });}
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                index++;
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Technologia", e.Stereotype });
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Technology", e.Stereotype });}
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                index++;
                string dostawca = "";
                foreach (Connector c in e.Connectors)
                {
                    if (c.Type == "Realisation")
                    {
                        
//                        Element realizator = Repo.GetElementByID(c.ClientID);
                        Element realizator = modelProjektu.Repozytorium.GetElementByID(c.ClientID);
                        dostawca += realizator.Name + ", ";
                    }
                }
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Dostawca", dostawca});
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Provider", dostawca});}
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                index++;
                string konsument = "";
                foreach (Connector c in e.Connectors)
                {
                    if (c.Type == "Usage")
                    {
                       // Element kons = Repo.GetElementByID(c.ClientID);
                        Element kons = modelProjektu.Repozytorium.GetElementByID(c.ClientID);
                        konsument += kons.Name + ", ";
                    }
                }
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Konsument", konsument });
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Consumer", konsument });}
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                index++;
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Operacje", "" },false);
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Operations", "" },false);}
              //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                index++;
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Nazwa operacji", "Opis" });
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Operation name", "Description" });}
                //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                index++;

                foreach (Method m in e.Methods)
                {
                 
                  
                  //  i++;
                    word.wstawWierszDoTabeli("", tab, index, new string[] { "&lt;&lt;" + m.Stereotype + "&gt;&gt;\n" + m.Name, m.Notes });
                    //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    index++;

                }
                if (e.Methods.Count <= 0)
                {
                    if(jezykPolski)
                    {
                    word.wstawWierszDoTabeli("", tab, index, new string[] { "Brak operacji", "" },false);
                    }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "None", "" },false);}
                    //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    index++;
                }
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Atrybuty", "" },false);
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Atributes", "" },false);}
                //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                index++;
                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Nazwa atrybutu", "Opis" });
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Atribute name", "Description" });}
                //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                index++;
                foreach (EA.Attribute a in e.Attributes)
                {
                    word.wstawWierszDoTabeli("", tab, index, new string[] { a.Name, a.Notes });
                    //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    index++;
                }
                if (e.Attributes.Count <= 0)
                {
                    if(jezykPolski)
                    {
                    word.wstawWierszDoTabeli("", tab, index, new string[] { "Brak atrybutów", "" },false);
                    }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "None", "" },false);}
                    //  tab.Rows[index].Cells[1].Merge(tab.Rows[1].Cells[2]);
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                    index++;
                }

                if(jezykPolski)
                {
                word.wstawWierszDoTabeli("", tab, index, new string[] { "Parametry dla operacji:", "" },false);
                }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Operation parameters:", "" },false);}
                tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                tab.Rows[index].Cells[1].Range.Font.Name = "Calibri";
                index++;

                foreach (Method m in e.Methods)
                {
                    word.wstawWierszDoTabeli("", tab, index, new string[] {  m.Name,"" },false);
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                    index++;
                    if(jezykPolski)
                    {
                    word.wstawWierszDoTabeli("", tab, index, new string[] { "Nazwa parametru : Typ danych", "Opis" });
                    }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "Parameter name : Type", "Description" });}
                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorGray25;
                    index++;

                    if (m.Parameters.Count <= 0)
                    {
                        if(jezykPolski)
                        {
                        word.wstawWierszDoTabeli("", tab, index, new string[] { "Brak parametrów dla operacji", "" },false);
                        }else{word.wstawWierszDoTabeli("", tab, index, new string[] { "None", "" },false);}
                        tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                        tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                        tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                        tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                        index++;
                    }
                    else {
                        foreach (EA.Parameter par in m.Parameters)
                        {
                            word.wstawWierszDoTabeli("", tab, index, new string[] {par.Name+" "+par.Type,par.Notes });
                            tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                            tab.Rows[index].Cells[1].Range.Font.Bold = 0;
                            tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                            tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = Wordy.WdColor.wdColorWhite;
                            index++;
                        }
                    }

                }
                /// dodanie opisu
                /// jesli jest załączony
                string d = e.GetLinkedDocument();
                if (d.Length > 0)
                {
                    if (jezykPolski)
                    {
                        word.wstawWierszDoTabeli("", tab, index, new string[] { "Dodatowy opis interfejsu", "" }, false);
                    }
                    else { word.wstawWierszDoTabeli("", tab, index, new string[] { "Additional description", "" }, false); }

                    tab.Rows[index].Cells[1].Shading.BackgroundPatternColor = kolor1;
                    tab.Rows[index].Cells[1].Range.Font.Bold = 1;
                    tab.Rows[index].Cells[2].Range.Font.Bold = 0;
                    tab.Rows[index].Cells[2].Shading.BackgroundPatternColor = kolor1;
                    index++;

                    //tu fragment prawdopodobnie zjebany bo mergowanie coś średnio bangla i potem tabela
                    //może się rozjechać, dlatego bez tabeli
                      word.wstawZalacznikRTF(e);
                    // koniec zjebu

                }
                i++;

            }

            return w + "</div>\n";
        }
        private String dajInterfejsAgreementSystemu(Package pakiet, ref int nrRozdz, int lp)
        {
            String w = "<div id=\"IA-" + pakiet.Name + "\">";
            // String w="<div id=\"r"+numer+"-"+lp+"\">";  
            w += nrRozdz + "." + lp + " " + pakiet.Name; ///dla wersji html poprawic to i dodac formatowanie
            word.wstawParagraf(nrRozdz + "." + lp + " " + pakiet.Name, 2);                                 
   
            w += dajInterfejsyRealizacjaOpis(pakiet, ref nrRozdz, lp, pakiet);
 
            return w + "</div>";
        }
        public String dajRozdzial()
        {
            int nrRozdz = 0;
            String w = "<div id=\"Rozdzial" + NrRozdzialu + "\">";
            w += dajTytulRozdz("1", ref nrRozdz);
            w += "\nNiniejszy rozdział opisuje interfejsy w podziale na wystawiające je systemy.<BR>\n";
            if(jezykPolski)
            {
            word.wstawParagraf("Niniejszy rozdział opisuje interfejsy w podziale na wystawiające je systemy.", 0);
            }else{word.wstawParagraf("This paragrapf is presenting the systems with their interfaces.", 0);}
            int lp = 1;
            // kzg nowy model start
           // foreach (Package p in wkladyPckg.Packages)
            foreach(Package p in modelProjektu.WkladyPckg[Obszar].Packages)
                //kzg koniec
            {
                int tmp = nrRozdz;
                w += dajInterfejsAgreementSystemu(p, ref tmp, lp);
                lp++;
            }

            w += "</div>";
            return w;
        }
    }
}
