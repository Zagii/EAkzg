using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wordy = Microsoft.Office.Interop.Word;


namespace EAkzg
{
    class Word
    {
        public string stylTab = "HLD_tabela";
        public string stylRozdz = "HLD_tytul_rozdzialu";
        public string stylPodrozdz = "HLD_tytul_podrozdzialu";
        public string stylPodrozdz2= "HLD_tytul_podrozdzialu2";
        public string stylPodrozdz3 = "HLD_tytul_podrozdzialu3";
        public string stylNorm = "HLD_normalny";
        public string stylPodpis = "HLD_podpis";

        bool sprawdzajGramatyke;
        bool sprawdzajPisownie;
        Wordy.TableOfContents spisTresci;
        object readOnly = false;
        object isVisible = true;
        object missing = null;
        Wordy.Application wordApp = null;
        Wordy.Document doc = null;
        object koniecDok=0;
        //the template file you will be using, you need to locate the template we   previously made
        //object fileToOpen = (object)@"D:\_Projekty\EAkzg\EAkzg\adintester\EAAddinTester\EAAddinTester\bin\Debug\Szablon.docx";

        object fileToOpen;
        object fileToSave;

        //    fileToOpen = EAkzg.Properties.Resources.Szablon;
        //Where the new file will be saved to + the filename (I have added  the name of the customer to filename)
        //object fileToSave = (object)@"D:\_Projekty\EAkzg\EAkzg\adintester\EAAddinTester\EAAddinTester\bin\Debug\SzablonHLD.docx";

        object oMissing = System.Reflection.Missing.Value;
      object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */ 

        public Word(String plikSzablonu,String plikHLD, bool widoczna=true)
        {
            fileToOpen = plikSzablonu;
            fileToSave = plikHLD;
            //missing oject to use with various word commands
             missing = System.Reflection.Missing.Value;

         
            //Create new instance of word and create a new document
             wordApp = new Wordy.Application();
            // doc = null;
            
            //Settings the application to invisible, so the user doesn't notice that anything is going on
            wordApp.Visible = widoczna;

            //Open and activate the chosen template
            doc = wordApp.Documents.Open(ref fileToOpen, ref missing,
                  ref readOnly, ref missing, ref missing, ref missing,
                  ref missing, ref missing, ref missing, ref missing,
                  ref missing, ref isVisible, ref missing, ref missing,
                  ref missing, ref missing);

            doc.Activate();
            doc.SpellingChecked = false;

            sprawdzajPisownie = doc.ShowSpellingErrors;
            sprawdzajGramatyke = doc.ShowGrammaticalErrors;

            doc.ShowGrammaticalErrors = false;
            doc.ShowSpellingErrors = false;
        }
        public void dajKoniecDoc()
        {
            koniecDok = doc.Content.End;
        }
        public void ustawKoniec(object bookmark)
        {
            
            Wordy.Range rng = doc.Bookmarks.get_Item(ref bookmark).Range;
            koniecDok = rng.Start;
        }
        public void wstawObrazek(String sciezkaZplikiem,String podpis="")
        {
         //   wordApp.Selection.InlineShapes.AddPicture(sciezkaZplikiem);

            //   Wordy.Range r = doc.Range(ref koniecDok);
      //      Wordy.Paragraph par = doc.Paragraphs.Add();
      //      Wordy.Range r = par.Range;
      //      r.Collapse();
           // r.Text = tekst + "\n";
      //      r.InlineShapes.AddPicture(sciezkaZplikiem);
      //      koniecDok = r.End;

            //object indentStyle = styl;
            //r.set_Style(indentStyle);

            Wordy.Paragraph oPara3;
           // object oRng = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
          //  oPara3 = doc.Content.Paragraphs.Add(ref oRng);
            oPara3 = doc.Content.Paragraphs.Add();
            oPara3.Range.InlineShapes.AddPicture(sciezkaZplikiem);
            oPara3.OutlineLevel = Wordy.WdOutlineLevel.wdOutlineLevelBodyText;
            object snorm = stylNorm;
            oPara3.Range.set_Style(ref snorm);
          //  oPara3.Range.InsertAfter(podpis);
           // oPara3.Range.Text = podpis;//nowe
         //   oPara3.Range.set_Style(stylPodpis);//nowe
           // oPara3.Range.InsertParagraphAfter();
        }
        public void wstawSpisTresci(object bookmark)
        {
          //  return;
            Wordy.Range oRng = doc.Bookmarks.get_Item(ref bookmark).Range;
            oRng.Text = "";
            object oTrueValue = true;
            object oFalseValue = false;
            oRng.Collapse();
         //   Wordy.Range rangeForTOC = doc.Range( oRng.Start, ref missing);
            /*spisTresci = doc.TablesOfContents.Add(oRng, ref oTrueValue, ref missing, ref missing,
                          ref missing, ref missing, ref oTrueValue, 
                             ref oTrueValue, ref oTrueValue, ref oTrueValue,
                             ref oTrueValue, ref oTrueValue);
            */
            spisTresci = doc.TablesOfContents.Add(oRng, ref oFalseValue, ref missing, ref missing,
                       ref missing, ref missing, ref oTrueValue,
                          ref oTrueValue, ref oTrueValue, ref oTrueValue,
                          ref oTrueValue, ref oTrueValue);
            
        object h1 =stylRozdz;
        object h2 =stylPodrozdz;
        object h3 =stylPodrozdz2;
        object h4 =stylPodrozdz3;

     /*       string txt="";
        foreach (Wordy.HeadingStyle x in spisTresci.HeadingStyles)
        {
            txt += x.ToString();
        }*/


      spisTresci.HeadingStyles.Add(ref h1, 1);
        spisTresci.HeadingStyles.Add(ref h2, 2);
        spisTresci.HeadingStyles.Add(ref h3, 3);
        spisTresci.HeadingStyles.Add(ref h4, 4);
            
            spisTresci.Update();
            
        }
       
        public void odswiezSpisTresci()
        {
            //return;
            if(spisTresci!=null)
            spisTresci.Update();
        }
        public void zapiszZakmnij()
        {
            doc.ShowSpellingErrors = sprawdzajPisownie;
            doc.ShowGrammaticalErrors = sprawdzajGramatyke;

            //Save the document
            doc.SaveAs2(ref fileToSave, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing);


            //Either you now choose to show the print preview so you can print the document, or you choose to just close the application so you save the document to your hard drive        

            //Making word visible to be able to show the print preview.
            //       wordApp.Visible = true;
            //       doc.PrintPreview();

            //Close the document and the application (otherwise the process will keep running)
     //       doc.Close();//(ref missing, ref missing, ref missing);
    //        wordApp.Quit();//(ref missing, ref missing, ref missing);
        }
        public Wordy.Table wstawTabele(object bookmark,String[] naglowki)
        {
            Wordy.Bookmark book = doc.Bookmarks.get_Item(ref oEndOfDoc);
  
            Wordy.Table tab=doc.Tables.Add(book.Range,1,naglowki.Count());
          //  tab.Range.ParagraphFormat.SpaceAfter = 6;

            object styleName = stylTab;
            tab.set_Style(ref styleName);
            
            int i = 1;
            foreach(String kol in naglowki)
            {
               
                Wordy.Cell cel= tab.Cell(1,i);
                cel.Range.Text = kol;
                object snorm = stylNorm;
                cel.Range.set_Style(ref snorm);
                i++;
            }
            
            return tab;
        }
        public Wordy.Range wstawWierszDoTabeli(object bookmark,Wordy.Table tab, int wiersz, string[] kolumny,bool Walidowac=true)
        {
        //    Wordy.Bookmark book = doc.Bookmarks.get_Item(ref bookmark);
            int i = 1;
            tab.Rows.Add();
          //  tab.Rows.SetHeight(12f, Wordy.WdRowHeightRule.wdRowHeightAuto);
            Wordy.Cell cel = null;
            foreach (String kol in kolumny)
            {
                String t = kol;
                t =  usunNieobslugiwaneZnacznikiHtml(t);
                t = kowertujZnacznikiHTML(t);
                cel = tab.Cell(wiersz, i);
                if (t == null) t = " ";
                cel.Range.Text = t;
                cel.Range.set_Style(stylNorm);
                if (Walidowac)
                {
                    if (t == "")
                        cel.Range.Comments.Add(cel.Range, "Niekompletny wiersz w tabeli - kolumna: " + i + ".");
                }
                i++;
               
            }
            return cel.Range;
  /*          koniecDok = tab.Range.End;
           object p = book.Start;
           object k = tab.Range.End;
            Wordy.Range rng = doc.Range(ref p, ref k);
            rng.Select();
            object rr = rng;
            doc.Bookmarks.Add((string)bookmark, ref rr);
    */    }
      
        public void wstawTekst(object bookmark,String tekst, String styl = "HLD_normalny")
        {
         //   Wordy.Range r = doc.Range(ref koniecDok);
            wstawTytulRozdzialu(bookmark, tekst, "HLD_normalny");
            return;
            Wordy.Bookmark book = doc.Bookmarks.get_Item(ref bookmark);

            Wordy.Paragraph par = doc.Paragraphs.Add();
            Wordy.Range r = par.Range; 
            r.Collapse();
            r.Text = tekst + "\n";
            koniecDok = r.End;
         
            object indentStyle = styl;
            r.set_Style(indentStyle);
        }
        public void usunKoniecRozdzialu(object bookmark)
        {
            Wordy.Range rng = doc.Bookmarks.get_Item(ref bookmark).Range;
            rng.Text = "";
        }
        public void dodajRozdzialNaKoncu(String tekst, String styl)
        {
            Wordy.Paragraph par = wordApp.ActiveDocument.Paragraphs.Add();

            String styl_txt = "";
            if (styl == "1") styl_txt = "HLD_tytul_rozdzialu";
            if (styl == "2") styl_txt = "HLD_tytul_podrozdzialu";
            if (styl == "3") styl_txt = "HLD_tytul_podrozdzialu";


            object indentStyle = styl_txt;
            par.set_Style(indentStyle);
            par.Range.Select();
            par.Range.InsertAfter(tekst);
        }
        public void wstawKoniecTabeli(object bookmark)
        {
         //   Wordy.Bookmark book = doc.Bookmarks.get_Item(ref bookmark);
         //   book.Range.InsertParagraphAfter();
            //book.Range.Select();
            //book.Range.InsertAfter("\n");
            //book.Range.Select();
            //object r = book.Range;
            //doc.Bookmarks.Add((String)bookmark, ref r);
        }
        public void wstawTytulRozdzialu(object bookmark, String tekst,String styl)
        {
             Wordy.Paragraph par=null;
             Wordy.Range rng;
       
            Wordy.Bookmark book= doc.Bookmarks.get_Item(ref bookmark);
        
            rng = book.Range;
                
                rng.Select();
                //wordApp.ActiveDocument.Content.InsertParagraphBefore();
              //  rng.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseEnd) ;
                rng.Select();
            object p=rng.End;
            object k=rng.End;

            String styl_txt = "HLD_normalny";
            if (styl == "1") styl_txt = "HLD_tytul_rozdzialu";
            if (styl == "2") styl_txt = "HLD_tytul_podrozdzialu";
            if (styl == "3") styl_txt = "HLD_tytul_podrozdzialu";
           

            object indentStyle = styl_txt;
         
            book.Range.InsertAfter(tekst+"\n");
      
            p = book.Range.End;
            k = book.Range.End + tekst.Length + 1;
            rng = doc.Range(ref p, ref k);
            rng.Select();
          rng.set_Style(indentStyle);
         
            book.Range.Select();
            p = book.Range.Start;
            k = rng.End;
            rng = doc.Range(ref p, ref k);
            rng.Select();
            object r = rng;
            doc.Bookmarks.Add((String)bookmark, ref r);
            book = doc.Bookmarks.get_Item(ref bookmark);
           
            book.Range.Select();
            
      
            
        }
        public void findAndReplace(Wordy.Document doc, object bookmark, object replaceWith)
        {
            Wordy.Range rng = doc.Bookmarks.get_Item(ref bookmark).Range;

            rng.Text = replaceWith.ToString();
            object oRng = rng;
            doc.Bookmarks.Add(bookmark.ToString(), ref oRng);
            koniecDok = rng.End;
        }
        public void wstawZnacznik( object bookmark, object replaceWith)
        {
            findAndReplace(doc, bookmark, replaceWith);
        }
        public int utworzDokument(String sciezka, String plik)
        {
            return 0;
        }
        public Wordy.Document dajDoc()
        {
            return doc;
        }
        public Wordy.Paragraph wstawParagraf(String txt, int h,String komentarz="")
    {
        Wordy.Paragraph parWynik=null;
         string styl_txt = stylNorm;
         txt=txt.Replace("\r", "");
         Wordy.WdOutlineLevel poziom = Wordy.WdOutlineLevel.wdOutlineLevelBodyText;

            if (h == 1)
         {
             styl_txt = stylRozdz;
             poziom = Wordy.WdOutlineLevel.wdOutlineLevel1;
         }
            if (h == 2)
            {
                styl_txt = stylPodrozdz;
                poziom = Wordy.WdOutlineLevel.wdOutlineLevel2;
            }
            if (h == 3)
            {
                styl_txt = stylPodrozdz2;
                poziom = Wordy.WdOutlineLevel.wdOutlineLevel3;
            }
            if (h == 4)
            {
                styl_txt = stylPodrozdz3;
                poziom = Wordy.WdOutlineLevel.wdOutlineLevel4;
            }
            string[] paragrafy = txt.Split('\n');
            int i = 0;
            foreach (string s in paragrafy)
            {
               parWynik= wstawParagraf(paragrafy[i], styl_txt, poziom,komentarz);
                i++;
            }
            return parWynik;
    }
        private void szukajZnacznikowHTML2(ref object poczatek, ref object koniec, String html, ref Wordy.Paragraph p, string tekstParagrafu)
        {
            // String tekstParagrafu = p.Range.Text;
            // object tx=p.Range.FormattedText;
            int poczStart = 0;
            int konStart = 0;
            object charUnit = Wordy.WdUnits.wdCharacter;
            object move = -1;  // move left 1

            while (poczStart >= 0)
            {
                poczStart = p.Range.FormattedText.Text.IndexOf("<" + html + ">", poczStart);
                if (poczStart < 0) break;
                p.Range.Select();
            
                p.Range.MoveEnd(ref charUnit, ref move);
                p.Range.Select();
                p.Range.FormattedText.Text = usunZnaki(p.Range.FormattedText.Text, poczStart, html.Length + 2);
                p.Range.Select();
                konStart = p.Range.FormattedText.Text.IndexOf("</" + html + ">", konStart);
                p.Range.Select();
                if (konStart < 0) break;
                p.Range.MoveEnd(ref charUnit, ref move);
                p.Range.FormattedText.Text = usunZnaki(p.Range.FormattedText.Text, konStart, html.Length + 3);
                //  tekstParagrafu.Replace((char)'\r',(char)' ');
             
               
                //  var oldParagraphFormat = myObject.range.ParagraphFormat.Duplicate;
                //   range.ParagraphFormat = oldParagraphFormat;
             //   p.Range.Text = tekstParagrafu;
                //  p.Range.FormattedText = tx;
                formatujFragmentParagrafu(poczStart, konStart, html, ref p);
            }
        }

        /// <summary>
        /// Usuwa nieobsługiwane formatowania html. 
        /// Formatowalny tekst moze byc tylko jako załącznik rtf
        /// </summary>
        /// <param name="tekstParagrafu">treść paragrafu</param>
        private string usunNieobslugiwaneZnacznikiHtml( string tekstParagrafu)
        {
            Regex rgx = new Regex("<(.*?)>");
            return rgx.Replace(tekstParagrafu, "");
        }
        /***
         * Szukaj wszystkich wystapien danego znacznika -html w paragrafie p
         * **/
     private void szukajZnacznikowHTML(ref object poczatek,ref object koniec,String html,ref Wordy.Paragraph p,string tekstParagrafu)
     {
        // String tekstParagrafu = p.Range.Text;
        // object tx=p.Range.FormattedText;
         int poczStart = 0;
         int konStart = 0;
         while(poczStart>=0)
         {
             poczStart = tekstParagrafu.IndexOf("<" + html + ">", poczStart);
         if (poczStart < 0) break;
         tekstParagrafu = usunZnaki(tekstParagrafu, poczStart, html.Length + 2);
         konStart = tekstParagrafu.IndexOf("</" + html + ">", konStart);
         if (konStart < 0) break;
         tekstParagrafu = usunZnaki(tekstParagrafu, konStart, html.Length + 3);
           //  tekstParagrafu.Replace((char)'\r',(char)' ');
         object charUnit = Wordy.WdUnits.wdCharacter;
         object move = -1;  // move left 1

         p.Range.MoveEnd(ref charUnit, ref move);
       //  var oldParagraphFormat = myObject.range.ParagraphFormat.Duplicate;
      //   range.ParagraphFormat = oldParagraphFormat;
        p.Range.Text = tekstParagrafu;
       //  p.Range.FormattedText = tx;
         formatujFragmentParagrafu(poczStart, konStart, html, ref p);
        }
     }
     /*** 
  * usuwa ile znakow od miejsca od w stringu s + referencja stringu wejsciowego
  * i zwraca ten string
      * todo!!!!!!!!!!!!!!!!!!!!!!!
  * ***/
     private string usunZnaki2(ref string s, int od, int ile)
     {
         if (s.Length < od) return s;
         if (od + ile > s.Length) ile = s.Length - od;
         string wynik = s.Substring(0, od);
         for (int i = od; i < s.Length - ile; i++)
         {

             wynik += s[i + ile];

         }
         return wynik.ToString();
     }
        /*** 
         * usuwa ile znakow od miejsca od w stringu s
         * i zwraca ten string
         * ***/
     private string usunZnaki(string s, int od, int ile)
     {
         if (s.Length < od) return s;
         if (od + ile > s.Length) ile = s.Length - od;
         string     wynik = s.Substring(0, od);
         for (int i = od; i < s.Length-ile; i++)
         {
             
             wynik+=s[i + ile];
         }
         return wynik.ToString();
     }
      
        [STAThread]
     void CopySTA(String d)//, Wordy.Paragraph oPara)
{
    Wordy.Paragraph oPara;
    oPara = doc.Content.Paragraphs.Add();
    
 Clipboard.SetText(d, TextDataFormat.Rtf);
 oPara.Range.PasteAndFormat(Wordy.WdRecoveryType.wdFormatOriginalFormatting);
//Thread.CurrentThread.Join();
}

     /// <summary>
     /// Wkleja w aktualne miejsce zawartość załącznika z EAP
     /// </summary>
     public void wstawZalacznikRTF(EA.Element element)
     {
         
         string d=element.GetLinkedDocument();
         if (d.Length == 0)
         {
             if (element.Notes.Length == 0)
             {
                 Wordy.Paragraph oPar = doc.Content.Paragraphs.Add();
                 oPar.Range.Text = "Do uzupełnienia";
                 oPar.Range.Comments.Add(oPar.Range, "Brak treści - pole do uzupełnienia");
                
             }
             return;
         }
       //  Wordy.Paragraph oPara1;
       //  oPara1 = doc.Content.Paragraphs.Add();
       //  oPara1.Range.Text = d;

        // Wordy.Paragraph oPara;
        // oPara = doc.Content.Paragraphs.Add();
       /*  String returnHtmlText = null;
         if(Clipboard.
         if (Clipboard.ContainsText(TextDataFormat.Rtf))
         {
             returnHtmlText = Clipboard.GetText(TextDataFormat.Rtf);
             Clipboard.Clear();
         }*/
      
       //  Clipboard.SetText(d, TextDataFormat.Rtf);

         Thread th=null;
         if (th != null)
         {
             if (th.IsAlive)th.Abort();
         }
             th=new Thread(()=>CopySTA(d));

             th.SetApartmentState(ApartmentState.STA);
            //th.ApartmentState = ApartmentState.STA ;

            th.Start();
            th.Join();
         //Clipboard.SetData(d,TextDataFormat.Rtf);
    //     oPara.Range.PasteAndFormat(Wordy.WdRecoveryType.wdFormatOriginalFormatting);
       // }

         /*
          * for (int i = 0; i < foundList.Count; i++)
{
    oPara[i] = oDoc.Content.Paragraphs.Add();
    string tempS = foundList[i].Paragraph;
    tempS = tempS.Replace("\\pard", "");
    tempS = tempS.Replace("\\par", "");
    Clipboard.SetText(tempS, TextDataFormat.Rtf);
    oPara[i].Range.InsertParagraphAfter();
    oPara[i].Range.Paste();
    oPara[i].KeepTogether = -1;
    oPara[i].Range.Font.Size = 10;
    oPara[i].Range.Font.Name = "Arial";
}
          * */
     }
     /*** 
      * Szuka pojedynczego znacznika w p zaczynajac od miejsca poczatek
      * zwraca jego indeks
      * ***/
     private int szukajZnacznikaHTML(int poczatek, String html, Wordy.Paragraph p)
        {
            return 0;
  /*       poczatek =p.Range.Text.IndexOf("<"+html+">",(int)poczatek);
         if ((int)poczatek < 0) 
         { poczatek = 0; }
         else
         {
             koniec = p.Range.Text.IndexOf("</" + html, (int)poczatek);
             if ((int)koniec < 0)
             { koniec = 0; }
             else
             {
                 //         p.Range.Text = p.Range.Text.Remove((int)poczatek, 3);
                 int dlstara = p.Range.Text.Length;
                 p.Range.Text = p.Range.Text.Substring((int)poczatek + 3, (int)koniec - (int)poczatek - 3);
                 koniec = (int)koniec - (dlstara - p.Range.Text.Length)+4;
             }
         }
   */     
   //      koniec =p.Range.Text.IndexOf("</"+html,(int)poczatek);
   //      if ((int)koniec < 0)
   //      { koniec = 0; }
   //      else
   //      {
            //p.Range.Text= p.Range.Text.Remove((int)koniec, 4);
        //    p.Range.Text.Replace("</" + html, "");
       //     koniec = (int)koniec - 4;
    //     }
        
         
    }
        private void formatujFragmentParagrafu(int poczatek, int koniec, String znacznik, ref Wordy.Paragraph p)
        {
            object pocz = poczatek + p.Range.Start;
            object kon = koniec + p.Range.Start;
            if ((int)kon < (int)pocz) return;
            Wordy.Range r = doc.Range(ref pocz, ref kon);
            r.Select();
            switch (znacznik.ToLower())
            {
                case "b":
                    r.Font.Bold = 1;
                    break;
               case "i":
                    r.Font.Italic = 1;
                    break;
               case "u":
                    //r.Font.Underline = ;
                    break;
                case "ul":
                case "ol":
                    break;
                case "li":
                    p.Range.Text = "*" + p.Range.Text;
                    break;

            }
           
        }
        private void formatujParagrafHTML(Wordy.Paragraph p,string txt)
        {
            object poczatek=0;
            object koniec=0;
            
          //  usunNieobslugiwaneZnacznikiHtml(ref poczatek, ref koniec, ref p, txt);
            poczatek = koniec = 0;
            szukajZnacznikowHTML(ref poczatek, ref koniec, "b", ref p,txt);
            poczatek = koniec = 0;
            szukajZnacznikowHTML(ref poczatek, ref koniec, "i", ref p, txt);
            poczatek = koniec = 0;
            szukajZnacznikowHTML(ref poczatek, ref koniec, "u", ref p, txt);
           
         //   szukajZnacznikowHTML(ref poczatek, ref koniec, "ol", ref p, txt);
            //poczatek = koniec = 0;
           // szukajZnacznikowHTML(ref poczatek, ref koniec, "li", ref p, txt);
            //poczatek = koniec = 0;
            //szukajZnacznikowHTML(ref poczatek, ref koniec, "ul", ref p, txt);
           
           
        }
        public Wordy.Paragraph wstawParagraf(String txt, string styl, Wordy.WdOutlineLevel poziom = Wordy.WdOutlineLevel.wdOutlineLevelBodyText,String komentarz="")
        {
            //Insert another paragraph.
	        Wordy.Paragraph oPara3;
           // zmiana test object oRng = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
	        //zmiana test oPara3 = doc.Content.Paragraphs.Add(ref oRng);
            oPara3 = doc.Content.Paragraphs.Add();
            oPara3.Range.set_Style(styl);
            //usunwamy znaczniki htmla z formatowania
            txt=usunNieobslugiwaneZnacznikiHtml(txt);
            txt=kowertujZnacznikiHTML(txt);
           oPara3.Range.Text = txt;
           if (komentarz != "")
           {
             //  if (txt == "")
               {
                   oPara3.Range.Comments.Add(oPara3.Range, komentarz);
               }
           }
     //       if (poziom != Wordy.WdOutlineLevel.wdOutlineLevelBodyText)
            {
                oPara3.OutlineLevel = poziom;
            }
           // formatujParagrafHTML( oPara3,txt);
            if (stylNorm == styl)
            {
                oPara3.Format.SpaceAfter = 0;
                oPara3.Format.SpaceBefore = 0;
            }
            if( stylPodpis==styl)
            {
                    //oPara3.Format.SpaceAfter = 24;
                    oPara3.Format.SpaceBefore = 0;
            }
            if( stylPodrozdz==styl)
            {
                 
                    //oPara3.Format.SpaceAfter = 24;
                    oPara3.Format.SpaceBefore = 36;
            }
            if (stylPodrozdz2 == styl)
            {
                //oPara3.Format.SpaceAfter = 24;
                oPara3.Format.SpaceBefore = 24;
            }
            if (stylPodrozdz3 == styl)
            {
                //oPara3.Format.SpaceAfter = 24;
                oPara3.Format.SpaceBefore = 12;
            } 
            if( stylRozdz==styl)
            {    
                    oPara3.Format.SpaceBefore = 48;
                

            }
	    //    oPara3.Format.SpaceAfter = 24;
            oPara3.Range.set_Style(styl);
	        oPara3.Range.InsertParagraphAfter();
            return oPara3;
        }
        public String kowertujZnacznikiHTML(String txt)
        {
            String wynik = txt.Replace("&lt;", "<");
           wynik= wynik.Replace("&gt;", ">");
            return wynik;
        }
        public void wstawTabele()
        {
            //Insert a 3 x 5 table, fill it with data, and make the first row
            //bold and italic.
            Wordy.Table oTable;
            Wordy.Range wrdRng = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oTable = doc.Tables.Add(wrdRng, 3, 5, ref oMissing, ref oMissing);
           // oTable.Range.ParagraphFormat.SpaceAfter = 6;
            object styleName = stylTab;
            oTable.set_Style(ref styleName); 
            int r, c;
            string strText;
            for (r = 1; r <= 3; r++)
                for (c = 1; c <= 5; c++)
                {
                    strText = "r" + r + "c" + c;
                    oTable.Cell(r, c).Range.Text = strText;
                    object snorm = stylNorm;
                    oTable.Cell(r, c).Range.set_Style(ref snorm);
                }
            //oTable.Rows[1].Range.Font.Bold = 1;
            //oTable.Rows[1].Range.Font.Italic = 1;
           
            
        }
    }
}
