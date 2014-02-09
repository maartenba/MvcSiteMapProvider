using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// This is an example FluentSiteMapNodeProvider that is a mirror of the ~/Mvc.sitemap.
    /// </summary>
    public class StoreFluentSiteMapProvider : FluentSiteMapNodeProvider
    {
        public StoreFluentSiteMapProvider(IFluentFactory fluentFactory)
            : base(fluentFactory)
        {

        }

        public override void BuildSitemapNodes(IFluentSiteMapNodeFactory fluentSiteMapNodeFactory)
        {
            fluentSiteMapNodeFactory.Add()
                .Title("$resources:SiteMapLocalizations,HomeTitle")
                .Description("This is the home page")
                .Controller("Home")
                .Action("Index")
                .ChangeFrequency(ChangeFrequency.Always)
                .UpdatePriority(UpdatePriority.Normal)
                .LastModifiedDate(DateTime.Parse("2002-05-30T09:00:00"))
                .Items(homeChildren =>
                {
                    homeChildren.Add()
                        .Title("$resources:SiteMapLocalizations,BrowseGenresTitle")
                        .Controller("Store")
                        .Action("Index")
                        .Items(genresChildren => genresChildren.Add()
                            .Title("Browse")
                            .Action("Browse")
                            .DynamicNodeProvider("MvcMusicStore.Code.StoreBrowseDynamicNodeProvider, Mvc Music Store")
                            .PreservedRouteValues(new[] { "browse" })
                            .Items(browseChildren => browseChildren.Add()
                                .Title("Details")
                                .Action("Details")
                                .DynamicNodeProvider("MvcMusicStore.Code.StoreDetailsDynamicNodeProvider, Mvc Music Store")));
                    homeChildren.Add()
                        .Title("$resources:SiteMapLocalizations,ReviewCartTitle")
                        .Controller("ShoppingCart")
                        .Action("Index");
                    homeChildren.Add()
                        .Key("Checkout")
                        .Title("$resources:SiteMapLocalizations,CheckoutTitle")
                        .Controller("Checkout")
                        .Clickable(false);
                    homeChildren.Add()
                        .Title("$resources:SiteMapLocalizations,AccountTitle")
                        .Controller("Account")
                        .Clickable(false)
                        .Items(accountChildren =>
                        {
                            accountChildren.Add()
                                .Title("$resources:SiteMapLocalizations,LogOnTitle")
                                .Action("LogOn")
                                .VisibilityProvider("MvcMusicStore.Code.NonAuthenticatedVisibilityProvider, Mvc Music Store");
                            accountChildren.Add()
                                .Title("$resources:SiteMapLocalizations,LogOffTitle")
                                .Action("LogOff")
                                .VisibilityProvider("MvcMusicStore.Code.AuthenticatedVisibilityProvider, Mvc Music Store");
                            accountChildren.Add()
                                .Title("$resources:SiteMapLocalizations,RegisterTitle")
                                .Action("Register")
                                .VisibilityProvider("MvcMusicStore.Code.NonAuthenticatedVisibilityProvider, Mvc Music Store");
                            accountChildren.Add()
                                .Title("$resources:SiteMapLocalizations,ChangePasswordTitle")
                                .Action("ChangePassword")
                                .VisibilityProvider("MvcMusicStore.Code.AuthenticatedVisibilityProvider, Mvc Music Store");
                        });
                    homeChildren.Add()
                        .Title("$resources:SiteMapLocalizations,AdministrationTitle")
                        .Area("Admin")
                        .Controller("Home")
                        .Action("Index")
                        .Attribute("visibility", "SiteMapPathHelper,!*")
                        .VisibilityProvider("MvcSiteMapProvider.FilteredSiteMapNodeVisibilityProvider, MvcSiteMapProvider")
                        .Items(administrationChildren => administrationChildren.Add()
                            .Title("$resources:SiteMapLocalizations,StoreManagerTitle")
                            .Area("Admin")
                            .Controller("StoreManager")
                            .Action("Index")
                            .Items(storeManagerChildren =>
                            {
                                storeManagerChildren.Add()
                                    .Title("$resources:SiteMapLocalizations,CreateAlbumTitle")
                                    .Action("Create");
                                storeManagerChildren.Add()
                                    .Title("$resources:SiteMapLocalizations,EditAlbumTitle")
                                    .Action("Edit");
                                storeManagerChildren.Add()
                                    .Title("$resources:SiteMapLocalizations,DeleteAlbumTitle")
                                    .Action("Delete");
                            }));
                    homeChildren.Add()
                        .Title("$resources:SiteMapLocalizations,SitemapTitle")
                        .Action("SiteMap")
                        .UrlResolver("MvcMusicStore.Code.UpperCaseSiteMapNodeUrlResolver, Mvc Music Store");
                });
        }

        public override bool UseNestedDynamicNodeRecursion
        {
            get { return true; }
        }
    }
}