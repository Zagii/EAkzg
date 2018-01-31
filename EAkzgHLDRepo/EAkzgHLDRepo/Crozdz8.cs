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
    class Crozdz8 : CrozdzialUtils
    {
        string[,] spis = new string[,] {{"6 Wskazówki dotyczące testów","r12abc","spis1"},
              {"6.1 Testy automatyczne","r1a1","spis1-1"}//1
        };//0

        string[,] spisEN = new string[,] {{"6 Test-hints","r12abc","spis1"},
            {"6.1 Automatic tests","r1a1","spis1-1"}//1
        };//0




      //  EA.Repository rep;
      //  Package defPckg;
        bool jezykPolski;
        Word word;
        CModel modelProjektu;
        public Crozdz8(CModel ModelProjektu, String sciezkaZrodlo, String sciezkaDocelowa, Word W,bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            word = W;
            //rep = r;
            jezykPolski = jezykPl;
            //defPckg = EAUtils.dajPakietSciezkiP(ref modelPckg, "Definicje");
            modelProjektu = ModelProjektu;

        }
        private String dajTytulRozdz(String h, ref int nrRozdz)
        {
            String w = "";

            w = dajNaglowek(h, spis[nrRozdz, (int)poziom.ID], spis[nrRozdz, (int)poziom.TRESC]);
            if (jezykPolski)
            {
                word.wstawParagraf(spis[nrRozdz, (int)poziom.TRESC], Int16.Parse(h));
            }
            else { word.wstawParagraf(spisEN[nrRozdz, (int)poziom.TRESC], Int16.Parse(h)); }

            nrRozdz++;
            return w;
        }


        public String dajSpisTresci()
        {

            return base.dajSpisTresci(spis);

        }

           private String dajTesty( ref int nrRozdz)
           {
               //  okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
               String w = "";
               w += "<div class=\"img\">";
          
                      // word.wstawParagraf(modelProjektu.TestyElem.Notes, 0);
               word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, modelProjektu.TestyElem);
                       word.wstawZalacznikRTF(modelProjektu.TestyElem);

              
               w += "</div>";
              
               return w;
           }
           private String dajTestyAutomat(ref int nrRozdz)
           {
               //  okno.Log(Statystyki.LogMsgType.Info, "Eksport koncepcji ogólnej");
               String w = "";
               w += "<div class=\"img\">";
               w += dajTytulRozdz("2", ref nrRozdz);
               // word.wstawParagraf(modelProjektu.TestyElem.Notes, 0);
               word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, modelProjektu.TestyElemAutomat);
               word.wstawZalacznikRTF(modelProjektu.TestyElemAutomat);


               w += "</div>";

               return w;
           }
    
        public String dajRozdzial()
        {
            int nrRozdz = 0;
            String w = "<div id=\"Rozdzial 12 \">";
            w += dajTytulRozdz("1", ref nrRozdz);


            w += dajTesty(/*defPckg,*/ ref nrRozdz);
            w += dajTestyAutomat(/*defPckg,*/ ref nrRozdz);
            w += "</div>";
            return w;
        }
    }
}
