using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Globalization;
using Ninject;
using Ninject.Modules;

namespace DI.Ninject.Modules
{
    public class MvcSiteMapProviderModule
        : NinjectModule
    {
        public override void Load()
        {
            // TODO: Add Ninject MvcSiteMapProvider registration implementation here
            throw new NotImplementedException();
        }
    }
}
