using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for the fluent interface that builds a SiteMapNode fluently
    /// </summary>
    public interface IFluentSiteMapNodeBuilder
    {
        IFluentSiteMapNodeBuilder Items(Action<IFluentSiteMapNodeFactory> children);

        IList<IFluentSiteMapNodeBuilder> Children { get; }

        IFluentSiteMapNodeBuilder Attribute(string key, object value);

        IFluentSiteMapNodeBuilder Area(string value);

        IFluentSiteMapNodeBuilder Controller(string value);

        IFluentSiteMapNodeBuilder Action(string value);

        IFluentSiteMapNodeBuilder HttpMethod(HttpVerbs? method);

        IFluentSiteMapNodeBuilder Title(string value);

        IFluentSiteMapNodeBuilder Description(string value);

        IFluentSiteMapNodeBuilder Key(string value);

        IFluentSiteMapNodeBuilder Url(string value);

        IFluentSiteMapNodeBuilder Clickable(bool? clickable);

        IFluentSiteMapNodeBuilder Roles(string[] values);

        IFluentSiteMapNodeBuilder ResourceKey(string value);

        IFluentSiteMapNodeBuilder VisibilityProvider(string value);

        IFluentSiteMapNodeBuilder DynamicNodeProvider(string value);

        IFluentSiteMapNodeBuilder ImageUrl(string value);

        IFluentSiteMapNodeBuilder TargetFrame(string value);

        IFluentSiteMapNodeBuilder CachedResolvedUrl(bool? cacheResolvedUrl);

        IFluentSiteMapNodeBuilder CanonicalUrl(string value);

        IFluentSiteMapNodeBuilder CanonicalKey(string value);

        IFluentSiteMapNodeBuilder MetaRobotsValues(string[] values);

        IFluentSiteMapNodeBuilder ChangeFrequency(ChangeFrequency? value);

        IFluentSiteMapNodeBuilder UpdatePriority(UpdatePriority? value);

        IFluentSiteMapNodeBuilder LastModifiedDate(DateTime? value);

        IFluentSiteMapNodeBuilder Order(int order);

        IFluentSiteMapNodeBuilder Route(string value);

        IFluentSiteMapNodeBuilder RouteValues(object routeValues);

        IFluentSiteMapNodeBuilder PreservedRouteValues(string[] values);

        IFluentSiteMapNodeBuilder UrlResolver(string value);

        IFluentSiteMapNodeBuilder InheritedRouteParameters(string[] values);

        ISiteMapNodeToParentRelation CreateNode(ISiteMapNodeHelper helper, ISiteMapNode parentNode);
    }
}