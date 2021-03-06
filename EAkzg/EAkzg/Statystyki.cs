﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
	
using System.Collections.ObjectModel;
using System.Xml;



namespace EAkzg
{
    public partial class Statystyki : Form
    {
       
        private SynchronizationContext m_SynchronizationContext;
        string[] spis ={"1 ORGANIZACYJNE","1.1 Zawartość, cel i przeznaczenie dokumentu","1.2 Słownik pojęć","1.3 Załączniki, powiązane dokumenty","1.4 Zespół projektowy",
                            "2 PERSPEKTYWA FUNKCJONALNA","2.1 Krótki opis projektu","2.2 Wymagania biznesowe",
                            "3 OPIS ROZWIĄZANIA","3.1 Koncepcja ogólna","3.2 Architektura Statyczna","3.3 Architektura dynamiczna"};
        string[] spisIdk ={"r1","r1-1","r1-2","r1-3","r1-4",
                              "r2","r2-1","r2-2",
                               "r3","r3-1","r3-2","r3-3" };
        string[] spisCss ={"spis1","spis1-1","spis1-1","spis1-1","spis1-1",
                              "spis1","spis1-1","spis1-1",
                              "spis1","spis1-1","spis1-1","spis1-1"};
        public enum LogMsgType { WynikOK, WynikNOK, Normal, Info, Warning, Error,cd };
        private Color[] LogMsgTypeColor = { Color.Green, Color.Blue, Color.Black,Color.Black, Color.Orange, Color.Red,Color.Black };
        EA.Repository rep;

        bool generowanieBool=false;
        Word w;
        string sciezkaZrodlo ="";// @"D:\_Projekty\EAkzg\EAkzg\EAkzg\bin\Debug";
        Package wymaganiaPckg = null;
        Package aktorzyPckg = null;
     //   Package usecasePckg = null;
     //   Package sekwenjePckg = null;
        Package archStatPckg = null;
        Package definicjePckg = null;
        Package koncepcjaPckg = null;
        EA.Package projekt = null;
        EA.Project projektInterfejs;
        String sciezkaDocelowa;
        String logFile;
        String[] rozdzialy=new String[12+1];
        String stopkaHTML;
        String SpisTresciHTML;
        Thread watekGeneratora = null;
        DateTime dt_Start;
        CheckBox[] checkBoxy = new CheckBox[12];
        Label[] labele = new Label[12];
        private DateTime m_PreviousTime = DateTime.Now;
       

       
        CModel modelRepo;

         public delegate void DelLogPisz(LogMsgType msgtype,string msg);
        
        public Statystyki( EA.Repository repository)
        {
            m_SynchronizationContext = SynchronizationContext.Current;
            DelLogPisz LogPisz=Log;

            rep=repository;
            InitializeComponent();
            checkBoxy[0] = R1cb; checkBoxy[1] = R2cb; checkBoxy[2] = checkBoxIT; checkBoxy[3] = checkBoxNT; checkBoxy[4] = R5cb;
            checkBoxy[5] = R6cb; checkBoxy[6] = R7cb; checkBoxy[7] = R8cb; checkBoxy[8] = R9cb; checkBoxy[9] = R10cb;
            checkBoxy[10] = R11cb; checkBoxy[11] = R12cb;

            labele[0] = r1lbl; labele[1] = r2lbl; labele[2] = r3lbl; labele[3] = r4lbl; labele[4] = r5lbl;
            labele[5] = r6lbl; labele[6] = r7lbl; labele[7] = r8lbl; labele[8] = r9lbl; labele[9] = r10lbl;
            labele[10] = r11lbl; labele[11] = r12lbl;
           
            GUI_czysc(); 
        }

        /// <summary>
        /// inicjalne odczytanie modelu na starcie formularza
        /// </summary>
        private async void odczytajModelStart()
        {
            generujBtn.Visible = false;
            generowanieLbl.Text = "Czekaj, trwa wczytywanie modelu EA...";
             System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(() =>odczytajModelWatek());
             try
             {

                 await task;
             }
             catch (OperationCanceledException e)
             { //sprzatanie 
             }
             generujBtn.Visible = true;
             UstawStatystykiMini(/*pmodelRepo*/);
             generowanieLbl.Text = "Wybierz zakres generowanych rozdziałów.";
        }
        private void odczytajModelWatek()
        {
            projekt = EAUtils.dajModelPR(ref rep);
           // modelRepo=new CModel(ref rep);
        }
        private void odczytajModel()
        {
             
              projekt = EAUtils.dajModelPR(ref rep);
          //  projektInterfejs = rep.GetProjectInterface();
            Log(LogMsgType.Info, "Odczytany model: " + projekt.Name+"\n");

            modelRepo = new CModel(ref rep);
            UstawStatystyki(modelRepo);

            /* nowa wersja
             *  var count = 0;
             * foreach (Package package in projekt.Packages)
                 {
                     count += 1; //CountClasses(package);

                     if (package.Name == "Architektura Statyczna")
                     {
                         archStatPckg = package;
                         Log(LogMsgType.Info, "Odczytany pakiet: " + package.Name + "\n");
                     }
                     if (package.Name == "Definicje")
                     {
                         definicjePckg = package;
                         Log(LogMsgType.Info, "Odczytany pakiet: " + package.Name + "\n");
                     }
           
                     if (package.Name == "Wymagania")
                     {
                         wymaganiaPckg = package;
                         Log(LogMsgType.Info, "Odczytany pakiet: " + package.Name + "\n");
                     }
                     if (package.Name == "Aktorzy")
                     {
                         aktorzyPckg = package;
                         Log(LogMsgType.Info, "Odczytany pakiet: " + package.Name + "\n");
                     }
             
                 } 
             * nowa wersja */

        }
        private void odswiezGUI()
        {
            try
            {

               /* nowy generator
                * 
                  ustawText( Projekt_nazwaLbl, projekt.Name + " " + EAUtils.dajNazweProjektu(ref projekt));

                  ustawText(AutorLbl, "IT-" + EAUtils.dajAutoraProjektu(ref projekt, "SD IT") + "\n NT-" + EAUtils.dajAutoraProjektu(ref projekt, "SD NT"));
                       
                  ustawText(LiczbaPakietowLbl,"Aktorów: " + aktorzyPckg.Elements.Count.ToString() + "\n" +
                           "Wymagań: " + wymaganiaPckg.Elements.Count.ToString() + "\n");
                  * nowy generator*/
                ustawText(Projekt_nazwaLbl, modelRepo.dajNazweModelu() + " " + modelRepo.dajPelnaNazweProjektu());

                ustawText(AutorLbl, "IT-" + modelRepo.dajAutoraProjektu(CModel.IT) + "\n NT-" + modelRepo.dajAutoraProjektu(CModel.NT));

            //    ustawText(LiczbaPakietowLbl, "Aktorów: " + aktorzyPckg.Elements.Count.ToString() + "\n" +
              //           "Wymagań: " + wymaganiaPckg.Elements.Count.ToString() + "\n");
            }
            catch (Exception e)
            {
                MessageBox.Show("Wyjątek "+ e.Message.ToString()+" Statystyki.odswiezGUI() {" + wymaganiaPckg.ToString() + "}\n projekt:" + Projekt_nazwaLbl.Text + "\n autorzy:" + AutorLbl.Text);
            }
        }

    
        private void odczytRejestru()
        {
            const string userRoot = "HKEY_CURRENT_USER"; 
            const string subkey = @"SOFTWARE\Sparx Systems\EAAddins\EAkzg2";
            const string keyName = userRoot + "\\" + subkey;

            try
            {
               
                sciezkaZrodloLbl.Text = Registry.GetValue(keyName, "DLL", "D:\\").ToString();
                sciezkaZrodlo = sciezkaZrodloLbl.Text;

                sciezka_proj.Text = Registry.GetValue(keyName, "Proj", "D:\\").ToString();
            }

            catch (Exception exc)
            {

                Log(new CLog(LogMsgType.Error, "odczytRejestru - Wyjątek: " + exc.Message.ToString()));
            }
        }
        private void zapisRejestru()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = @"SOFTWARE\Sparx Systems\EAAddins\EAkzg2";
            const string keyName = userRoot + "\\" + subkey;

            try
            {

                Registry.SetValue(keyName, "DLL", sciezkaZrodloLbl.Text);
                Registry.SetValue(keyName, "Proj", sciezka_proj.Text);
            }

            catch (Exception exc)
            {

                Log(new CLog(LogMsgType.Error, "zapisRejestru - Wyjątek: " + exc.Message.ToString()));
            }
        }


        private void Statystyki_Load(object sender, EventArgs e)
        {
            Stopwatch st = new Stopwatch();
            st.Start();

            
            odczytajModelStart();
            Log(Statystyki.LogMsgType.Info, "Odczyt modelu " + st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + "\n");

           
            st.Stop();
            try
            {
               ///KZG zmiana 08-09-2015
               

              //  string registry_key = @"HKEY_CURRENT_USER\SOFTWARE\Sparx Systems\EAAddins\EAkzg2\";
               /** var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                
                //using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
                using (RegistryKey key = hklm.OpenSubKey(registry_key,true))
                {


                    sciezkaZrodloLbl.Text = key.GetValue("DLL", "D:\\").ToString();
                    sciezkaZrodlo = sciezkaZrodloLbl.Text;
                    // key.Close();
                    sciezka_proj.Text = key.GetValue("Proj", "D:\\").ToString();
                    key.Close();
                }*/

                odczytRejestru();

            }
            catch (Exception exc)
            {

             //   Log(new CLog(LogMsgType.Error, "Odczyt rejestru - Wyjątek: " + exc.Message.ToString()));
                // timer1.Enabled = false;
             //   MessageBox.Show("Błąd odczytu rejestru, sprawdź swoje uprawnienia: " + exc.Message);
             
            }
          //KZG koniec zmian 08-09-2015
        //  odswiezGUI();
          publikujBtn.Enabled = true;////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!<--- do wywalenia
       
        }

        private void HtmlRtb_VScroll(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = sciezka_proj.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sciezka_proj.Text = folderBrowserDialog1.SelectedPath + "\\";
             /* KZG 08-09-2015
                string registry_key = @"SOFTWARE\Sparx Systems\EAAddins\EAkzg\";
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registry_key))
                {

                    key.SetValue("Proj", sciezka_proj.Text);
                    key.Close();
                }
              */
                zapisRejestru();
                //KZG koniec 08-09-2015
              
            }
        }

        private string doklejPlik(string plik,params string[] p)
        {
           // String tresc= System.IO.File.ReadAllText(sciezkaZrodlo + "\\" + plik);
            String tresc = txt.dajTekst(plik);
            String wynik = "";
           
            

            string[] lista = tresc.Split(new string[] { "^@^" }, StringSplitOptions.None);
          
            for (int i = 0; i < p.Length; i++)
            {
                wynik += lista[i] + p[i];
            }
            wynik += lista[lista.Length -1];
            return wynik;
        }
        private void GUI_czysc()
        {
            
            for (int i = 0; i < checkBoxy.Length; i++)
            {
                ustawColor(checkBoxy[i], SystemColors.Control);
                ustawText(labele[i], "");
            }
            ustawText(generowanieLbl, "Generowanie");
           
         
      
        }
       
        private void ustawText(Control o, String t)
        {
            if (o.InvokeRequired)
            {
                o.Invoke(new MethodInvoker(() => { o.Text = t; }));
            }
            else
            {
                o.Text = t;
            }

        }
        private void ustawVisible(Control o, bool t)
        {
            if (o.InvokeRequired)
            {
                o.Invoke(new MethodInvoker(delegate() { o.Visible = t; }));
            }
            else
            {
                o.Visible = t;
            }

        }
        private void ustawColor(Control o, Color t)
        {
            if (o.InvokeRequired)
            {
                o.Invoke(new MethodInvoker(delegate() { o.BackColor = t; }));
            }
            else
            {
                o.BackColor = t;
            }

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            generujWatekAsync();
            return;
          
            //    Thread watekGeneratora;
            if (watekGeneratora!=null)
            {
                if (watekGeneratora.IsAlive)
                {
                    watekGeneratora.Abort();
                    ustawText(generowanieLbl, "Generowanie");
                    Log(LogMsgType.Warning, "Generowanie wstrzymane");
                }
                watekGeneratora = null;
                return;
               
            }
       
           // backgroundWorker1.RunWorkerAsync();


         //   watekGeneratora = new Thread(() => generujWatek());
      //      watekGeneratora = new System.Threading.Thread(generujWatek);
         
           
            
            watekGeneratora.Name = "Wątek generatora HLD";
           watekGeneratora.IsBackground = true;
       //     watekGeneratora.Priority = ThreadPriority.Normal;
            watekGeneratora.Start();
           
           
        }
     
      
        private Element DajElement(Package p, int elId)
        {
            Element el=null;
            foreach(Element elem in p.Elements)
            {
                if (elem.ElementID == elId)
                {
                    el = elem;
                    return el;
                }
            }
            return el;
        }
      /*  private String ArchitekturaStatyczna(Package arch)
        {
            String wynik = "<h2>3.2 Architektura statyczna</h2>";
            int i = 1;
            if (arch.Diagrams.Count > 0)
            {
                
                foreach (Diagram diag in arch.Diagrams)
                {
                    wynik += "<div class=\"img\">";
                    wynik += "<h3>3.2." + i + " " + diag.Name + "</h3>";
                    String plik = sciezka_proj.Text + "img/" + diag.Name + ".png";
                    projektInterfejs.PutDiagramImageToFile(diag.DiagramGUID, plik, 1);
                    wynik += "<img src='" + plik + "'>";
                    wynik += "<div class=\"desc\">Diagram architektury statycznej "+i+". "+diag.Notes+"</div>";
                    wynik +="<table><tr><th>Lp</th><th>Nazwa systemu</th><th>Typ zmian</th><th>Osoba odpowiedzialna</th></tr>";
                    int j=1;
                    foreach (IDualDiagramObject el in diag.DiagramObjects)
                    {
                        Element elem = DajElement(arch, el.ElementID);
                        
                        wynik += "<tr><td>" + j + "</td><td>" + elem.Name + "</td><td> 'todo' </td><td> 'todo'</td></tr>";
                        j++;
                    }
                    wynik += "</table>";
                    wynik+="<br> diagram object liczba " + diag.DiagramObjects.Count.ToString();
                  /*  foreach (IDualDiagramLink el in diag.DiagramLinks)
                    {
                        wynik += "<tr><td>" + j + "</td><td>" + el.ConnectorID + "</td><td> 'todo' </td><td> 'todo'</td></tr>";
                    }
                    wynik += "<p> diagram links liczba " + diag.DiagramLinks.Count.ToString();
                   */
        /*            i++;
                    wynik += "</div><br>";
                }

            }
            return wynik;
        }*/
     /*   private string ListaWymagan(Collection c)
        {
            String s = "";
            int i=0;
            foreach (Element el in c)
            {i++;
                s+="<tr><td>"+i+"</td><td>"+el.Name+" </td><td>"+el.Notes+"</td></tr>";
            }
            s += "</table>";
            return s;

        }
        */
        private void HtmlRtb_TextChanged(object sender, EventArgs e)
        {
            //webBrowser1.DocumentText = HtmlRtb.Text;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void EksportujPliki()
        {

            System.IO.File.WriteAllText(sciezkaDocelowa + "\\spisTresci.html", SpisTresciHTML);
            int i = 0;
            foreach (String s in rozdzialy)
            {

                System.IO.File.WriteAllText(sciezkaDocelowa + "\\" + "r" + i + ".html", rozdzialy[i]);
                i++;
            }
            publikujBtn.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
          
        }
        public void Loguj(String s)
        {
            LogRtb.Text += s + "\n";
        }
        private void sciezkaEksportTB_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = hostTB.Text;
            folderBrowserDialog1.ShowDialog();
            hostTB.Text = folderBrowserDialog1.SelectedPath + "\\";
        }
        private void doklejwierszListy(FtpClient ftp,String folder,String opis,int typ)
        {
            String ktoryPlik = "";
            try
            {
                
                if (typ == 0)
                {
                    ktoryPlik = this.sciezkaPublikujLbl.Text + "lista.html";
                }
                else {
                    ktoryPlik = this.sciezkaPublikujLbl.Text + folder + "\\lista.html";
                }

                if (ftp == null)
                {
                    System.IO.File.Copy(ktoryPlik, sciezkaDocelowa + "lista.html");
                }
                else
                {

                    ftp.DownloadFiles(sciezkaDocelowa, "lista.html");
                }
                String[] zawart = System.IO.File.ReadAllLines(sciezkaDocelowa + "lista.html");
                int i = 0;
                for (i = 0; i < zawart.Length; i++)
                {
                    if (zawart[i].IndexOf("<ul>") >= 0) break;
                }
                if (typ == 1)
                {
                    zawart[i + 1] = "<li><a href='" + folder + "/index.html'>" + opis + "</a></li>\n" + zawart[i + 1];
                }
                else {
                    zawart[i + 1] = "<li><a href='" + folder + "/lista.html'>" + opis + "</a></li>\n"+ zawart[i + 1];
                }
                    System.IO.File.WriteAllLines(sciezkaDocelowa + "lista.html", zawart, Encoding.UTF8);
                    if (ftp == null)
                    {
                        System.IO.File.Copy(sciezkaDocelowa + "lista.html",ktoryPlik );
                    }
                    else
                    {
                        ftp.UploadFiles(sciezkaDocelowa + "lista.html");
                    }
            }
            catch 
            {
                String t = "";
                if(typ==1)
                {
                    t+="<!DOCTYPE html>\n<html>\n<head>\n";
                    t += "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" >\n";
                    t += "<link rel=\"stylesheet\" type=\"text/css\" href=\"css/styl.css\">\n";
                    t += "</head>\n<body>\n<div class=\"img\">\n";
                   //   t += "<h1>Dokumentacja projektowa, repozytorium dokumentów HLD</h1>";
                   // t += "<h2>Obecnie opublikowane projekty</h2>";
                    t += "<h1>Dokumentacja projektu " + projekt.Name + "</h1>";
                    t += "<h2> Obecnie opublikowane wersje HLD</h2>";
                    t += "<a href='../'>Powrót do listy projektów </a>";
                    t += "\n<ul>\n<li><a href='" + folder + "/index.html'>" + opis + "</a></li>\n</ul>";
                    t += "<img src='img/logo.png'>";
                    t += "\n</div></body></html>";
                }
                else
                {
                    
                    t += "\n<ul>\n<li><a href='" + folder + "/lista.html'>" + opis + "</a></li>\n</ul>";
                }
              
           
                System.IO.File.WriteAllText(sciezkaDocelowa + "lista.html", t);
                if (ftp == null)
                {
                    System.IO.File.Copy(sciezkaDocelowa + "lista.html", ktoryPlik);
                }
                else
                {
                    ftp.UploadFiles(sciezkaDocelowa + "lista.html");
                }
            }
        }
        public void wyslijdoFTP()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            //kopia z sciezkaDocelowa, do sciezkaEksportTB.Text
            //       FTPClass ftp = new FTPClass();
            //    ftp.KonfigurujPolaczenie(@"ftp:\\ciekawi-swiata.ugu.pl\\HLD\\","ciekawi-swiata.ugu.pl","haslo123");
            //     ftp.wyslij(@"D:\_Projekty\EAkzg\PR-NNN\img\test.png");
            FtpClient ftp = new FtpClient(this);
            ftp.Host = hostTB.Text;// "ftp://ciekawi-swiata.ugu.pl/";
            ftp.Username = userTB.Text;// "ciekawi-swiata.ugu.pl";
            ftp.Password = passTB.Text;// "haslo123";

            Loguj("Połączono");

          //  ftp.ChangeDirectory("HLD");

            if (!ftp.DirectoryExists(projekt.Name))
            {
                ftp.CreateDirectory(projekt.Name);
                doklejwierszListy(ftp, projekt.Name, projekt.Name + " " + projekt.Notes,0);

            } 
            ftp.ChangeDirectory(projekt.Name);
            DateTime dt = DateTime.Now;
            //DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
              //DateTimeFormatInfo myDTFI=dt.GetDateTimeFormats;
            String folder = projekt.Name + "_" + dt.ToString("s");
            folder=folder.Replace(":", "_");
                //+ dt.Year;
            //if (dt.Month < 10) folder += "0"; folder += dt.Month;
            //if (dt.Day < 10) folder += "0";dt.ToFileTimeUtc
            //folder+= dt.Day+"-";
            ftp.CreateDirectory(folder);
            progressBar1.Value++;
            doklejwierszListy(ftp,folder,folder,1);

            ftp.ChangeDirectory(folder);
            //sciezkaDocelowa = @"D:\_Projekty\EAkzg\PR-NNN\";

            string[] dd = System.IO.Directory.GetDirectories(sciezkaDocelowa);
            progressBar1.Maximum = 2+(int)System.IO.Directory.EnumerateFiles(sciezkaDocelowa,"*", SearchOption.AllDirectories).Count();
            progressBar1.Value++;
            foreach (String d in dd)
            {
                
                String dx = d.Substring(d.LastIndexOf("\\") + 1, d.Length - d.LastIndexOf("\\") - 1);
               // if (dx != "css" && dx != "js") // tych folderów nie kopiuj
               // {
                    ftp.CreateDirectory(dx);
                    ftp.ChangeDirectory(dx);
                    string[] ff = System.IO.Directory.GetFiles(d);
                    foreach (String f in ff)
                    {
                        ftp.UploadFiles(f);
                        progressBar1.Value++;
                    }
                    ftp.ChangeDirectory("..");
                    //ftp.UploadFiles(@"D:\_Projekty\EAkzg\PR-NNN\img\test.png");
               // }
            }
            string[] fff = System.IO.Directory.GetFiles(sciezkaDocelowa);
            foreach (String f in fff)
            {
                ftp.UploadFiles(f);
                progressBar1.Value++;
            }
            ftp.ChangeDirectory("..");
            ftp.CreateDirectory("css");
            ftp.ChangeDirectory("css");
            foreach(String d in System.IO.Directory.GetFiles(sciezkaDocelowa+"css"))
            {
                ftp.UploadFiles(d);
            }
            ftp.ChangeDirectory("..");
            ftp.CreateDirectory("js");
            ftp.ChangeDirectory("js");
            foreach (String d in System.IO.Directory.GetFiles(sciezkaDocelowa + "js"))
            {
                ftp.UploadFiles(d);
            }
                progressBar1.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            LogRtb.Text = "";
            wyslijdoFTP();

            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = sciezkaZrodloLbl.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sciezkaZrodloLbl.Text = folderBrowserDialog1.SelectedPath + "\\";
              /* KZG poczatek
                string registry_key = @"SOFTWARE\Sparx Systems\EAAddins\EAkzg\";
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registry_key))
                {

                    key.GetValue("DLL", sciezkaZrodloLbl.Text);

                    key.Close();
                }
               */
                zapisRejestru();
                //KZG koniec 08-09-2015
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = sciezkaPublikujLbl.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sciezkaPublikujLbl.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            wyslijdoFolderu();

            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        public void wyslijdoFolderu()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
         //   using (new Impersonator(userBezFtpTB.Text, domenaBezFtpTB.Text, hasloBezFtpTb.Text))
            {
                //kopia z sciezkaDocelowa, do sciezkaEksportTB.Text
                //       FTPClass ftp = new FTPClass();
                //    ftp.KonfigurujPolaczenie(@"ftp:\\ciekawi-swiata.ugu.pl\\HLD\\","ciekawi-swiata.ugu.pl","haslo123");
                //     ftp.wyslij(@"D:\_Projekty\EAkzg\PR-NNN\img\test.png");
           //     FtpClient ftp = new FtpClient();
                //  ftp.Host = hostTB.Text;// "ftp://ciekawi-swiata.ugu.pl/";
                // ftp.Username = userTB.Text;// "ciekawi-swiata.ugu.pl";
                // ftp.Password = passTB.Text;// "haslo123";
                // ftp.ChangeDirectory("HLD");
                //if (!ftp.DirectoryExists(projekt.Name))
                String sciez=sciezkaPublikujLbl.Text+projekt.Name;
               if (!System.IO.Directory.Exists(sciez))
               {
                   System.IO.Directory.CreateDirectory(sciez);
                    //ftp.CreateDirectory(projekt.Name);
                    doklejwierszListy(null, projekt.Name, projekt.Name + " " + projekt.Notes, 0);

                }
              //  ftp.ChangeDirectory(projekt.Name);
                DateTime dt = DateTime.Now;
                String folder = projekt.Name + "_" + dt.ToString("s");

           //     ftp.CreateDirectory(folder);
                String folderExt = sciez + "\\" + folder;
                System.IO.Directory.CreateDirectory(folderExt);
                progressBar1.Value++;
                doklejwierszListy(null, folder, folder, 1);

                //ftp.ChangeDirectory(folder);

                string[] dd = System.IO.Directory.GetDirectories(sciezkaDocelowa);
                progressBar1.Maximum = 2 + (int)System.IO.Directory.EnumerateFiles(sciezkaDocelowa, "*", SearchOption.AllDirectories).Count();
                progressBar1.Value++;
                foreach (String d in dd)
                {

                    String dx = d.Substring(d.LastIndexOf("\\") + 1, d.Length - d.LastIndexOf("\\") - 1);
                  //  if (dx != "css" && dx != "js") // tych folderów nie kopiuj
                    {
                        //ftp.CreateDirectory(dx);
                        //ftp.ChangeDirectory(dx);
                        System.IO.Directory.CreateDirectory(folderExt + "\\" + dx);
                        string[] ff = System.IO.Directory.GetFiles(d);
                        foreach (String f in ff)
                        {
                          //  ftp.UploadFiles(f);
                            System.IO.File.Copy(sciezkaDocelowa + "\\" + dx + "\\" + f, folderExt + "\\" + dx + "\\" + f);
                            progressBar1.Value++;
                        }
                      //  ftp.ChangeDirectory("..");

                    }
                }
                string[] fff = System.IO.Directory.GetFiles(sciezkaDocelowa);
                foreach (String f in fff)
                {
                   // ftp.UploadFiles(f);
                    System.IO.File.Copy(sciezkaDocelowa  + "\\" + f, folderExt  + "\\" + f);
                           
                    progressBar1.Value++;
                }
            }
                progressBar1.Visible = false;
            
        }

       
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

         ///   for (int i = 1; i <= 10; i++)
         //   {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                  //  break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                  //  System.Threading.Thread.Sleep(500);
                   // worker.ReportProgress(i * 10);
      //              generujWatek(/*worker*/);
                }
           // }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Log(LogMsgType.Warning, e.ProgressPercentage.ToString()+"\n");
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log(LogMsgType.Warning,   "Koniec \n");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label6_Click(null, null);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label9_Click(null, null);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }
         enum Kolor {Blad,Warning,Info,Zwykly};
        static void LogF(string logMessage, TextWriter txtWriter)
        {
            try
            {
              //  txtWriter.Write("\r\nLog : ");
                txtWriter.WriteLine("Log - {0} {1}: \t\t {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(),logMessage);
                //txtWriter.Write("  :");
                //txtWriter.WriteLine("  :{0}", logMessage);
                //txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
        void LogWrite(string logMessage)
        {

            try
            {
                using (StreamWriter w = System.IO.File.AppendText(logFile))
                {
                    LogF(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void Log(LogMsgType msgtype, string msg)
         {
             try
             {

                 logRTF.Invoke(new EventHandler(delegate
                 {
                     logRTF.SelectedText = string.Empty;
                     if (msgtype == LogMsgType.WynikNOK || msgtype == LogMsgType.WynikOK|| msgtype==LogMsgType.Info)
                     {
                         logRTF.SelectionFont = new Font(logRTF.SelectionFont, FontStyle.Regular);

                         if (msgtype == LogMsgType.Info)
                         {
                             msg = "## " + DateTime.Now.ToLongTimeString() + ": " + msg;
                         }
                     }
                     else
                     {
                         if (msgtype == LogMsgType.cd)
                         {
                             msg = " " + msg;
                         }
                         else
                         {
                             logRTF.SelectionFont = new Font(logRTF.SelectionFont, FontStyle.Bold);
                             msg = "# " + DateTime.Now.ToLongTimeString() + ": " + msg;
                         }
                     }
                     logRTF.SelectionColor = LogMsgTypeColor[(int)msgtype];
                    
                     logRTF.AppendText(msg);
                     logRTF.ScrollToCaret();
                 }));
                LogWrite(msg);
             }
             catch (Exception)
             {
             }
         }
        private void Log1(String t, Kolor k=Kolor.Zwykly,bool nowy=true)
        {
            DateTime czas = DateTime.Now;
          
            Color kolor = Color.Black;
            bool bold = false;
            if (k == Kolor.Blad) { kolor = Color.Red; bold = true; }
            if (k == Kolor.Info) { kolor = Color.Black; bold = true; }
            if (k == Kolor.Warning) { kolor = Color.Orange; bold = true; }
            if (k == Kolor.Zwykly) { kolor = Color.Black; bold = false; }

            String txt = "";
            if (bold) txt += @"\b";
            if (nowy) txt += czas.ToLongTimeString();
            txt += t; 
           
            if (bold) txt += @"\b0";
            if (logRTF.InvokeRequired)
            {
                logRTF.Invoke(new EventHandler(delegate
                {
                    int length = logRTF.TextLength;  // at end of text
                    logRTF.AppendText(txt);
                    logRTF.SelectionStart = length;
                    logRTF.SelectionLength = txt.Length;

                    logRTF.SelectionColor = kolor;
                    logRTF.SelectionStart = logRTF.TextLength;
                    logRTF.ScrollToCaret();
                }));
            }
            else 
            {
                int length = logRTF.TextLength;  // at end of text
                logRTF.AppendText(txt);
                logRTF.SelectionStart = length;
                logRTF.SelectionLength = txt.Length;
                logRTF.SelectionColor = kolor;
                logRTF.SelectionStart = logRTF.TextLength;
                logRTF.ScrollToCaret();
            }

            
 
        }
        public void Report(Tuple<int, int> value)
        {
            DateTime now = DateTime.Now;

            if ((now - m_PreviousTime).Milliseconds > 20)
            {
                m_SynchronizationContext.Post((@object) =>
                {
                    Tuple<int, int> minMax = (Tuple<int, int>)@object;
                    progressBar2.Maximum = minMax.Item1;
                    progressBar2.Value = minMax.Item2;
                }, value);

                m_PreviousTime = now;
            }
        }
        private void ProgressBarKrok()
        {
             m_SynchronizationContext.Post((@object) =>
                {
                   progressBar2.Value++;
                },null);
        }
        private void wordBtn_Click(object sender, EventArgs e)
        {
            if (generowanieBool)
            {
                generowanieBool = false;
                source.Cancel();
                wordBtn.Visible = false;
                generujBtn.Visible = true;
                progressBar2.Visible = false;
                timer1.Enabled = false;
                generowanieLbl.Text += " proces zatrzymano.";
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                
            }

        }
        CancellationTokenSource source; 
        private async void generujWatekAsync()
        {
           
            if (!generowanieBool)
            {
                generujBtn.Visible = false;
                wordBtn.Visible = true;
                generowanieBool = true;
                progressBar2.Visible = true;
                progressBar2.Value = 0;
                progressBar2.Maximum = 20;
                source = new CancellationTokenSource();
                timer1.Enabled = true;
                timer1.Interval = 100;
              //  generujBtn.Text = "Wstrzymaj";
                dt_Start = DateTime.Now;
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                GUI_czysc();
                bool jezykPolski = rbPolski.Checked;
                bool pokaWord = pokaWordCB.Checked;
                sciezkaZrodlo = sciezkaZrodloLbl.Text;
                sciezkaDocelowa = sciezka_proj.Text + projekt.Name + "\\";
                logFile = sciezkaDocelowa + "log_" + projekt.Name + ".txt";
                //await System.Threading.Tasks.Task.Run(() => ( /*generujWatek();*/dlugapetla(source.Token),source.Token);
                //  Task<int> task = System.Threading.Tasks.Task.Run(() => dlugapetla( source.Token), source.Token);
                System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(() => generujWatek(jezykPolski,sciezkaZrodlo,sciezkaDocelowa,pokaWord,source.Token), source.Token);
                try
                {

                    await task;
                }
                catch (OperationCanceledException e)
                { //sprzatanie 
                    generowanieBool = false;
                    source.Cancel();
                    wordBtn.Visible = false;
                    generujBtn.Visible = true;
                    progressBar2.Visible = false; 
                    timer1.Enabled = false;
                    progressBar2.Visible = false;
                   
                    generowanieLbl.Text += " - błąd.";
                    System.Windows.Forms.Cursor.Current = Cursors.Default;
                    return;
                }
               // generujBtn.Text = "Generuj";
                progressBar2.Visible = false;
                timer1.Enabled = false;
                generowanieLbl.Text += " HLD wygenerowane.";
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                TimeSpan span = DateTime.Now.Subtract(dt_Start);
                MessageBox.Show("Generowanie HLD zakończone, czas generowania: " + span.ToString(@"hh\:mm\:ss\.ff"));
                wordBtn.Visible = false;
                generujBtn.Visible = true;
                GUI_czysc();
            }
        }
        private void KolorujCB(Tuple<int, int> value)
        {
            m_SynchronizationContext.Post((@object) =>
            {
                Tuple<int, int> intkol = (Tuple<int, int>)@object;
                Color c;
                if(intkol.Item2==0){c=Color.Green;}
                else {c=Color.Red;}
                checkBoxy[intkol.Item1].BackColor =c ;
            }, value);
                
        }
          
          private void UstawLblCzas(int ktory)
          {
               m_SynchronizationContext.Post((@object) =>
            {
            DateTime dt_Teraz = DateTime.Now;
            TimeSpan span = dt_Teraz.Subtract(dt_Start);
            labele[ktory].Text= " czas generowania: " + span.ToString(@"hh\:mm\:ss\.ff");
            }, ktory);
         }
            /// <summary>
            /// log z synchronizacja watkow
            /// </summary>
            /// <param name="l">obiekt CLog</param>
          public void Log(CLog l)
          {
              m_SynchronizationContext.Post((@object) =>
              {
                  Log(l.typ, l.txt);
              }, l);
          }
        private int dlugapetla(CancellationToken cancellationToken) 
        {
            int i = 0;
            int j=0;
            while (i<1000)
            {
                Thread.Sleep(100);
                if (i % 30 == 0)
                {
                    if (j < 12)
                    {
                        KolorujCB(new Tuple<int, int>(j,0));
                        UstawLblCzas(j);
                        j++;
                    }
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(cancellationToken);
                    return -1;
                }
                //cancellationToken.ThrowIfCancellationRequested();
                Report(new Tuple<int, int>(1000, i++));
            }
            return 0;
        }
        public class CLog
        {
            public LogMsgType typ;
            public String txt;
           public CLog(LogMsgType typL, String txtL)
            {
                typ = typL;
                txt = txtL;
            }
        }
        private void UstawStatystykiMini(/*CModel modelProjektu*/)
        {
            m_SynchronizationContext.Post((@object) =>
            {
                Package Rootpckg=EAUtils.dajModelPR(ref rep);
                Package HLDpckg = EAUtils.dajPakietSciezkiP(ref Rootpckg, "HLD");
               
                Projekt_nazwaLbl.Text = EAUtils.dajNazweProjektu(ref HLDpckg);
                AutorLbl.Text = "SD IT - " + EAUtils.dajAutoraProjektu(ref Rootpckg, "SD IT") + "\nSD NT - " + EAUtils.dajAutoraProjektu(ref Rootpckg, "SD NT");
                LiczbaPakietowLbl.Text = "";
                String sql = "select status, count(*) as c from t_object where object_type='Requirement' group by status";
                try
                {
                 //   Log(LogMsgType.Info, sql);
                    String wynikSQL = rep.SQLQuery(sql);
                    //Log(LogMsgType.Info, wynikSQL);
                   // chartReq.Series.Add(wynikSQL);
                    DataSet ds = new DataSet();
                    //ds.ReadXml(@"..\..\Departments.xml");
                    ds.ReadXml(XmlReader.Create(new StringReader(wynikSQL)));


                    
                        // Set chart data source
                    DataTable dt = ds.Tables["Row"];
                           chartReq.DataSource = dt;
                    Series serie1 = new Series();
                    serie1.Name = "Statusy Requirement";
                    serie1.Color = Color.FromArgb(112, 255, 200);
                    serie1.BorderColor = Color.FromArgb(164, 164, 164);
                    serie1.ChartType = SeriesChartType.Column;
                    serie1.BorderDashStyle = ChartDashStyle.Solid;
                    serie1.BorderWidth = 1;
                    serie1.ShadowColor = Color.FromArgb(128, 128, 128);
                    serie1.ShadowOffset = 1;
                    serie1.IsValueShownAsLabel = true;
                    serie1.XValueMember = "status";
                    serie1.YValueMembers = "c";
                    serie1.Font = new Font("Tahoma", 8.0f);
                    serie1.BackSecondaryColor = Color.FromArgb(0, 102, 153);
                    serie1.LabelForeColor = Color.FromArgb(150, 150, 100);
                    chartReq.Series.Add(serie1);

                     sql = "select status, count(*) as c from t_object where object_type='Feature' group by status";
                     wynikSQL = rep.SQLQuery(sql);
                     DataSet ds2 = new DataSet();
                     ds2.ReadXml(XmlReader.Create(new StringReader(wynikSQL)));

                     DataTable dt2 = ds2.Tables["Row"];
                     chartFeature.DataSource = dt2;
                     Series serie2 = new Series();
                     serie2.Name = "Statusy Feature";
                     serie2.Color = Color.FromArgb(200, 255, 112);
                     serie2.BorderColor = Color.FromArgb(164, 164, 164);
                     serie2.ChartType = SeriesChartType.Column;
                     serie2.BorderDashStyle = ChartDashStyle.Solid;
                     serie2.BorderWidth = 1;
                     serie2.ShadowColor = Color.FromArgb(128, 128, 128);
                     serie2.ShadowOffset = 1;
                     serie2.IsValueShownAsLabel = true;
                     serie2.XValueMember = "status";
                     serie2.YValueMembers = "c";
                     serie2.Font = new Font("Tahoma", 8.0f);
                     serie2.BackSecondaryColor = Color.FromArgb(0, 102, 153);
                     serie2.LabelForeColor = Color.FromArgb(100, 150, 150);
                     chartFeature.Series.Add(serie2);
                
                               
                         
                    }
               
                catch (Exception e)
                {
                    Log(LogMsgType.Error, "SQL error Load: " + e.Message+" EA:"+rep.GetLastError());
                }
                /*"Liczba wymagań biznesowych: " + modelProjektu.WymaganiaBiznesoweLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba wymagań architektonicznych: " + modelProjektu.WymaganiaArchitektoniczneLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba wymagań infrastruktury: " + modelProjektu.WymaganiaInfrastrukturaLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba kwestii otwartych: " + (modelProjektu.OgraniczeniaPckg.Elements.Count + modelProjektu.ListaIssue.Count).ToString() + "\n";
                LiczbaPakietowLbl.Text += "Liczba modyfikowanych obszarów IT: " + modelProjektu.WkladyPckg[CModel.IT].Packages.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba modyfikowanych obszarów NT: " + modelProjektu.WkladyPckg[CModel.NT].Packages.Count + "\n";
                 * */
            },null/* modelProjektu*/);
        }
        private void UstawStatystyki(CModel modelProjektu)
        {
            m_SynchronizationContext.Post((@object) =>
            {
               
                Projekt_nazwaLbl.Text = modelProjektu.dajPelnaNazweProjektu();
                AutorLbl.Text = "SD IT - " + modelProjektu.dajAutoraProjektu(CModel.IT) + "\nSD NT - " + modelProjektu.dajAutoraProjektu(CModel.NT);
                LiczbaPakietowLbl.Text = "Liczba wymagań biznesowych: " + modelProjektu.WymaganiaBiznesoweLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba wymagań architektonicznych: " + modelProjektu.WymaganiaArchitektoniczneLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba wymagań infrastruktury: " + modelProjektu.WymaganiaInfrastrukturaLista.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba kwestii otwartych: " + (modelProjektu.OgraniczeniaPckg.Elements.Count+modelProjektu.ListaIssue.Count).ToString() + "\n";
                LiczbaPakietowLbl.Text += "Liczba modyfikowanych obszarów IT: " + modelProjektu.WkladyPckg[CModel.IT].Packages.Count + "\n";
                LiczbaPakietowLbl.Text += "Liczba modyfikowanych obszarów NT: " + modelProjektu.WkladyPckg[CModel.NT].Packages.Count + "\n";
            }, modelProjektu);
        }
        private DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
        //  private void generujWatek()
        private void generujWatek(/*BackgroundWorker worker=null*/bool jezykPolski, String sciezkaZrodlo, String sciezkaDocelowa, bool pokWord, CancellationToken cancellationToken)
    {
       try
            {
                Stopwatch st = new Stopwatch();
                st.Start();

               // Log(LogMsgType.Normal,"Rozpoczęcie generowania HLD\n");
           Log(new CLog(LogMsgType.Normal,"Rozpoczęcie generowania HLD\n"));
           Log(new CLog(LogMsgType.Normal,"Wersja z dnia: "+ RetrieveLinkerTimestamp().ToLongDateString() + " " + RetrieveLinkerTimestamp().ToLongTimeString()));
                ProgressBarKrok();
           if (cancellationToken.IsCancellationRequested)throw new OperationCanceledException(cancellationToken);

            /*
             * nowy generator
             * EAUtils.utworzPustyModel(ref rep);
             * nowy generator */
              if (modelRepo!=null)
              {
                  //modelRepo.odswiezModel();
              }
              else 
              {
                  modelRepo = new CModel(ref rep);
              }
              ProgressBarKrok();
             
              if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
            
           if (jezykPolski)
                {
                    Log(new CLog(LogMsgType.Normal, "Język generowania - Polski\n"));
                }
                else {
                    Log(new CLog(LogMsgType.Normal, "Język generowania - Angielski\n"));
                }
        
                Log(new CLog(LogMsgType.WynikOK," GUI [init] \n"));
                odczytajModel();
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
         
                Log(new CLog(LogMsgType.WynikOK, " model [odczytany] \n"));
             //   odswiezGUI();
                Log(new CLog(LogMsgType.WynikOK, " GUI [ok] \n"));
                   Image logo = Properties.Resources.logo;


                System.IO.Directory.CreateDirectory(sciezkaDocelowa + "img\\");
                logo.Save(sciezkaDocelowa + "img\\logo.png");
                //  System.IO.File.Copy(sciezkaZrodlo+"\\logo.png",sciezkaDocelowa+"img\\logo.png",true);
                var js = Properties.Resources.skrypt;
                System.IO.Directory.CreateDirectory(sciezkaDocelowa + "js\\");
                System.IO.File.WriteAllText(sciezkaDocelowa + "js\\skrypt.js", js);
                //System.IO.File.Copy(sciezkaZrodlo + "\\skrypt.js", sciezkaDocelowa + "js\\skrypt.js", true);
                var css = Properties.Resources.styl;
                System.IO.Directory.CreateDirectory(sciezkaDocelowa + "css\\");
                System.IO.File.WriteAllText(sciezkaDocelowa + "css\\styl.css", css);
               Log(new CLog(LogMsgType.WynikOK, " folery [ok] "));
                //   System.IO.File.Copy(sciezkaZrodlo + "\\styl.css", sciezkaDocelowa + "css\\styl.css", true);
               ProgressBarKrok();
               if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                //  String t0 = "";

               DateTime teraz = DateTime.Now;
               String format = "yyyy-MM-dd@HH-mm-ss"; 
                if (jezykPolski)
                {
                    w = new Word(sciezkaZrodlo + "Szablon.docx", sciezkaDocelowa + "HLD" + projekt.Name+"_"+teraz.ToString(format) + ".docx", pokWord);
                }
                else
                {
                    w = new Word(sciezkaZrodlo + "SzablonEN.docx", sciezkaDocelowa + "HLD" + projekt.Name + "_" + teraz.ToString(format) + ".docx", pokWord);
                }
                 Log(new CLog(LogMsgType.WynikOK, " Word [ok] \n"));

                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział - wstęp "));
  
               Crozdz0 r0= new Crozdz0(modelRepo, sciezkaZrodlo, sciezkaDocelowa, w,jezykPolski);
                rozdzialy[0] = r0.dajRozdzial();
  

                Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                
                 Log(new CLog(LogMsgType.Normal, "\n***** Rozdział 1 \n"));
                Crozdz1 r1 = null;
     
                    if (R1cb.Checked)
                    {
                        r1 = new Crozdz1(/* nowy generator rep, projekt,*/modelRepo, sciezkaZrodlo, sciezkaDocelowa, w,this,jezykPolski);
                        rozdzialy[1] = r1.dajRozdzial();
                        KolorujCB(new Tuple<int,int>(0,0));
                     
                    }
                    else
                    {
                         KolorujCB(new Tuple<int,int>(0,1));
                        //R1cb.BackColor = Color.Red;
                    }
            
                DateTime dt_Teraz = DateTime.Now;
                TimeSpan span = dt_Teraz.Subtract(dt_Start);
  
                    UstawLblCzas(0);
                 Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);


                  Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 2 \n"));
                Crozdz2 r2 = null;
  
                    if (R2cb.Checked)
                    {
                      
                        r2 = new Crozdz2(modelRepo, sciezkaZrodlo, sciezkaDocelowa, w, this, jezykPolski,cbTresciWymagan.Checked);
                        rozdzialy[2] = r2.dajRozdzial();
       
                        KolorujCB(new Tuple<int, int>(1, 0));
                    }
                    else
                    {
                        KolorujCB(new Tuple<int, int>(1, 1));
            
                    }
                    UstawLblCzas(1);

                Log(new CLog(  LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
     
                 
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 3 IT"));
                Crozdz3 r3it = null;
                rozdzialy[3] = null;
        
                   if (checkBoxIT.Checked) 
                   {
                      
                       r3it = new Crozdz3(modelRepo, CModel.IT, sciezkaZrodlo, sciezkaDocelowa, "3", w, this, jezykPolski);
                       rozdzialy[3] = r3it.dajRozdzial();

                       KolorujCB(new Tuple<int, int>(2, 0));
                   }
                   else
                   {
          
                       KolorujCB(new Tuple<int, int>(2, 1));
                   }
                   UstawLblCzas(2);
                   Log(new CLog(LogMsgType.WynikOK," [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 /************************************************************/
                Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 4 NT "));
                Crozdz3 r3nt = null;
                rozdzialy[4] = null;
          
                    if (checkBoxNT.Checked)
                    {
                        //r3nt = new Crozdz3(rep, projekt, EAUtils.dajPakietSciezkiP(ref projekt, "NT"), sciezkaZrodlo, sciezkaDocelowa, "4", w,this,jezykPolski);
                        r3nt = new Crozdz3(modelRepo, CModel.NT, sciezkaZrodlo, sciezkaDocelowa, "4", w, this, jezykPolski);
                        rozdzialy[4] = r3nt.dajRozdzial();
                        KolorujCB(new Tuple<int, int>(3, 0));
                   
                    }
                    else
                    {
                        KolorujCB(new Tuple<int, int>(3, 1));
                
                    }
                    UstawLblCzas(3);
                     Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 /************************************************************/
                
                  Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 5 Architektura Transmisyjna "));
                 Crozdz7 r7 = null;
                 rozdzialy[10] = null;
      
                     if (R11cb.Checked)
                     {
                       
                         r7 = new Crozdz7(modelRepo, sciezkaZrodlo, sciezkaDocelowa, w, jezykPolski);
                         rozdzialy[10] = r7.dajRozdzial();
                         KolorujCB(new Tuple<int, int>(10, 0));
                         
                     }
                     else
                     {
               
                         KolorujCB(new Tuple<int, int>(10, 1));
                     }
                     UstawLblCzas(10);
                     Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 /************************************************************/
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 6 Wskazówki dotyczące testów "));
             
                 Crozdz8 r8 = null;
                 rozdzialy[11] = null;
               
                     if (R12cb.Checked)
                     {
                      
                         r8 = new Crozdz8(modelRepo, sciezkaZrodlo, sciezkaDocelowa, w, jezykPolski);
                         rozdzialy[11] = r8.dajRozdzial();
                         KolorujCB(new Tuple<int, int>(11, 0));
                        
                     }
                     else
                     {
                         KolorujCB(new Tuple<int, int>(11, 1));
                        
                     }
                     UstawLblCzas(11);
                    Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 /************************************************************/

                 /*********************** IT *************************************************************************/

                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 7 wkłady IT "));
                Crozdz4 r4it = null;
                rozdzialy[5] = null;
             
                   if (R5cb.Checked)
                   {
                     
                       r4it = new Crozdz4(modelRepo,CModel.IT, sciezkaZrodlo, sciezkaDocelowa, "IT", w, this, jezykPolski);
                       rozdzialy[5] = r4it.dajRozdzial();
                       KolorujCB(new Tuple<int, int>(4, 0));
                       
                   }
                   else
                   {
                      
                       KolorujCB(new Tuple<int, int>(4, 1));
                   }
                   UstawLblCzas(4);
                  Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
                /*********************** NT *************************************************************************/
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 8 wkłady NT "));
                Crozdz4 r4nt = null;
                rozdzialy[6] = null;
      
                   if (R6cb.Checked)
                   {
                      
                       r4nt = new Crozdz4(modelRepo, CModel.NT, sciezkaZrodlo, sciezkaDocelowa, "NT", w, this, jezykPolski);
                       rozdzialy[6] = r4nt.dajRozdzial();
                    
                       KolorujCB(new Tuple<int, int>(5, 0));
                   }
                   else
                   {
                       
                       KolorujCB(new Tuple<int, int>(5, 1));
                   }
                   UstawLblCzas(5);
                    Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                 ProgressBarKrok();
                 if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                 /************************************************************/
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 9 interfejsy IT "));
                Crozdz5 r5it = null;
                rozdzialy[7] = null;
      
                   if (R7cb.Checked)
                   {
                    
                       r5it = new Crozdz5(modelRepo,CModel.IT, sciezkaZrodlo, sciezkaDocelowa, "IT", w, jezykPolski);
                       rozdzialy[7] = r5it.dajRozdzial();
      
                       KolorujCB(new Tuple<int, int>(6, 0));
                   }
                   else
                   {
              
                       KolorujCB(new Tuple<int, int>(6, 1));
                   }
                   UstawLblCzas(6);
                 Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 10 interfejsy NT "));
                Crozdz5 r5nt = null;
                rozdzialy[8] = null;
      
                   if (R8cb.Checked)
                   {
                     
                       r5nt = new Crozdz5(modelRepo, CModel.NT, sciezkaZrodlo, sciezkaDocelowa, "NT", w, jezykPolski);
                       rozdzialy[8] = r5nt.dajRozdzial();
               
                       KolorujCB(new Tuple<int, int>(7, 0));
                   }
                   else
                   {
                  
                       KolorujCB(new Tuple<int, int>(7, 1));
                   }
                   UstawLblCzas(7);
                 Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
                Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 11 IT "));
                Crozdz6 r6it = null;
                rozdzialy[9] = null;
         
                   if (R9cb.Checked)
                   {
                      
                       r6it = new Crozdz6(modelRepo,CModel.IT, sciezkaZrodlo, sciezkaDocelowa, w,this, jezykPolski);
                       rozdzialy[9] = r6it.dajRozdzial();
                  
                       KolorujCB(new Tuple<int, int>(8, 0));
                   }
                   else
                   {
                       KolorujCB(new Tuple<int, int>(8, 1));
             
                   }
                   UstawLblCzas(8);
                   Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
                 Log(new CLog(LogMsgType.Normal, "\n**** Rozdział 12 NT "));
            Crozdz6 r6nt = null;
                rozdzialy[10] = null;
            
                   if (R10cb.Checked)
                   {
                      
                       r6nt = new Crozdz6(modelRepo, CModel.NT, sciezkaZrodlo, sciezkaDocelowa, w,this, jezykPolski);
                       rozdzialy[10] = r6nt.dajRozdzial();
                       KolorujCB(new Tuple<int, int>(9, 0));
        
                   }
                   else
                   {
                       KolorujCB(new Tuple<int, int>(9, 1));
                     
                   }
                   UstawLblCzas(9);
                Log(new CLog(LogMsgType.WynikOK, " [ok] \n"));
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
             
                stopkaHTML = doklejPlik("stopka.kzg");

                //todo spis tresci
              
                w.odswiezSpisTresci();
                SpisTresciHTML = "<div id=\"spis\">";
                if (r0 != null) SpisTresciHTML += r0.dajSpisTresci();
                if (r1 != null) SpisTresciHTML += r1.dajSpisTresci();
                if (r2 != null) SpisTresciHTML += r2.dajSpisTresci();
                if (r3it != null) SpisTresciHTML += r3it.dajSpisTresci();
                if (r4it != null) SpisTresciHTML += r4it.dajSpisTresci();
                if (r3nt != null) SpisTresciHTML += r3nt.dajSpisTresci();
                if (r4nt != null) SpisTresciHTML += r4nt.dajSpisTresci();
                if (r5it != null) SpisTresciHTML += r5it.dajSpisTresci();
                if (r5nt != null) SpisTresciHTML += r5nt.dajSpisTresci();
                //  if (r6 != null)         SpisTresciHTML += r6.dajSpisTresci();

                SpisTresciHTML += "</div>";
              //  progressBar1.Invoke(new MethodInvoker(delegate() { progressBar1.Value++;}));
                String index = "<!DOCTYPE html>\n<html>\n<head>\n<script type='text/javascript' src=\"js/skrypt.js\"></script>\n";
                index += "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" >\n";
                index += "<title>HLD projektu " + projekt.Name + "</title>";
                index += "<link rel=\"stylesheet\" type=\"text/css\" href=\"css/styl.css\">\n</head>\n<body>\n";
                index += "<div><a href=\"../lista.html\">Powrót do listy wersji HLD..</a>\n";
                index += "<div id=\"glowny\">";
                index += "<div data-include=\"r0.html\"></div>\n";
                index += "<div data-include=\"spisTresci.html\"></div>\n";
                for (int i = 0; i < rozdzialy.Length; i++)
                {
                    if (rozdzialy[i] != null)
                    {
                        if (i > 0) index += "<div data-include=\"r" + i + ".html\"></div>\n";
                  //      System.IO.File.WriteAllText(sciezkaDocelowa + "r" + i + ".html", rozdzialy[i]);
                    }
                }
                index += "</div>";
                index += stopkaHTML;
             //   System.IO.File.WriteAllText(sciezkaDocelowa + "spisTresci.html", SpisTresciHTML);
            //    System.IO.File.WriteAllText(sciezkaDocelowa + "index.html", index);
                
                w.zapiszZakmnij(pokWord,this);
               
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
                 Log(new CLog(LogMsgType.Normal, " Zapisanie wyników "));
         
                 Log(new CLog(LogMsgType.WynikOK, " GUI [sprzątanie] \n"));
               
              
                 Log(new CLog(LogMsgType.WynikOK, " GUI [ok] \n"));
                 Log(new CLog(LogMsgType.Normal, "Koniec procesu. "+ st.Elapsed.ToString("hh\\:mm\\:ss\\.fff")+"\n"));
            
                ProgressBarKrok();
                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
                /************************************************************/
            }
            catch (Exception exc)
            {
                Log(new CLog(LogMsgType.Error, "Wyjątek: " + exc.Message.ToString()));
              
                MessageBox.Show("Błąd generowania, upewnij się, że ścieżki do plików są poprawnie zdefiniowane i masz odpowiednie uprawnienia. " + exc.Message);
                w.zapiszZakmnij(pokWord,this);
                throw new OperationCanceledException(cancellationToken);
               
            }
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt_Teraz = DateTime.Now;
            TimeSpan span = dt_Teraz.Subtract(dt_Start);
         
            ustawText(generowanieLbl,"Początek generowania HLD: " + dt_Start.ToLongTimeString() + ", obecny czas generowania: " + span.ToString(@"hh\:mm\:ss\.ff"));
          
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
