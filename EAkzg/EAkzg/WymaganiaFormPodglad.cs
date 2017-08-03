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
    public partial class WymaganiaFormPodglad : Form
    {
        CModel modelProjektu;
        EA.Element element;
        bool nameChange=false;
        bool notesChange=false;
        bool statusChange = false;
        int row, col;
        WymaganiaForm rodzic;
        public WymaganiaFormPodglad( CModel m, WymaganiaForm r)
        {
            InitializeComponent();
            modelProjektu = m;
            rodzic = r;
        }

        private void WymaganiaFormPodglad_MouseClick(object sender, MouseEventArgs e)
        {

            this.Hide();
        }
        public void ustawTxt(String name,String note)
        {
            rtfName.Text = name;
            rtfNotes.Text = note;
        }
        public void ustawElement(EA.Element el,int r,int c)
        {
            row = r; col = c;
            if (el == null) return;
            element = el;
            rtfName.Text = el.Name;
            rtfNotes.Rtf = modelProjektu.Repozytorium.GetFormatFromField("RTF",el.Notes);
            rtfLinkedDoc.Rtf=el.GetLinkedDocument();
          
            typLbl.Text = el.Type;
            stereotypLbl.Text = el.Stereotype;
            CBstatus.Items.Clear();            
            if (el.Type == "Feature")
            {
                string[] stat = {"Analiza ChM","Weryfikacja SD","Z uwagami do SD","Uzgodnione","Anulowane przez BO","Anulowane przez IT" };
                CBstatus.Items.AddRange(stat);
            }
            if (el.Type == "Requirement")
            {
                string[] stat = { "01-Otwarte", "02-Potwierdzone", "03-Weryfikacja tech.", "04-Wykonalne", "05-Niewykonalne", "14-Zamknięte", "15-Odrzucone" };
                CBstatus.Items.AddRange(stat);
            }
            CBstatus.Text = el.Status;

            listBox1.Items.Clear();
          /*  foreach (EA.Connector c in el.Connectors)
            { 
                EA.Element sup=modelProjektu.Repozytorium.GetElementByID(c.SupplierID);
                EA.Element cli=modelProjektu.Repozytorium.GetElementByID(c.ClientID);

                
            }
            */
            string sql = "select c.end_object_id as object_id " +
                     " from t_connector c where " +
                         "c.Start_Object_ID="+el.ElementID +
                        " union ALL " +
                        " select c.START_object_id " +
                      " from t_connector c where " +
                       " c.END_Object_ID="+el.ElementID;
               //     Log(new CLog(LogMsgType.Info, "sql=" + sql + "\n"));
           foreach (EA.Element sysEl in modelProjektu.Repozytorium.GetElementSet(sql, 2))
           {
               listBox1.Items.Add(sysEl.Name);
           }
            button1.Enabled = false;
            nameChange = false;
            notesChange = false;
            statusChange = false;
            //dodac zmiane na datagridzie
        }

        private void zmianaStatusu()
        {
            if (statusChange)
            {
                element.Status = CBstatus.Text;
                element.Update();
                statusChange = false;
            }
        }
        private void zmianaName()
        {
            if (nameChange)
            {
                element.Name = rtfName.Text;
                element.Update();
                nameChange = false;
            }
        }
        private void zmianaNotes()
        {
            if (notesChange)
            {
                element.Notes = modelProjektu.Repozytorium.GetFieldFromFormat("RTF", rtfNotes.Rtf);
                element.Update();
                notesChange = false;
            }
        }
    

        private void CBstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
           // statusChange = true;
        //    button1.Enabled = true;
          //  zmianaStatusu();
        }

        private void rtfNotes_Leave(object sender, EventArgs e)
        {
        //    zmianaNotes();
        }

        private void rtfName_Leave(object sender, EventArgs e)
        {

           //  zmianaName();
        }

        private void CBstatus_SelectedValueChanged(object sender, EventArgs e)
        {
          //  zmianaStatusu();
        }

        private void rtfName_TextChanged(object sender, EventArgs e)
        {
            nameChange = true;
            button1.Enabled = true;
        }

        private void rtfNotes_TextChanged(object sender, EventArgs e)
        {
            notesChange = true;
            button1.Enabled = true;
        }

        private void CBstatus_TextChanged(object sender, EventArgs e)
        {
            statusChange = true;
            button1.Enabled = true;
        }

        private void CBstatus_Leave(object sender, EventArgs e)
        {
            //zmianaStatusu();
        }

        private void zapiszZmiany()
        {
            zmianaStatusu();
            zmianaNotes();
            zmianaName();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            zapiszZmiany();
            rodzic.zmienObiektDataGridView(row,col);
            button1.Enabled = false;
        }
    }
}
