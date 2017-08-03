using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;
using Wordy = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

namespace EAkzg
{
    class Crozdz6 : CrozdzialUtils
    {
        string[,] spis = new string[,] {{"11 ASPEKTY POZAFUNKCJONALNE IT","r9abc","spis1"},
            {"11.1 Wymagania dotyczące migracji danych","r9a1","spis1-1"},
              {"11.2 Architektura Transmisyjna","r9a2","spis1-1"},
                {"11.3 Wskazówki przeprowadzania testów IT","r9a3","spis1-1"}
        };//0
                             

        string[,] spis2 = new string[,] {{"12 ASPEKTY POZAFUNKCJONALNE NT","r10abc","spis1"},
             {"12.1 Wymagania dotyczące migracji danych","r10a1","spis1-1"},
              {"12.2 Architektura Transmisyjna","r10a2","spis1-1"},
                {"12.3 Wskazówki przeprowadzania testów NT","r10a3","spis1-1"}
        };//0

        string[,] spisEN = new string[,] {{"11 NON-FUNCTIONAL ASPECTS IN IT","r9abc","spis1"},
            {"11.1 Data migration requirements","r9a1","spis1-1"},
              {"11.2 Transmission Architecture","r9a2","spis1-1"},
                {"11.3 IT test-hints","r9a3","spis1-1"}
        };//0


        string[,] spis2EN = new string[,] {{"12 NON-FUNCTIONAL ASPECTS IN NT","r10abc","spis1"},
             {"12.1 Data migration requirements","r10a1","spis1-1"},
              {"12.2 Transmission Architecture","r10a2","spis1-1"},
                {"12.3 NT test-hints","r10a3","spis1-1"}
        };//0
        
        EA.Repository rep;
        Package koncepcjaPckg;
        String NrRozdzialu;
        Word word;
        bool jezykPolski;
        int obszar;
        CModel modelProjektu;
        public Crozdz6(CModel ModelProj,int Obszar, String sciezkaZrodlo, String sciezkaDocelowa, Word W, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski = jezykPl;
            word = W;
           // rep = r;
            modelProjektu = ModelProj;
            obszar = Obszar;
          //  koncepcjaPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Koncepcja");
          //  NrRozdzialu = nrRozdzialu;
        }
        public Crozdz6(EA.Repository r, EA.Package p, Package dzialPckg, String sciezkaZrodlo, String sciezkaDocelowa, String nrRozdzialu, Word W,bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski = jezykPl;
            word = W;
            rep = r;

            koncepcjaPckg = EAUtils.dajPakietSciezkiP(ref dzialPckg, "Koncepcja");
            NrRozdzialu = nrRozdzialu;
        }
        private String dajTytulRozdz_nowyModel(String h, ref int nrRozdz)
        {
            String w = "";
            if (obszar==CModel.IT)
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

     
        public String dajSpisTresci()
        {
           // kzg nowy model if (NrRozdzialu == "IT")
            if(obszar==CModel.IT)
                return base.dajSpisTresci(spis);
            else
                return base.dajSpisTresci(spis2);
        }
      
        // kzg nowy model private String dajMigracje(Package k, ref int nrRozdz)
        private String dajMigracje(Element e, ref int nrRozdz)
        {
          //  okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("2", ref nrRozdz);
            

/* kzg nowy model start
            foreach (Element e in k.Elements)
            {
                if (e.Name == "Migracja")
                {
  */                  w += e.Notes;
                    word.wstawParagraf(e.Notes, 0);
                    word.wstawZalacznikRTF(e);
    /*                
                }
            } kzg nowy model koniec*/
            w += "</div>";
       
            return w;
        }
        private String dajTesty(Package k, ref int nrRozdz)
        {
            //  okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("2", ref nrRozdz);



            foreach (Element e in k.Elements)
            {
                if (e.Name == "Testy - wskazówki")
                {
                    w += e.Notes;
                    word.wstawParagraf(e.Notes, 0);
                    word.wstawZalacznikRTF(e);

                }
            }
            w += "</div>";
            // okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            return w;
        }
        private String dajArchTransmisyjna(Package k, ref int nrRozdz)
        {
            //  okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
            String w = "";
            w += "<div class=\"img\">";
            w += dajTytulRozdz("2", ref nrRozdz);



            foreach (Element e in k.Elements)
            {
                if (e.Name == "Architektura Transmisyjna")
                {
                    w += e.Notes;
                    word.wstawParagraf(e.Notes, 0);
                    word.wstawZalacznikRTF(e);

                }
            }
            w += "</div>";
            // okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            return w;
        }
        public String dajRozdzial()
        {
            int nrRozdz = 0;
            String w = "<div id=\"Rozdzial" + obszar + "\">";
            w += dajTytulRozdz("1", ref nrRozdz);
            /* kzg nowy model
            w += dajMigracje(koncepcjaPckg, ref nrRozdz);
          */
            w += dajMigracje(modelProjektu.MigracjaElem[obszar], ref nrRozdz);
            w += "</div>";
            return w;
        }
    }
}
