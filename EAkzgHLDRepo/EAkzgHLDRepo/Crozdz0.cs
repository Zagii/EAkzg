using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;

namespace EAkzg
{
    class Crozdz0:CrozdzialUtils
    {
        Package projekt;
        CModel modelProjektu;
        Word word;
        bool jezykPolski;
        public Crozdz0(EA.Package p, String sciezkaZrodlo,String sciezkaDocelowa,Word W,bool jezykPl) : base(sciezkaZrodlo,sciezkaDocelowa)
        {
            projekt = p;
            word = W;
            jezykPolski = jezykPl;

        }
        public Crozdz0(CModel p, String sciezkaZrodlo, String sciezkaDocelowa, Word W, bool jezykPl)
            : base(sciezkaZrodlo, sciezkaDocelowa)
        {
            modelProjektu = p;
            word = W;
            jezykPolski = jezykPl;

        }
        public String dajRozdzial()
        {
            String w = "";
            DateTime dt = DateTime.Now;

            base.doklejPlik("naglowek.kzg", modelProjektu.dajNazweModelu(), dt.ToShortDateString());

            word.wstawSpisTresci("spis");

            base.doklejPlik("wstep.kzg", "", modelProjektu.dajNazweModelu()+" "+modelProjektu.dajPelnaNazweProjektu(), "Część IT -" + modelProjektu.dajAutoraProjektu(CModel.IT) + " Część NT -" + modelProjektu.dajAutoraProjektu(CModel.NT), dt.ToLongTimeString());
            word.wstawZnacznik("tytul_projektu", modelProjektu.dajNazweModelu() + " " + modelProjektu.dajPelnaNazweProjektu());
            word.wstawZnacznik("SD_IT", modelProjektu.dajAutoraProjektu(CModel.IT));
            word.wstawZnacznik("SD_NT", modelProjektu.dajAutoraProjektu(CModel.NT)); 
            

            word.wstawZnacznik("data_generowania",dt.ToLongDateString()+" "+dt.ToLongTimeString());

            return w;
        }
        public string dajSpisTresci()
        {
            
            return "<a id=\"spisTresci\"><h1> Spis Treści </h1></a>";
        }
    }
}
