using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA;
using System.Windows.Forms;
using System.Threading;

namespace EAkzg
{

    /***
     * repo.terms -słownik
     * repo.issue -issue
     * repo.tasks -taski
     * ***/

    static class EAUtils
    {
        static string nazwaModelu = "";
        static String nazwaProjektu="";
      //  static String autor = "";

        static public void zapiszNazweModelu(string nazwa)
        {
            nazwaModelu = nazwa;
        }
        static public string dajNazweModelu()
        {
            return nazwaModelu;
        }
        static public String dajNazwiskoTA(EA.Repository rep,EA.Package p)
        {
            String nazwisko = "brak podpisu";
            try
            {
                EA.Element e2 = dajComponentSystemZpakietu(rep, p);
                nazwisko = e2.TaggedValues.GetByName("Rozwój").Value;

                /*foreach (Connector c in p.Connectors)
                {
                 
                    EA.Element e2 = rep.GetElementByID(c.SupplierID);
                    try
                    {
                        nazwisko = e2.TaggedValues.GetByName("Rozwój").Value;
                        return nazwisko;
                    }
                    catch (Exception) { }
                }*/
            }
            catch (Exception) { }   
            
            return nazwisko;
        }
        static public EA.Element dajComponentSystemZpakietu(EA.Repository rep,EA.Package p)
        {
            EA.Element systEl=null;
            try
            {
                
                String sql="select os.object_id from t_object op,t_object os, t_connector c where op.object_id="+p.Element.ElementID+
                    " and ((c.start_object_id=os.object_id and c.end_object_id=op.object_id) or "+
                          "(c.start_object_id=op.object_id and c.end_object_id=os.object_id)) and os.object_type='Component'";
                systEl = rep.GetElementSet(sql, 2).GetAt(0); //powinien byc tylko jeden component powiazany z tym pakietem
            }
            catch(Exception) { }
            {
            }
            return systEl;
        }
        static public String dajDostawceSystemu(EA.Repository rep, EA.Element p)
        {
            String nazwisko = "nie określono";
            try
            {
                nazwisko = p.TaggedValues.GetByName("Dostawca").Value;
              /* 
               * Zmiana z pola taggedValue na pakiecie na taggedValue na komponencie
               * foreach (Connector c in p.Connectors)
                {

                    EA.Element e2 = rep.GetElementByID(c.SupplierID);
                    try
                    {
                        nazwisko = e2.TaggedValues.GetByName("Dostawca").Value;
                        return nazwisko;
                    }
                    catch (Exception) { }
                }
               * */
            }
            catch (Exception) { }

            return nazwisko;
        }
        static public void zmienTaggedValues(ref Element element, String tagg, String nowaWartosc)
        {

            usunTaggedValues(ref element, tagg);
            dodajTaggedValues(ref element, tagg, nowaWartosc);

        }
        static public void usunElement(ref Package RodzicElement,ref Element element)
        {
            for (short i = 0; i < element.TaggedValues.Count; i++)
            {
                TaggedValue t = element.TaggedValues.GetAt(i);
                element.TaggedValues.DeleteAt(i, true);
                element.Refresh();
                
            }
            for(short i=0;i<RodzicElement.Elements.Count;i++)
            {
                Element e = RodzicElement.Elements.GetAt(i);
                if (e.Name == element.Name)
                    RodzicElement.Elements.DeleteAt(i, true);
            }
            
        }
       
        static public void usunTaggedValues(ref Element element, String tagg)
        {

            for (short i = 0; i < element.TaggedValues.Count; i++)
            {
                TaggedValue t = element.TaggedValues.GetAt(i);

                if (t.Name == tagg)
                {
                    element.TaggedValues.DeleteAt(i, true);
                    element.Refresh();
                }
            }
 
        }
       
        static public String dajTaggedValue(ref Element element, String tagg)
        {
            String wynik = "";
            TaggedValue t = null;
            try
            {
                t = element.TaggedValues.GetByName(tagg);
                wynik = t.Value;
            }
            catch {
                return wynik;
            }

            return wynik;
        }
        static public void dodajTaggedValues(ref Element element, String tagg, String nowaWartosc)
        {
            TaggedValue t = null;
            try
            {
                t = element.TaggedValues.GetByName(tagg);
                t.Value = nowaWartosc;
            }
            catch { }
            if (t == null)
            {
                try
                {
                    t = element.TaggedValues.AddNew(tagg, nowaWartosc);
                }
                catch (Exception e)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString()+ ") " + element.Name + " tag " + tagg + " nowaWartosc=" + nowaWartosc + " #" + e.Message);
                }
            }
            t.Update();
            element.Refresh();
        }
        /**
         * Dodaje element typu object gdy go nie ma w pakiecie
         * */
        static public Element dodajElement(ref Element RodzicElement, String nazwa, String notatka, String typElementu = "Object")
        {
            Element e = null;
            try
            {
                e = RodzicElement.Elements.GetByName(nazwa);
            }
            catch { }
            if (e == null)
            {
                try{
                e = RodzicElement.Elements.AddNew(nazwa, typElementu);
                e.Notes = notatka;
                e.Update();
                e.Refresh();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }

            return e;
        }
        static public Element dodajElement(ref Package RodzicPackage, String nazwa, String notatka,String typElementu="Object")
        {

            Element e = null;
            try
            {
                e = RodzicPackage.Elements.GetByName(nazwa);
            }
            catch { }
            if (e == null)
            {
                try{
                e=RodzicPackage.Elements.AddNew(nazwa, typElementu);
                e.Notes = notatka;
                e.Update();
                e.Refresh();
               }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }
                
            return e;
        }
        static public Element dodajElementBezWeryfikacji(ref Package RodzicPackage, String nazwa, String notatka, String typElementu = "Object")
        {

            Element e = null;
            
                try
                {
                    e = RodzicPackage.Elements.AddNew(nazwa, typElementu);
                    e.Notes = notatka;
                    e.Update();
                    e.Refresh();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            

            return e;
        }
        static public Method dodajOperacje(ref Element RodzicElement, String nazwa, String notatka, String typElementu = "Object")
        {
            Method e = null;
            try
            {
                e = RodzicElement.Methods.GetByName(nazwa);
            }
            catch { }
            if (e == null)
            {
                try{
                e = RodzicElement.Methods.AddNew(nazwa, typElementu);
                e.Notes = notatka;
                RodzicElement.Update();
                RodzicElement.Connectors.Refresh();
                    }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }

            return e;
        }
        static public Connector dodajRelacje(Element A, Element B, String typRelacji, String nazwa, String notatka)
        {
            Connector con = null;
            try
            {
                foreach (Connector c in A.Connectors)
                {
                    if (B.ElementID == c.SupplierID && typRelacji == c.Type)
                    {
                        con = c;
                        break;
                    }
                }
            }
            catch (Exception){}
            if (con == null)
            {
                try{
                con = A.Connectors.AddNew(nazwa, typRelacji);
                con.SupplierID = B.ElementID;
                con.Notes = notatka;
                con.Update();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("EAUtils.dodajRelacje( " + A.Name+", "+B.Name + ") #" + exc.Message);
                }
            }
            return con;
        }
        static public bool sprawdzCzyJest(Element element, bool czyUtworzycJakNieMa)
        {
            return false;
        }
        static public void ustawNazweProjektu(String nazwa)
        {
            nazwaProjektu = nazwa;
        }
        static public String dajNazweProjektu(ref Package Model)
        {
            Element elProjekt = null;
            Package definicjePckg = null;
            try
            {
                definicjePckg = Model.Packages.GetByName("Definicje");
            }
            catch { }
            if (definicjePckg == null)
            {
                try{
                definicjePckg=Model.Packages.AddNew("Definicje","");
                    }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }

            try
            {
                elProjekt = definicjePckg.Elements.GetByName("Projekt-Nazwa");
            }
            catch { }
            if (elProjekt == null)
            {
                elProjekt = EAUtils.dodajElement(ref definicjePckg, "Projekt-Nazwa", "Nazwa projektu nie została uzupełniona");

            }
            return elProjekt.Notes;
        }
        
        static public String dajAutoraProjektu(ref Package Model,String ktory)
        {
            try
            {
                Package hldpkg = Model.Packages.GetByName("HLD");

                Package pckg = hldpkg.Packages.GetByName("Definicje");
                pckg = pckg.Packages.GetByName("Słownik");
                Element element = pckg.Elements.GetByName(ktory);

                TaggedValue t = element.TaggedValues.GetByName("Imię i Nazwisko");
                return t.Value.ToString();
            }
            catch { }
            return "Nie uzupełniono pola SD:" +ktory;
        }
        static public Package utworzPakietGdyBrak(ref Package RodzicPckg,String nazwa,String typ)
        {
            Package pckg = null;
            try
            {
                pckg = RodzicPckg.Packages.GetByName(nazwa);
            }
            catch(Exception e) {
               // MessageBox.Show("EAUtils.utworzPakietGdyBrak " + nazwa + " " + typ + " exc=" + e.Message);
                
            }
            if (pckg == null)
            {
                try{
                pckg = RodzicPckg.Packages.AddNew(nazwa, typ);
              //  pckg.ParentID = RodzicPckg.PackageID;
                pckg.Update();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + RodzicPckg.Name+", "+nazwa+", "+typ + ") #" + exc.Message);
                    
                }
            }
          
            
            return pckg;
        }
        static public Package dajPakietSciezki(ref Package RodzicPckg, String[] sciezka)
        {
            return utworzSciezke(ref RodzicPckg, sciezka);
        }
        static public Package dajPakietSciezkiP(ref Package RodzicPckg, params String[] sciezka)
        {
            return utworzSciezke(ref RodzicPckg, sciezka);
        }

        static public Package utworzSciezke(ref Package RodzicPckg, String[] sciezka)
        {
           
            Package tmpPckg = RodzicPckg;
            for (int i = 0; i < sciezka.Count(); i++)
            {
                tmpPckg=utworzPakietGdyBrak(ref tmpPckg, sciezka[i], "");
            }
            
            return tmpPckg;
        }
        static public void utworzPustyModel(ref Repository rep)
        {
            Package modelPckg = null;
            try
            {
                modelPckg = EAUtils.dajModelPR(ref rep);//rep.Models.GetAt(0);
            }
            catch { }
            if (modelPckg == null)
            {
                try{
                modelPckg = rep.Models.AddNew("PR-NNN", "");
                modelPckg.Update();
               }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }

            
         

            Package definPckg = utworzPakietGdyBrak(ref modelPckg, "Definicje","Simple");
            Element e = dodajElement(ref definPckg, "Projekt-Nazwa", "Pełna nazwa projektu nie została zdefiniowana");
            e = dodajElement(ref definPckg, "Testy - wskazówki", "Wskazówek do przeprowadzania testów jeszcze nie opisano");
            e = dodajElement(ref definPckg, "Architektura Transmisyjna", "Architektury Transmisyjnej jeszcze nie opisano");
            Package slPckg = utworzPakietGdyBrak(ref definPckg, "Słownik","Simple");
            e=dodajElement(ref slPckg, "BA", "Business Analityk");
              //  dodajTaggedValues(ref e, "Imię i Nazwisko", "pole nie uzupełnione!!!!");
                //dodajTaggedValues(ref e, "Opis", "Business Analityk");

            e = dodajElement(ref slPckg, "BO", "Business Owner");
              //  dodajTaggedValues(ref e, "Imię i Nazwisko", "pole nie uzupełnione!!!!");
               // dodajTaggedValues(ref e, "Opis", "Business Owner");

            e = dodajElement(ref slPckg, "PM", "Project Manager");
            //    dodajTaggedValues(ref e, "Imię i Nazwisko", "pole nie uzupełnione!!!!");
             //   dodajTaggedValues(ref e, "Opis", "Project Manager");

            e = dodajElement(ref slPckg, "SD IT", "Solution Designer części IT");
               // dodajTaggedValues(ref e, "Imię i Nazwisko", "pole nie uzupełnione!!!!");
              //  dodajTaggedValues(ref e, "Opis", "Solution Designer części IT");

            e = dodajElement(ref slPckg, "SD NT", "Solution Designer części NT");
              //  dodajTaggedValues(ref e, "Imię i Nazwisko", "pole nie uzupełnione!!!!");
               // dodajTaggedValues(ref e, "Opis", "Solution Designer części NT");

                Package zaleznosciPckg = utworzPakietGdyBrak(ref definPckg, "Zależności", "Simple");
                Package zalacznikiPckg = utworzPakietGdyBrak(ref definPckg, "Załączniki", "Simple");
                Package HistoriaPckg = utworzPakietGdyBrak(ref definPckg, "Historia zmian", "Simple");
                Package ograniczeniaPckg =  utworzPakietGdyBrak(ref definPckg, "Ograniczenia rozwiązania", "Simple");


            Package wymaganiaPckg = utworzPakietGdyBrak(ref modelPckg, "Wymagania","Simple");
            Package aktorzyPckg = utworzPakietGdyBrak(ref modelPckg, "Aktorzy","Use Case");

            Package itPckg = utworzPakietGdyBrak(ref modelPckg, "IT","Deployment");
            Package koncPckg = utworzPakietGdyBrak(ref itPckg, "Koncepcja","Simple");
            e = dodajElement(ref koncPckg, "Koncepcja", "Koncepcji ogólnej rozwiązania jeszcze nie opisano");
            e = dodajElement(ref koncPckg, "Skrot", "Skróconej koncepcji rozwiązania jeszcze nie opisano");
            e = dodajElement(ref koncPckg, "Migracja", "Opisu wpływu na migrację jeszcze nie opisano");
          
            Package ucPckg = utworzPakietGdyBrak(ref itPckg, "Przypadki Użycia","Use Case");
            Package sekPckg = utworzPakietGdyBrak(ref itPckg, "Diagramy Sekwencji","Dynamic");
            Package sysPckg = utworzPakietGdyBrak(ref itPckg, "Architektura Statyczna","Component");
            Package wkladyPckg = utworzPakietGdyBrak(ref itPckg, "Wkłady Systemowe", "Component");

             itPckg = utworzPakietGdyBrak(ref modelPckg, "NT", "Deployment");
             koncPckg = utworzPakietGdyBrak(ref itPckg, "Koncepcja", "Simple");
            e = dodajElement(ref koncPckg, "Koncepcja", "Koncepcji ogólnej rozwiązania jeszcze nie opisano");
            e = dodajElement(ref koncPckg, "Skrot", "Skróconej koncepcji rozwiązania jeszcze nie opisano");
            e = dodajElement(ref koncPckg, "Migracja", "Opisu wpływu na migrację jeszcze nie opisano");
          
             ucPckg = utworzPakietGdyBrak(ref itPckg, "Przypadki Użycia", "Use Case");
             sekPckg = utworzPakietGdyBrak(ref itPckg, "Diagramy Sekwencji", "Dynamic");
             sysPckg = utworzPakietGdyBrak(ref itPckg, "Architektura Statyczna", "Component");
             wkladyPckg = utworzPakietGdyBrak(ref itPckg, "Wkłady Systemowe", "Component");

               
           
        }
        static public Diagram utworzDajDiagramGdyGoNieMa(ref Package pakiet, String nazwa, String typ)
        {
            Diagram diagram = null;
            try
            {
               diagram = pakiet.Diagrams.GetByName(nazwa);
            }
            catch { }
            if (diagram == null)
            {
                try{
                diagram = pakiet.Diagrams.AddNew(nazwa, typ);
                diagram.Update();
                    }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }

            }
            return diagram;
        }
        static public DiagramObject dodajElementDoDiagramuGdyGoNieMa(ref Diagram diagram, ref Element element, String parametry)
        {
            DiagramObject jaDiagObj = null;
            DiagramObject tmpObj = null;
            try
            {
                for (short i = 0; i < diagram.DiagramObjects.Count; i++)
                {
                    tmpObj = diagram.DiagramObjects.GetAt(i);
                    if (tmpObj.ElementID == element.ElementID)
                    {
                        jaDiagObj = tmpObj;
                        break;
                    }
                  
                }
            }
            catch { }
            if (jaDiagObj == null)
            {
                try{
                jaDiagObj = diagram.DiagramObjects.AddNew(parametry, "");
                jaDiagObj.ElementID = element.ElementID;
                jaDiagObj.Update();
                diagram.Update();
                    }
                catch (Exception exc)
                {
                    String err = jaDiagObj.GetLastError();
                    if (err != "") // bo tej zjebany EA rzuca wyjątkiem gdy wszystko jest ok !!!! ??? wtf?
                    {
                        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().GetParameters().ToString() + ") #" + exc.Message + " => " + err);
                    }
                 }
            }
            return jaDiagObj;
        }
        static public void utworzLinkPakietElementGdyBrak(ref Package pakiet, ref Element element,String typ)
        {
            try
            {
                bool jest = false;
                foreach (Connector con in pakiet.Connectors)
                {
                    if (con.Type == typ)
                    {
                        if (con.SupplierID == element.ElementID ) jest = true;
                          
                    }
                }
                if (!jest)
                {
                    try{
                    Connector k=pakiet.Connectors.AddNew("", typ);
                    k.SupplierID = element.ElementID;
                    k.Update();
 }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
                }
            }
            catch { }
        }

        static public void dodajWymaganieDostepnoscDoSystemyGdyGoNieMa(CModel modelProjektu, ref Package wymagPckg, int obszar)
        {
            /// szukaj wymagania
            bool czyJest = false;
            String nazwa = "Wpływ na dostępność systemu.";
            foreach (Element e in wymagPckg.Elements)
            {
                //  if (e.Stereotype == "Bezp.")
                if (CmodelKonfigurator.czyZawiera(e.Stereotype, CmodelKonfigurator.stereotypyFeatureSystemowychDostepnosc))
                {
                    if (e.Name == nazwa)
                    {
                        czyJest = true;
                        break;
                    }
                }
            }
            /// jesli trza zrobic to je zrob
            if (!czyJest)
            {

                //uzupełnij stereotyp
                String stereotyp = CmodelKonfigurator.stereotypyFeatureSystemowychDostepnosc[0];
                //uzupełnij status
                String stat = "Nowy";
                //uzupełnij notatkę
                String notatka = "Brak zmian";
                //dodaj wymaganie
                Element wymaganie = dodajElement(ref wymagPckg, nazwa, notatka, "Feature");
                wymaganie.Stereotype = stereotyp;
                wymaganie.Status = stat;
                // Package model=dajModelPR(ref repo);
                wymaganie.Author = modelProjektu.dajAutoraProjektu(obszar);//dajAutoraProjektu(ref model , pakiet);
                wymaganie.Update();
                wymaganie.Refresh();
            }
        }
        static public void dodajWymaganiePojemnoscDoSystemyGdyGoNieMa(CModel modelProjektu, ref Package wymagPckg, int obszar)
        {
            /// szukaj wymagania
            bool czyJest = false;
            String nazwa = "Wpływ na pojemność systemu.";
            foreach (Element e in wymagPckg.Elements)
            {
                //  if (e.Stereotype == "Bezp.")
                if (CmodelKonfigurator.czyZawiera(e.Stereotype, CmodelKonfigurator.stereotypyFeatureSystemowychPojemnosc))
                {
                    if (e.Name == nazwa)
                    {
                        czyJest = true;
                        break;
                    }
                }
            }
            /// jesli trza zrobic to je zrob
            if (!czyJest)
            {

                //uzupełnij stereotyp
                String stereotyp = CmodelKonfigurator.stereotypyFeatureSystemowychPojemnosc[0];
                //uzupełnij status
                String stat = "Nowy";
                //uzupełnij notatkę
                String notatka = "Brak zmian";
                //dodaj wymaganie
                Element wymaganie = dodajElement(ref wymagPckg, nazwa, notatka, "Feature");
                wymaganie.Stereotype = stereotyp;
                wymaganie.Status = stat;
                // Package model=dajModelPR(ref repo);
                wymaganie.Author = modelProjektu.dajAutoraProjektu(obszar);//dajAutoraProjektu(ref model , pakiet);
                wymaganie.Update();
                wymaganie.Refresh();
            }
        }

        //static public void dodajWymaganieBezpieczenstwaDoSystemyGdyGoNieMa(ref Repository repo,ref Package wymagPckg,String pakiet)
         static public void dodajWymaganieBezpieczenstwaDoSystemyGdyGoNieMa(CModel modelProjektu, ref Package wymagPckg,int obszar)    
          {
            /// szukaj wymagania
            bool czyJest=false;
            String nazwa="Wymaganie bezpieczeństwa - wpływ projektu na deklarację zgodności systemu";
            foreach (Element e in wymagPckg.Elements)
            {
              //  if (e.Stereotype == "Bezp.")
                if(CmodelKonfigurator.czyZawiera(e.Stereotype,CmodelKonfigurator.stereotypyFeatureSystemowychBezpieczeństwa))
                {
                    if (e.Name ==nazwa )
                    {
                        czyJest = true;
                        break;
                    }
                }
            }
            /// jesli trza zrobic to je zrob
            if (!czyJest)
            {
               
                //uzupełnij stereotyp
                String stereotyp = CmodelKonfigurator.stereotypyFeatureSystemowychBezpieczeństwa[0];
                //uzupełnij status
                String stat = "Nowy";
                //uzupełnij notatkę
                String notatka = "Instrukcja uzupełniania. \n W zależności od zmian projektowych w systemie, zmień treść tego pola na zgodną z analizą systemową. \n";
                notatka += "Jeśli zmiany projektowe: \n a) nie wpływają na ostatnią wersję zaakceptowanej deklaracji zgodności to: podaj nazwę deklaracji zgodności, do której się odwołujesz i wskaż link odwołujący się do tej deklaracji\n";
                notatka += "b) zmieniają deklarację zgodności (zmianie ulega poziom bezpieczeństwa) to: załącz deklarację zgodności uwzględniającą zmiany w systemie wynikające z tego projektu (SoC_NazwaSystemu_vXX_PRXXX)\n";
                notatka += "c) w przypadku braku deklaracji zgodności dla systemu, skontaktuj się z osobą z Departamentu Bezpieczeństwa i ustal termin dostarczenia deklaracji zgodności, ustalenia zapisz w tym polu.";
                 //dodaj wymaganie
                Element wymaganie = dodajElement(ref wymagPckg, nazwa, notatka, "Feature");
                wymaganie.Stereotype = stereotyp;
                wymaganie.Status = stat;
               // Package model=dajModelPR(ref repo);
                wymaganie.Author = modelProjektu.dajAutoraProjektu(obszar);//dajAutoraProjektu(ref model , pakiet);
                wymaganie.Update();
                wymaganie.Refresh();
            }
        }
        
       // static public void generujWklady(ref Repository repo, String pakiet,int obszar)
         static public void generujWklady(CModel modelProjektu, int obszar)
        {
              
            try
            {
                int lay = 1073741824;// ConstLayoutStyles.lsLayoutDirectionRight
               //CModel modelProjektu = new CModel(ref repo);
          

                // CgenerujPakietyWkladow gen = new CgenerujPakietyWkladow(ref repo, ref systemyPckg, ref wkladyPckg);
                List<Element> systemyLista;
                List<Element> interfejsyLista;

                CgenerujPakietyWkladow gen = new CgenerujPakietyWkladow(modelProjektu,obszar);

               
                bool autoNumeracja = false;

                if (gen.ShowDialog() == DialogResult.OK)
                {
                    systemyLista = gen.dajSystemy();
                    interfejsyLista = gen.dajInterfejsy();
                    autoNumeracja = gen.czyAutonumeracjaFeature();

                }
                else
                {
                    gen.Dispose();
                    return;
                }
                gen.Dispose();
           
                foreach(Element elem in systemyLista)
                {
                    Element element = elem;
                    if (element.Type == "Component")
                        {
                            bool ft = false;
                            if (element.Name == "Fasttrack") ft = true;
                            Package wklPckg = utworzPakietGdyBrak(ref /*wkladyPckg kzg nowy model*/ modelProjektu.WkladyPckg[obszar], element.Name, "");
                            utworzLinkPakietElementGdyBrak(ref wklPckg, ref element, CmodelKonfigurator.TypLinkuPakietSystemowyComponent);
                            // tag value Imię i Nazwisko TA
                            Element koncepcjaElem = dajElementLubGoZrob(ref wklPckg, "Koncepcja Systemowa");
                            // odniesienie do wymagań
                            Package wymagPckg = utworzPakietGdyBrak(ref wklPckg, "Wymagania Systemowe", "");

                        // kzg nowy model    dodajWymaganieBezpieczenstwaDoSystemyGdyGoNieMa(ref repo,ref wymagPckg,pakiet);
                            if (!ft)
                            {
                                dodajWymaganieBezpieczenstwaDoSystemyGdyGoNieMa(modelProjektu, ref wymagPckg, obszar);
                                dodajWymaganiePojemnoscDoSystemyGdyGoNieMa(modelProjektu, ref wymagPckg, obszar);
                                dodajWymaganieDostepnoscDoSystemyGdyGoNieMa(modelProjektu, ref wymagPckg, obszar);
                                //
                                Package interfejsyPckg = utworzPakietGdyBrak(ref wklPckg, "Realizowane Interfejsy", "");

                                //diagram systemo centryczny
                                Diagram systemoCentrycznyDiagram = utworzDajDiagramGdyGoNieMa(ref wklPckg, "Diagram systemocentryczny-" + element.Name, "Component");

                                DiagramObject sysDiagObj = dodajElementDoDiagramuGdyGoNieMa(ref systemoCentrycznyDiagram, ref element, "");

                                Element interfejsElement = null;
                                int i = 0;
                                foreach (Connector c in element.Connectors)
                                {
                                    if (c.Type == "Realisation")
                                    {

                                        interfejsElement = interfejsyLista.Find(x => x.ElementID == c.SupplierID);
                                        if (interfejsElement == null)
                                        {
                                            /// gdy nie ma interfejsu na liście do generowania to tylko wstaw na arch systemocentryczną
                                            interfejsElement = modelProjektu.Repozytorium.GetElementByID(c.SupplierID); // kzg nowy model repo.GetElementByID(c.SupplierID);
                                            if (interfejsElement.Stereotype == "Interface")
                                            {
                                                dodajElementDoDiagramuGdyGoNieMa(ref systemoCentrycznyDiagram, ref interfejsElement, ""); //"l=200;r=400;t=400;b=600;");
                                                i++;
                                            }
                                            continue;
                                        }

                                      
                                        try
                                        {       // kopia interfejsów do podfolderu systemowego
                                            if (interfejsElement.Type != "Interface") continue;

                                            interfejsElement.PackageID = interfejsyPckg.PackageID;
                                            interfejsElement.Update();
                                            zrobDiagramInterfejsocentryczny(/*ref repo*/modelProjektu, ref interfejsyPckg, ref interfejsElement);
                                           
                                        }
                                        catch { }
                                    }
                                    if (c.Type == "Usage")
                                    {
                                       
                                        interfejsElement = modelProjektu.Repozytorium.GetElementByID(c.SupplierID);
                                        dodajElementDoDiagramuGdyGoNieMa(ref systemoCentrycznyDiagram, ref interfejsElement, ""); //"l=200;r=400;t=400;b=600;");
                                        i++;
                                    }

                                }

                                foreach (Element interfejs in interfejsyPckg.Elements)
                                {
                                    //dodaj interfejsy do diagramu
                                    Element elementDoDodania = interfejs;
                                    dodajElementDoDiagramuGdyGoNieMa(ref systemoCentrycznyDiagram, ref elementDoDodania, ""); //"l=200;r=400;t=400;b=600;");
                                    i++;
                                }



                               
                                modelProjektu.projektInterfejs.LayoutDiagramEx(systemoCentrycznyDiagram.DiagramGUID, lay, 4, 20, 20, false);
                                systemoCentrycznyDiagram.Update();
                                modelProjektu.Repozytorium.CloseDiagram(systemoCentrycznyDiagram.DiagramID);
                            }
                        }

                        
                                    }
                //kopiowanie fetaturów do systemów
                rekuKopiaFiczer(/*ref repo,ref rodzicPckg, ref wymaganiaPckg*/ modelProjektu,ref modelProjektu.ObszarPckg[obszar],ref modelProjektu.WymaganiaPckg);

             
              

            }
            catch { 
            
            }
        }
        static private void zrobDiagramInterfejsocentryczny(/*kzg nowy model ref Repository repo*/ CModel modelProjektu,ref Package interfejsPckg, ref Element interfejsElem)
        {
               Diagram interfejsoCentrycznyDiagram = utworzDajDiagramGdyGoNieMa(ref interfejsPckg, "Diagram interfejsoCentryczny-" + interfejsElem.Name, "Component");

               dodajElementDoDiagramuGdyGoNieMa(ref interfejsoCentrycznyDiagram, ref interfejsElem, "");
               foreach (Connector c in interfejsElem.Connectors)
               {
                  Element e=modelProjektu.Repozytorium.GetElementByID(c.ClientID);
                  if (e.Type == "Component" || e.Type == "Interface")
                  {
                      dodajElementDoDiagramuGdyGoNieMa(ref interfejsoCentrycznyDiagram, ref e, "");
                  }
               }
               interfejsoCentrycznyDiagram.Update();
               int lay = 1073741824;
     
            modelProjektu.projektInterfejs.LayoutDiagramEx(interfejsoCentrycznyDiagram.DiagramGUID, lay, 4, 20, 20, false);
            modelProjektu.Repozytorium.CloseDiagram(interfejsoCentrycznyDiagram.DiagramID);
        }
      //  static private void rekuKopiaFiczer(ref Repository repo,ref Package rodzicPckg,ref Package p)
        /// <summary>
        /// Kopiuje rekurencyjnie feature
        /// </summary>
        /// <param name="modelProjektu">Model</param>
        /// <param name="rodzicPckg">Rodzic pakietu P</param>
        /// <param name="p">Pakiet w ktorym rekurencyjnie szuka feature</param>
        static private void rekuKopiaFiczer(CModel modelProjektu, ref Package rodzicPckg, ref Package p)
        {
            Package pak=p;
            foreach (Package pp in p.Packages)
            {
                 pak= pp;
                 rekuKopiaFiczer(modelProjektu, ref rodzicPckg, ref pak);
            }
           
            foreach (Element elem in p.Elements)
            {
               

                if (elem.Type == "Feature")
                {
                    // policz ile konektorow ma ficzer do komponentów
                    int ile_ficzerow = 0;

                    String sql = "select s.object_id from t_object f,t_object s, t_connector c where f.object_type='Feature' and " +
                  "f.object_id=" + elem.ElementID + " and s.object_type='Component' and " +
                  "((c.start_object_id=f.object_id and c.end_object_id=s.object_id) or " +
                  "(c.start_object_id=s.object_id and c.end_object_id=f.object_id))";

                    List<Element> listaComp = new List<Element>();
                    try
                    {
                       
                            foreach(Element sysC in modelProjektu.Repozytorium.GetElementSet(sql, 2))
                            {
                                listaComp.Add(sysC);
                            }
                        ile_ficzerow = listaComp.Count;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("rekuKopiaFiczer- sqlErr=" + elem.Name+"->"+e.Message);
                    }

              /*sql      foreach (Connector cc in elem.Connectors)
                    {
                        try
                        {
                            Element e = modelProjektu.Repozytorium.GetElementByID(cc.ClientID);
                          
                            if (e.Type == "Component")
                            {
                                ile_ficzerow++;
                            }
                        }
                        catch
                        {
                            //jesli nie ma takiego elementu to znaczy ze jest niezly syf
                            MessageBox.Show("rekuKopiaFiczer- syf=" + elem.Name);
                        }
                    }*/
                    // jesli zero, a do tego wymaganie nie jest anulowane to rzuc błędem, bo nikt go nie robi!
                    if (ile_ficzerow != 1)
                    {
                       //kzg nowy model if (elem.Status != "Anulowane przez BO" && elem.Status != "Anulowane przez IT")
                        if(!CmodelKonfigurator.czyZawiera(elem.Status,CmodelKonfigurator.statusyFeatureGotowe)) 
                        {
                            try
                            {
                                string text = "rekuKopiaFiczer ficzer powinien mieć powiązanie tylko z jednym Componentem ->" + elem.Name + " posiada więcej relacji, zostanie pominięty";
                                Thread t = new Thread(() => MessageBox.Show(text));
                                t.Start();
                                continue;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("rekuKopiaFiczer wyjątek: " + e.Message + " | ficzer=" + elem.Name);
                            }
                        }
                    }

                    try
                    {
                        Element e = listaComp[0];
                        Package pakietSystPckg = dajPakietSciezkiP(ref rodzicPckg, "Wkłady Systemowe", e.Name, "Wymagania Systemowe");
                        // kopia wymagań do podfolderu systemowego
                        elem.PackageID = pakietSystPckg.PackageID;
                        elem.Update();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("rekuKopiaFiczer wyjątek kopiowania: " + e.Message + " | ficzer=" + elem.Name +"\n EA->"+modelProjektu.Repozytorium.GetLastError());
                    }

                    /* sql
                    foreach (Connector con in elem.Connectors)
                    {
                       
                        try
                        {
                            Element e = modelProjektu.Repozytorium.GetElementByID(con.ClientID);//repo.GetElementByID(con.ClientID);

                            if (e.Type != "Component") continue;
                            Package pakietSystPckg = dajPakietSciezkiP(ref rodzicPckg, "Wkłady Systemowe", e.Name, "Wymagania Systemowe");
                            // kopia wymagań do podfolderu systemowego
                            elem.PackageID = pakietSystPckg.PackageID;
                            elem.Update();
                            break;
                            
                        }
                        catch
                        {
                            //jesli nie ma takiego elementu to znaczy ze brakuje go lub jest w drugiej czesci IT/NT
                        }
                    }*/
                }

            }
        }
        static public String zapiszDiagramJakoObraz(CModel modelProjektu, ref Diagram diag, String sciezka)
        {
            String plik = "";// "img/" + diag.Name + ".png";
          /*  plik = diag.Name.Replace("/", "_");
            plik = plik.Replace(":", "_");
            plik = plik.Replace("\\", "-");*/
            plik = CmodelKonfigurator.prefixPlik+CmodelKonfigurator.nrPliku++;
            plik = "img\\" + plik + ".png";
            modelProjektu.projektInterfejs.PutDiagramImageToFile(diag.DiagramGUID, sciezka + plik, 1);
            return plik;

        }
        static public String zapiszDiagramJakoObrazStare(ref Repository Repo,ref Diagram diag,String sciezka)
        {
            String plik = "";// "img/" + diag.Name + ".png";
            plik = diag.Name.Replace("/", "_");
            plik = plik.Replace(":", "_");
            plik = plik.Replace("\\", "-");
            plik = "img\\" + plik + ".png";
            EA.Project projektInterfejs = Repo.GetProjectInterface();
                projektInterfejs.PutDiagramImageToFile(diag.DiagramGUID, sciezka + plik, 1);
                return plik;
            
        }
        /*
        ///Daj konkretny model
        */
        /// <summary>
        /// zwraca pakiet root modelu o nazwie podanej w parametrze
        /// </summary>
        /// <param name="repo">repozytorium</param>
        /// <param name="nazwa">nazwa szukanego modelu</param>
        /// <returns>znaleziony model lub null gdy taki nie istnieje</returns>
        static public Package dajModelPRoNazwie(ref Repository repo, String nazwa)
        {
            Package wynik = null;
            foreach (Package p in repo.Models)
            {
              if (p.Name == nazwa)
                    {
                        wynik = p;
                        break;
                    }
              
            }
            return wynik;
        }
        /*****
         * szuka modelu o nazwie zaczynającej się na PR
         * 
         * */
        static public Package dajModelPR(ref Repository repo)
        {
            Package wynik = null;
        
            
            foreach (Package p in repo.Models)
            {
                if (nazwaModelu != "")
                {
                    if (p.Name == nazwaModelu)
                    {
                        wynik = p;
                        break;
                    }
                }
                else
                {
                     //KZG poczatek zmian 09-08-2015
                    //if (p.Name.Substring(0, 2) == "PR")
                    if (CmodelKonfigurator.czyZawiera(p.Name.Substring(0, 2), CmodelKonfigurator.symboleNazwProjektow))
                    //KZG koniec zmian 09-08-2015
                    {
                        wynik = p;
                        break;
                    }
                }
            }
            return wynik;
        }
        static public Element dajElementLubGoZrob(ref Package pckg,String nazwa)
        {
            Element e = null;
            try
            {
                e = pckg.Elements.GetByName(nazwa);
            }
            catch { }
            if (e == null)
            {
                try{
                e = pckg.Elements.AddNew(nazwa, "Object");
                e.Update();
                e.Refresh();
                    }
                catch (Exception exc)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "( " + System.Reflection.MethodBase.GetCurrentMethod().Attributes.ToString() + ") #" + exc.Message);
                }
            }
            return e;
        }
        static private void odswiezStatusyPakietuWymaganRek(Package pakiet, ref Repository repo)
        {
            foreach (Package dd in pakiet.Packages)
                {
                    odswiezStatusyPakietuWymaganRek(dd,ref repo);
                }
            foreach (Element req in pakiet.Elements)
            {
                if (req.Type != "Requirement") continue;

                if (req.Status == "Anulowane przez BO") continue;

                if (req.Status == "Nowy") continue;

                string stat = "Uzgodnione";
                bool blokerUzg = false;
                foreach (Connector c in req.Connectors)
                {

                    Element elP = repo.GetElementByID(c.ClientID);
                    Element elK = repo.GetElementByID(c.SupplierID);

                    Element ficz = null;
                    if (elP.ElementID == req.ElementID)
                    {
                        ficz= elK;
                    }  else { ficz = elP; }
                    if (ficz.Type != "Feature" && ficz.Type != "Requirement") continue;

                    

                    switch (ficz.Status)
                    {
                        case "Nowy":
                            blokerUzg = true;
                            stat = "OK";
                            break;
                        case "OK":
                            blokerUzg = true;
                            stat = "OK";
                            break;
                        case "Uzgodnione":
                            //if (stat == "") stat = "Uzgodnione";
                            break;
                        case "Z uwagami":
                            if (stat != "Nowy" && stat != "OK" && stat != "Anulowane przez IT")
                            {
                                stat = "Z uwagami";
                            }
                            break;
                        case "Uwzględnione uwagi":
                            if (stat != "Nowy" && stat != "OK" && stat != "Anulowane przez IT")
                            {
                                stat = "Z uwagami";
                            }
                            break;
                           
                        case "Anulowane przez BO":
                            stat = "Anulowane przez BO";
                            blokerUzg = true;
                            break;
                        case "Anulowane przez IT":
                            stat= "Anulowane przez IT";
                            blokerUzg = true;
                            break;
                    }
                   
                }
                if (stat == "Uzgodnione")
                {
                    if (!blokerUzg && stat != "")
                    {
                        req.Status = stat;
                        req.Update();
                    }
                }
                else 
                {
                    if (stat != "")
                    {
                        req.Status = stat;
                        req.Update();
                    }
                }

                
            }

        }
        static public void odswiezStatusyRequirement(ref EA.Repository repo)
        {
 
            Package model=dajModelPR(ref repo);
         //  zmienStatusyMoje(model); ///////////////////////////////////////////<<<<<<<<<<------------------ do wywalenia to jednorazowy szczał
            Package wymPckg = dajPakietSciezkiP(ref model, "Wymagania");
            odswiezStatusyPakietuWymaganRek(wymPckg, ref repo);
        }

        static public void zmienStatusyMoje( EA.Package pakiet)
        {
             foreach (Package dd in pakiet.Packages)
                {
                    zmienStatusyMoje(dd );
                }
             foreach (Element e in pakiet.Elements)
             {
                 string stat=e.Status;
                 if(e.Type=="Requirement")
                 {
                     if (e.Status == "Proposed") stat = "Nowy";
                     if (e.Status == "Approved") stat = "OK";
                     if (e.Status == "do wyjaśnienia") stat = "Z uwagami";
                     if (e.Status == "Brak zmian") stat = "Uzgodnione";
                     if (e.Status == "Nierealizowane") stat = "Anulowane przez BO";
                     if (e.Status == "Implemented") stat = "Uzgodnione";
                     e.Stereotype = "Biznesowe";
                     
                   //  if (e.Status == "Uzgodnione") stat = "OK";
                 }
                 if (e.Type == "Feature")
                 {
                     if (e.Status == "Proposed") stat = "Nowy";
                     if (e.Status == "Approved") stat = "Uzgodnione";
                     if (e.Status == "do wyjaśnienia") stat = "Z uwagami";
                     if (e.Status == "Brak zmian") stat = "Uzgodnione";
                     if (e.Status == "Nierealizowane") stat = "Anulowane przez IT";
                     if (e.Status == "Implemented") stat = "Uzgodnione";
                     if (e.Status == "Validated") stat = "Uzgodnione";
                     

                 }
                 e.Status = stat;
                 e.Update();

             }
 
        }
    }
}
