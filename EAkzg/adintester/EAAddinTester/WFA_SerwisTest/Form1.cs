using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EAkzg_WindowsService;
using System.Diagnostics;

namespace WFA_SerwisTest
{
    public partial class Form1 : Form
    {
        private EA_APISerwis eapi;
    
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;  //Tell the user how the process went
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true; //Allow for the process to be cancelled

          
            
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            

                try
                {
                    timer.Stop();
                    label2.BackColor = Color.Red;

                    EA_APISerwis eapi = new EA_APISerwis();

                    eapi.log(DateTime.Now.ToString() + " Monitoring repozytorium Start", "DoWork");

                    eapi.dzialajDlaWszystkich(this,backgroundWorker1);

                    eapi.log(DateTime.Now.ToString() + " Monitoring repozytorium Stop", "DoWork");

                    

                   // eapi.EA_Close();
                    
                }
                catch (Exception exc)
                {
                    log("Wyjątek Form1:" + exc.Message);
                }
            backgroundWorker1.ReportProgress(100);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                log(DateTime.Now.ToString() + " WorkerCompleted - Anulowano proces");
            }
            else if (e.Error != null)
            {
              //  lblStatus.Text = "There was an error running the process. The thread aborted";
                log(DateTime.Now.ToString() + " WorkerCompleted - Błąd, wątek zawieszono");
            }
            else
            {
                log(DateTime.Now.ToString() + " WorkerCompleted - Koniec procesu");
              
               
            }
            getConfig();

            timer.Start();

            button2.Enabled = false;
            button1.Enabled = true;
            progressBar1.Visible = false;
            label3.Visible = false;

            label2.BackColor = Color.Green;
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                string[] s = new string[2];
                s =(string[]) e.UserState;
                if(s[0]!=null)
                    label3.Text = s[0];
                if (s[1] != null)
                    log(DateTime.Now.ToString() +" "+ s[1]);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //eapi.EA_Close();
        }
        public void logStart(string t)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new MethodInvoker(() => { listBox1.Items.Insert(0, t); }));
            }
            else
            {
                listBox1.Items.Insert(0, t);
            }

        }
        public void logKoniec(string t)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new MethodInvoker(() => { listBox1.Items[0]+=t; }));
            }
            else
            {
                listBox1.Items[0] += t;
            }

        }
        private void log(string t)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new MethodInvoker(() => { listBox1.Items.Insert(0, t); }));
            }
            else
            {
                listBox1.Items.Insert(0, t);
            }

        }
        private void akcja()
        {
            try
            {
                // TODO: Insert monitoring activities here.  
                log(DateTime.Now.ToString() + " Monitoring repozytorium Start");
                button1.Enabled = false;
                button2.Enabled = true;
                progressBar1.Visible = true;
                label3.Text = "Init... 0%";
                label3.Visible = true;
                
                backgroundWorker1.RunWorkerAsync();
                
            }
            catch (Exception ex)
            {
                // Log the exception.
                log(DateTime.Now.ToString() + " akcja() exc: " + ex.Message+ " TargetSite: "+ex.TargetSite.ToString());
            }
        }
   
        private void button1_Click(object sender, EventArgs e)
        {
           
            akcja();
            

         
        }

        private double licz(double a, double b, int o, int krok, int a3, int b3,int a8, int b8, string t)
        {
           
            return 0;  
              
        
        }

       
       
        private void button2_Click(object sender, EventArgs e)
        {
            
            //Check if background worker is doing anything and send a cancellation if it is
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            button2.Enabled = false;
            button1.Enabled = true;
            progressBar1.Visible = false;
        }

        private void getConfig()
        {
             eapi = new EA_APISerwis();
            eapi.getConfig();
            timer.Interval = eapi.getInterwal()*1000*60;
            label2.Text = eapi.getInterwal().ToString()+" min.\n"+eapi.getSciezka();
          //  eapi.EA_Close();
        }
        

        private void Form1_Shown(object sender, EventArgs e)
        {
            getConfig();
            timer.Start();
            progressBar1.Visible = false;
            label3.Visible = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            akcja();
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void EAKiller_Tick(object sender, EventArgs e)
        {
            try
            {
              
                foreach (Process proc in Process.GetProcessesByName("EA"))
                {
                    eapi.EA_Disconnect();
                    proc.Kill();
                    
                    log("EAKiller - bang");
                }// plus opcjonalnie wywalać WerFault.exe
            }
            catch (Exception ex)
            {
                log("EAKiller ex: "+ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            EAKiller.Enabled = checkBox1.Checked;
            log("EAKiller status=" + EAKiller.Enabled.ToString());
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox2.Checked;
            log("Timer enabled status=" + timer.Enabled.ToString());
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Process proc in Process.GetProcessesByName("EA"))
            {
                eapi.EA_Disconnect();
                proc.Kill();

             
            }// plus opcjonalnie wywalać WerFault.exe
            foreach (Process proc in Process.GetProcessesByName("WFA_SerwisTest"))
            {
                eapi.EA_Disconnect();
                proc.Kill();


            }// 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int coIle = 60 * 1000 * 60;
            if (int.TryParse(textBox1.Text, out coIle))
                timer1.Interval = coIle * 1000 * 60;
        }  
    }
}
