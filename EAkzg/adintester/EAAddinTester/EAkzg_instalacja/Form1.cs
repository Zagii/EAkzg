using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace EAkzg_instalacja
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            string registry_key = @"SOFTWARE\Sparx Systems\EA400\EA\";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                     EASciezkaLbl.Text=key.GetValue("Install Path")+"\n";
                     InfoLbl.Text = key.GetValue("Version") + "\n";
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void PluginSciezkaLbl_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = PluginSciezkaLbl.Text;
            folderBrowserDialog1.ShowDialog();
            PluginSciezkaLbl.Text = folderBrowserDialog1.SelectedPath + "\\";
        }
        // <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            Cursor.Current = Cursors.WaitCursor;
            rtb.Text = "";
            progressBar1.Value++;
            Thread.Sleep(500);
            System.IO.Directory.CreateDirectory(PluginSciezkaLbl.Text);
            progressBar1.Value++;
            Thread.Sleep(500);
            // skopiuj dll
           // var x = Properties.Resources.EAkzg2;
           // System.IO.File.WriteAllBytes(PluginSciezkaLbl.Text + "EAkzg2.dll", x);

          //  var x1 = Properties.Resources.Szablon;
          //  System.IO.File.WriteAllBytes(PluginSciezkaLbl.Text + "Szablon.docx", x);

          //  var x2 = Properties.Resources.SzablonEN;
          //  System.IO.File.WriteAllBytes(PluginSciezkaLbl.Text + "SzablonEN.docx", x);

            String plik=PluginSciezkaLbl.Text + "EAkzg2.dll";
            rtb.Text += "\n *******  Plik *****\n" + plik +"=>" ;

            if (System.IO.File.Exists(plik))
            {
                rtb.Text += "ok\n";
            }
            else
            {
                rtb.Text += "brak!!!!!!!!!!!!! \n Błąd";
                return;
            }
            plik = PluginSciezkaLbl.Text + "Szablon.docx";
            rtb.Text += "\n *******  Plik *****\n" + plik + "=>";

            if (System.IO.File.Exists(plik))
            {
                rtb.Text += "ok\n";
            }
            else
            {
                rtb.Text += "brak!!!!!!!!!!!!! \n Błąd";
                return;
            }
            plik = PluginSciezkaLbl.Text + "SzablonEN.docx";
            rtb.Text += "\n *******  Plik *****\n" + plik + "=>";

            if (System.IO.File.Exists(plik))
            {
                rtb.Text += "ok\n";
            }
            else
            {
                rtb.Text += "brak!!!!!!!!!!!!! \n Błąd";
                return;
            }

            
            progressBar1.Value++;
            Thread.Sleep(500);
      /*      var js = Properties.Resources.skrypt;
            System.IO.File.WriteAllText(PluginSciezkaLbl.Text + "skrypt.js", js) ;
            progressBar1.Value++;
            Thread.Sleep(1000);
            var css = Properties.Resources.styl;
            System.IO.File.WriteAllText(PluginSciezkaLbl.Text + "styl.css", css);
            progressBar1.Value++;
            Thread.Sleep(1000);
            Image logo = Properties.Resources.logo;
            logo.Save(PluginSciezkaLbl.Text + "logo.png");
        */  
  


            // zarejestruj dll
            string pathvar = System.Environment.GetEnvironmentVariable("WINDIR");
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pathvar+@"\Microsoft.NET\Framework\v4.0.30319\regasm"; // Specify exe name.
            rtb.Text += "\n Wywołanie: \n";
            rtb.Text += start.FileName;

            start.Arguments="\""+PluginSciezkaLbl.Text + "EAkzg2.dll\" /codebase";
            rtb.Text+=start.Arguments;
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.RedirectStandardOutput = true;
            start.StandardOutputEncoding = Console.Out.Encoding;

            progressBar1.Value++;
            Thread.Sleep(1000);

            //
            // Start the process.
            //
            using (Process process = Process.Start(start))
            {
                //
                // Read in all the text from the process with the StreamReader.
                //
                process.WaitForExit();
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    rtb.Text+=result;
                }
               
            }

            progressBar1.Value++;
            Thread.Sleep(1000);

            // wrzuc do rejestru

            string registry_key = @"SOFTWARE\Sparx Systems\EAAddins\EAkzg2\";
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registry_key))
            {
                key.SetValue("", "EAkzg.KzgAddinClassv2");
                key.SetValue("DLL", PluginSciezkaLbl.Text);
                key.Close();
            }

            progressBar1.Value++;
            Thread.Sleep(1000);

            progressBar1.Value++;
            Thread.Sleep(1000);

            Cursor.Current = Cursors.Default;
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PluginSciezkaLbl_Click(null, null);
        }
    }
}
