using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Sql;
using System.Xml;
using System.Windows.Forms;
using DevExpress.DataAccess.ConnectionParameters;
using System.IO;
using DevExpress.DataAccess.Wizard.Native;
using DevExpress.DataAccess.Wizard.Services;

namespace T119350 {
    class CustomConnectionStorageService : ConnectionStorageService, IConnectionStorageService {        
        const string xmlRootName = "Connections";

        string _filename = "connections.xml";
        bool _includeApplicationConnections = false;

        public string FileName {
            get {
                return _filename;
            }
            set {
                _filename = value;
            }
        }

        public bool IncludeApplicationConnections {
            get {
                return _includeApplicationConnections;
            }
            set {
                _includeApplicationConnections = value;
            }
        }

        protected IEnumerable<SqlDataConnection> GetConnectionsFromXml() {
            var result = new List<SqlDataConnection>();
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);
                foreach (XmlNode node in doc.SelectNodes("Connections/Connection[Name][ConnectionString]"))
                    result.Add(new CustomSqlDataConnection(node["Name"].InnerText,
                        new CustomStringConnectionParameters(node["ConnectionString"].InnerText)));
                return result;
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format("Cannot get connections from '{0}' because of exception:{1}{1}{2}",
                    FileName, Environment.NewLine, ex.Message));
                return new SqlDataConnection[0];
            }
        }

        public bool SaveConnection(SqlDataConnection connection) {

            try {
                XmlDocument doc = new XmlDocument();
                XmlElement root = null;
                if (File.Exists(FileName)) {
                    doc.Load(FileName);
                    root = doc.DocumentElement;
                    if (root != null) {
                        if (root.Name != xmlRootName) {
                            MessageBox.Show(string.Format("Document element is '{0}', '{1}' expected",
                                root.Name, xmlRootName));
                            return false;
                        }
                        if (root.SelectSingleNode(string.Format("Connection[Name = '{0}']", connection.Name)) != null)
                            return false;
                    }
                }
                if (root == null) {
                    root = doc.CreateElement(xmlRootName);
                    doc.AppendChild(root);
                }

                XmlElement nameElement = doc.CreateElement("Name");
                nameElement.AppendChild(doc.CreateTextNode(connection.Name));

                XmlElement connectionStringElement = doc.CreateElement("ConnectionString");
                connectionStringElement.AppendChild(doc.CreateTextNode(connection.CreateConnectionString()));

                XmlElement connectionElement = doc.CreateElement("Connection");
                connectionElement.AppendChild(nameElement);
                connectionElement.AppendChild(connectionStringElement);

                root.AppendChild(connectionElement);
                doc.Save(FileName);
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format("Cannot save connection to '{0}' because of exception:{1}{1}{2}", FileName,
                    Environment.NewLine, ex.Message));
                return false;
            }
        }

        bool IConnectionStorageService.Contains(string connectionName) {
            return IncludeApplicationConnections ? 
                base.Contains(connectionName) || GetConnectionsFromXml().Any(c => string.Equals(c.Name, connectionName)) :
                GetConnectionsFromXml().Any(c => string.Equals(c.Name, connectionName));
        }

        IEnumerable<DevExpress.DataAccess.Sql.SqlDataConnection> IConnectionStorageService.GetConnections() {
            if (!File.Exists(FileName)) {
                try {
                    //Create empty XML file
                    XmlDocument doc = new XmlDocument();
                    XmlElement root = doc.CreateElement(xmlRootName);
                    doc.AppendChild(root);
                    doc.Save(FileName);

                    //Add a connection to the XML file:
                    //SqlDataConnection defaultConnection = new CustomSqlDataConnection() {
                    //    Name = "Default Connection",
                    //    ConnectionString = "Data Source=localhost\\MSSQLSERVER2012;Initial Catalog=NWind;User Id=sa; Password=dx"
                    //};
                    //SaveIfNotSavedYet(defaultConnection);
                }
                catch (Exception ex) {
                    MessageBox.Show(string.Format("Cannot create '{0}' file because of exception:{1}{1}{2}", FileName,
                        Environment.NewLine, ex.Message));
                    return null;
                }
            }

            return IncludeApplicationConnections ?
                base.GetConnections().Union(GetConnectionsFromXml()) :
                GetConnectionsFromXml();
        }
    }
}
