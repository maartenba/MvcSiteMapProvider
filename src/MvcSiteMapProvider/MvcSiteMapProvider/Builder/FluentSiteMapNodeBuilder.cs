using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Builder
{
    public class FluentSiteMapNodeBuilder
        : IFluentSiteMapNodeBuilder
    {
        private readonly IFluentFactory _fluentFactory;
        private readonly ISiteMapNodeHelper _siteMapNodeHelper;
        private readonly IList<IFluentSiteMapNodeBuilder> _children = new List<IFluentSiteMapNodeBuilder>();
        private readonly IDictionary<string, object> _additionalAttributes = new Dictionary<string, object>(); 
        private string _area;
        private string _controller;
        private string _action;
        private string _httpMethod;
        private string _title;
        private string _description;
        private string _key;
        private string _url;
        private bool? _clickable;
        private string[] _roles;
        private string _resourceKey;
        private string _visibilityProvider;
        private string _dynamicNodeProvider;
        private string _imageUrl;
        private string _targetFrame;
        private bool? _cacheResolvedUrl;
        private string _canonicalUrl;
        private string _canonicalKey;
        private string[] _metaRobotsValues;
        private ChangeFrequency? _changeFrequency;
        private UpdatePriority? _updatePriority;
        private DateTime? _lastModifiedDate;
        private int? _order;
        private string _route;
        private object _routeValues;
        private string[] _preservedRouteParameters;
        private string _urlResolver;
        private string[] _inheritedRouteParameters;

        public FluentSiteMapNodeBuilder(IFluentFactory fluentFactory, ISiteMapNodeHelper siteMapNodeHelper)
        {
            if (siteMapNodeHelper == null)
                throw new ArgumentNullException("siteMapNodeHelper");
            if (fluentFactory == null)
                throw new ArgumentNullException("fluentFactory");

            _siteMapNodeHelper = siteMapNodeHelper;
            _fluentFactory = fluentFactory;
        }

        public IFluentSiteMapNodeBuilder Items(Action<IFluentSiteMapNodeFactory> children)
        {
            if (children == null)
                return this;

            var factory = _fluentFactory.CreateSiteMapNodeFactory(_children, _siteMapNodeHelper);
            children(factory);

            return this;
        }

        public IList<IFluentSiteMapNodeBuilder> Children { get { return _children; } }

        public IFluentSiteMapNodeBuilder Attribute(string key, object value)
        {
            if (_additionalAttributes.ContainsKey(key))
                _additionalAttributes[key] = value;
            else
                _additionalAttributes.Add(key, value);
            return this;
        }

        public IFluentSiteMapNodeBuilder Area(string value)
        {
            _area = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Controller(string value)
        {
            _controller = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Action(string value)
        {
            _action = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder HttpMethod(HttpVerbs? method)
        {
            _httpMethod = method.HasValue ? method.ToString().ToUpperInvariant() : null;
            return this;
        }

        public IFluentSiteMapNodeBuilder Title(string value)
        {
            _title = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Description(string value)
        {
            _description = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Key(string value)
        {
            _key = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Url(string value)
        {
            _url = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Clickable(bool? clickable)
        {
            _clickable = clickable;
            return this;
        }

        public IFluentSiteMapNodeBuilder Roles(string[] values)
        {
            _roles = values != null ? values.Where(x => !string.IsNullOrEmpty(x)).ToArray() : null;
            return this;
        }

        public IFluentSiteMapNodeBuilder ResourceKey(string value)
        {
            _resourceKey = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder VisibilityProvider(string value)
        {
            _visibilityProvider = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder DynamicNodeProvider(string value)
        {
            _dynamicNodeProvider = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder ImageUrl(string value)
        {
            _imageUrl = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder TargetFrame(string value)
        {
            _targetFrame = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder CachedResolvedUrl(bool? cacheResolvedUrl)
        {
            _cacheResolvedUrl = cacheResolvedUrl;
            return this;
        }

        public IFluentSiteMapNodeBuilder CanonicalUrl(string value)
        {
            _canonicalUrl = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder CanonicalKey(string value)
        {
            _canonicalKey = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder MetaRobotsValues(string[] values)
        {
            _metaRobotsValues = values != null ? values.Where(x => !string.IsNullOrEmpty(x)).ToArray() : null;
            return this;
        }

        public IFluentSiteMapNodeBuilder ChangeFrequency(ChangeFrequency? value)
        {
            _changeFrequency = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder UpdatePriority(UpdatePriority? value)
        {
            _updatePriority = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder LastModifiedDate(DateTime? value)
        {
            _lastModifiedDate = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder Order(int order)
        {
            _order = order;
            return this;
        }

        public IFluentSiteMapNodeBuilder Route(string value)
        {
            _route = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder RouteValues(object routeValues)
        {
            _routeValues = routeValues;
            return this;
        }
        public IFluentSiteMapNodeBuilder PreservedRouteValues(string[] values)
        {
            _preservedRouteParameters = values != null ? values.Where(x => !string.IsNullOrEmpty(x)).ToArray() : null;
            return this;
        }

        public IFluentSiteMapNodeBuilder UrlResolver(string value)
        {
            _urlResolver = value;
            return this;
        }

        public IFluentSiteMapNodeBuilder InheritedRouteParameters(string[] values)
        {
            _inheritedRouteParameters = values != null ? values.Where(x => !string.IsNullOrEmpty(x)).ToArray() : null;
            return this;
        }

        public string CreateNodeKey(ISiteMapNodeHelper helper, string parentKey)
        {
            return helper.CreateNodeKey(
                parentKey,
                _key ?? string.Empty,
                _url ?? string.Empty,
                _title ?? string.Empty,
                _area ?? string.Empty,
                _controller ?? string.Empty,
                _action ?? string.Empty,
                _httpMethod ?? "GET",
                !_clickable.HasValue || _clickable.Value);
        }

        public ISiteMapNodeToParentRelation CreateNode(ISiteMapNodeHelper helper, ISiteMapNode parentNode)
        {
            var parentKey = parentNode == null ? "" : parentNode.Key;
            var key = CreateNodeKey(helper, parentKey);

            var nodeParentMap = helper.CreateNode(key, parentKey, "Fluent", _resourceKey ?? string.Empty);
            var siteMapNode = nodeParentMap.Node;

            AddAttributesToNode(siteMapNode);
            if (_routeValues != null)
                foreach (var routeValue in new System.Web.Routing.RouteValueDictionary(_routeValues))
                    siteMapNode.RouteValues.Add(routeValue);
            if (_preservedRouteParameters != null)
                foreach (var preservedRouteParameter in _preservedRouteParameters)
                    siteMapNode.PreservedRouteParameters.Add(preservedRouteParameter);
            if (_metaRobotsValues != null)
                foreach (var metaRobotValue in _metaRobotsValues)
                    siteMapNode.MetaRobotsValues.Add(metaRobotValue);
            if (_roles != null)
                foreach (var role in _roles)
                    siteMapNode.Roles.Add(role);

            siteMapNode.Title = _title ?? string.Empty;
            siteMapNode.Description = string.IsNullOrEmpty(_description) ? siteMapNode.Title : _description;

            siteMapNode.Route = _route ?? string.Empty;
            if (!string.IsNullOrEmpty(_area)) siteMapNode.RouteValues.Add("area", _area);
            if (!string.IsNullOrEmpty(_controller)) siteMapNode.RouteValues.Add("controller", _controller);
            if (!string.IsNullOrEmpty(_action)) siteMapNode.RouteValues.Add("action", _action);

            siteMapNode.Clickable = !_clickable.HasValue || _clickable.Value;
            siteMapNode.VisibilityProvider = _visibilityProvider ?? string.Empty;
            siteMapNode.DynamicNodeProvider = _dynamicNodeProvider ?? string.Empty;
            siteMapNode.ImageUrl = _imageUrl ?? string.Empty;
            siteMapNode.TargetFrame = _targetFrame ?? string.Empty;
            siteMapNode.HttpMethod = _httpMethod ?? "GET";
            siteMapNode.Url = _url ?? string.Empty;
            siteMapNode.CacheResolvedUrl = !_cacheResolvedUrl.HasValue || _cacheResolvedUrl.Value;
            siteMapNode.CanonicalUrl = _canonicalUrl ?? string.Empty;
            siteMapNode.CanonicalKey = _canonicalKey ?? string.Empty;
            siteMapNode.ChangeFrequency = _changeFrequency.HasValue ? _changeFrequency.Value : MvcSiteMapProvider.ChangeFrequency.Undefined;
            siteMapNode.UpdatePriority = _updatePriority.HasValue ? _updatePriority.Value : MvcSiteMapProvider.UpdatePriority.Undefined;
            siteMapNode.LastModifiedDate = _lastModifiedDate.HasValue ? _lastModifiedDate.Value : DateTime.MinValue;
            siteMapNode.Order = _order.HasValue ? _order.Value : 0;
            siteMapNode.UrlResolver = _urlResolver ?? string.Empty;

            if (_inheritedRouteParameters != null && parentNode != null)
            {
                foreach (var inheritedRouteParameter in _inheritedRouteParameters)
                {
                    var item = inheritedRouteParameter.Trim();
                    if (siteMapNode.RouteValues.ContainsKey(item))
                        throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeSameKeyInRouteValueAndInheritedRouteParameter, key, _title ?? string.Empty, item));
                    if (parentNode.RouteValues.ContainsKey(item))
                        siteMapNode.RouteValues.Add(item, parentNode.RouteValues[item]);
                }
            }

            if (parentNode != null)
            {
                if (string.IsNullOrEmpty(_area) && !siteMapNode.RouteValues.ContainsKey("area"))
                {
                    siteMapNode.Area = parentNode.Area;
                }
                if (string.IsNullOrEmpty(_controller) && !siteMapNode.RouteValues.ContainsKey("controller"))
                {
                    siteMapNode.Controller = parentNode.Controller;
                }
            }
            if (!siteMapNode.RouteValues.ContainsKey("area"))
            {
                siteMapNode.RouteValues.Add("area", "");
            }

            return nodeParentMap;
        }

        private void AddAttributesToNode(ISiteMapNode node)
        {
            if (_area != null)
                node.Attributes.Add(new KeyValuePair<string, object>("area", _area));

            if (_controller != null)
                node.Attributes.Add(new KeyValuePair<string, object>("controller", _controller));

            if (_action != null)
                node.Attributes.Add(new KeyValuePair<string, object>("action", _action));

            if (_httpMethod != null)
                node.Attributes.Add(new KeyValuePair<string, object>("httpMethod", _httpMethod));

            if (_key != null)
                node.Attributes.Add(new KeyValuePair<string, object>("key", _key));

            if (_url != null)
                node.Attributes.Add(new KeyValuePair<string, object>("url", _url));

            if (_clickable.HasValue)
                node.Attributes.Add(new KeyValuePair<string, object>("clickable", _clickable.Value.ToString().ToLowerInvariant()));

            if (_roles != null)
                node.Attributes.Add(new KeyValuePair<string, object>("roles", string.Join(",", _roles)));

            if (_resourceKey != null)
                node.Attributes.Add(new KeyValuePair<string, object>("resourceKey", _resourceKey));

            if (_visibilityProvider != null)
                node.Attributes.Add(new KeyValuePair<string, object>("visibilityProvider", _visibilityProvider));

            if (_dynamicNodeProvider != null)
                node.Attributes.Add(new KeyValuePair<string, object>("dynamicNodeProvider", _dynamicNodeProvider));

            if (_imageUrl != null)
                node.Attributes.Add(new KeyValuePair<string, object>("imageUrl", _imageUrl));

            if (_targetFrame != null)
                node.Attributes.Add(new KeyValuePair<string, object>("targetFrame", _targetFrame));

            if (_cacheResolvedUrl.HasValue)
                node.Attributes.Add(new KeyValuePair<string, object>("cacheResolvedUrl", _cacheResolvedUrl.Value.ToString().ToLowerInvariant()));

            if (_canonicalUrl != null)
                node.Attributes.Add(new KeyValuePair<string, object>("canonicalUrl", _canonicalUrl));

            if (_canonicalKey != null)
                node.Attributes.Add(new KeyValuePair<string, object>("canonicalKey", _canonicalKey));

            if (_metaRobotsValues != null)
                node.Attributes.Add(new KeyValuePair<string, object>("metaRobotsValues", string.Join(" ", _metaRobotsValues)));

            if (_changeFrequency != null)
                node.Attributes.Add(new KeyValuePair<string, object>("changeFrequency", _changeFrequency));

            if (_updatePriority.HasValue)
                node.Attributes.Add(new KeyValuePair<string, object>("updatePriority", _updatePriority.Value.ToString()));

            if (_lastModifiedDate.HasValue)
                node.Attributes.Add(new KeyValuePair<string, object>("lastModifiedDate", _lastModifiedDate.Value.ToString()));

            if (_order.HasValue)
                node.Attributes.Add(new KeyValuePair<string, object>("order", _order.Value.ToString()));

            if (_route != null)
                node.Attributes.Add(new KeyValuePair<string, object>("route", _route));

            if (_preservedRouteParameters != null)
                node.Attributes.Add(new KeyValuePair<string, object>("preservedRouteParameters", string.Join(",", _preservedRouteParameters)));

            if (_urlResolver != null)
                node.Attributes.Add(new KeyValuePair<string, object>("urlResolver", _urlResolver));

            if (_inheritedRouteParameters != null)
                node.Attributes.Add(new KeyValuePair<string, object>("inheritedRouteParameters", string.Join(",", _inheritedRouteParameters)));

            foreach (var additionalAttribute in _additionalAttributes)
            {
                if(node.Attributes.ContainsKey(additionalAttribute.Key))
                    throw new InvalidOperationException(string.Format("An attempt was made to set attribute {0} manually, but it is already defined internally.", additionalAttribute.Key));
                node.Attributes.Add(additionalAttribute);
            }
        }
    }
}
