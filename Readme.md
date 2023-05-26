# Reporting for WinForms - How to Store Connections Available in the Data Source Wizard

When a user adds a new data source, the list of available connections in the Data Source Wizard is populated with connections defined in the application configuration file. This example demonstrates how to use a custom storage (a `connectons.xml`) file to load and save Data Source Wizard connections. For this,  implement the [IConnectionStorageService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService) and register it in the Report Designer component. 

The `CustomSqlDataConnection` class (the [SqlDataConnection](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SqlDataConnection) descendant) in this example holds the names displayed in the connection list.

<strong>Important</strong>: In this example, the connection string in the XML file is not encrypted and exposes sensitive information (a username and a password). You should protect the file storage to prevent unauthorized access in a real-world application. 

## Files to Review

* [CustomConnectionStorageService.cs](./CS/T119350/CustomConnectionStorageService.cs) (VB: [CustomConnectionStorageService.vb](./VB/T119350/CustomConnectionStorageService.vb))
* [CustomSqlDataConnection.cs](./CS/T119350/CustomSqlDataConnection.cs) (VB: [CustomSqlDataConnection.vb](./VB/T119350/CustomSqlDataConnection.vb))
* [Form1.cs](./CS/T119350/Form1.cs) (VB: [Form1.vb](./VB/T119350/Form1.vb))

## Documentation

- [Customize Data Connections in the Data Source Wizard (WinForms)](https://docs.devexpress.com/XtraReports/403352/winforms-reporting/end-user-report-designer-for-winforms/api-and-customization/customize-data-connections)
- [Data Sources in Web End-User Report Designer (ASP.NET Core)](https://docs.devexpress.com/XtraReports/401896/web-reporting/asp-net-core-reporting/end-user-report-designer-in-asp-net-applications/use-data-sources-and-connections)
## More Examples

- [https://github.com/DevExpress-Examples/ReportingEUDSaveReportWithoutConnectionParams](https://github.com/DevExpress-Examples/ReportingEUDSaveReportWithoutConnectionParams)