Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel.Design
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.DataAccess.Wizard.Services

Namespace T119350

    Public Partial Class fmDesigner
        Inherits Form

        Private connectionStorageService As CustomConnectionStorageService

        Public Sub New()
            InitializeComponent()
            connectionStorageService = New CustomConnectionStorageService() With {.FileName = "connections.xml", .IncludeApplicationConnections = False}
            ReplaceService(reportDesignerMDIController, GetType(IConnectionStorageService), connectionStorageService)
            AddHandler reportDesignerMDIController.DesignPanelLoaded, AddressOf DesignMdiControllerOnDesignPanelLoaded
        End Sub

        Private Sub ReplaceService(ByVal container As IServiceContainer, ByVal serviceType As Type, ByVal serviceInstance As Object)
            If container.GetService(serviceType) IsNot Nothing Then container.RemoveService(serviceType)
            container.AddService(serviceType, serviceInstance)
        End Sub

        Private Sub DesignMdiControllerOnDesignPanelLoaded(ByVal sender As Object, ByVal e As DesignerLoadedEventArgs)
            ReplaceService(e.DesignerHost, GetType(IConnectionStorageService), connectionStorageService) ' insert this line!
            Dim componentChangeService As IComponentChangeService = CType(e.DesignerHost.GetService(GetType(IComponentChangeService)), IComponentChangeService)
            If componentChangeService IsNot Nothing Then
                AddHandler componentChangeService.ComponentAdded, AddressOf ComponentChangeServiceOnComponentAdded
                AddHandler componentChangeService.ComponentChanged, AddressOf ComponentChangeServiceOnComponentChanged
            End If
        End Sub

        Private Sub ComponentChangeServiceOnComponentAdded(ByVal sender As Object, ByVal e As ComponentEventArgs)
            Dim sqlDataSource As SqlDataSource = TryCast(e.Component, SqlDataSource)
            If sqlDataSource Is Nothing Then Return
            connectionStorageService.SaveConnection(sqlDataSource.Connection)
        End Sub

        Private Sub ComponentChangeServiceOnComponentChanged(ByVal sender As Object, ByVal e As ComponentChangedEventArgs)
            Dim sqlDataSource As SqlDataSource = TryCast(e.Component, SqlDataSource)
            If sqlDataSource Is Nothing Then Return
            connectionStorageService.SaveConnection(sqlDataSource.Connection)
        End Sub

        Private Sub fmDesigner_Load(ByVal sender As Object, ByVal e As EventArgs)
            'Open new report
            Dim report As XtraReport = New XtraReport()
            reportDesignerMDIController.OpenReport(report)
        End Sub
    End Class
End Namespace
