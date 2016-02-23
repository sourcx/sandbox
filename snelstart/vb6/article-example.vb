'Met behulp van dit component kunnen Artikelgegevens worden opgehaald, gemuteerd en weer worden opgeslagen. Tevens kunnen nieuwe Artikelen worden toegevoegd.

'Voorbeeld tonen Artikelgegevens en lezen van volgende Artikel
Private Sub DcmdArtikelenTonen_Click()
  On Error GoTo ErrorHandler

    Dim mvrGWArtikel As clsGWArtikel
    Set mvrGWArtikel = New clsGWArtikel

    mvrGWArtikel.mtdGWayArtikelRead 10015
    List1.AddItem mvrGWArtikel.prpArtikelcodeGet & " - " & mvrGWArtikel.prpOmschrijvingGet
    mvrGWArtikel.mtdGWayArtikelReadNext
    List1.AddItem mvrGWArtikel.prpArtikelcodeGet & " - " & mvrGWArtikel.prpOmschrijvingGet

    Exit Sub

ErrorHandler:
  MsgBox mtdGWayFoutBoodschap("cmdArtikelenTonen_Click"), vbInformation
End Sub


'Voorbeeld toevoegen nieuwe Artikel.
Private Sub DcmdArtikelToevoegen_Click()
  On Error GoTo ErrorHandler

  Dim mvrGWArtikel As clsGWArtikel
  Dim mvrGWArtikelCode As Long

  Set mvrGWArtikel = New clsGWArtikel
  mvrGWArtikelCode = mvrGWArtikel.mtdArtikelcodeAutomatischBepalen

  If mvrGWArtikelCode = 0 Then
     mvrGWArtikelCode = 1
     mvrGWArtikel.prpArtikelcodeSet = 1
  End If

  mvrGWArtikel.prpOmschrijvingSet = "Artikel " & mvrGWArtikelCode
  mvrGWArtikel.prpInkoopprijsSet = 5
  mvrGWArtikel.prpVerkoopprijsSet = 10
  mvrGWArtikel.prpArtikelOmzetgroepIDSet = 1 ' Dit moet een bestaande artikelomzetgroep zijn
                                             ' anders kan het artikel niet worden opgeslagen.
  mvrGWArtikel.mtdGWayArtikelWrite

  Exit Sub

 ErrorHandler:
   MsgBox mtdGWayFoutBoodschap("cmdArtikelToevoegen_Click"), vbInformation
 End Sub
