using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OutlookStyleControls;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace EAkzg
{

    public partial class WymaganiaForm : Form
    {
        EA.Repository rep;
        EA.Package projekt = null;
        CModel modelRepo;
        //lista systemów z arch stat
        List<EA.Element> systemy = new List<EA.Element>();
        //lista wszystkich wymagan biznesowych
        List<EA.Element> wymagania = new List<EA.Element>();
        //lista ficzerów dla danego wymagania w podziale na systemy ficzeryList[3].list[4] to lista ficzerow dla wymagania[3], system[4]
        List<List<List<EA.Element>>> ficzeryList = new List<List<List<EA.Element>>>();
        //lista indeksow rodzica (wymagania biznesoweg) danego ficzera ficzeryListRodzic[3] oznacza ficzer dla wymagania[3]
        List<int> ficzeryListRodzic=new List<int>();
        //lista indeksow rodzica (wymagania biznesoweg) danego ficzera ficzeryListLpRodzic[3] oznacza pozycje w tabeli Lp ficzera dla wymagania[3]
        List<int> ficzeryListLpRodzic = new List<int>();

        const int KOL_Lp = 0;
        const int KOL_R = 1;
        const int KOL_Wym = 2;

        WymaganiaFormPodglad podgladForm;
        CzekajWnd czekajWnd;

        public WymaganiaForm(EA.Repository repository)
        {
            
            InitializeComponent();
            rep = repository;
            m_SynchronizationContext = SynchronizationContext.Current;
        }

        private void WymaganiaForm_Load(object sender, EventArgs e)
        {
           
            dataGridView1.Enabled = false;
            generowanieLbl.Visible = false;
           Log(new CLog(LogMsgType.Info, "Wczytywanie..."));
           bool ba = rep.BatchAppend;
           Log(new CLog(LogMsgType.cd, " batchAppend " +ba ));
           rep.BatchAppend = true;
            odczytajModelStart();
            rep.BatchAppend = ba;
           // this.podgladForm.Closed += (ssender, args) => this.podgladForm = null;       
        }

        private async void odczytajModelStart()
        {
            czekajWnd=new CzekajWnd();
            czekajWnd.Show(this);
            czekajWnd.ustawPB(0,5);
            czekajWnd.ustawPBkrok(1);
            czekajWnd.ustawTxt("Czekaj, trwa wczytywanie modelu EA...\n ..spokojnie możesz iść po kawę..");

            //generowanieLbl.Text = "Czekaj, trwa wczytywanie modelu EA...\n ..spokojnie możesz iść po kawę..";
            generowanieLbl.Visible = true;
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(() => odczytajModelWatek());
           
            try
            {

                await task;
                Log(new CLog(LogMsgType.Info, "[ok]\n"));
               
                dataGridView1.Enabled = true;
                ProgressBarKrok();
                m_SynchronizationContext.Post((@object) =>
                {
                    
                    Log(new CLog(LogMsgType.Info, "Wypełnianie tabelki.."));
                }, null);
                
            //  RobTabelke();
                dataGridView1.Visible = false;
                System.Threading.Tasks.Task task2 = System.Threading.Tasks.Task.Run(() => RobTabelke());
              
               await task2;
               dataGridView1.Visible = true;
                m_SynchronizationContext.Post((@object) =>
                {
                    Log(new CLog(LogMsgType.Info, "[ok]\n"));
                }, null);
             
                generowanieLbl.Visible = false;
                czekajWnd.Close();
            }
            catch (OperationCanceledException e)
            { //sprzatanie 
                generowanieLbl.Text = "Błąd... "+e.Message;
            }

        }
        private void odczytajModelWatek()
        {
            ProgressBarKrok();
            projekt = EAUtils.dajModelPR(ref rep);
            modelRepo=new CModel(ref rep);
         
        }
        private void ProgressBarKrok()
        {
            m_SynchronizationContext.Post((@object) =>
            {
                czekajWnd.ustawPBNast();
            }, null);
        }

        private void invA()
        {
            dataGridView1.Columns.Add("NrWiersza", "NrWiersza");
            dataGridView1.Columns.Add("Rodzic", "ReqNo / Lp / RodzicLp");
            dataGridView1.Columns.Add("REQ", "Wymaganie Biznesowe \\ System");
            dataGridView1.Columns[KOL_Lp].ReadOnly = true;
            dataGridView1.Columns[KOL_Lp].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[KOL_R].ReadOnly = true;
            dataGridView1.Columns[KOL_R].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[KOL_Wym].ReadOnly = true;
            dataGridView1.Columns[KOL_Wym].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dodajWierszWymaganiaInv(ref int i, int reqNo,EA.Element req)
        {

            i = dataGridView1.Rows.Add();
            ficzeryListRodzic.Add(reqNo);
            ficzeryListLpRodzic.Add(i);

                            dataGridView1.Rows[i].Cells[KOL_Lp].Value = i;
                            dataGridView1.Rows[i].Cells[KOL_R].Value = reqNo;
                            dataGridView1.Rows[i].Cells[KOL_Wym].Value = req.Name;
                            dataGridView1.Rows[i].Cells[KOL_Wym].ToolTipText = req.Notes;
                            dataGridView1.Rows[i].Cells[KOL_Wym].Style.Font = new Font("Arial", 10, FontStyle.Bold);

                            var checkedButton = groupBox2.Controls.OfType<RadioButton>()
                                             .FirstOrDefault(r => r.Text == req.Status);
                            if (checkedButton != null)
                            {
                                dataGridView1.Rows[i].Cells[KOL_Wym].Style.BackColor = checkedButton.BackColor;
                                dataGridView1.Rows[i].Cells[KOL_Wym].Style.ForeColor = checkedButton.ForeColor;
                            }

        }

        private void komorkaValue(int r, int c, object s)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.Rows[r].Cells[c].Value = s;
                }));
            }
            else
            {
                dataGridView1.Rows[r].Cells[c].Value = s;
            }
        }
        private void komorkaToolTip(int r, int c, object s)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.Rows[r].Cells[c].ToolTipText = s.ToString();
                }));
            }
            else
            {
                dataGridView1.Rows[r].Cells[c].ToolTipText = s.ToString();
            }
        }
        private void komorkaBackColor(int r, int c, Color s)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.Rows[r].Cells[c].Style.BackColor = s;
                }));
            }
            else
            {
                dataGridView1.Rows[r].Cells[c].Style.BackColor = s;
            }
        }
        private void komorkaForeColor(int r, int c, Color s)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.Rows[r].Cells[c].Style.ForeColor=s;
                }));
            }
            else
            {
                dataGridView1.Rows[r].Cells[c].Style.ForeColor = s;
            }
        }
        private String komorkaDajValue(int r, int c)
        {
            String s = "$$$$$$";
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() => { s = dataGridView1.Rows[r].Cells[c].Value.ToString(); }));

            }
            else
            {
                s = dataGridView1.Rows[r].Cells[c].Value.ToString();
            }
            return s;
        }
        private String komorkaDajToolTip(int r, int c)
        {
            String s = "$$$$$$";
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() => { s = dataGridView1.Rows[r].Cells[c].ToolTipText; }));

            }
            else
            {
                s = dataGridView1.Rows[r].Cells[c].ToolTipText;
            }
            return s;
        }
        private void dodajWierszFiczeraInv(int i,int ind,int reqNo,EA.Element featureEl,EA.Element req, List<List<EA.Element>> ficzeryDlaReq, List<EA.Element> ficzeryDlaSyst, int liczbaWierszyReq)
        {
           //wstaw feature
                                                if (ficzeryDlaSyst.Count > 0 && ficzeryDlaSyst.Count >= liczbaWierszyReq)
                                                {
                                                    int lp = 0; ;
                                                    if (dataGridView1.InvokeRequired)
                                                    {
                                                        dataGridView1.Invoke(new Action(() => lp = dataGridView1.Rows.Add()));
                                                    }
                                                    else
                                                    {
                                                        dataGridView1.Invoke(new Action(() => lp = dataGridView1.Rows.Add()));
                                                    }
                                                    
                                                    int gdzieLpRodzic = ficzeryListRodzic.FindIndex(ir => ir == reqNo);
                                                    //dataGridView1.Rows[lp].Cells[KOL_Lp].Value = lp;
                                                    komorkaValue(lp, KOL_Lp, lp);
                                                   // dataGridView1.Rows[lp].Cells[KOL_R].Value = "Klon->" + reqNo + " / " + (lp - gdzieLpRodzic) + " / " + gdzieLpRodzic;
                                                    komorkaValue(lp, KOL_R, "Klon->" + reqNo + " / " + (lp - gdzieLpRodzic) + " / " + gdzieLpRodzic);
                                                    //dataGridView1.Rows[lp].Cells[KOL_Wym].Value = dataGridView1.Rows[lp - 1].Cells[KOL_Wym].Value;
                                                    komorkaValue(lp,KOL_Wym,komorkaDajValue(lp-1,KOL_Wym));
                                                   // dataGridView1.Rows[lp].Cells[KOL_Wym].ToolTipText = dataGridView1.Rows[lp - 1].Cells[KOL_Wym].ToolTipText;
                                                    komorkaToolTip(lp,KOL_Wym,komorkaDajToolTip(lp-1,KOL_Wym));
                                               //     dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].Value = featureEl.Name;
                                                    komorkaValue(lp, ind + KOL_Wym + 1, featureEl.Name);
                                                    //dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].ToolTipText = featureEl.Notes;
                                                    komorkaToolTip(lp, ind + KOL_Wym + 1, featureEl.Notes);

                                                    Color fc=Color.Black, bc=Color.White;
                                                    if(groupBox2.InvokeRequired)
                                                    {
                                                        groupBox2.Invoke(new Action(() =>
                                                            {
                                                                var checkedButton1 = groupBox2.Controls.OfType<RadioButton>()
                                                                  .FirstOrDefault(r => r.Text == req.Status);
                                                                if (checkedButton1 != null)
                                                                {
                                                                    fc = checkedButton1.ForeColor;
                                                                    bc = checkedButton1.BackColor;
                                                                   
                                                                }
                                                            }));
                                                    }else
                                                    {
                                                    
                                                    var checkedButton1 = groupBox2.Controls.OfType<RadioButton>()
                                                                   .FirstOrDefault(r => r.Text == req.Status);
                                                    if (checkedButton1 != null)
                                                    {
                                                        fc = checkedButton1.ForeColor;
                                                        bc = checkedButton1.BackColor;
                                                       
                                                    }
                                                    }

                                                    //dataGridView1.Rows[lp].Cells[KOL_Wym].Style.BackColor = checkedButton1.BackColor;
                                                    //dataGridView1.Rows[lp].Cells[KOL_Wym].Style.ForeColor = checkedButton1.ForeColor;
                                                    komorkaBackColor(lp, KOL_Wym, bc);
                                                    komorkaForeColor(lp, KOL_Wym, fc);

                                                    fc = Color.Black; bc = Color.White;

                                                    if (groupBox1.InvokeRequired)
                                                    {
                                                        groupBox1.Invoke(new Action(() =>
                                                            {
                                                                var checkedButton2 = groupBox1.Controls.OfType<RadioButton>()
                                                                             .FirstOrDefault(r => r.Text == featureEl.Status);
                                                                if (checkedButton2 != null)
                                                                {
                                                                    //    dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].Style.BackColor = checkedButton2.BackColor;
                                                                    //  dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].Style.ForeColor = checkedButton2.ForeColor;
                                                                    fc = checkedButton2.ForeColor;
                                                                    bc = checkedButton2.BackColor;
                                                                }
                                                            }));
                                                    }
                                                    else
                                                    {
                                                        var checkedButton2 = groupBox1.Controls.OfType<RadioButton>()
                                                                .FirstOrDefault(r => r.Text == featureEl.Status);
                                                        if (checkedButton2 != null)
                                                        {
                                                            //    dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].Style.BackColor = checkedButton2.BackColor;
                                                            //  dataGridView1.Rows[lp].Cells[ind + KOL_Wym + 1].Style.ForeColor = checkedButton2.ForeColor;
                                                            fc = checkedButton2.ForeColor;
                                                            bc = checkedButton2.BackColor;
                                                        }
                                                    }
                                                    // dt.Rows.Add(req.Name);
                                                    komorkaBackColor(lp, ind + KOL_Wym + 1, bc);
                                                    komorkaForeColor(lp, ind + KOL_Wym + 1, fc);
                                                    ficzeryDlaSyst.Add(featureEl);

                                                    ficzeryListRodzic.Add(reqNo);
                                                    ficzeryListLpRodzic.Add(ficzeryListLpRodzic[lp - 1]);

                                                }
                                                else //dodaj ficzer ale nie dodawaj wiersza w tabelce
                                                {
                                                    ficzeryListRodzic[i + ficzeryDlaSyst.Count] = reqNo;
                                                    if (ficzeryListLpRodzic[i + ficzeryDlaSyst.Count] == i + ficzeryDlaSyst.Count)
                                                    {
                                                      //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[KOL_R].Value =
                                                          //   ficzeryListRodzic[i + ficzeryDlaSyst.Count] + " / " + ficzeryListLpRodzic[i + ficzeryDlaSyst.Count];
                                                        komorkaValue(i + ficzeryDlaSyst.Count, KOL_R, ficzeryListRodzic[i + ficzeryDlaSyst.Count] + " / " + ficzeryListLpRodzic[i + ficzeryDlaSyst.Count]);
                                                    }
                                                    else
                                                    {
                                                        int gdzieLpRodzic = ficzeryListRodzic.FindIndex(ir => ir == reqNo);
                                                    //    dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[KOL_R].Value =
                                                     //   "Klon->" + ficzeryListRodzic[i + ficzeryDlaSyst.Count] + " / " + (i + ficzeryDlaSyst.Count - gdzieLpRodzic) + " / " + gdzieLpRodzic;
                                                        komorkaValue(i + ficzeryDlaSyst.Count, KOL_R, "Klon->" + ficzeryListRodzic[i + ficzeryDlaSyst.Count] + " / " + (i + ficzeryDlaSyst.Count - gdzieLpRodzic) + " / " + gdzieLpRodzic);

                                                    }
                                                    // dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[KOL_R].Value = reqNo + " / " + ficzeryListLpRodzic[i + ficzeryDlaSyst.Count];
                                                  //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].Value = featureEl.Name;
                                                    komorkaValue(i + ficzeryDlaSyst.Count, ind + KOL_Wym + 1, featureEl.Name);
                                                  //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].ToolTipText = featureEl.Notes;
                                                    komorkaToolTip(i + ficzeryDlaSyst.Count, ind + KOL_Wym + 1, featureEl.Notes);

                                                   Color fc = Color.Black, bc = Color.White;

                                                   if (groupBox1.InvokeRequired)
                                                   {
                                                       groupBox1.Invoke(new Action(() =>
                                                           {

                                                               var checkedButton3 = groupBox1.Controls.OfType<RadioButton>()
                                                                          .FirstOrDefault(r => r.Text == featureEl.Status);
                                                               if (checkedButton3 != null)
                                                               {
                                                                   //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].Style.BackColor = checkedButton3.BackColor;

                                                                   //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].Style.ForeColor = checkedButton3.ForeColor;
                                                                   fc = checkedButton3.ForeColor;
                                                                   bc = checkedButton3.BackColor;
                                                               }
                                                           }));
                                                   }
                                                   else
                                                   {
                                                       var checkedButton3 = groupBox1.Controls.OfType<RadioButton>()
                                                                          .FirstOrDefault(r => r.Text == featureEl.Status);
                                                       if (checkedButton3 != null)
                                                       {
                                                           //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].Style.BackColor = checkedButton3.BackColor;

                                                           //  dataGridView1.Rows[i + ficzeryDlaSyst.Count].Cells[ind + KOL_Wym + 1].Style.ForeColor = checkedButton3.ForeColor;
                                                           fc = checkedButton3.ForeColor;
                                                           bc = checkedButton3.BackColor;
                                                       }
                                                   }
                                                   komorkaBackColor(i + ficzeryDlaSyst.Count, ind + KOL_Wym + 1, bc);
                                                   komorkaForeColor(i + ficzeryDlaSyst.Count, ind + KOL_Wym + 1, fc);

                                                    ficzeryDlaSyst.Add(featureEl);
                                                }
                                                //pokoloruj status
        }
        private void pobierzSystSQL(ref long maxTmp, ref EA.Element maxEl, ref long maxGetElem, ref Stopwatch stLoop)
        {
         
            String sql = "select distinct o.object_id from t_diagram d,t_diagramobjects do,t_object o "+
                "where (d.Package_ID=" + modelRepo.ArchStatPckg[CModel.IT].PackageID + " or d.Package_ID=" + modelRepo.ArchStatPckg[CModel.NT].PackageID +
                ") and d.diagram_id=do.diagram_id and o.object_id=do.object_id and o.object_type='Component'";
            Log(new CLog(LogMsgType.Info, "sql="+sql+"\n"));
            ProgressBarKrok();
            foreach (EA.Element element in rep.GetElementSet(sql, 2))
            {

                maxTmp = stLoop.ElapsedMilliseconds;
                if (maxTmp > maxGetElem)
                {
                    maxGetElem = maxTmp;
                    maxEl = element;
                    if (maxTmp > 5000)
                        Log(new CLog(LogMsgType.Info, "Diag max ping=" + maxGetElem + "->" + maxEl.Name + "\n"));
                }
                stLoop.Restart();
                czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID);

                systemy.Add(element);
                DataGridViewRolloverCellColumn col = new DataGridViewRolloverCellColumn();
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderText = element.Name;

                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() =>
                    {

                        dataGridView1.Columns.Add(col);

                    }));
                }
                else
                {
                    dataGridView1.Columns.Add(col);
                }

            }
        }
        private void pobierzSyst(ref long maxTmp, ref EA.Element maxEl, ref long maxGetElem, ref Stopwatch stLoop)
        {
            
            for (int obszar = 0; obszar < 2; obszar++)
            {
                foreach (EA.Diagram diag in modelRepo.ArchStatPckg[obszar].Diagrams)
                {
                    foreach (EA.DiagramObject diagObj in diag.DiagramObjects)
                    {
                        ProgressBarKrok();
                        try
                        {


                            EA.Element element = rep.GetElementByID(diagObj.ElementID);
                            czekajWnd.ustawTxtElem(element.ElementID.ToString());
                            maxTmp = stLoop.ElapsedMilliseconds;
                            if (maxTmp > maxGetElem)
                            {
                                maxGetElem = maxTmp;
                                maxEl = element;
                                if (maxTmp > 5000)
                                    Log(new CLog(LogMsgType.Info, "Diag max ping=" + maxGetElem + "->" + maxEl.Name + "\n"));
                            }
                            stLoop.Restart();
                            czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID);
                            if (element.Type == "Component")
                            {
                                if (systemy.Exists(x => x.ElementID == element.ElementID))
                                { }
                                else
                                {
                                    systemy.Add(element);
                                    DataGridViewRolloverCellColumn col = new DataGridViewRolloverCellColumn();
                                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                                    col.HeaderText = element.Name;

                                    if (dataGridView1.InvokeRequired)
                                    {
                                        dataGridView1.Invoke(new Action(() =>
                                        {

                                            dataGridView1.Columns.Add(col);

                                        }));
                                    }
                                    else
                                    {
                                        dataGridView1.Columns.Add(col);
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            String exc = e.Message;
                        }
                    }
                }
            }
        }


        private void obslugaWymagania(EA.Element req, ref Stopwatch stEl, ref long maxTmp, ref long maxGetElem, ref EA.Element maxEl, int i, int reqNo)
        {
            foreach (EA.Connector c in req.Connectors)
            {
                stEl.Restart();
                EA.Element featureEl = rep.GetElementByID(c.ClientID);
                czekajWnd.ustawTxtElem("R:" + req.ElementID + " F " + featureEl.ElementID.ToString());
                maxTmp = stEl.ElapsedMilliseconds;
                if (maxTmp > maxGetElem)
                {
                    maxGetElem = maxTmp;
                    maxEl = featureEl;
                    if (maxTmp > 5000)
                        Log(new CLog(LogMsgType.Info, "Feature max ping=" + maxGetElem + "->" + maxEl.Name + " connNo:" + maxEl.Connectors.Count + "\n"));
                }
                czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID + " connNo:" + maxEl.Connectors.Count);

                if (featureEl != null)
                {
                    Stopwatch stSys = new Stopwatch();

                    foreach (EA.Connector sysC in featureEl.Connectors)
                    {
                        stSys.Restart();
                        EA.Element sysEl = rep.GetElementByID(sysC.ClientID);
                        czekajWnd.ustawTxtElem("R:" + req.ElementID + " F " + featureEl.ElementID.ToString() + " S: " + sysEl.ElementID.ToString());
                        maxTmp = stSys.ElapsedMilliseconds;
                        if (maxTmp > maxGetElem)
                        {
                            maxGetElem = maxTmp;
                            maxEl = featureEl;
                            if (maxTmp > 5000)
                                Log(new CLog(LogMsgType.Info, "Syst max ping=" + maxGetElem + "->" + maxEl.Name + " connNo:" + maxEl.Connectors.Count + "\n"));
                        }
                        czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID + " connNo:" + maxEl.Connectors.Count);

                        if (sysEl != null)
                        {
                            //sprawdz do ktorego systemu jest feature
                            int ind = systemy.FindIndex(x => x.ElementID == sysEl.ElementID);

                            if (ind >= 0)
                            {


                                List<List<EA.Element>> ficzeryDlaReq = ficzeryList[reqNo];
                                List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[ind];

                                ///ile wierszy w tab jest dla tego req
                                int liczbaWierszyReq = ficzeryDlaReq.Max(mm => mm.Count);

                                if (reqNo == 1)
                                {
                                    int a = 3;
                                    // return;
                                }
                                dodajWierszFiczeraInv(i, ind, reqNo, featureEl, req, ficzeryDlaReq, ficzeryDlaSyst, liczbaWierszyReq);

                            }

                        }
                    }

                }

            }
        }
        private void obslugaWymaganiaSQL(EA.Element req, ref Stopwatch stEl, ref long maxTmp, ref long maxGetElem, ref EA.Element maxEl, int i, int reqNo)
        {
            String sql = "select fo.object_id from t_connector c, t_object ro, t_object fo " +
                "where c.Start_Object_ID=fo.object_id and c.end_object_id=ro.object_id and fo.object_type='Feature' " +
                "  and ro.object_id="+req.ElementID;
         //   Log(new CLog(LogMsgType.Info, "sql=" + sql + "\n"));
            foreach ( EA.Element featureEl  in rep.GetElementSet(sql, 2))
            {
                stEl.Restart();
          
                czekajWnd.ustawTxtElem("R:" + req.ElementID + " F " + featureEl.ElementID.ToString());
              
                if (featureEl != null)
                {
                    Stopwatch stSys = new Stopwatch();
                    sql="select so.object_id from t_connector c, t_object so, t_object fo "+ 
                       "where c.Start_Object_ID=so.object_id and c.end_object_id=fo.object_id and so.object_type='Component' "+  
                        "and fo.object_id="+featureEl.ElementID;
               //     Log(new CLog(LogMsgType.Info, "sql=" + sql + "\n"));
                    foreach (EA.Element sysEl in  rep.GetElementSet(sql, 2))
                    {
                        stSys.Restart();
                      
                       
                        if (sysEl != null)
                        {
                            //sprawdz do ktorego systemu jest feature
                            int ind = systemy.FindIndex(x => x.ElementID == sysEl.ElementID);

                            if (ind >= 0)
                            {


                                List<List<EA.Element>> ficzeryDlaReq = ficzeryList[reqNo];
                                List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[ind];

                                ///ile wierszy w tab jest dla tego req
                                int liczbaWierszyReq = ficzeryDlaReq.Max(mm => mm.Count);

                                if (reqNo == 1)
                                {
                                    int a = 3;
                                    // return;
                                }
                                dodajWierszFiczeraInv(i, ind, reqNo, featureEl, req, ficzeryDlaReq, ficzeryDlaSyst, liczbaWierszyReq);

                            }

                        }
                        czekajWnd.ustawTxtElem("R:" + req.ElementID + " F " + featureEl.ElementID.ToString() + " S: " + sysEl.ElementID.ToString());
                        maxTmp = stSys.ElapsedMilliseconds;
                        if (maxTmp > maxGetElem)
                        {
                            maxGetElem = maxTmp;
                            maxEl = featureEl;
                            if (maxTmp > 5000)
                                Log(new CLog(LogMsgType.Info, "Syst max ping=" + maxGetElem + "->" + maxEl.Name + " connNo:" + maxEl.Connectors.Count + "\n"));
                        }
                        czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID + " connNo:" + maxEl.Connectors.Count);

                    }

                }
                maxTmp = stEl.ElapsedMilliseconds;
                if (maxTmp > maxGetElem)
                {
                    maxGetElem = maxTmp;
                    maxEl = featureEl;
                    if (maxTmp > 5000)
                        Log(new CLog(LogMsgType.Info, "Feature max ping=" + maxGetElem + "->" + maxEl.Name + " connNo:" + maxEl.Connectors.Count + "\n"));
                }
                czekajWnd.ustawTxtGetElemByID(maxTmp.ToString() + " max=" + maxGetElem + ":" + maxEl.ElementID + " connNo:" + maxEl.Connectors.Count);


            }
        }
        private void RobTabelke()
        {
           
            long maxLoop = 0, maxGetElem = 0,maxTmp=0;
            EA.Element maxEl = null;
            Stopwatch stDiag = new Stopwatch();
            Stopwatch stLoop=new Stopwatch();
            Stopwatch stRobTab = new Stopwatch();
            
            stRobTab.Start();
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    invA();
                }));
            }
            else
            {
                invA();
            }
           
            systemy.Clear();
            
            Log(new CLog(LogMsgType.Info, "Init ping="+stRobTab.ElapsedMilliseconds+"\n"));
   

            stLoop.Start();


       //    pobierzSyst(ref maxTmp, ref  maxEl, ref  maxGetElem, ref  stLoop);
            pobierzSystSQL(ref maxTmp, ref  maxEl, ref  maxGetElem, ref  stLoop);
           
            Log(new CLog(LogMsgType.Info, "SysAdd ping=" + stRobTab.ElapsedMilliseconds + "\n"));
            
            wymagania.Clear();

            wymagania.AddRange(modelRepo.WymaganiaBiznesoweLista);
            wymagania.AddRange(modelRepo.WymaganiaArchitektoniczneLista);
            wymagania.AddRange(modelRepo.WymaganiaInfrastrukturaLista) ;

            czekajWnd.ustawTxt("Trwa wypełnianie macierzy...");
            czekajWnd.ustawPB(0, wymagania.Count);

            try
            {
                int reqNo = 0;
               
                Stopwatch stWiersza = new Stopwatch();
               
                foreach (EA.Element req in wymagania)
                {
                    stLoop.Restart();
                    stWiersza.Restart();
                    
              
                    ProgressBarKrok();
                    if (reqNo == 3)
                    {
                        //break;
                    }
                    ficzeryList.Add(new List<List<EA.Element>>());
                    for(int intSys=0;intSys<systemy.Count;intSys++)
                    {
                        ficzeryList[reqNo].Add(new List<EA.Element>());
                    }
                    int i = 0;

                 
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke(new Action(() =>
                        {
                            dodajWierszWymaganiaInv(ref i, reqNo, req);

                        }));
                    }
                    else
                    {
                        dodajWierszWymaganiaInv(ref i, reqNo, req);
                    }
                    stWiersza.Stop();
                   
                    //daj liste linkow do feature

                    Stopwatch stEl = new Stopwatch();

                    obslugaWymaganiaSQL( req, ref stEl, ref maxTmp, ref maxGetElem, ref maxEl,  i, reqNo);

                    reqNo++;
                    if (stLoop.ElapsedMilliseconds > maxLoop)
                    {
                        maxLoop = stLoop.ElapsedMilliseconds;
                        
                        Log(new CLog(LogMsgType.Info, "Req max loop=" + maxLoop + "->" + req.Name + " connNo:" + req.Connectors.Count + "\n"));
                    }
                    czekajWnd.ustawTxtReqLoop((stLoop.ElapsedMilliseconds + " / " + maxLoop)+" stWiersza: "+stWiersza.ElapsedMilliseconds + "-"+ ((long)stLoop.ElapsedMilliseconds-(long)stWiersza.ElapsedMilliseconds).ToString());
                    stLoop.Restart();
                   
                }
                Log(new CLog(LogMsgType.Info, "Koniec " + stRobTab.Elapsed.ToString("hh\\:mm\\:ss") + "\n"));
          
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void pokazPodglad(EA.Element el,int r,int c)
        {

            if (podgladForm == null)
            {
                podgladForm = new WymaganiaFormPodglad(modelRepo,this);
            }
            if (!podgladForm.Visible)
            {
                podgladForm.Dispose();
                podgladForm = new WymaganiaFormPodglad(modelRepo,this);
                podgladForm.Show(this);
            }

            try
            {


                podgladForm.ustawElement(el,r,c);
            }
            catch (Exception e)
            {
                Log(new CLog(LogMsgType.Error, "Błąd-Okno podgląd: " + el.Name + " exc:" + e.Message));
            }
        }
        public void zmienObiektDataGridView(int r,int c)
        {
            if (c == KOL_Wym)
            { 
                ////////////dla requirementów

            }
            if (c > KOL_Wym)
            {
                int rq = ficzeryListRodzic[r];
                List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
                List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[c - KOL_Wym - 1];

                int nrficzSys = r - ficzeryListLpRodzic[r];
                if (nrficzSys >= ficzeryDlaSyst.Count)
                {
                    MessageBox.Show("Nie ma wymagania do odświeżenia!, zmienObiektDataGridView()");
                    return;
                }
                EA.Element ficzKlik = ficzeryDlaSyst[nrficzSys];

                komorkaValue(r, c, ficzKlik.Name);
                komorkaToolTip(r, c, ficzKlik.Notes);
                label1.Text = ficzKlik.Name;
                labelNote.Text = ficzKlik.Notes;

                Color fc = Color.Black, bc = Color.White;
                if (groupBox1.InvokeRequired)
                {
                    groupBox1.Invoke(new Action(() =>
                    {
                        var checkedButton1 = groupBox1.Controls.OfType<RadioButton>()
                          .FirstOrDefault(rrr => rrr.Text == ficzKlik.Status);
                        if (checkedButton1 != null)
                        {
                            fc = checkedButton1.ForeColor;
                            bc = checkedButton1.BackColor;

                        }
                    }));
                }
                else
                {

                    var checkedButton1 = groupBox1.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(rrr => rrr.Text == ficzKlik.Status);
                    if (checkedButton1 != null)
                    {
                        fc = checkedButton1.ForeColor;
                        bc = checkedButton1.BackColor;

                    }
                }

               komorkaBackColor(r, c, bc);
                komorkaForeColor(r, c, fc);

            }

           
        }
     
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log(new CLog(LogMsgType.Info, "Mysz Klik r=" + e.RowIndex + " c=" + e.ColumnIndex+" btn="+e.Button));
            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 0) return;
                if (e.Button == MouseButtons.Left)
                {
                    if (e.ColumnIndex == KOL_Wym)
                    {
                        Log(new CLog(LogMsgType.cd, " Kolumna wymaganie: " + ficzeryListRodzic[e.RowIndex] + "\n"));
                        label1.Text = wymagania[ficzeryListRodzic[e.RowIndex]].Name;
                        labelNote.Text = wymagania[ficzeryListRodzic[e.RowIndex]].Notes;
                  //      pokazPodglad(wymagania[ficzeryListRodzic[e.RowIndex]].Name,
                    //           wymagania[ficzeryListRodzic[e.RowIndex]].Notes);
                        pokazPodglad(wymagania[ficzeryListRodzic[e.RowIndex]],e.RowIndex,e.ColumnIndex);
                        if (rbReq0.Checked) return;
                        //// kolorowanie Requirement
                        var checkedButton = groupBox2.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);


                        for (int i = 0; i < ficzeryListRodzic.Count;i++ )
                        {
                            if (ficzeryListRodzic[e.RowIndex] == ficzeryListRodzic[i])
                            {
                                dataGridView1.Rows[i].Cells[e.ColumnIndex].Style.BackColor = checkedButton.BackColor;
                                dataGridView1.Rows[i].Cells[e.ColumnIndex].Style.ForeColor = checkedButton.ForeColor;
                            }
                        }
                            wymagania[ficzeryListRodzic[e.RowIndex]].Status = checkedButton.Text;
                          
                            wymagania[ficzeryListRodzic[e.RowIndex]].Update();
                           
                        

                    }
                    if (e.ColumnIndex > KOL_Wym)
                    {
                        Log(new CLog(LogMsgType.cd, " Wymaganie: " + ficzeryListRodzic[e.RowIndex] + "\n"));
                      
                      
                        int rq=ficzeryListRodzic[e.RowIndex];
                        List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
                        List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[e.ColumnIndex - KOL_Wym-1];

                        int lpRodzica = ficzeryListRodzic[e.RowIndex];
                        int nrficzSys =e.RowIndex- ficzeryListLpRodzic[e.RowIndex];
                        if (nrficzSys >= ficzeryDlaSyst.Count || ficzeryDlaSyst.Count == 0) return;
                        EA.Element ficzKlik = ficzeryDlaSyst[nrficzSys];
                         label1.Text = ficzKlik.Name;
                        labelNote.Text = ficzKlik.Notes;
                       // pokazPodglad(ficzKlik.Name, ficzKlik.Notes);
                        pokazPodglad(ficzKlik,e.RowIndex,e.ColumnIndex);
                        Log(new CLog(LogMsgType.cd, "System: " + systemy[e.ColumnIndex - KOL_Wym - 1].Name + " FiczerNo: " + nrficzSys + "\n"));


                        //// kolorowanie Requirement
                        var checkedButton = groupBox1.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);

                        if (rbFicz0.Checked) return;
                        ficzKlik.Status = checkedButton.Text;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = checkedButton.BackColor;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = checkedButton.ForeColor;
                       
                        ficzKlik.Update();
                      
                       
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();
                 

                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];


                    // Here you can do whatever you want with the cell
                    this.dataGridView1.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    if (e.ColumnIndex <= KOL_Wym) return;

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);
                    MenuItem mi=new MenuItem("Wymaganie biznesowe => "+dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    m.MenuItems.Add(mi);
                    mi.Click+= (sender1, x) => maleMenu_Click(sender1, x, e.RowIndex,e.ColumnIndex,0);

               //     if (clickedCell.Value != null)
                    {
                        mi = new MenuItem("Dodaj ficzer dla: " + dataGridView1.Columns[e.ColumnIndex].HeaderText);
                        m.MenuItems.Add(mi);
                        mi.Click += (sender1, x) => maleMenu_Click(sender1, x, e.RowIndex, e.ColumnIndex, 1);

                  //      m.MenuItems.Add(new MenuItem("##########################################"));

                   //     mi = new MenuItem("Usuń ten ficzer => " + clickedCell.Value);
                   //     m.MenuItems.Add(mi);
                    //    mi.Click += (sender1, x) => maleMenu_Click(sender1, x, e.RowIndex, e.ColumnIndex, 2);
                    }
                    //m.MenuItems.Add(new MenuItem(clickedCell.Value.ToString()));

                    // Show the context menu
                    m.Show(dataGridView1, relativeMousePosition);
                }
            }
        }

       void maleMenu_Click(object sender, EventArgs x, int r, int c,int index)
        {
            //MakeSomethingWithPopupParameter(parameter);  
            label1.Text = " Req => " + dataGridView1.Rows[r].Cells[0].Value.ToString() +
                        "\n System=> " + dataGridView1.Columns[c].HeaderText.ToString() +
                        "\n ficzer => " + dataGridView1.Rows[r].Cells[c].Value +
                        " akcja => " + index +
                        " rodzic= " + ficzeryListRodzic[r] +
                        " r,c= " + r+", "+c;

            switch (index)
            {
                case 0: //wymaganie
                    break;
                case 1: //dodaj
                    dodajFiczer(c,r);
                    break;
                case 2: //usun
                    break;

            }
        }
        private void dodajFiczer(int c,int r)
        {
            Log(new CLog(LogMsgType.Info, "Dodaj ficzer r=" + r + " c=" + c ));
            if (c <= KOL_Wym) return;
            string xxx = "";
            for (int i = 0; i < ficzeryList.Count; i++)
            {
                xxx += ficzeryList[i].Count+", ";
            }
            String s = "FetureList: " + xxx;
            for (int i = 0; i < ficzeryListRodzic.Count; i++)
            {
                xxx += ficzeryListRodzic[i] + ", ";
            }
            s="\n FeatureListRodzic:" + xxx;

            int rq = ficzeryListRodzic[r];
            List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
            List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[c - KOL_Wym - 1];

            int nrficzSys = r - ficzeryListLpRodzic[r];
            EA.Element ficzKlik =null;
            label1.Text = "";
            int liczbaWierszyReq = ficzeryDlaReq.Max(mm => mm.Count);
            int gdzieLpRodzic = ficzeryListRodzic.FindIndex(ir => ir == rq);
            int indexDodawanegoWiersza = (gdzieLpRodzic + ficzeryDlaSyst.Count);
            if (ficzeryDlaSyst.Count <= nrficzSys)
                {
                    label1.Text = "Last=" + ficzeryDlaSyst.Count + "; \n";
                    label1.Text += "powinno być Lp=" + indexDodawanegoWiersza + "\n";
                    if (ficzeryDlaSyst.Count > 0)
                    {
                       
                        ficzKlik = ficzeryDlaSyst.Last();
                    }
                   
                }
                else
                {
                    if (ficzeryDlaSyst.Count > 0)
                    {
                        ficzKlik = ficzeryDlaSyst[nrficzSys];
                    }
                   
                }
            Match m=Regex.Match(wymagania[rq].Name, @"\d+");
            string ficzerTxt="";
            if (m.Success)
            {
                ficzerTxt = "F-" + m.Value + "-" + systemy[c - KOL_Wym - 1].Name + "#" + (ficzeryDlaSyst.Count + 1) + ": " + wymagania[rq].Name;
            }
            else 
            {
                ficzerTxt = "F-" + "XXX" + "-" + systemy[c - KOL_Wym - 1].Name + "#" + (ficzeryDlaSyst.Count + 1) + ": " + wymagania[rq].Name;
            }  
            
            if (ficzeryDlaSyst.Count >= liczbaWierszyReq && liczbaWierszyReq > 0)
                {

                   
                    label1.Text += "\n dodam wiersz lp=" + indexDodawanegoWiersza + "\n";
                    dataGridView1.Rows.Insert(indexDodawanegoWiersza, 1);// dataGridView1.Rows[indexDodawanegoWiersza-1].Cells[0].Value);
                    
                 
                    //rodzic
                   
                    ficzeryListRodzic.Insert(indexDodawanegoWiersza, rq);

                    ficzeryListLpRodzic.Insert(indexDodawanegoWiersza, ficzeryListLpRodzic[indexDodawanegoWiersza-1]);
                    for (int ii = 0; ii < ficzeryListLpRodzic.Count;ii++ )
                    {
                        if (ficzeryListLpRodzic[ii] >= indexDodawanegoWiersza)
                        {
                            ficzeryListLpRodzic[ii]++;
                        }
                    }

                    ///lp
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_Lp].Value = indexDodawanegoWiersza;

                    for (int ii = indexDodawanegoWiersza; ii < dataGridView1.Rows.Count; ii++)
                    {
                        int gdzieLpRodzic1 = ficzeryListRodzic.FindIndex(ir => ir == ii);
                        dataGridView1.Rows[ii].Cells[KOL_Lp].Value = ii;
                       
                        /////////////
                        if (ficzeryListLpRodzic[ii] == ii)
                        {
                            dataGridView1.Rows[ii].Cells[KOL_R].Value =
                                 ficzeryListRodzic[ii]+ " / " + ficzeryListLpRodzic[ii];
                        }
                        else
                        {
                           
                            dataGridView1.Rows[ii].Cells[KOL_R].Value =
                            "Klon->" + ficzeryListRodzic[ii] + " / " + (ii - gdzieLpRodzic1) + " / " + gdzieLpRodzic1;
                        }
                    }
                   
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_R].Value = //indexDodawanegoWiersza;
                    "Klon->" + rq + " / " + (indexDodawanegoWiersza - gdzieLpRodzic) + " / " + gdzieLpRodzic;
                    

                    //wymag
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_Wym].Value = dataGridView1.Rows[indexDodawanegoWiersza-1].Cells[KOL_Wym].Value;
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_Wym].ToolTipText = dataGridView1.Rows[indexDodawanegoWiersza - 1].Cells[KOL_Wym].ToolTipText;
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_Wym].Style.BackColor = dataGridView1.Rows[indexDodawanegoWiersza - 1].Cells[KOL_Wym].Style.BackColor;
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[KOL_Wym].Style.ForeColor = dataGridView1.Rows[indexDodawanegoWiersza - 1].Cells[KOL_Wym].Style.ForeColor;



                    //ficzer
                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Value = ficzerTxt;
                   


                    EA.Element e = EAUtils.dodajElementBezWeryfikacji(ref modelRepo.WymaganiaPckg, (String)ficzerTxt, "", "Feature");
                    e.Status = "Analiza ChM";
                    EAUtils.dodajRelacje(systemy[c - KOL_Wym - 1], e, "Realization", "", "");
                    EAUtils.dodajRelacje(e, wymagania[rq], "Realization", "", "");
                    e.Update();
                    ficzeryDlaSyst.Add(e);
                    var checkedButton3 = groupBox1.Controls.OfType<RadioButton>()
                                                        .FirstOrDefault(rr => rr.Text == e.Status);
                    if (checkedButton3 != null)
                    {
                        dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Style.BackColor = checkedButton3.BackColor;
                        dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Style.ForeColor = checkedButton3.ForeColor;
                    }
                  Log(new CLog(LogMsgType.Info,"Dodano ficzer: "+e.Name+", do syst: "+systemy[c - KOL_Wym - 1].Name+" w nowym wierszu: "+indexDodawanegoWiersza+
                      "  Rodzic reqNum="+ rq+" wiersz rodzica="+ficzeryListLpRodzic[indexDodawanegoWiersza]));
                }
                else
                {

                    label1.Text += "\n uzupelnie wiersz lp=" + indexDodawanegoWiersza + "\n";

                    dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Value = ficzerTxt;

                  EA.Element e = EAUtils.dodajElementBezWeryfikacji(ref modelRepo.WymaganiaPckg, (String)ficzerTxt, "", "Feature");
                    e.Status = "Analiza ChM";
                    EAUtils.dodajRelacje(systemy[c - KOL_Wym - 1], e, "Realization", "", "");
                    EAUtils.dodajRelacje(e, wymagania[rq], "Realization", "", "");
                    e.Update();
                    ficzeryDlaSyst.Add(e);

                 var checkedButton3 = groupBox1.Controls.OfType<RadioButton>()
                                                        .FirstOrDefault(rr => rr.Text == e.Status);
                    if (checkedButton3 != null)
                    {
                        dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Style.BackColor = checkedButton3.BackColor;
                        dataGridView1.Rows[indexDodawanegoWiersza].Cells[c].Style.ForeColor = checkedButton3.ForeColor;
                    }
                  Log(new CLog(LogMsgType.Info,"Dodano ficzer: "+e.Name+", do syst: "+systemy[c - KOL_Wym - 1].Name+" w wierszu: "+indexDodawanegoWiersza+
                      "  Rodzic reqNum="+ rq+" wiersz rodzica="+gdzieLpRodzic));
                   
                }
           // }
            label1.Text += "Klikniete => r=" + r + "; c=" + c +
                "\nrq=" + rq + "; lp=" + ficzeryListLpRodzic[r] +"nrFicz=" + nrficzSys;
            if (ficzKlik != null)
            {
                label1.Text += "\nFicz=" + ficzKlik.Name;
            }
            else
            {
                label1.Text += "\nFicz=null";
            }
            return;

            if (ficzeryList[ficzeryListRodzic[r]].Count > 0 && dataGridView1.Rows[r].Cells[c].Value!=null)
            {
                //wstaw wiersz poniżej klikniętego

                dataGridView1.Rows.Insert(r+1, dataGridView1.Rows[r].Cells[0].Value);
                // dataGridView1.Rows[r + 1].Cells[0].Value = dataGridView1.Rows[r].Cells[0].Value;
                dataGridView1.Rows[r + 1].Cells[0].ToolTipText = dataGridView1.Rows[r].Cells[0].ToolTipText;

                dataGridView1.Rows[r + 1].Cells[c].Value = dataGridView1.Rows[r].Cells[0].Value;

               

                EA.Element e = EAUtils.dodajElementBezWeryfikacji(ref modelRepo.WymaganiaPckg, (String)dataGridView1.Rows[r].Cells[0].Value, "", "Feature");
                e.Status = "Analiza ChM";
                EAUtils.dodajRelacje(systemy[c-1], e, "Realization", "", "");
                EAUtils.dodajRelacje(e, wymagania[ficzeryListRodzic[r]], "Realization", "", "");
                e.Update();
                dataGridView1.Rows[r+1].Cells[c].Value = dataGridView1.Rows[r+1].Cells[0].Value;
                ficzeryListRodzic.Insert(r + 1, ficzeryListRodzic[r] - ficzeryListRodzic[r]);
      //          ficzeryList[r].Insert(r - ficzeryListRodzic[r], e);
            }
            else
            {   // nie dodawaj wiersza tylko zmien opis w pustym polu
                EA.Element e = EAUtils.dodajElementBezWeryfikacji(ref modelRepo.WymaganiaPckg, (String)dataGridView1.Rows[r].Cells[0].Value, "", "Feature");
                e.Status = "Analiza ChM";
                EAUtils.dodajRelacje( systemy[c-1],e, "Realization", "", "");
                EAUtils.dodajRelacje(e,wymagania[ficzeryListRodzic[r]], "Realization", "", "");
                e.Update();
                dataGridView1.Rows[r].Cells[c].Value = dataGridView1.Rows[r].Cells[0].Value;
        //        ficzeryList[r].Insert(r - ficzeryListRodzic[r], e);
            }
            String x2 = "\nFetureList\n";
            for (int i = 0; i < ficzeryList.Count; i++)
            {
                x2 += ficzeryList[i].Count + ", ";
            }
            x2 += "\nFetureListRodzic:\n " ;
            for (int i = 0; i < ficzeryListRodzic.Count; i++)
            {
               x2 += ficzeryListRodzic[i] + ", ";
            }
  
            MessageBox.Show("Przed\n"+s+"\nPo\n"+ x2);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private SynchronizationContext m_SynchronizationContext;
        /// <summary>
        /// log z synchronizacja watkow
        /// </summary>
        /// <param name="l">obiekt CLog</param>
        public void Log(CLog l)
        {
            m_SynchronizationContext.Post((@object) =>
            {
                Log(l.typ, l.txt);
            }, l);
        }
        public class CLog
        {
            public LogMsgType typ;
            public String txt;
            public CLog(LogMsgType typL, String txtL)
            {
                typ = typL;
                txt = txtL;
            }
        }
        public enum LogMsgType { WynikOK, WynikNOK, Normal, Info, Warning, Error,cd };
        private Color[] LogMsgTypeColor = { Color.Green, Color.Blue, Color.Black, Color.Black, Color.Orange, Color.Red, Color.Black };
        public void Log(LogMsgType msgtype, string msg)
        {
            try
            {
                logRTF.Invoke(new EventHandler(delegate
                {
                    logRTF.SelectedText = string.Empty;
                    if (msgtype == LogMsgType.WynikNOK || msgtype == LogMsgType.WynikOK || msgtype == LogMsgType.Info)
                    {
                        logRTF.SelectionFont = new Font(logRTF.SelectionFont, FontStyle.Regular);

                        if (msgtype == LogMsgType.Info)
                        {
                            msg = "## " + DateTime.Now.ToLongTimeString() + ": " + msg;
                        }
                    }
                    else
                    {
                        if (msgtype == LogMsgType.cd)
                        {
                            msg = " " + msg;
                        }
                        else
                        {
                            logRTF.SelectionFont = new Font(logRTF.SelectionFont, FontStyle.Bold);
                            msg = "# " + DateTime.Now.ToLongTimeString() + ": " + msg;
                        }
                    }
                    logRTF.SelectionColor = LogMsgTypeColor[(int)msgtype];

                    logRTF.AppendText(msg);
                    logRTF.ScrollToCaret();
                }));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        private void edytujFiczer(int c, int r)
        {
            int rq = ficzeryListRodzic[r];
            List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
            List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[c - KOL_Wym - 1];

            int nrficzSys = r - ficzeryListLpRodzic[r];
            if (nrficzSys >= ficzeryDlaSyst.Count)
            {
                MessageBox.Show("Nie ma wymagania do edycji!");
                return;
            }
          

           

            EA.Element ficzKlik = ficzeryDlaSyst[nrficzSys];

            if (ficzKlik.Name.CompareTo(dataGridView1.Rows[r].Cells[c].Value)!=0)
            {
                label1.Text = "**Stara treść wymagania:\n" +
                  ficzKlik.Name + "\n Nowa treść:\n" +
                  dataGridView1.Rows[r].Cells[c].Value;

                DialogResult dialogResult = MessageBox.Show(label1.Text, "Czy jesteś pewien edycji?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ficzKlik.Name = dataGridView1.Rows[r].Cells[c].Value.ToString();
                    ficzKlik.Update();
                }
                else if (dialogResult == DialogResult.No)
                {
                    dataGridView1.Rows[r].Cells[c].Value = ficzKlik.Name;
                }
            }
            else
            {
                label1.Text = "**Nie było zmian treść wymagania:\n" +
                 ficzKlik.Name;
            }
          
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            label1.Text = "Edit value r=" + e.RowIndex + " c=" + e.ColumnIndex+"\n";
            if (e.ColumnIndex > KOL_Wym)
            {
                edytujFiczer(e.ColumnIndex, e.RowIndex);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            int rq = ficzeryListRodzic[r];
            List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
            List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[c - KOL_Wym - 1];

            int nrficzSys = r - ficzeryListLpRodzic[r];
            if (nrficzSys >= ficzeryDlaSyst.Count)
            {
                MessageBox.Show("Aby edytować treść wymagania musisz najpierw je utworzyć!");
                e.Cancel = true;
                return;
            }
    
        }
        ToolTip toolTip1 = new ToolTip();
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
           // toolTip1.AutomaticDelay = 100;
          //  toolTip1.AutoPopDelay = 1000;
         //   toolTip1.ReshowDelay = 100;
            dataGridView1.ShowCellToolTips = false;
            try
            {
                toolTip1.SetToolTip(dataGridView1, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText.ToString());
            }
            catch { }
        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs ee)
        {
            DataGridViewCell e = ee.Cell;

             
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 0) return;
                if (e.ColumnIndex == KOL_Wym)
                    {
                       
                        label1.Text = wymagania[ficzeryListRodzic[e.RowIndex]].Name;
                        labelNote.Text = wymagania[ficzeryListRodzic[e.RowIndex]].Notes;
    
                        pokazPodglad( wymagania[ficzeryListRodzic[e.RowIndex]],e.RowIndex,e.ColumnIndex);
                        if (rbReq0.Checked) return;
                       
                    }
                    if (e.ColumnIndex > KOL_Wym)
                    {
                     
                        int rq=ficzeryListRodzic[e.RowIndex];
                        List<List<EA.Element>> ficzeryDlaReq = ficzeryList[rq];
                        List<EA.Element> ficzeryDlaSyst = ficzeryDlaReq[e.ColumnIndex - KOL_Wym-1];

                        int lpRodzica = ficzeryListRodzic[e.RowIndex];
                        int nrficzSys =e.RowIndex- ficzeryListLpRodzic[e.RowIndex];
                        if (nrficzSys >= ficzeryDlaSyst.Count || ficzeryDlaSyst.Count == 0) return;
                        EA.Element ficzKlik = ficzeryDlaSyst[nrficzSys];
                         label1.Text = ficzKlik.Name;
                        labelNote.Text = ficzKlik.Notes;
                       // pokazPodglad(ficzKlik.Name, ficzKlik.Notes);
                        pokazPodglad(ficzKlik,e.RowIndex,e.ColumnIndex);
                      
                       
                    }
                }
        }
    }

  

   
}
