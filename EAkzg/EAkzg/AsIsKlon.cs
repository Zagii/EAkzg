using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;

namespace EAkzg
{
    public partial class AsIsKlon : Form
    {
        Repository repo;
        public AsIsKlon(Repository rep)
        {
            repo = rep;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Plik .EAP (.eap)|*.eap";
            openFileDialog1.FilterIndex = 1;
            

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sciezkaBtn.Text = openFileDialog1.FileName;
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if(System.IO.File.Exists(sciezkaBtn.Text))
            {
              try{
                 //create the repository object
                  Repository m_Repository = new Repository();
                 //open an EAP file
                 m_Repository.OpenFile(sciezkaBtn.Text);
                 //use the Repository in any way required
                 Package asisPckg=m_Repository.Models.GetAt(0);
                 asisPckg = asisPckg.Packages.GetByName("AS-IS Architecture");

                  Package mojModel=repo.Models.GetAt(0);
                  Package klon = EAUtils.utworzPakietGdyBrak(ref mojModel, "AS-IS Architecture", "");
                  klon = asisPckg.Clone();
                  klon.Notes = "Kopia repozytorium " + sciezkaBtn.Text + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                                  
                  
                  klon.Update();

               //    EA.Collection queryResults ;
                   // Element theElement;
                 //    var sql = "<insert sql query here>";

                     var targetPackageID = m_Repository.GetTreeSelectedPackage().Packages;//Repository.GetTreeSelectedPackage().PackageID;
     
              //       queryResults = Repository.GetElementSet( sql, 2 );
     
                  //   for ( var i = 0; i < queryResults.Count; i++ )
                     {
                 //          theElement = queryResults.GetAt(i);
                 //          theElement.PackageID = targetPackageID;
                 //          theElement.Update();
                     }
                 //close the repository and tidy up
                 m_Repository.Exit();
                 m_Repository = null; 
                  }
                catch(Exception ee)
                  {

                    MessageBox.Show("Wyjątek kopiowania As Is-"+ee.Message);
                    }
            }
            else
            {
                MessageBox.Show("Brak pliku: "+sciezkaBtn.Text);
            }
            Cursor.Current = Cursors.Default;
        }
         
    }
}
