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
    public partial class Slownik : Form
    {
        Repository repository;
        Package slownikPckg;
        Package definicjePckg;
      //  String TAG = "";
        String Folder = "";
        String podFolder = "";
        String[] Kolumny = null;
        String[] Tagi = null;
        string typElementu;
        public Slownik(EA.Repository repo,String typElementow,String folder,String Podfolder,String [] kolumny,String [] tagi, int[] szerokoscKolumn)
        {
            repository = repo;
          //  TAG = waznytag;
            Folder = folder;
            typElementu = typElementow;
            podFolder = Podfolder;
            InitializeComponent();
            Kolumny = kolumny;
            Tagi = tagi;
           
            dataGridView1.Columns.Clear();
            for(int i=0;i<Kolumny.Count();i++)
            {
                dataGridView1.Columns.Add(Kolumny[i], Kolumny[i]);
                dataGridView1.Columns[i].Width = szerokoscKolumn[i];
                dataGridView1.Columns[i].Name = Kolumny[i];
            }
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void anuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {

            int ile = dataGridView1.Rows.Count;
            if (slownikPckg.Elements.Count > ile) ile = slownikPckg.Elements.Count;

            for (short i = 0; i < ile; i++)
            {
                Element sl = null;
                if(slownikPckg.Elements.Count>i)sl=slownikPckg.Elements.GetAt(i);
                String d="";
                if(dataGridView1.Rows[i].Cells[1].Value!=null)
                    d=dataGridView1.Rows[i].Cells[1].Value.ToString();
                  String v="";
                if(dataGridView1.Rows[i].Cells[2].Value!=null)
                    v=dataGridView1.Rows[i].Cells[2].Value.ToString();
                // czy usunac - gdy bylo pole a teraz jest puste, lub gdy nazwa jest inna

                if (d == "") //do usuniecia na koncu i od konca by indeksy sie nie przesuwały
                {
                    if (sl != null)
                    {
                        slownikPckg.Elements.Delete(i);
                   }
                }
                if (sl != null && sl.Name != d)
                {
                    slownikPckg.Elements.Delete(i);
                }
                // czy zmienic - gdy nazwa ta sama
                if (sl != null && sl.Name == d)
                {
                    for (short j = 0; j < Tagi.Count(); j++)
                    {
                        //EAUtils.zmienTaggedValues(ref sl, TAG, v);
                        v = ""; 
                        if(dataGridView1.Rows[i].Cells[j+2].Value!=null)
                            v=dataGridView1.Rows[i].Cells[j+2].Value.ToString();
                        if (Tagi[j] == "NOTATKA")
                        {
                            sl.Notes = v;
                            sl.Update();
                           // sl.Refresh();
                        }
                        else
                        {
                            EAUtils.zmienTaggedValues(ref sl, Tagi[j], v);
                        }
                    }
                }
                // czy dodac - gdy nie bylo lub nazwa inna
                if ((d!="") &&(sl == null || sl.Name != d))
                {
                   Element elem= EAUtils.dodajElement(ref slownikPckg, d, Folder+"->"+podFolder,typElementu);
                   //EAUtils.dodajTaggedValues(ref elem, TAG, v);
                   for (short j = 0; j < Tagi.Count(); j++)
                   {
                       //EAUtils.zmienTaggedValues(ref sl, TAG, v);
                       v = "";
                       if (dataGridView1.Rows[i].Cells[j + 2].Value != null)
                           v = dataGridView1.Rows[i].Cells[j + 2].Value.ToString();
                       if (Tagi[j] == "NOTATKA")
                       {
                           elem.Notes = v;
                           elem.Update();
                          // elem.Refresh();
                       }
                       else
                       {
                           EAUtils.dodajTaggedValues(ref elem, Tagi[j], v);
                       }
                   }
                }

            }
            slownikPckg.Update();
            slownikPckg.Elements.Refresh();
            this.Close();
            return;
           
        }

        private void Slownik_Load(object sender, EventArgs e)
        {
           
        
            slownikPckg = null;
            definicjePckg = null;
            Package model = EAUtils.dajModelPR(ref repository); //repository.Models.GetAt(0);

           
                try
                {
                    definicjePckg = model.Packages.GetByName(Folder);
                }
                catch
                {//wyjatek brak pakietu?
                }
                if (definicjePckg == null)
                {
                    try{
                    definicjePckg = model.Packages.AddNew(Folder, "");
                    definicjePckg.Update();
                    model.Packages.Refresh();
                        }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
                }
                if (podFolder == "")
                {
                    slownikPckg = definicjePckg;
                }
                else
                {
                    try
                    {
                        slownikPckg = definicjePckg.Packages.GetByName(podFolder);
                    }
                    catch (Exception)
                    {

                    }
                    if (slownikPckg == null)
                    {
                        try{
                        slownikPckg = definicjePckg.Packages.AddNew(podFolder, "");
                        slownikPckg.Update();
                        model.Packages.Refresh();
                            }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
                    }
                }
            int i=1;
            foreach(Element elem in slownikPckg.Elements)
            {
                String[] pole = new String[Tagi.Count() + 2];
                for (short j = 0; j < Tagi.Count(); j++)
                {
                    
                //    DataGrindViewRow dr;
                    try
                    {
                        //    String pole = elem.TaggedValues.GetByName(TAG).Value;
                      
                        pole[0]=i.ToString();
                        pole[1]=elem.Name;
                        if (Tagi[j] == "NOTATKA")
                        {
                            pole[j + 2] = elem.Notes;
                        }
                        else
                        {
                            TaggedValue t = elem.TaggedValues.GetByName(Tagi[j]);
                            if (t != null)
                            {//dataGridView1.Rows.Add(i, elem.Name, pole);
                                pole[j + 2] = t.Value.ToString();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                       // dataGridView1.Rows.Add(i, elem.Name, exc.Message);
                        pole[j + 2] = exc.Message;
                    }
                }
                dataGridView1.Rows.Add(pole);
                i++;
            }
            

        }
    }
}
