using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.ConnectionParameters;

namespace T119350 {
    class CustomSqlDataConnection : SqlDataConnection, INamedItem {
        public CustomSqlDataConnection(string name, DataConnectionParametersBase connectionParameters)
            : base(name, connectionParameters) {
        }

        string INamedItem.Name {
            get {
                return Name + " (Custom)";
            }
            set {
                Name = value;
            }
        }
    }
}
