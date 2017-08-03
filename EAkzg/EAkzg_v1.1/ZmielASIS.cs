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
    public partial class ZmielASIS : Form
    {
        Repository Repo;
        Package asis;
        Package model;
        public ZmielASIS(Repository rep)
        {
            Repo = rep;
            InitializeComponent();
            InicjujGUI();
        }
        private void InicjujGUI()
        {
            EA.Project projektInterfejs = Repo.GetProjectInterface();
            model = EAUtils.dajModelPR(ref Repo);// Repo.Models.GetAt(0);
            comboBox1.Items.Clear();
            foreach (Package p in model.Packages)
            {
                comboBox1.Items.Add(p.Name);
                
            }
            comboBox1.SelectedIndex=0;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Package p in model.Packages)
            {
                if (p.Name == comboBox1.SelectedItem.ToString())
                {
                    asis = p;
                }

            }
            int ileFID=ZmielPakiet(asis);
            LiczbaFidLbl.Text = ileFID.ToString();
        }
        private int ZmielPakiet( Package jaPckg)
        {
            int wynik = 0;
            foreach(Package p1 in jaPckg.Packages)
            {
                wynik+=ZmielPakiet(p1);
            }
            foreach (Element e in jaPckg.Elements)
            {
                wynik+=ZmielElement(e);
            }
            return wynik;
        }
        private int ZmielElement( Element jaElem)
        {
            int wynik = 0;
            //ZmielElement(rodzicPckg,jaElem
            foreach (Element e in jaElem.Elements)
            {
                wynik+=ZmielElement( e);
            }
            //mielenie
            //daj wszystkie konektory
            foreach (Connector c in jaElem.Connectors)
            {
                ////dla kazdego konektora typu information flow daj source
                if (c.Type != "InformationFlow") continue;
                ////dla kazdego konektora daj destination
                Element elSource = Repo.GetElementByID(c.ClientID);
                Element elDestination = Repo.GetElementByID(c.SupplierID);
              //patrz tylko na te ktorych realizatorem jest jaElem
                if (jaElem.ElementID == elDestination.ElementID)
                {
                   //wez interfejs lub go dodaj
                    Element interf = EAUtils.dodajElement(ref jaElem, "Interfejs " + jaElem.Name, "", "Interface");
                   
                    ////do interfejsu dodaj operację (nazwa to fid, notatka to notatka, parametr to nazwa systemu target)
                    EA.Method m = EAUtils.dodajOperacje(ref interf, c.Name, c.Notes);
                   try{
                    m.Parameters.AddNew(elSource.Name, "");
                    m.Update();
                    m.Parameters.Refresh();
                       }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
                    ////utworz relacje use z source do interfejs
                    Connector conUse = EAUtils.dodajRelacje(elSource, interf, "Usage", "", "");
                    ////utworz relacje realize z destination do interfejs
                    Connector conReal = EAUtils.dodajRelacje(elSource, interf, "Realisation","","");
                   
                    wynik++;
                }
               
            }
            return wynik;
        }
        
    }
}
