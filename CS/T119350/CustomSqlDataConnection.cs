using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Sql;

class CustomSqlDataConnection : SqlDataConnection, INamedItem
{
    public CustomSqlDataConnection(string name, DataConnectionParametersBase connectionParameters)
        : base(name, connectionParameters) { }
    string INamedItem.Name
    {
        get => Name + " (Custom)";
        set
        {
            Name = value;
        }
    }
}
