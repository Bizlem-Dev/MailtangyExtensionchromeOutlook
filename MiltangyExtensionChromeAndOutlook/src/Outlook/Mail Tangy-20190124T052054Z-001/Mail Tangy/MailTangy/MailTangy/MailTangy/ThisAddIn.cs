using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json.Linq;

namespace MailTangy
{
    public partial class ThisAddIn
    {
        private MailTangyContainer mailTangyContainer;
        //public MailTangyContainer MailTangyContainer
        //{
        //    get { return this.mailTangyContainer; }
        //}

        public string LoggedinUserID;
        public string SenderEmailID;
        public MailItem SelectedMailItem;
        private Microsoft.Office.Tools.CustomTaskPane taskPaneContainer;

        public Credentials myCredentials;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            
        }

        
        public void CurrentExplorer_SelectionChangeEvent()
        {
            
            try
            {
                if (myCredentials==null)
                {
                    return;
                }
                
                if (this.Application.ActiveExplorer().Selection.Count > 0)
                {
                    Object selObject = Globals.ThisAddIn.Application.ActiveExplorer().Selection[1];
                    if (selObject is Microsoft.Office.Interop.Outlook.MailItem)
                    {
                        this.Application.ActiveExplorer().SelectionChange -= new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(CurrentExplorer_SelectionChangeEvent);
                        SelectedMailItem = selObject as Microsoft.Office.Interop.Outlook.MailItem;
                        //LoggedinUserID = "rishi.khan0909@gmail.com";
                        
                        SenderEmailID = SelectedMailItem.SenderEmailAddress;
                        if (mailTangyContainer == null)
                        {
                            mailTangyContainer = new MailTangyContainer(PaneType.UserSpecificPane);
                            taskPaneContainer = Globals.ThisAddIn.CustomTaskPanes.Add(mailTangyContainer, "MailTangy UserSpecific");

                            //taskPaneValue.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                            //taskPaneValue.Height = 420;
                            taskPaneContainer.Width = 340;
                            var fPane = Globals.ThisAddIn.CustomTaskPanes.Where(pane => pane.Title == "MailTangy Features");
                            if (fPane==null)
                            {
                                taskPaneContainer.Visible = true;
                            }

                        }
                        
                            SenderEmailID = SelectedMailItem.SenderEmailAddress;
                            Globals.ThisAddIn.Application.ActiveExplorer().AddToSelection(selObject);                     
                     }
                    
                }
            }
            catch(System.Exception ex) { throw ex; }
            //this.Application.ActiveExplorer().SelectionChange += new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(CurrentExplorer_SelectionChangeEvent);
        }

        

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
