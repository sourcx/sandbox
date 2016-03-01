static void login(clsGWaySnelStart snelstart, string user, string pass)
{
    clsGWaySnelStart snelstart = new clsGWaySnelStart();
    clsGWayLoginSettings settings = snelstart.prpGWayLoginSettingsGet;

    snelstart.Login(user, pass);

    if (snelstart.IsLoggedOn())
    {
        Console.WriteLine("Ingelogd. Denk ik.");
    }
    else
    {
        Console.WriteLine("Ingelogd. Denk niet dat het zo is.");
    }
}

static void openAdministration()
{
    clsGWaySnelStart snelstart = new clsGWaySnelStart();
    login(snelstart, "USER", "PASS");

    clsGWayLoginSettings loginSettingsFake = new clsGWayLoginSettings();
    loginSettingsFake.prpGWayDbAdministratieNaamSet = "Mijn administratie";
    loginSettingsFake.prpGWayDbTypeJetSet = false;
    loginSettingsFake.prpGWayDbJetFolderSet = "C:\\SnelStart\\Administratie";
    loginSettingsFake.prpGWayDbSqlServerNameSet = "(localdb)\\snelstart";
    loginSettingsFake.prpGWayDbSqlConnectTypeWindowsSet = true;

    Console.WriteLine("-- open administration");
    if (snelstart.mtdGWayAdmiOpenenViaLoginSettings(0, loginSettingsFake))
    {
        Console.WriteLine("Openen van administratie is gelukt.");
    }
    else
    {
        Console.WriteLine("Openen van administratie is NIET gelukt.");
    }
}