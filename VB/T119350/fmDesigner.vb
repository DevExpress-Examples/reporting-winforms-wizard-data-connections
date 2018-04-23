Imports T119350
Imports DevExpress.DataAccess.Wizard.Services
Imports DevExpress.DataAccess.Sql
Imports System.ComponentModel.Design
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraReports.UI

Public Class fmDesigner
    Private connectionStorageService As CustomConnectionStorageService

    Public Sub New()
        InitializeComponent()

        connectionStorageService = New CustomConnectionStorageService() With {.FileName = "connections.xml", .IncludeApplicationConnections = False}
        ReplaceService(Me.reportDesignerMDIController, GetType(IConnectionStorageService), connectionStorageService)
        AddHandler reportDesignerMDIController.DesignPanelLoaded, AddressOf DesignMdiControllerOnDesignPanelLoaded
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

    Private Sub fmDesigner_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Open new report
        Dim report As New XtraReport()
        Me.reportDesignerMDIController.OpenReport(report)
    End Sub
End Class