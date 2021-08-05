Imports System.IO
Imports System.Xml
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Wizard.Model
Imports DevExpress.DataAccess.Wizard.Native
Imports DevExpress.DataAccess.Wizard.Services

Friend Class CustomConnectionStorageService
	Implements IConnectionStorageService

	Public ReadOnly Property CanSaveConnection() As Boolean Implements IConnectionStorageService.CanSaveConnection
		Get
			Return True
		End Get
	End Property

	Private Function Contains(ByVal connectionName As String) As Boolean Implements IConnectionStorageService.Contains
		Return If(IncludeApplicationConnections, DefaultStorage.Contains(connectionName) OrElse GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)), GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)))
	End Function

	Private Function GetConnections() As IEnumerable(Of SqlDataConnection) Implements IConnectionStorageService.GetConnections
		If Not File.Exists(FileName) Then
			Try
				Dim doc As New XmlDocument()
				Dim root As XmlElement = doc.CreateElement(xmlRootName)
				doc.AppendChild(root)
				doc.Save(FileName)

				Dim defaultConnection As SqlDataConnection = New CustomSqlDataConnection("Default Connection", New MsSqlConnectionParameters("localhost", "Northwind", Nothing, Nothing, MsSqlAuthorizationType.Windows))
				SaveConnection(defaultConnection.Name, defaultConnection, True)
			Catch ex As Exception
				MessageBox.Show(String.Format("Cannot create '{0}' file because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
				Return Nothing
			End Try
		End If

		Return If(IncludeApplicationConnections, DefaultStorage.GetConnections().Union(GetConnectionsFromXml()), GetConnectionsFromXml())
	End Function

	Public Sub SaveConnection(ByVal connectionName As String, ByVal connection As IDataConnection, ByVal saveCredentials As Boolean) Implements IConnectionStorageService.SaveConnection
		Try
			Dim doc As New XmlDocument()
			Dim root As XmlElement = Nothing
			If File.Exists(FileName) Then
				doc.Load(FileName)
				root = doc.DocumentElement
				If root IsNot Nothing Then
					If root.Name <> xmlRootName Then
						MessageBox.Show(String.Format("Document element is '{0}', '{1}' expected", root.Name, xmlRootName))
						Return
					End If
					If root.SelectSingleNode(String.Format("Connection[Name = '{0}']", connectionName)) IsNot Nothing Then
						Return
					End If
				End If
			End If
			If root Is Nothing Then
				root = doc.CreateElement(xmlRootName)
				doc.AppendChild(root)
			End If

			Dim nameElement As XmlElement = doc.CreateElement("Name")
			nameElement.AppendChild(doc.CreateTextNode(connectionName))

			Dim connectionStringElement As XmlElement = doc.CreateElement("ConnectionString")
			connectionStringElement.AppendChild(doc.CreateTextNode(connection.CreateConnectionString(Not saveCredentials)))

			Dim connectionElement As XmlElement = doc.CreateElement("Connection")
			connectionElement.AppendChild(nameElement)
			connectionElement.AppendChild(connectionStringElement)

			root.AppendChild(connectionElement)
			doc.Save(FileName)
		Catch ex As Exception
			MessageBox.Show(String.Format("Cannot save connection to '{0}' because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
		End Try
	End Sub

	Private Const xmlRootName As String = "Connections"
	Public Property FileName() As String = "connections.xml"
	Public Property IncludeApplicationConnections() As Boolean = False
	Protected ReadOnly Property DefaultStorage() As New ConnectionStorageService()

	Protected Function GetConnectionsFromXml() As IEnumerable(Of SqlDataConnection)
		Dim result = New List(Of SqlDataConnection)()
		Try
			Dim doc As New XmlDocument()
			doc.Load(FileName)
			For Each node As XmlNode In doc.SelectNodes("Connections/Connection[Name][ConnectionString]")
				result.Add(New CustomSqlDataConnection(node("Name").InnerText, New CustomStringConnectionParameters(node("ConnectionString").InnerText)))
			Next node
			Return result
		Catch ex As Exception
			MessageBox.Show(String.Format("Cannot get connections from '{0}' because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
			Return New SqlDataConnection(){}
		End Try
	End Function
End Class
