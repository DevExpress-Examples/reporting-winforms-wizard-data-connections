Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.DataAccess.Native
Imports DevExpress.DataAccess.Sql
Imports System.Xml
Imports System.Windows.Forms
Imports DevExpress.DataAccess.ConnectionParameters
Imports System.IO
Imports DevExpress.DataAccess.Wizard.Native
Imports DevExpress.DataAccess.Wizard.Services

Namespace T119350
    Friend Class CustomConnectionStorageService
        Inherits ConnectionStorageService
        Implements IConnectionStorageService

        Private Const xmlRootName As String = "Connections"

        Private _filename As String = "connections.xml"
        Private _includeApplicationConnections As Boolean = False

        Public Property FileName() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

        Public Property IncludeApplicationConnections() As Boolean
            Get
                Return _includeApplicationConnections
            End Get
            Set(ByVal value As Boolean)
                _includeApplicationConnections = value
            End Set
        End Property

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

        Public Function SaveConnection(ByVal connection As SqlDataConnection) As Boolean

            Try
                Dim doc As New XmlDocument()
                Dim root As XmlElement = Nothing
                If File.Exists(FileName) Then
                    doc.Load(FileName)
                    root = doc.DocumentElement
                    If root IsNot Nothing Then
                        If root.Name <> xmlRootName Then
                            MessageBox.Show(String.Format("Document element is '{0}', '{1}' expected", root.Name, xmlRootName))
                            Return False
                        End If
                        If root.SelectSingleNode(String.Format("Connection[Name = '{0}']", connection.Name)) IsNot Nothing Then
                            Return False
                        End If
                    End If
                End If
                If root Is Nothing Then
                    root = doc.CreateElement(xmlRootName)
                    doc.AppendChild(root)
                End If

                Dim nameElement As XmlElement = doc.CreateElement("Name")
                nameElement.AppendChild(doc.CreateTextNode(connection.Name))

                Dim connectionStringElement As XmlElement = doc.CreateElement("ConnectionString")
                connectionStringElement.AppendChild(doc.CreateTextNode(connection.CreateConnectionString()))

                Dim connectionElement As XmlElement = doc.CreateElement("Connection")
                connectionElement.AppendChild(nameElement)
                connectionElement.AppendChild(connectionStringElement)

                root.AppendChild(connectionElement)
                doc.Save(FileName)
                Return True
            Catch ex As Exception
                MessageBox.Show(String.Format("Cannot save connection to '{0}' because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
                Return False
            End Try
        End Function

        Private Function IConnectionStorageService_Contains(ByVal connectionName As String) As Boolean Implements IConnectionStorageService.Contains
            Return If(IncludeApplicationConnections, MyBase.Contains(connectionName) OrElse GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)), GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)))
        End Function

        Private Function IConnectionStorageService_GetConnections() As IEnumerable(Of DevExpress.DataAccess.Sql.SqlDataConnection) Implements IConnectionStorageService.GetConnections
            If Not File.Exists(FileName) Then
                Try
                    'Create empty XML file
                    Dim doc As New XmlDocument()
                    Dim root As XmlElement = doc.CreateElement(xmlRootName)
                    doc.AppendChild(root)
                    doc.Save(FileName)

                    'Add a connection to the XML file:
                    'SqlDataConnection defaultConnection = new CustomSqlDataConnection() {
                    '    Name = "Default Connection",
                    '    ConnectionString = "Data Source=localhost\\MSSQLSERVER2012;Initial Catalog=NWind;User Id=sa; Password=dx"
                    '};
                    'SaveIfNotSavedYet(defaultConnection);
                Catch ex As Exception
                    MessageBox.Show(String.Format("Cannot create '{0}' file because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
                    Return Nothing
                End Try
            End If

            Return If(IncludeApplicationConnections, MyBase.GetConnections().Union(GetConnectionsFromXml()), GetConnectionsFromXml())
        End Function
    End Class
End Namespace
