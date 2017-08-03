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
    public partial class Detale : Form
    {
      
        Repository repo;
        public Detale(Repository rep)
        {
            repo = rep;
            InitializeComponent();
          
        }

        private void AnulujBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {

            ZatwierdzBtn_Click(null, null);
            this.Close();
        }

        private void Detale_Load(object sender, EventArgs e)
        {
            modelCB.Text = EAUtils.dajNazweModelu();
    
           
            guiLoad(modelCB.Text);
           // modelCB.Text = modelCB.SelectedValue.ToString();
        }

        private void guiLoad(String nazwaModelu="")
        {
            Package model = null;
            if (nazwaModelu == "")
            {
                model = EAUtils.dajModelPR(ref repo);
            }
            else
            {
                model = EAUtils.dajModelPRoNazwie(ref repo, nazwaModelu);
            }

            if (model == null)
            {
                MessageBox.Show("Model o nazwie '" + nazwaModelu + "' nie istnieje");
                return;
            }

            sdITTb.Text = EAUtils.dajAutoraProjektu(ref model, "SD IT");
            sdNTTB.Text = EAUtils.dajAutoraProjektu(ref model, "SD NT");
            symbolTB.Text = model.Name;
            String[] s = { "HLD","Definicje" };
            Package pckg = EAUtils.dajPakietSciezki(ref model, s);
            Element el = EAUtils.dajElementLubGoZrob(ref pckg, "Projekt-Nazwa");

            nazwaProjektuTB.Text = el.Notes;

            modelCB.Items.Clear();
            foreach (Package p in repo.Models)
            {
                modelCB.Items.Add(p.Name);
            }
            modelCB.SelectedItem = model.Name;
        }

        private void modelCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
            guiLoad(modelCB.SelectedItem.ToString());
           
        }

        private void ZatwierdzBtn_Click(object sender, EventArgs e)
        {
            try
            {

                ///odczyt
                ///
                EAUtils.zapiszNazweModelu(modelCB.SelectedItem.ToString());
                Package model = EAUtils.dajModelPR(ref repo);// repo.Models.GetAt(0);
                model.Name = symbolTB.Text.Trim();
               // model.Name = symbolTB.Text;
                model.Update();
                EAUtils.zapiszNazweModelu(model.Name);
                repo.RefreshModelView(model.PackageID);
                String[] s = { "HLD","Definicje", "Słownik" };
                Package pckg = EAUtils.dajPakietSciezki(ref model, s);
                Element el = EAUtils.dajElementLubGoZrob(ref pckg, "SD IT");
                EAUtils.dodajTaggedValues(ref el, "Imię i Nazwisko", sdITTb.Text);

                el = EAUtils.dajElementLubGoZrob(ref pckg, "SD NT");
                EAUtils.dodajTaggedValues(ref el, "Imię i Nazwisko", sdNTTB.Text);

                String[] s2 = {  "HLD","Definicje" };
                pckg = EAUtils.dajPakietSciezki(ref model, s2);
                el = EAUtils.dajElementLubGoZrob(ref pckg, "Projekt-Nazwa");
                el.Notes = nazwaProjektuTB.Text;
                el.Update();
                el.Refresh();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Detale.ZatwierdzBtnKlik \n {" + modelCB.SelectedItem.ToString() + "," + symbolTB.Text.Trim() + "} \n, wyjątek " + exc.Message.ToString());
            }
        }
    }
}
