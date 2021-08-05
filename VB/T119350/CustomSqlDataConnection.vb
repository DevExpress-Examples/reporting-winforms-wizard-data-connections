Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Native
Imports DevExpress.DataAccess.Sql

Friend Class CustomSqlDataConnection
    Inherits SqlDataConnection
    Implements INamedItem
    Public Sub New(name As String, connectionParameters As DataConnectionParametersBase)
        MyBase.New(name, connectionParameters)
    End Sub
    Private Property INamedItem_Name() As String Implements INamedItem.Name
        Get
            Return Name & " (Custom)"
        End Get
        Set
            Name = Value
        End Set
    End Property
End Class
