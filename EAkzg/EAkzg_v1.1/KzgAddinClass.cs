using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;

namespace EAkzg
{
    public class KzgAddinClassv2
    {
        // define menu constants
        const string menuSeparator = "#################################";
        const string menuZbiorcze = "zbiorcze";
        const string menuHeader = "-&KZGAddin v2";
        const string menuModelujASIS = "Zmiel AS IS";
        const string Oprogramie = "O &wtyczce...";
        const string menuHello = "&Say Hello";
        const string menuGoodbye = "&Say Goodbye";
        const string menuWstepne = "-&Prace nad modelem EA";
        const string menuWstepneEAP = "&Przygotuj czysty Model EA";
        const string menuWstepneAsIs = "&Importuj AsIs";
        const string menuWstepneDetale = "Podstawowe &detale projektu";
        const string menuWstepneOdswiezStatusyWymagan = "Odśwież &statusy wymagań biznesowych";
        const string menuGeneruj = "&Generuj HLD";
        const string menuEdytuj = "-&Edytuj HLD";
        const string menuNT = "-&NT Rozdział";
        const string menuIT = "-&IT Rozdział";
        const string menuEdytujKoncepcjaOgolnaNT = "Edytuj Koncepcję &Ogólną HLD NT";
        const string menuEdytujKoncepcjaSkrotNT = "Edytuj Koncepcję S&króconą HLD NT";
        const string menuEdytujWkladyIT = "Generuj pakiety wkładów systemowych IT";
        const string menuEdytujWkladyNT = "Generuj pakiety wkładów systemowych NT";
        const string menuEdytujKoncepcjaOgolnaIT = "Edytuj Koncepcję &Ogólną HLD IT";
        const string menuEdytujKoncepcjaSkrotIT = "Edytuj Koncepcję S&króconą HLD IT";
        const string menuEdytujSlownik = "Edytuj &Słownik pojęć/skrótów HLD";
        const string menuEdytujZaleznosci = "Edytuj &zależności projektowe";
        const string menuEdytujZalaczniki = "Edytuj &listę załączników";
        const string menuEdytujWymagania = "Edytuj &wymagania";

        // remember if we have to say hello or goodbye
        private bool shouldWeSayHello = true;

        ///
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        ///
        /// <param name="Repository" />the EA repository
        /// a string
        public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.
            return "a string";
        }

        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {

            switch (MenuName)
            {
                // defines the top level menu option
                case "":
                    return menuHeader;
                // defines the submenu options
                case menuHeader:
                    string[] subMenus = { /*menuHello, menuGoodbye,*/menuWstepne,/* menuEdytuj,*/menuGeneruj,Oprogramie/*,menuModelujASIS*/ };
                    return subMenus;
                case menuEdytuj:
                    string[] subMenus1 = { menuEdytujSlownik, menuEdytujZalaczniki, menuEdytujZaleznosci,menuEdytujWymagania, menuIT,menuNT};
                    return subMenus1;
                case menuWstepne:
                    string[] subMenus2 = {/* menuWstepneEAP, menuSeparator,*/ menuEdytujWkladyIT, menuEdytujWkladyNT,/* menuSeparator, menuWstepneOdswiezStatusyWymagan,*/ menuSeparator, menuWstepneDetale };
                    return subMenus2;
                case menuNT:
                    string[] subMenus3 = { menuEdytujKoncepcjaSkrotNT, menuEdytujKoncepcjaOgolnaNT,menuEdytujWkladyNT };
                    return subMenus3;
                case menuIT:
                    string[] subMenus4 = { menuEdytujKoncepcjaSkrotIT, menuEdytujKoncepcjaOgolnaIT, menuEdytujWkladyIT };
                    return subMenus4;
                case menuZbiorcze:
                    string[] subMenus5 = { };
                    return subMenus5;
            }

            return "";
        }

        ///
        /// returns true if a project is currently opened
        ///
        /// <param name="Repository" />the repository
        /// true if a project is opened in EA
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;
                EAUtils.zapiszNazweModelu(EAUtils.dajModelPR(ref Repository).Name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            if (IsProjectOpen(Repository))
            {
                switch (ItemName)
                {
                    // define the state of the hello menu option
                    case menuHello:
                        IsEnabled = shouldWeSayHello;
                        break;
                    case menuSeparator:
                        IsEnabled = false;
                        break;
                    // define the state of the goodbye menu option
                    case menuGoodbye:
                        IsEnabled = !shouldWeSayHello;
                        break;
                    case menuModelujASIS:
                        IsEnabled = false;
                        break;
                    case menuGeneruj:
                        IsEnabled = true;
                        break;
                    case menuEdytuj:
                        IsEnabled = true;
                        break;
                    case menuEdytujKoncepcjaSkrotNT:
                        IsEnabled = false;
                        break;
                    case menuEdytujKoncepcjaOgolnaNT:
                        IsEnabled = false;
                        break;
                    case menuEdytujKoncepcjaSkrotIT:
                        IsEnabled = false;
                        break;
                    case menuEdytujKoncepcjaOgolnaIT:
                        IsEnabled = false;
                        break;
                    case menuEdytujSlownik:
                        IsEnabled = false;
                        break;
                    case menuEdytujZalaczniki:
                        IsEnabled = false;
                        break;
                    case menuEdytujZaleznosci:
                        IsEnabled = false;
                        break;
                    case Oprogramie:
                        IsEnabled = true;
                        break;
                    case menuWstepneEAP:
                        IsEnabled = true;
                        break;
                    case menuWstepneDetale:
                        IsEnabled = true;
                        break;
                    case menuWstepneAsIs:
                        IsEnabled = false;
                        break;
                    case menuEdytujWymagania:
                        IsEnabled = false;
                       
                        break;
                    case menuEdytujWkladyIT:
                        IsEnabled = true;
                        break;
                    case menuEdytujWkladyNT:
                        IsEnabled = true;
                        break;
                    case menuWstepneOdswiezStatusyWymagan:
                        IsEnabled = true;
                        break;
                    // there shouldn't be any other, but just in case disable it.
                    default:
                        IsEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                IsEnabled = false;
            }
        }
        private static int CountClasses(Package package)
        {
            var count = 0;
            foreach (Element e in package.Elements)
                if (e.Type == "Package")
                    count++;
            foreach (Package p in package.Packages)
                count += CountClasses(p);
            return count;
        }
        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                // user has clicked the menuHello menu option
                case menuHello:
                    this.sayHello();
                    break;
                // user has clicked the menuGoodbye menu option
                case menuGoodbye:
                    this.sayGoodbye();
                    break;
                case menuGeneruj:
                    Statystyki oknoStatystyki = new Statystyki(Repository);
                    oknoStatystyki.ShowDialog();


                  //  Package model;
                  //  model.
                    break;
                case menuEdytujSlownik:
                    String[] kol = { "Lp", "Skrót/pojęcie", "Rozwinięcie - opis" };
                    String[] tag = { "Opis" };
                    int[] szer = { 100, 150, 470 };
                    Slownik sl = new Slownik(Repository,"Object","Definicje","Słownik",kol,tag,szer);
                    sl.ShowDialog();
                    sl.Dispose();
                    break;
                case menuEdytujZalaczniki:
                    String[] kol1 = { "Lp", "Nazwa załącznika", "Autor", "Ścieżka"};
                    String[] tag1 = {"Autor", "Ścieżka" };
                    int[] szer1 = { 100, 200, 150,270 };
                    Slownik zal = new Slownik(Repository,"Object","Definicje","Załączniki",kol1,tag1,szer1);
                    zal.ShowDialog();
                    zal.Dispose();
                    break;
                case menuEdytujZaleznosci:
                    String[] kol2 = { "Lp", "Nazwa projektu", "Rodzaj zależności", "Termin","Opis" };
                    String[] tag2 = { "Krytycznosc", "Termin","Opis" };
                    int[] szer2 = { 100, 100, 100,200, 270 };
                    Slownik zal2 = new Slownik(Repository, "Object", "Definicje", "Zależności", kol2, tag2, szer2);
                    zal2.ShowDialog();
                    zal2.Dispose();
                    break;
                case menuEdytujWymagania:
                    String[] kol3 = { "Lp", "Nazwa wymagania", "Treść wymagania", "Realizacja IT", "Status IT","Realizacja NT","Status NT" };
                    String[] tag3 = { "NOTATKA", "RealizacjaIT", "StatusIT","RealizacjaNT","StatusNT" };
                    int[] szer3 = { 50, 100, 220, 120, 50,120,50 };
                    Slownik zal3 = new Slownik(Repository, "Requirement", "Wymagania", "", kol3, tag3, szer3);
                    zal3.ShowDialog();
                    zal3.Dispose();
                    break;
                case Oprogramie:
                    Oprogramie opr = new Oprogramie();
                    opr.ShowDialog();
                    opr.Dispose();
                    break;
                case menuEdytujKoncepcjaOgolnaIT:
                    String[] sciezka = { "IT", "Koncepcja" };
                    Koncepcja konc = new Koncepcja(Repository,sciezka,"Koncepcja","Koncepcja ogólna IT");
                    konc.ShowDialog();
                    konc.Dispose();
                    break;
                case menuEdytujKoncepcjaSkrotIT:
                    String[] sciezka1 = { "IT", "Koncepcja" };
                    Koncepcja skr = new Koncepcja(Repository, sciezka1, "Skrot", "Koncepcja skrócona IT");
                    skr.ShowDialog();
                    skr.Dispose();
                    break;
                case menuEdytujKoncepcjaOgolnaNT:
                    String[] sciezka3 = { "NT", "Koncepcja" };
                    Koncepcja konc1 = new Koncepcja(Repository, sciezka3, "Koncepcja", "Koncepcja ogólna NT");
                    konc1.ShowDialog();
                    konc1.Dispose();
                    break;
                case menuEdytujKoncepcjaSkrotNT:
                    String[] sciezka4 = { "NT", "Koncepcja" };
                    Koncepcja skr1 = new Koncepcja(Repository, sciezka4, "Skrot", "Koncepcja skrócona NT");
                    skr1.ShowDialog();
                    skr1.Dispose();
                    break;
                case menuWstepneEAP:
                    EAUtils.utworzPustyModel(ref Repository);
                    MessageBox.Show("Model EAP został utworzony");
                    break;
                case menuEdytujWkladyIT:
                  
                    //EAUtils.generujWklady(ref Repository, "IT",CModel.IT);
                    EAUtils.generujWklady(new CModel(ref Repository),CModel.IT);
                    MessageBox.Show("Pakiety wkładów systemowych IT zostały wygenerowane");
                    break;
                case menuEdytujWkladyNT:
                    //EAUtils.generujWklady(ref Repository, "NT",CModel.NT);
                    EAUtils.generujWklady(new CModel(ref Repository), CModel.NT);
                    MessageBox.Show("Pakiety wkładów systemowych IT zostały wygenerowane");
                    break;
                case menuWstepneDetale: 
                    Detale det = new Detale(Repository);
                    det.ShowDialog();
                    det.Dispose();
                    break;
                case menuWstepneAsIs:
                    AsIsKlon asis = new AsIsKlon(Repository);
                    asis.ShowDialog();
                    asis.Dispose();
                    
                    break;
                case menuModelujASIS:
                    ZmielASIS zmiel = new ZmielASIS(Repository);
                    zmiel.ShowDialog();
                    zmiel.Dispose();
                    break;
                case menuWstepneOdswiezStatusyWymagan:
                    EAUtils.odswiezStatusyRequirement(ref Repository);
                    MessageBox.Show("Odświeżono statusy wymagań biznesowych");
                    break;
            }
        }

        ///
        /// Say Hello to the world
        ///
        private void sayHello()
        {
            MessageBox.Show("Hello World");
            this.shouldWeSayHello = false;
        }

        ///
        /// Say Goodbye to the world
        ///
        private void sayGoodbye()
        {
            MessageBox.Show("Goodbye World");
            this.shouldWeSayHello = true;
        }

        ///
        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        ///
        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public KzgAddinClassv2() { }
    }
}
