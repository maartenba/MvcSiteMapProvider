using System;
namespace MvcSiteMapProvider
{
    interface IDefaultSiteMapProvider
    {
        //MvcSiteMapProvider.Extensibility.IAclModule AclModule { get; set; }
        //MvcSiteMapProvider.Extensibility.IActionMethodParameterResolver ActionMethodParameterResolver { get; set; }
        System.Web.SiteMapNode BuildSiteMap(); // From IStaticSiteMapProvider
        int CacheDuration { get; }
        //MvcSiteMapProvider.Extensibility.IControllerTypeResolver ControllerTypeResolver { get; set; }
        System.Web.SiteMapNode CurrentNode { get; } // From ISiteMapProvider
        System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context);
        System.Web.SiteMapNode FindSiteMapNode(System.Web.Mvc.ControllerContext context);
        void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes); // From ISiteMapProvider
        bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node); // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.INodeKeyGenerator NodeKeyGenerator { get; set; }
        void Refresh();
        System.Web.SiteMapNode RootNode { get; } // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeUrlResolver SiteMapNodeUrlResolver { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeVisibilityProvider SiteMapNodeVisibilityProvider { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapProviderEventHandler SiteMapProviderEventHandler { get; set; }
    }
}

