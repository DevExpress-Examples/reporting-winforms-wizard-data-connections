Imports DevExpress.DataAccess.Wizard.Services
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace T119350
    Partial Public Class Form1
        Inherits DevExpress.XtraEditors.XtraForm

        Private connectionStorageService As CustomConnectionStorageService
		Public Sub New()
			InitializeComponent()

			connectionStorageService = New CustomConnectionStorageService() With {
				.FileName = "connections.xml",
				.IncludeApplicationConnections = True}
			ReplaceService(Me.reportDesigner1, GetType(IConnectionStorageService), connectionStorageService)
			AddHandler Me.reportDesigner1.DesignPanelLoaded, AddressOf DesignMdiControllerOnDesignPanelLoaded
		End Sub

		Private Sub ReplaceService(ByVal container As IServiceContainer, ByVal serviceType As Type, ByVal serviceInstance As Object)
			If container.GetService(serviceType) IsNot Nothing Then
				container.RemoveService(serviceType)
			End If
			container.AddService(serviceType, serviceInstance)
		End Sub

		Private Sub DesignMdiControllerOnDesignPanelLoaded(ByVal sender As Object, ByVal e As DesignerLoadedEventArgs)
			ReplaceService(e.DesignerHost, GetType(IConnectionStorageService), connectionStorageService)
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			Dim report As New XtraReport()
			Me.reportDesigner1.OpenReport(report)
		End Sub
	End Class
End Namespace
