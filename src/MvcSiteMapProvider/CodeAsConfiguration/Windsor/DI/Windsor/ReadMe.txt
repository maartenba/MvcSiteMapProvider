Integrating MvcSiteMapProvider with Castle Windsor

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

    // Create the DI container
    var container = new WindsorContainer();

    // Setup configuration of DI
    container.Install(new MvcSiteMapProviderInstaller());

For more help consult the Castle Windsor documantation at
http://docs.castleproject.org/Default.aspx?Page=Installers&NS=Windsor&AspxAutoDetectCookieSupport=1