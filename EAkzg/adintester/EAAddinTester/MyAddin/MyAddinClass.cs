using System;
using System.Windows.Forms;

namespace MyAddin
{
    public class MyAddinClass
    {
        // define menu constants
        const string menuHeader = "-&MyAddin";
        const string menuHello = "&Say Hello";
        const string menuGoodbye = "&Say Goodbye";

        // remember if we have to say hello or goodbye
        private bool shouldWeSayHello = true;
                 
        /// <summary>
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        /// </summary>
        /// <param name="Repository">the EA repository</param>
        /// <returns>a string</returns>
        public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.
            return "a string";
        }


        /// <summary>
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        /// </summary>
        /// <param name="Repository">the repository</param>
        /// <param name="Location">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <returns></returns>
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            
                switch (MenuName)
                {
                    // defines the top level menu option
                    case "":
                        return menuHeader;
                    // defines the submenu options
                    case menuHeader:
                        string[] subMenus = { menuHello, menuGoodbye};
                        return subMenus;
                }

            return "";
        }

        
        /// <summary>
        /// returns true if a project is currently opened
        /// </summary>
        /// <param name="Repository">the repository</param>
        /// <returns>true if a project is opened in EA</returns>
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Called once Menu has been opened to see what menu items should active.
        /// </summary>
        /// <param name="Repository">the repository</param>
        /// <param name="Location">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the menu item</param>
        /// <param name="IsEnabled">boolean indicating whethe the menu item is enabled</param>
        /// <param name="IsChecked">boolean indicating whether the menu is checked</param>
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
                    // define the state of the goodbye menu option
                    case menuGoodbye:
                        IsEnabled = !shouldWeSayHello;
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

        /// <summary>
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        /// </summary>
        /// <param name="Repository">the repository</param>
        /// <param name="Location">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the selected menu item</param>
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
            }
        }

        /// <summary>
        /// Say Hello to the world
        /// </summary>
        private void sayHello()
        {
            MessageBox.Show("Hello World");
            this.shouldWeSayHello = false;
        }

        /// <summary>
        /// Say Goodbye to the world
        /// </summary>
        private void sayGoodbye()
        {
            MessageBox.Show("Goodbye World");
            this.shouldWeSayHello = true;
        }
    
        /// <summary>
        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        /// </summary>
        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}
