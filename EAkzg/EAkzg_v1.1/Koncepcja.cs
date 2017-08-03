using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using EA;
namespace EAkzg
{
    public partial class Koncepcja : Form
    {
        bool B = false;
        bool I = false;
        bool U = false;
        bool H1 = false;
        bool H2 = false;
        bool H3 = false;
        bool fcolor = false;
       
        Color fontColor=Color.Black;
        Color tloColor=Color.White;
        Repository repo;
        Element objElem;
        Package koncepcjaPckg;
        String obiekt;
        String [] Folder;
        public Koncepcja(Repository rep,String [] folder,String o,String opis)
        {
            repo = rep;
            Folder = folder;
            obiekt = o;
            InitializeComponent();
            richTextBox1.ForeColor = fontColor;
            richTextBox1.BackColor = tloColor;
         //   webBrowser1.DocumentText = richTextBox1.Text;
            TytulLbl.Text = opis;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            String t=richTextBox1.Text.Replace("\n", "<BR>") ;
            Regex rgx = new Regex("imgsrc=@(.*?)@");
            string result = rgx.Replace(t, "<img src='$1'>");
            webBrowser1.DocumentText = result;
            
            
        }

        private void butonCzcionkaKlik(ref Button btn,ref bool flaga,String znacznik)
        {
             if (richTextBox1.SelectionLength > 0)
            {
                String pocz = richTextBox1.Text.Substring(0,richTextBox1.SelectionStart);
                String srod = richTextBox1.SelectedText;
                String kon = richTextBox1.Text.Substring(richTextBox1.SelectionStart + richTextBox1.SelectionLength);
                richTextBox1.Text=pocz+ "<"+znacznik+">"+srod+"</"+znacznik+">"+kon;
            }
            else
            {
                flaga = !flaga;
                if (flaga)
                {
                    richTextBox1.Text += "<" + znacznik + ">";
                    btn.BackColor = Color.Magenta;
                }
                else
                {
                    richTextBox1.Text += "</" + znacznik + ">";
                    btn.BackColor = SystemColors.Control;
                }
            }
        }

        private void Bbtn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref B, "B");
            return;
           
        }

        private void Ibtn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref I, "I");
            return;
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref U, "U");
            return;
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //dodaj obrazek
            // openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "Grafika (.png, .jpg, .jpeg, .gif, .bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Wszystkie pliki (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
           // openFileDialog1.ShowDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
              //  richTextBox1.Text += "<img src='" + openFileDialog1.FileName + "'>";
                richTextBox1.Text += "imgsrc=@" + openFileDialog1.FileName + "@";
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
        
            if (richTextBox1.SelectionLength > 0)
            {
                String pocz = richTextBox1.Text.Substring(0, richTextBox1.SelectionStart);
                String srod = richTextBox1.SelectedText;
                String kon = richTextBox1.Text.Substring(richTextBox1.SelectionStart + richTextBox1.SelectionLength);
                colorDialog1.Color = richTextBox1.ForeColor;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                  
                    richTextBox1.Text =pocz+ "<font color=rgb(" + colorDialog1.Color.R.ToString() + "," + colorDialog1.Color.G.ToString() + "," + colorDialog1.Color.B.ToString() + ")>";
                    richTextBox1.Text += srod + "</font>" + kon;
                   
                }
           
            }
            else
            {
                if (fcolor)
                {
                    richTextBox1.Text += "</font>";
                    KolorBtn.ForeColor = fontColor;
                    fcolor = false;
                }
                else
                {
                    colorDialog1.Color = richTextBox1.ForeColor;
                    if (colorDialog1.ShowDialog() == DialogResult.OK)
                    {
                        KolorBtn.ForeColor = colorDialog1.Color;

                        richTextBox1.Text += "<font color=rgb(" + colorDialog1.Color.R.ToString() + "," + colorDialog1.Color.G.ToString() + "," + colorDialog1.Color.B.ToString() + ")>";
                        fcolor = true;
                    }
                }
            }
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref H1, "H1");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref H2, "H2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            butonCzcionkaKlik(ref b, ref H3, "H3");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //zapisz zmiany
            objElem.Notes = richTextBox1.Text;
            objElem.Update();
            objElem.Refresh();

            this.Close();
        }

        private void Koncepcja_Load(object sender, EventArgs e)
        {
            Package model = EAUtils.dajModelPR(ref repo); //repo.Models.GetAt(0);
            
                koncepcjaPckg = EAUtils.utworzSciezke(ref model,Folder);
          
                objElem=EAUtils.dodajElement(ref koncepcjaPckg, obiekt, "");
                
            
            if (objElem.Notes != "")
            {
                richTextBox1.Text = objElem.Notes;
             }
           
        }
    }
}
