using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAkzg
{
    public partial class CzekajWnd : Form
    {
        public CzekajWnd()
        {
            InitializeComponent();
        }
        public void ustawTxt(string s)
        {
            if (generowanieLbl.InvokeRequired)
            {
                generowanieLbl.Invoke(new Action(() => generowanieLbl.Text = s));
            }
            else {
                generowanieLbl.Text = s;
            }
        }
        public void ustawTxtElem(string s)
        {
            if (lblElementID.InvokeRequired)
            {
                lblElementID.Invoke(new Action(() => lblElementID.Text = s));
            }
            else
            {
                lblElementID.Text = s;
            }
        }
     
        public void ustawTxtReqLoop(string s)
        {
            if (lblReqLoop.InvokeRequired)
            {
                lblReqLoop.Invoke(new Action(() => lblReqLoop.Text = s));
            }
            else
            {
                lblReqLoop.Text = s;
            }
        }
        public void ustawTxtGetElemByID(string s)
        {
            if (lblGetElemByID.InvokeRequired)
            {
                lblGetElemByID.Invoke(new Action(() => lblGetElemByID.Text = s));
            }
            else
            {
                lblGetElemByID.Text = s;
            }
        }
        public void ustawPB(int min, int max)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    progressBar1.Minimum = min;
                    progressBar1.Maximum = max;
                    progressBar1.Value = min;
                }));
            }
            else
            {
                progressBar1.Minimum = min;
                progressBar1.Maximum = max;
            
            }

        }
        public void ustawPBkrok(int k)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    if (k > progressBar1.Maximum)
                        k = progressBar1.Maximum;
                    if (k < progressBar1.Minimum)
                        k = progressBar1.Minimum;
                    progressBar1.Value = k;
                }));
            }
            else
            {
                if (k > progressBar1.Maximum)
                    k = progressBar1.Maximum;
                if (k < progressBar1.Minimum)
                    k = progressBar1.Minimum;
                progressBar1.Value = k;
            }
        }
        public void ustawPBNast()
        {
   
            ustawPBkrok(progressBar1.Value + 1);
        }
    }
   
}
