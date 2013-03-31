﻿using System;
using System.Web.Hosting;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Xml;
using DI;

public class MvcSiteMapProviderConfig
{
    public static void Register(IDependencyInjectionContainer container)
    {
        // Setup global sitemap loader
        MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

        // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
        var validator = container.Resolve<ISiteMapXmlValidator>();
        validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));
    }
}
