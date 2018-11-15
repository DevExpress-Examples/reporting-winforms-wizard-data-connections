<!-- default file list -->
*Files to look at*:

* **[CustomConnectionStorageService.cs](./CS/T119350/CustomConnectionStorageService.cs) (VB: [CustomConnectionStorageService.vb](./VB/T119350/CustomConnectionStorageService.vb))**
* [CustomSqlDataConnection.cs](./CS/T119350/CustomSqlDataConnection.cs) (VB: [CustomSqlDataConnection.vb](./VB/T119350/CustomSqlDataConnection.vb))
* [fmDesigner.cs](./CS/T119350/fmDesigner.cs) (VB: [fmDesigner.vb](./VB/T119350/fmDesigner.vb))
<!-- default file list end -->
# WinForms End-User Designer - How to customize the data source wizard connection string storage


<p>By default, the report connections list is populated based on the connections stored in the application configuration file. In any case, the application configuration cannot be modified at runtime, that's why the report connections list is not changed when the End-User Report Designer is used.</p>
<p>To initialize, save and restore the connection strings in the End-User Report Designer, use the approach demonstrated in this code example.<br><br>The main idea of this approach is to create a custom connection storage service implementing <a href="https://documentation.devexpress.com/#CoreLibraries/clsDevExpressDataAccessWizardServicesIConnectionStorageServicetopic">IConnectionStorageService</a> interface. Also, the <a href="https://documentation.devexpress.com/#CoreLibraries/clsDevExpressDataAccessSqlSqlDataConnectiontopic">SqlDataConnection</a> class descendant (<strong>CustomSqlDataConnection</strong>) is used in this example to customize the connection names in the connections list.<br><br></p>
<p><strong>Important</strong>: In this example, the connection string is not encrypted and is stored openly in an XML file, exposing potentially sensitive information (such as a password to connect to a database). Please take this issue into account when translating this implementation to a real-world business application (e.g., you may be required to use the <a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressDataAccessSqlSqlDataConnection_BlackoutCredentialstopic">SqlDataConnection.BlackoutCredentials</a> method to avoid saving the user credentials to the configuration file).</p>

<br/>


