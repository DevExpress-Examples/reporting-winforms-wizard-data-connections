using DevExpress.DataAccess.Wizard.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T119350
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        CustomConnectionStorageService connectionStorageService;
        public Form1()
        {
            InitializeComponent();

            connectionStorageService = new CustomConnectionStorageService()
            {
                FileName = "connections.xml",
                IncludeApplicationConnections = true
            };
            ReplaceService(this.reportDesigner1, 
                typeof(IConnectionStorageService), connectionStorageService);
            this.reportDesigner1.DesignPanelLoaded += DesignMdiControllerOnDesignPanelLoaded;
        }

        private void ReplaceService(IServiceContainer container, 
            Type serviceType, 
            object serviceInstance)
        {
            if (container.GetService(serviceType) != null)
                container.RemoveService(serviceType);
            container.AddService(serviceType, serviceInstance);
        }

        private void DesignMdiControllerOnDesignPanelLoaded(object sender, 
            DesignerLoadedEventArgs e)
        {
            ReplaceService(e.DesignerHost, typeof(IConnectionStorageService), connectionStorageService);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XtraReport report = new XtraReport();
            this.reportDesigner1.OpenReport(report);
        }
    }
}
