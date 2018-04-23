using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using System.ComponentModel.Design;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Wizard.Services;

namespace T119350 {
    public partial class fmDesigner : Form {
        CustomConnectionStorageService connectionStorageService;

        public fmDesigner() {
            InitializeComponent();

            connectionStorageService = new CustomConnectionStorageService() {
                FileName = "connections.xml",
                IncludeApplicationConnections = false
            };
            ReplaceService(this.reportDesignerMDIController, typeof(IConnectionStorageService), connectionStorageService);
            reportDesignerMDIController.DesignPanelLoaded += DesignMdiControllerOnDesignPanelLoaded;
        }

        private void ReplaceService(IServiceContainer container, Type serviceType, object serviceInstance) {
            if (container.GetService(serviceType) != null)
                container.RemoveService(serviceType);
            container.AddService(serviceType, serviceInstance);
        }

        private void DesignMdiControllerOnDesignPanelLoaded(object sender, DesignerLoadedEventArgs e) {
            ReplaceService(e.DesignerHost, typeof(IConnectionStorageService), connectionStorageService);
        }        

        private void fmDesigner_Load(object sender, EventArgs e) {
            //Open new report
            XtraReport report = new XtraReport();
            this.reportDesignerMDIController.OpenReport(report);
        }
    }
}
