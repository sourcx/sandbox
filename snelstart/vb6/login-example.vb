'Referentie naar GateWay-object
 Private varGWaySnelStart As clsGWaySnelStart
 Referentie naar LogInSettings-object (tbv her-openen administratie)
 Private varGWayLoginSettings As clsGWayLoginSettings
 'Lokale opslag Login-gegevens (tbv "nabootsen" her-openen administratie via eerder opgeslagen Login-gegevens)
 Private varDbAdministratieNaam As String
 Private varDbTypeJet As Boolean
 'JetData
 Private varDbJetFolder As String
 'SqlData
 Private varDbSqlServerName As String
 Private varDbSqlConnectTypeWindows As Boolean
 Private varDbSqlUserName As Strin
 'Initialiseren
 Private Sub Form_Load()
 ' Aanmaken GateWay-object
  Set varGWaySnelStart = New clsGWaySnelStart
 End Sub
 '
 'Openen adminstratie via de FolderDialog
 Private Sub cmdAdmiOpenViaDialog_Click()
  On Error GoTo ErrHandler
  '1. Selecteren van de administratie en opslag gegevens betreffende de geselecteerde administratie in het LoginSettings-object
  '----------------------------------------------------------------------------------------------------------------------------
  Set varGWayLoginSettings = varGWaySnelStart.prpGWayLoginSettingsGet
  'Tonen gegevens geselecteerde administratie
  With varGWayLoginSettings                                                     'Jet (Access):                                Sql:
    Debug.Print "Administratie         = " & .prpGWayDbAdministratieNaamGet     'Naam van de administratie                    Naam van de administratie
    Debug.Print "Omschrijving          = " & .prpGWayDbTypeOmschrijvingGet      '"MDB"                                        "SQL"
    Debug.Print "JetType               = " & .prpGWayDbTypeJetGet               'True                                         False
    Debug.Print "JetData:"
    Debug.Print "JetFolder             = " & .prpGWayDbJetFolderGet             'Pad naar de administratie, inclusief \       ""
    Debug.Print "SqlData:"
    Debug.Print "ServerName            = " & .prpGWayDbSqlServerNameGet         '""                                           Naam van de Server
    Debug.Print "WindowsAuthentication = " & .prpGWayDbSqlConnectTypeWindowsGet 'False                                        True (WindowsAuthentication) of False (SqlServerAuthentication)
    Debug.Print "UserName              = " & .prpGWayDbSqlUserNameGet           '""                                           "" of Naam van de User
    'Gegevens in het LoginSettings-object die wel aanwezig zijn maar niet uitgelezen kunnen worden:
    'Sql: LoginPassword SqlServer
  End With
  '2. Openen administratie met het LoginSettings-object
  '----------------------------------------------------
  If varGWaySnelStart.mtdGWayAdmiOpenenViaLoginSettings(Me.hWnd, varGWayLoginSettings) = True Then
    Debug.Print "Resultaat             = " & "Het is gelukt om de administratie via de Dialog te openen"
    'Controle of dat echt zo is (zo niet, dan wordt de ErrHandler geactiveerd)
    Debug.Print "AdmiInEuro            = " & varGWaySnelStart.prpGWayAdmiInEuroGet
  Else
    Debug.Print "Resultaat             = " & "Het is NIET gelukt om de administratie via de Dialog te openen"
  End If
  Debug.Print ""
  Exit Sub
 ErrHandler:
  mtdMsg mtdGWayFoutBoodschap("cmdAdmiOpenViaDialog_Click"), vbInformation
 End Sub
 '
 'Sluiten geopende adminstratie
 Private Sub cmdAdmiSluiten_Click()
  On Error GoTo ErrHandler
  '3. Sluiten geopende administratie (geen probleem als de administratie al gesloten was)
  '--------------------------------------------------------------------------------------
  varGWaySnelStart.mtdGWayAdmiSluiten
  Debug.Print "Admi is gesloten"
  Debug.Print ""
  Exit Sub
 ErrHandler:
  mtdMsg mtdGWayFoutBoodschap("cmdAdmiSluiten_Click"), vbInformation
 End Sub
 '
 'Openen adminstratie via het LoginSettings-object
 Private Sub cmdAdmiOpenViaLoginObject_Click()
  On Error GoTo ErrHandler
  '4. Openen administratie via de aktuele gegevens in het LoginSettings-object

  '---------------------------------------------------------------------------
  '- beschikbaar nadat administratie eerst via varGWaySnelStart.prpGWayLoginSettingsGet in cmdAdmiOpenViaDialog_Click
  '  geopend is geweest
  '- Een eventueel geopende administratie wordt gesloten alvorens de nieuwe geopend wordt
  If varGWaySnelStart.mtdGWayAdmiOpenenViaLoginSettings(Me.hWnd, varGWayLoginSettings) = True Then
    Debug.Print "Resultaat             = " & "Het is gelukt om de administratie te openen"
    'Controle of dat echt zo is (zo niet, dan wordt de ErrHandler geactiveerd)
    Debug.Print "AdmiInEuro            = " & varGWaySnelStart.prpGWayAdmiInEuroGet
  Else
    Debug.Print "Resultaat             = " & "Het is NIET gelukt om de administratie te openen"
  End If
  Debug.Print ""
  Exit Sub
 ErrHandler:
  mtdMsg mtdGWayFoutBoodschap("cmdAdmiOpenViaLoginObject_Click"), vbInformation
 End Sub


 'Nabootsen opslag Inlog-gegevens
 Private Sub cmdAdmiGegevensOpslaan_Click()
  On Error GoTo ErrHandler
  '5. Overnemen gegevens uit het LoginSettings-object in lokale variabelen
  '-----------------------------------------------------------------------
  With varGWayLoginSettings                                          'Jet (Access):                                Sql:
    varDbAdministratieNaam = .prpGWayDbAdministratieNaamGet          'Naam van de administratie                    Naam van de administratie
    'Niet nodig om te openen:  .prpGWayDbTypeOmschrijvingGet         '"MDB"                                        "SQL"
    varDbTypeJet = .prpGWayDbTypeJetGet                              'True                                         False
    'JetData
    varDbJetFolder = .prpGWayDbJetFolderGet                          'Pad naar de administratie, inclusief \       ""
    'SqlData
    varDbSqlServerName = .prpGWayDbSqlServerNameGet                  '""                                           Naam van de Server
    varDbSqlConnectTypeWindows = .prpGWayDbSqlConnectTypeWindowsGet  'False                                        True (WindowsAuthentication) of False (SqlServerAuthentication)
    varDbSqlUserName = .prpGWayDbSqlUserNameGet                      '""                                           "" of Naam van de User
  End With
  Debug.Print "De Login-gegevens zijn lokaal opgeslagen"
  Debug.Print ""
  Exit Sub
 ErrHandler:
  mtdMsg mtdGWayFoutBoodschap("cmdAdmiGegevensOpslaan_Click"), vbInformation
 End Sub


 'Nabootsen openen administratie via lokaal opgeslagen gegevens
 Private Sub cmdAdmiOpenViaOpgeslagenGegevens_Click()
  On Error GoTo ErrHandler
  Dim mvrGWayLoginSettings As clsGWayLoginSettings
  'Openen nieuw LoginSettings-object
  Set mvrGWayLoginSettings = New clsGWayLoginSettings
  '6. Invullen gegevens in het LoginSettings-object
  '------------------------------------------------
  With mvrGWayLoginSettings                                          'Jet (Access):                                Sql:
    .prpGWayDbAdministratieNaamSet = varDbAdministratieNaam          'Naam van de administratie                    Naam van de administratie
    .prpGWayDbTypeJetSet = varDbTypeJet                              'True                                         False
    'JetData
    .prpGWayDbJetFolderSet = varDbJetFolder                          'Pad naar de administratie, inclusief \       ""
    'SqlData
    .prpGWayDbSqlServerNameSet = varDbSqlServerName                  '""                                           Naam van de Server
    .prpGWayDbSqlConnectTypeWindowsSet = varDbSqlConnectTypeWindows  'False                                        True (WindowsAuthentication) of False (SqlServerAuthentication)
    .prpGWayDbSqlUserNameSet = varDbSqlUserName                      '""                                           "" of Naam van de User
  End With
  '7. Openen administratie via de gegevens in het LoginSettings-object
  '-------------------------------------------------------------------
  '- beschikbaar nadat administratie eerst via varGWaySnelStart.prpGWayLoginSettingsGet in cmdAdmiOpenViaDialog_Click
  '  geopend is geweest
  '- Een eventueel geopende administratie wordt gesloten alvorens de nieuwe geopend wordt
  If varGWaySnelStart.mtdGWayAdmiOpenenViaLoginSettings(Me.hWnd, mvrGWayLoginSettings) = True Then
    Debug.Print "Resultaat             = " & "Het is gelukt om de administratie te openen via de lokaal opgeslagen gegevens"
    'Controle of dat echt zo is (zo niet, dan wordt de ErrHandler geactiveerd)
    Debug.Print "AdmiInEuro            = " & varGWaySnelStart.prpGWayAdmiInEuroGet
 Else
    Debug.Print "Resultaat             = " & "Het is NIET gelukt om de administratie te openen via de lokaal opgeslagen gegevens"
  End If
  Debug.Print ""
  Exit Sub
 ErrHandler:
  mtdMsg mtdGWayFoutBoodschap("cmdAdmiOpenViaOpgelagenGegevens_Click"), vbInformation
 End Sub