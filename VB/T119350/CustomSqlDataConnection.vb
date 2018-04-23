Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Native
Imports DevExpress.DataAccess.ConnectionParameters

Namespace T119350
	Friend Class CustomSqlDataConnection
		Inherits SqlDataConnection
		Implements INamedItem

		Public Sub New(ByVal name As String, ByVal connectionParameters As DataConnectionParametersBase)
			MyBase.New(name, connectionParameters)
		End Sub

        Private Property INamedItem_Name() As String Implements INamedItem.Name
            Get
                Return Name & " (Custom)"
            End Get
            Set(ByVal value As String)
                Name = value
            End Set
        End Property

    End Class
End Namespace
