using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAkzg
{
   class CrozdzialUtils
    {
       public enum poziom { TRESC=0,ID=1, CSS=2 };
        String  sciezkaZrodlo;
        String sciezkaDocelowa;
        protected String dajSciezkeZrodlo()        { return sciezkaZrodlo; }
        protected String dajSciezkeDocelowa() { return sciezkaDocelowa; }
        public CrozdzialUtils(string sciezka,string sciezka2)
        {
            sciezkaZrodlo = sciezka;
            sciezkaDocelowa = sciezka2;
        }
        protected void kopiujPlik(String Zrodlo, String Cel)
        {
          //  string sciezkaDocelowa = sciezka_proj.Text + "img\\";
         //   System.IO.Directory.CreateDirectory(sciezkaDocelowa);
            if (System.IO.File.Exists(Zrodlo))
            {
                System.IO.File.Copy(Zrodlo, Cel, true);
            }
            else {
                System.Windows.Forms.MessageBox.Show("Brak pliku z obrazem w ścieżce: "+Zrodlo,"Brak pliku z obrazkiem");
            }
        }
        protected String parsujImg( String co, String plk,Word word)
        {
            String w="";
            //parsowanie notatki szukanie obrazków
            string[] obr = co.Split(new string[] { "imgsrc" }, StringSplitOptions.None);
            int j = 0;
            foreach (String txt in obr)
            {
                if (txt.IndexOf("=") == 0)
                {
                    //mamy obrazek
                    //znajdz apostrof koncowy
                    int kon = txt.IndexOf("@", 2);
                    String nazwapliku = txt.Substring(2, kon - 2);
                    int pozKropka = txt.IndexOf(".");
                    String rozszerzenie = txt.Substring(pozKropka, kon - pozKropka);
                    String nowyplik = "img/"+plk + j++ + rozszerzenie;
                    kopiujPlik(nazwapliku, sciezkaDocelowa+nowyplik);
                    w += "<img src=\"" + nowyplik + "\">";
                    //obrazek
                    word.wstawObrazek(sciezkaDocelowa + nowyplik);
                    word.wstawParagraf(txt.Substring(kon), 0);
                    w += txt.Substring(kon);
                }
                else
                {
                    w += txt;
                    word.wstawParagraf(txt, 0);
                }
                
            }
            return w;
        }
        protected String dajNaglowek(String h,String id,String tre)
        {
            String w = "";
            DateTime dt = DateTime.Now;
            //int spisId = 0;
            w = doklejPlik("h"+h+".kzg", id, tre);
            //todo slownik pojec
            //wynik += wstawSlownik(definicjePckg);
            //todo zalaczniki
            //todo zespol projektowy
            //todo powiązania z innymi projektami
            return w;
        }
        protected String dajLinijkeSpisuTresci(String ID, String CSS, String TRESC)
        {
            return "<a href=\"#" + ID + "\" class=\"" + CSS + "\">" + TRESC + "</a><br>\n"; 
        }
        public String dajSpisTresci(String[,] spis)
        {
            String w = "";
            for (int i = 0; i < spis.Length/3 ; i++)
            {
                
                //w += "<a href=\"#" + spis[i,  (int)poziom.ID] + "\" class=\"" + c + "\">" + spis[i, (int)poziom.TRESC] + "</a><br>\n";
                w += dajLinijkeSpisuTresci(spis[i, (int)poziom.ID], spis[i, (int)poziom.CSS], spis[i, (int)poziom.TRESC]);
            }
            return w;
        }
        protected string doklejPlik(string plik, params string[] p)
        {
            //String tresc = System.IO.File.ReadAllText(sciezkaZrodlo + "\\" + plik);
            String tresc = txt.dajTekst(plik);
            String wynik = "";
                   
            string[] lista = tresc.Split(new string[] { "^@^" }, StringSplitOptions.None);
          

            for (int i = 0; i < p.Length; i++)
            {
                wynik += lista[i] + p[i];
            }
            wynik += lista[lista.Length - 1];
            return wynik;
        }
        protected EA.Element DajElement(EA.Package p, int elId)
        {
            EA.Element el = null;
            foreach (EA.Element elem in p.Elements)
            {
                if (elem.ElementID == elId)
                {
                    el = elem;
                    return el;
                }
            }
            return el;
        }
    }
}
