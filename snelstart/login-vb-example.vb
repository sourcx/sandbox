Dim login As SnelStartGatewayNET.clsGWayLoginSettings
Dim gateway As New SnelStartGatewayNET. clsGWaySnelStart
Dim admiNaam As String

'Ophalen van een clsGWayLoginSettings object dat wordt gevuld met de snelstart open
'administratie dialoog

login = gateway.prpGWayLoginSettingsGet
If Not (login Is Nothing) Then
' De naam van de administratie bewaren
If login.OnlineAdministrationIdGet > 0 Then
admiNaam = login.OnlineAdministrationNameGet
Else
admiNaam = login.prpGWayDbAdministratieNaamGet
End If

Dim builder As New System.Data.Odbc.OdbcConnectionStringBuilder
builder.Driver = "{SQL Server Native Client 11.0}"
builder("Server") = login.prpGWayDbSqlServerNameGet
builder("Database") = login.prpGWayDbAdministratieNaamGet
builder("UID") = login.prpGWayDbSqlUserNameGet
builder("PWD") = login. prpGWayDbSqlPasswordGet
builder("Encrypt") = "yes"

Dim conn As New OdbcConnection(builder.ConnectionString)
conn.Open()
Dim command As New OdbcCommand("SELECT * FROM tblRelatie", conn)
Dim dr As OdbcDataReader = command.ExecuteReader()
While (dr.Read())
…
End While
End If​