'Directe database connectie met een online administratie
'Met de volgende stappen kan een directe database connectie opgezet worden naar een online administratie
'1. Login
'2. prpGWayLoginSettingsGet
'a. Het Administratiekeuze-scherm wordt geopend
'3. Gebruik de volgende members van het result voor het opzetten van een databaseconnectie met de online administratie:
'a. prpGWayDbSqlServerNameGet
'b. prpGWayDbAdministratieNaamGet (bevat de database naam van de SQL Azure database)
'c. prpGWayDbSqlUserNameGet
'd. prpGWayDbSqlPasswordGet
'Opmerking 1: het instellen van prpGWayAdmiUserNameSet en prpGWayAdmiUserPasswordSet moet hier niet gebeuren. Hierin wordt de gebruiker van de administratie zelf ingesteld.
'Opmerking 2: De geldigheidsduur van de verkregen SQL login is gelijk aan die van de Gateway login, zoals beschreven bij de methode Login().
'Let op: van een administratie die zich online bevindt (in SQL Azure), verschilt de administratienaam met de databasenaam. De administratienaam is de naam, zoals de SnelStart gebruiker die ooit heeft opgegeven. De databasenaam is een interne naam, waarvan het SnelStart Services Platform gebruik maakt.
'Hieronder staat een voorbeeld van het opzetten van een directe SQL-connectie:

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