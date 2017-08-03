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
    class Crozdz7 : CrozdzialUtils
    {
        string[,] spis = new string[,] {{"5 Architektura Transmisyjna","r11abc","spis1"}
        };//0

        string[,] spisEN = new string[,] {{"5 Transmission Architecture","r11abc","spis1"}
        };//0
      

      //  EA.Repository rep;
      //  Package defPckg;
        bool jezykPolski;
        Word word;
        CModel modelProjektu;
        public Crozdz7(CModel ModelProjektu, String sciezkaZrodlo, String sciezkaDocelowa,  Word W,bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            jezykPolski = jezykPl;
            word = W;
            //rep = r;
            modelProjektu = ModelProjektu;
           //defPckg = EAUtils.dajPakietSciezkiP(ref modelPckg, "Definicje");
           
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
   
        private String dajArchTransmisyjna( ref int nrRozdz)
        {
           
            String w = "";
            w += "<div class=\"img\">";

          /*  foreach (Element e in k.Elements)
            {
                if (e.Name == "Architektura Transmisyjna")
                {
                    w += e.Notes;
            */
            word.wstawNotatkeEAtoRTF(modelProjektu.Repozytorium, modelProjektu.ArchitekturaTransmisyjnaElem);
           // word.wstawParagraf(modelProjektu.ArchitekturaTransmisyjnaElem.Notes, 0);
            word.wstawZalacznikRTF(modelProjektu.ArchitekturaTransmisyjnaElem);

            /*    }
            }*/
            w += "</div>";
            // okno.Log(Statystyki.LogMsgType.WynikOK, " [ok]\n");
            return w;
        }
        public String dajRozdzial()
        {
            int nrRozdz = 0;
            String w = "<div id=\"Rozdzial 11 \">";
            w += dajTytulRozdz("1", ref nrRozdz);

              w += dajArchTransmisyjna( ref nrRozdz);

            w += "</div>";
            return w;
        }
    }
}
