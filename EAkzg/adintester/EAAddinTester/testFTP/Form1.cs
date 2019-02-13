using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using WinSCP;


namespace testFTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
 

        try
        {
            // Setup session options
      /*      SessionOptions sessionOptions = new SessionOptions {
                Protocol = Protocol.Sftp,
                HostName = "architect-new",
                UserName = "test",
                Password = "test"
                //SshHostKeyFingerprint = "ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx"
            };
 
            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);
 
                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;
 
                TransferOperationResult transferResult;
                transferResult = session.PutFiles(@"d:\toupload\*", "/home/user/", false, transferOptions);
 
                // Throw on any error
                transferResult.Check();
 
                // Print results
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                   richTextBox1.Text+="Upload of {0} succeeded"+ transfer.FileName;
                }
            }
 */
           
        }
        catch (Exception s)
        {
           richTextBox1.Text+="Error: "+s.Message;
            
        }
    }
}
        }
   
