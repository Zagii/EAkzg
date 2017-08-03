using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EAAddinTester
{
    static class EAAddinTesterProgram
    {
        // the addin we are testing
        // replace this next line with the addin you want to test
        internal static EAkzg.KzgAddinClassv2 addin = new EAkzg.KzgAddinClassv2();
        // reference to currently opened EA repository
        internal static EA.Repository eaRepository;
        // the tester form
        private static EAAddinTesterForm form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            eaRepository = getOpenedModel();
            if (eaRepository != null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new EAAddinTesterForm();
                Application.Run(form);
            }
        }
        /// <summary>
        /// gets the menu items from the addin
        /// </summary>
        /// <param name="location">the location in EA</param>
        /// <param name="addinMenu">the menu where to add the items</param>
        /// <param name="menuName">the name of the menu</param>
        internal static void SetMenu(string location, ToolStripMenuItem addinMenu,string menuName)
        {
            object menuItemsObject = addin.EA_GetMenuItems(eaRepository, location, menuName);
            string[] menuItems = null;
            // check if menuItemsObject is an array of strings
            if (menuItemsObject is string[])
            {
                menuItems = (string[])menuItemsObject;
            }
            else
            //must be a string then
            {
                menuItems = new string[1];
                menuItems[0] = (string)menuItemsObject;
            }
            // first remove dropdownItems
            addinMenu.DropDownItems.Clear();
            // then assign new items
            foreach (string menuItem in menuItems)
            {
                // if the menuItem starts with a "-" then it has submenu items
                if (menuItem.StartsWith("-"))
                {
                    // remove the "-";
                    string menuItemName = menuItem;
                    menuItemName = menuItem.Substring(1);
                    // add the menu item
                    addinMenu.DropDownItems.Add(menuItemName);
                    //get the newly added item
                    ToolStripMenuItem newMenuItem = (ToolStripMenuItem)addinMenu.DropDownItems[addinMenu.DropDownItems.Count - 1];
                    //add the eventhandler for its subItems
                    newMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(form.addInsToolStripMenuItem_DropDownItemClicked);
                    // add its submenu items
                    SetMenu(location, newMenuItem, menuItem);
                }
                // else it is a leaf menu item
                else
                {
                    // add the menu item
                    addinMenu.DropDownItems.Add(menuItem);
                    // get the newly added item
                    ToolStripMenuItem newMenuItem = (ToolStripMenuItem)addinMenu.DropDownItems[addinMenu.DropDownItems.Count - 1];
                    //set its state, only leaf items get their state set.
                    bool enabledValue = false;
                    bool checkedValue = false;
                    addin.EA_GetMenuState(eaRepository, location, newMenuItem.OwnerItem.Text, newMenuItem.Text, ref enabledValue, ref checkedValue);
                    newMenuItem.Enabled = enabledValue;
                    newMenuItem.Checked = checkedValue;
                }
            }
      
        }
        /// <summary>
        /// Menu is clicked, forward to addin
        /// </summary>
        /// <param name="location">the location within EA</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the clicked item</param>
        internal static void clickMenu(string location,string menuName, string itemName)
        {
            addin.EA_MenuClick(eaRepository, location, menuName, itemName);
        }
        /// <summary>
        /// Gets the Repository object from the currently running instance of EA.
        /// If multiple instances are running it returns the first one opened.
        /// </summary>
        /// <returns>Repository object for the running instance of EA</returns>
        private static EA.Repository getOpenedModel()
        {
            try
            {
                EA.App ap=(EA.App)Marshal.GetActiveObject("EA.App");
                
               return ap.Repository;
            }
            catch (COMException)
            {
                DialogResult result = MessageBox.Show("Nie włączono aplikacji EA.\nWłącz EA i uruchom ponownie"
                                   , "EA wyłączone",MessageBoxButtons.RetryCancel,MessageBoxIcon.Warning);
                if (result == DialogResult.Retry)
                {
                  //  eaRepository = new EA.Repository();
                   // eaRepository.OpenFile(@"D:\_Projekty\PR-NNN.eap");
                    //return eaRepository;
                    return getOpenedModel();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
