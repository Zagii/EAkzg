using EA;
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
  

    public partial class CgenerujPakietyWkladow : Form
    {
        Package archStatPckg;
        Package wkladyPackage;
        Repository repo;
        CModel modelProjektu;
        int obszar;
        List<CdrzewkoSystem>  systemy=new List<CdrzewkoSystem>();
        public List<Element> dajSystemy()
        {
            List<Element> wyn=new List<Element>();
            foreach(CdrzewkoSystem sys in systemy)
            {
               if(sys.czyZaznaczono())
                   wyn.Add(sys.dajElem());
            }
            return wyn;
        }
        public List<Element> dajInterfejsy()
        {
            List<Element> wyn=new List<Element>();
            foreach(CdrzewkoSystem sys in systemy)
            {
                if(sys.czyZaznaczono())
                 wyn.AddRange(sys.dajInterfejsy());
            }
            return wyn;
        }

        public CgenerujPakietyWkladow(CModel ModelProjektu,int Obszar)
        {
            obszar = Obszar;
            modelProjektu = ModelProjektu;
            archStatPckg = modelProjektu.ArchStatPckg[obszar];
            wkladyPackage = modelProjektu.WkladyPckg[obszar];
            repo = modelProjektu.Repozytorium;
            InitializeComponent();
            utworzListy();
            ustawDrzewko();
        }
        public CgenerujPakietyWkladow(ref Repository Repo,ref Package aPckg, ref Package wPckg)
        {
            
            archStatPckg=aPckg;
            wkladyPackage=wPckg;
            repo=Repo;
            InitializeComponent();
            utworzListy();
            ustawDrzewko();
        }
        private void utworzListy()
        {
            systemy.Clear();
           
            foreach (Diagram diag in archStatPckg.Diagrams)
            {
                foreach (DiagramObject diagObj in diag.DiagramObjects)
                {
                    try
                    {
                        Element element = repo.GetElementByID(diagObj.ElementID);
                        if (element.Type == "Component")
                        {
                            if (systemy.Exists(x => x.dajID() == element.ElementID))
                            { }
                            else
                            {
                                systemy.Add(new CdrzewkoSystem(element));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String exc = e.Message;
                    }
                }
            }
            foreach (Diagram diag in archStatPckg.Diagrams)
            {
                foreach (DiagramObject diagObj in diag.DiagramObjects)
                {
                    Element element = repo.GetElementByID(diagObj.ElementID);
                    if (element.Type == "Interface")
                    {
                        //daj system ktory go realizuje
                        Element CliElement=null;
                        Element SupElement=null;
                        foreach (Connector c in element.Connectors)
                        {
                            if (c.Type == "Realisation")
                            {

                                CliElement = repo.GetElementByID(c.ClientID);
                                SupElement = repo.GetElementByID(c.SupplierID);
                            }
                        }
                        if (SupElement == null)///blad
                        {
                            MessageBox.Show("Brak systemu dla interfejsu-" + element.Name);
                            return; 
                        }
                        //dodaj do jego drzewka interfejs jesli go nie ma
                        for (int i = 0; i < systemy.Count; i++)
                        {
                            if (systemy[i].dajID() == CliElement.ElementID)
                            {
                                systemy[i].dodajInterfejsJesliGoNieMa(SupElement);
                            }

                        }
                    }
                }
            }
        }
        public bool czyAutonumeracjaFeature()
        {
            return autoNumeracjeCB.Checked;
        }

        private void  ustawDrzewko()
        {
            treeView1.Nodes.Clear();
            TreeNode root=new TreeNode("Systemy wyszczególnione na diagramach architektury statycznej");
            treeView1.Nodes.Add(root);
            root.Checked = true;

            foreach (CdrzewkoSystem sys in systemy)
            {
               root.Nodes.Add(sys.zrobNody());
                
            }
            root.Expand();
            //root.Collapse(false);
            
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.ExpandAll();
            foreach (TreeNode t in node.Nodes)
            {
                t.Checked = node.Checked;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          /*
                for(int i=0;i<systemy.Count;i++)
            {
                if (!systemy[i].usunZbedneNody())
                {
                    systemy.Remove(systemy[i]);
                }
            }*/
            
        }
    }
   public class CdrzewkoInterfejs : System.Object
    {
        Element interfejs;
        //checkbox
        TreeNode e;
        public CdrzewkoInterfejs(Element el)
        {
            interfejs = el;

        }
        public Element dajElem()
        {
            return interfejs;
        }
        public int dajID()
        {
            return interfejs.ElementID;
        }
        public TreeNode zrobNode()
        {
            e = new TreeNode("Przenieś obiekt interfejsu: "+interfejs.Name);
            e.Checked = true;
            return e;
        }
         public bool czyZaznaczono()
        {
          return e.Checked;
        }
    }
    class CdrzewkoSystem : System.Object
    {
        List<CdrzewkoInterfejs> interfejsy = new List<CdrzewkoInterfejs>();
        Element system;
        //checkbox
        TreeNode e;
        public bool czyZaznaczono()
        {
            return e.Checked;
        }
        public List<Element> dajInterfejsy()
        {
            List<Element> wyn=new List<Element>();
            foreach(CdrzewkoInterfejs i in interfejsy)
            {
              if(i.czyZaznaczono())
                  wyn.Add(i.dajElem());
            }
            return wyn;
        }
        public void dodajInterfejsJesliGoNieMa(Element el)
        {
            if (interfejsy.Exists(x => x.dajID() == el.ElementID))
            { }
            else
            {
                interfejsy.Add(new CdrzewkoInterfejs(el));
            }
        }
        public Element dajElem()
        {
            return system;
        }
        public CdrzewkoSystem(Element el)
        {
            system = el;

        }
        public bool usunZbedneNody()
        {
            
                for(int i =0;i<interfejsy.Count;i++)
            {
                if(!interfejsy[i].czyZaznaczono())
                {
                    interfejsy.Remove(interfejsy[i]);
                }
            }
            return e.Checked;
        }
        public TreeNode zrobNody()
        {
            e = new TreeNode("Generuj pakiet wkładu systemowego: "+system.Name);
            e.Checked = true;
            foreach (CdrzewkoInterfejs i in interfejsy)
            {
                e.Nodes.Add(i.zrobNode());
                
            }
            return e;
        }

        public int dajID()
        {
            return system.ElementID;
        }
        /*    public void Add(Element el)
            {
                system = el;
            }*/

    }
}
