Private Sub mtdToevoegenJournaalPost()
  On Error GoTo ErrorHandler
  'Declaratie variabelen
  Dim mvrGWaySnelStart As clsGWaySnelStart
  Dim mvrAdmiInEuro As Boolean
  Dim mvrGbOmschrijving As String
  'Aanmaken nieuwe ToegangsObject
  Set mvrGWaySnelStart = New clsGWaySnelStart
  With mvrGWaySnelStart
    'Openen administratie
    .mtdGWayAdmiOpenen "C:\SnelStart\Administraties\", "SnelStart Voorbeeldbedrijf"
    'Uitlezen of Administratie in Euro's is
    mvrAdmiInEuro = .prpGWayAdmiInEuroGet
    Debug.Print "Adminitratie in Euro is " & mvrAdmiInEuro
    'Uitlezen omschrijving van Grootboek 9990
    mvrGbOmschrijving = .mtdGWayGrootboekOmschrijving(9990)
    Debug.Print "Omschrijving van GrootBoek 9990 is " & mvrGbOmschrijving
    'Journaalpost openen
    .mtdGWayJpAanmaken Now, 9990, "ABC", "OmschrijvingXYZ"
    'Regels aan JournaalPost toevoegen
    .mtdGWayJpRegelToevoegenV616 8001, "DebetZZZZ", 100, 0
    .mtdGWayJpRegelToevoegenV616 8002, "CreditYYYY", 0, 75
    .mtdGWayJpRegelToevoegenV616 8002, "CreditYYYY", 0, 25
    'Journaalpost sluiten
    .mtdGWayJpSluiten
    'Sluiten administratie
    .mtdGWayAdmiSluiten
    'Starten applicatie
    .mtdGWayRunSnelStart "C:\SnelStart\Administraties\SnelStart Voorbeeldbedrijf"
  End With
  Exit Sub
 ErrorHandler:
  mtdMsg mtdGWayFoutBoodschap("mtdToevoegenJournaalPost"), vbInformation
 End Sub


Private Sub mtdWijzigenJournaalPost()
  On Error GoTo ErrorHandler
  'Declaratie variabelen
  Dim mvrGWaySnelStart As clsGWaySnelStart
  Dim mvrGWayJournaalPost As clsGWayJournaalPost
  Dim mvrGWayJournaalRegel As clsGWayJournaalRegel
  'Aanmaken nieuwe ToegangsObject
  Set mvrGWaySnelStart = New clsGWaySnelStart
  With mvrGWaySnelStart
    'Openen administratie
    .mtdGWayAdmiOpenen "C:\SnelStart\Administraties\", "SnelStart Voorbeeldbedrijf"

    '-----------------------------------------------------
    'Voorbeeld 1 met verwijderen van een journaalpostregel
    '-----------------------------------------------------
    Set mvrGWayJournaalPost = .mtdGWayJournaalPostGet(502)
    'Wijzig het GrootboekNummer op vierde journaalpostregel
    mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(4).prpGWayGrootboekNummerSet = 3006
    'Voeg 4e en 5e journaalpostregel samen
    mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(4).prpGWayDebetSet = mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(4).prpGWayDebetGet +
                                                                         mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(5).prpGWayDebetGet
    mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(4).prpGWayCreditSet = mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(4).prpGWayCreditGet +
                                                                          mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(5).prpGWayCreditGet
    'Verwijder de 5e journaalpostregel
    mvrGWayJournaalPost.mtdJpRegelRemove 5
    'Voer wijziging door
    .mtdGWayJpWijzigen mvrGWayJournaalPost
    '---------------------------------------------------
    'Voorbeeld 2 met toevoegen van een journaalpostregel
    '---------------------------------------------------
    Set mvrGWayJournaalPost = .mtdGWayJournaalPostGet(503)
    'Wijzig totaalbedrag
    mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(1).prpGWayDebetSet = mvrGWayJournaalPost.mtdGetJournaalRegelOpIndex(1).prpGWayDebetGet + 100
    'Voeg extra regel toe
    Set mvrGWayJournaalRegel = mvrGWayJournaalPost.mtdJpRegelAdd
    'Vul de nieuwe regel
    mvrGWayJournaalRegel.prpGWayDebetSet = 0
    mvrGWayJournaalRegel.prpGWayCreditSet = 100
    mvrGWayJournaalRegel.prpGWayBtwIDSet = encGWayBtwIDGeen
    mvrGWayJournaalRegel.prpGWayBtwPercentageSet = 0
    mvrGWayJournaalRegel.prpGWayBtwTypeSet = encGWayBtwTypeNvt
    mvrGWayJournaalRegel.prpGWayFactuurNummerSet = "20050918"
    mvrGWayJournaalRegel.prpGWayFactuurRelatiecodeSet = 2
    mvrGWayJournaalRegel.prpGWayGrootboekNummerSet = 8095
    mvrGWayJournaalRegel.prpGWayOmschrijvingSet = "extra regel"
    'Voer wijziging door
    .mtdGWayJpWijzigen mvrGWayJournaalPost
    'Sluiten administratie
    .mtdGWayAdmiSluiten
    'Starten applicatie
    .mtdGWayRunSnelStart "C:\SnelStart\Administraties\SnelStart Voorbeeldbedrijf"
  End With
  Exit Sub
 ErrorHandler:
  mtdMsg mtdGWayFoutBoodschap("mtdWijzigenJournaalPost"), vbInformation
 End Sub


 Private Sub mtdInkoopOrderToevoegen()
  On Error GoTo ErrorHandler
  'Declaratie variabelen
  Dim mvrGWaySnelStart As clsGWaySnelStart
  Dim mvrGWayJournaalPost As clsGWayJournaalPost
  Dim mvrGWayJournaalRegel As clsGWayJournaalRegel
  'Aanmaken nieuwe ToegangsObject
  Set mvrGWaySnelStart = New clsGWaySnelStart
  With mvrGWaySnelStart
    'Openen administratie
    .mtdGWayAdmiOpenen "C:\SnelStart\Administraties\", "SnelStart Voorbeeldbedrijf"
    'Inkooporder aanmaken
    .mtdGWayInkoopOrderAanmaken 1, 30123, Now, "Inkoop via gateway", "memo-tekst", "bk inkoop"
    'Regels toevoegen
    .mtdGWayInkoopOrderRegelToevoegenArtikelMetDefaults 12, 100
    .mtdGWayInkoopOrderRegelToevoegenArtikel 12, "Eigen omschrijving", 50, 1.12, 10, 1
    .mtdGWayInkoopOrderRegelToevoegenTekst "Echte tekstregel", 0, 0
    .mtdGWayInkoopOrderRegelToevoegenTekst "Tekstregel met bedrag en kostenplaats", 10, 2
    'Voer wijziging door
    .mtdGWayInkoopOrderSluiten
    'Sluiten administratie
    .mtdGWayAdmiSluiten
    'Starten applicatie
    .mtdGWayRunSnelStart "C:\SnelStart\Administraties\SnelStart Voorbeeldbedrijf"
  End With
  Exit Sub
 ErrorHandler:
  mtdMsg mtdGWayFoutBoodschap("mtdWijzigenJournaalPost"), vbInformation
 End Sub


   Private Sub mtdVerkoopAbonnementOrderToevoegen()
  On Error GoTo ErrorHandler
  'Declaratie variabelen
  Dim mvrGWaySnelStart As clsGWaySnelStart
  Dim mvrSjabloonID As Long
  Dim mvrRelatieCode As Long
  Dim mvrDatum As Date
  Dim mvrOmschrijving As String
  Dim mvrOrderMemo As String
  Dim mvrKostenplaatsenNummer As Long
  Dim mvrBetalingsKenmerk As String
  Dim mvrVerkoperLoginNaam As String
  Dim mvrAbonnementIntervalSoort As enmGWayAbonnementInterval
  Dim mvrAbonnementIntervalAantal As Variant
  Dim mvrAbonnementTijdstipMaand As Variant
  Dim mvrAbonnementTijdstipWeekDag As Variant
  Dim mvrAbonnementTijdstipMaandDag As Variant
  Dim mvrAbonnementTijdstipWeekVanMaandDag As Variant
  Dim mvrAbonnementBegindatum As Variant
  Dim mvrAbonnementEindDatum As Variant
  Dim mvrAbonnementEindeNaAantal As Variant
  'Aanmaken nieuwe ToegangsObject
  Set mvrGWaySnelStart = New clsGWaySnelStart
  With mvrGWaySnelStart
    'Openen administratie
    .mtdGWayAdmiOpenen "C:\SnelStart\Administraties\", "SnelStart Voorbeeldbedrijf"
    'Verkooporder aanmaken
    mvrSjabloonID = 1
    mvrRelatieCode = 10001
    mvrDatum = Now
    mvrOrderMemo = "memo"
    mvrKostenplaatsenNummer = 0
    mvrBetalingsKenmerk = "BK"
    mvrOmschrijving = "De eerste maandag van elke 3 maand(en) abonnement"
    mvrAbonnementIntervalSoort = encGWayAbonnementIntervalDagInWeekPerMaand
    mvrAbonnementIntervalAantal = 3
    mvrAbonnementTijdstipMaand = Null
    mvrAbonnementTijdstipWeekDag = 2 'vbMonday
    mvrAbonnementTijdstipMaandDag = Null
    mvrAbonnementTijdstipWeekVanMaandDag = 1
    mvrAbonnementBegindatum = #7/1/2011#
    mvrAbonnementEindDatum = Null
    mvrAbonnementEindeNaAantal = 8
    .mtdGWayVerkoopOrderAanmakenE mvrSjabloonID, mvrRelatieCode, mvrDatum, mvrOmschrijving, mvrOrderMemo, mvrKostenplaatsenNummer, mvrBetalingsKenmerk, mvrVerkoperLoginNaam,
                                   mvrAbonnementIntervalSoort, mvrAbonnementIntervalAantal, mvrAbonnementTijdstipMaand, mvrAbonnementTijdstipWeekDag, mvrAbonnementTijdstipMaandDag, mvrAbonnementTijdstipWeekVanMaandDag,
                                   mvrAbonnementBegindatum, mvrAbonnementEindDatum, mvrAbonnementEindeNaAantal
    'Regel toevoegen
    parClsGWaySnelStart.mtdGWayVerkoopOrderRegelToevoegenArtikelB -1, "Vergoeding trainer", 1, 125, 0
    'Order sluiten
    .mtdGWayVerkoopOrderSluiten

    'Sluiten administratie
    .mtdGWayAdmiSluiten
    'Starten applicatie
    .mtdGWayRunSnelStart "C:\SnelStart\Administraties\SnelStart Voorbeeldbedrijf"
  End With
  Exit Sub
 ErrorHandler:
  mtdMsg mtdGWayFoutBoodschap("mtdWijzigenJournaalPost"), vbInformation
 End Sub
