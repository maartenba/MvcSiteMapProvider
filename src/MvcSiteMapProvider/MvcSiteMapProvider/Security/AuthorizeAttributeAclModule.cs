using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Reflection;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// AuthorizeAttributeAclModule class
    /// </summary>
    public class AuthorizeAttributeAclModule
        : IAclModule
    {
        public AuthorizeAttributeAclModule(
            IHttpContextFactory httpContextFactory,
            IControllerTypeResolver controllerTypeResolver,
            IObjectCopier objectCopier
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");
            if (objectCopier == null)
                throw new ArgumentNullException("objectCopier");

            this.httpContextFactory = httpContextFactory;
            this.controllerTypeResolver = controllerTypeResolver;
            this.objectCopier = objectCopier;
        }

        protected readonly IHttpContextFactory httpContextFactory;
        protected readonly IControllerTypeResolver controllerTypeResolver;
        protected readonly IObjectCopier objectCopier;

        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAccessibleToUser(ISiteMap siteMap, ISiteMapNode node)
        {
            // Is security trimming enabled?
            if (!siteMap.SecurityTrimmingEnabled)
            {
                return true;
            }

            var httpContext = httpContextFactory.Create();

            // Is it an external Url?
            if (node.HasExternalUrl(httpContext))
            {
                return true;
            }

            // Clickable? Always accessible.
            if (node.Clickable == false)
            {
                return true;
            }

            return this.VerifyNode(node, httpContext);
        }

        #endregion

        #region Protected Members

        protected virtual bool VerifyNode(ISiteMapNode node, HttpContextBase httpContext)
        {
            // Time to delve into the AuthorizeAttribute defined on the node.
            // Let's start by getting all metadata for the controller...
            var controllerType = controllerTypeResolver.ResolveControllerType(node.Area, node.Controller);
            if (controllerType == null)
                return true;

            var originalPath = httpContext.Request.Path;

            // Find routes for the sitemap node's url
            var routes = this.FindRoutesForNode(node, originalPath, httpContext);
            try
            {
                if (routes == null)
                    return true; // Static URL's will have no route data, therefore return true.

                return this.VerifyController(node, routes, controllerType);
            }
            finally
            {
                // Restore HttpContext
                httpContext.RewritePath(originalPath, true);
            }
        }

        protected virtual bool VerifyController(ISiteMapNode node, RouteData routes, Type controllerType)
        {
            // Get controller factory
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();

            // Create controller context
            bool factoryBuiltController = false;
            var controllerContext = this.CreateControllerContext(routes, controllerType, controllerFactory, out factoryBuiltController);
            try
            {
                return this.VerifyControllerAttributes(node, controllerType, controllerContext);
            }
            finally
            {
                // Release controller
                if (factoryBuiltController)
                    controllerFactory.ReleaseController(controllerContext.Controller);
            }
        }

        protected virtual RouteData FindRoutesForNode(ISiteMapNode node, string originalPath, HttpContextBase httpContext)
        {
            var originalRoutes = RouteTable.Routes.GetRouteData(httpContext);
            var nodeUrl = node.Url;
            httpContext.RewritePath(nodeUrl, true);

            RouteData routes = node.GetRouteData(httpContext);
            if (routes != null)
            {
                foreach (var routeValue in node.RouteValues)
                {
                    routes.Values[routeValue.Key] = routeValue.Value;
                }
                if (originalRoutes != null && (!routes.Route.Equals(originalRoutes.Route) || originalPath != nodeUrl || node.Area == String.Empty))
                {
                    routes.DataTokens.Remove("area");
                    //routes.DataTokens.Remove("Namespaces");
                    //routes.Values.Remove("area");
                }
            }
            return routes;
        }

        protected virtual bool VerifyControllerAttributes(ISiteMapNode node, Type controllerType, ControllerContext controllerContext)
        {
            // Get controller descriptor
            var controllerDescriptor = this.GetControllerDescriptor(controllerType);
            if (controllerDescriptor == null)
                return true;

            // Get action descriptor
            var actionDescriptor = this.GetActionDescriptor(node.Action, controllerDescriptor, controllerContext);
            if (actionDescriptor == null)
                return true;

            // fixes #130 - Check whether we have an AllowAnonymous Attribute
            var ignoreAuthorization = this.HasAllowAnonymousAttribute(actionDescriptor);
            if (ignoreAuthorization)
                return true;

            // Verify security
            var authorizeAttributes = this.GetAuthorizeAttributes(actionDescriptor, controllerContext);
            return this.VerifyAuthorizeAttributes(authorizeAttributes, controllerContext);
        }

        protected virtual bool VerifyAuthorizeAttributes(IEnumerable<AuthorizeAttribute> authorizeAttributes, ControllerContext controllerContext)
        {
            // Verify all attributes
            foreach (var authorizeAttribute in authorizeAttributes)
            {
                try
                {
                    var authorized = this.VerifyAuthorizeAttribute(authorizeAttribute, controllerContext);
                    if (!authorized)
                        return false;
                }
                catch
                {
                    // do not allow on exception
                    return false;
                }
            }
            return true;
        }



#if MVC2
        protected virtual IEnumerable<AuthorizeAttribute> GetAuthorizeAttributes(ActionDescriptor actionDescriptor, ControllerContext controllerContext)
        {
            return actionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true).OfType
                           <AuthorizeAttribute>().ToList()
                           .Union(
                               actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true).OfType
                                   <AuthorizeAttribute>().ToList());
        }
#else
        protected virtual IEnumerable<AuthorizeAttribute> GetAuthorizeAttributes(ActionDescriptor actionDescriptor, ControllerContext controllerContext)
        {
            IFilterProvider filterProvider = ResolveFilterProvider();
            IEnumerable<Filter> filters;

            // TODO: Inject the IFilterProvider rather than using DependencyResolver

            // If depencency resolver has an IFilterProvider registered, use it
            if (filterProvider != null)
            {
                filters = filterProvider.GetFilters(controllerContext, actionDescriptor);
            }
            // Otherwise use FilterProviders.Providers
            else
            {
                filters = FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor);
            }

            return filters
                    .Where(f => typeof(AuthorizeAttribute).IsAssignableFrom(f.Instance.GetType()))
                    .Select(f => f.Instance as AuthorizeAttribute);
        }


        protected virtual IFilterProvider ResolveFilterProvider()
        {
            var key = "__MVCSITEMAP_F255D59E-D3E4-4BA9-8A5F-2AF0CAB282F4";
            var requestCache = httpContextFactory.GetRequestCache();
            var filterProvider = requestCache.GetValue<IFilterProvider>(key);
            if (filterProvider == null)
            {
                filterProvider = DependencyResolver.Current.GetService<IFilterProvider>();
                requestCache.SetValue<IFilterProvider>(key, filterProvider);
            }
            return filterProvider;
        }
#endif

#if !MVC4
        protected virtual bool HasAllowAnonymousAttribute(ActionDescriptor actionDescriptor)
        {
            return false;
        }
#else
        protected virtual bool HasAllowAnonymousAttribute(ActionDescriptor actionDescriptor)
        {
            var allowAnonymousType = typeof(AllowAnonymousAttribute);
            return (actionDescriptor.IsDefined(allowAnonymousType, true) ||
                actionDescriptor.ControllerDescriptor.IsDefined(allowAnonymousType, true));
        }
#endif



        protected virtual bool VerifyAuthorizeAttribute(AuthorizeAttribute authorizeAttribute, ControllerContext controllerContext)
        {
            var currentAuthorizationAttributeType = authorizeAttribute.GetType();

            var builder = new AuthorizeAttributeBuilder();
            var subclassedAttribute =
                currentAuthorizationAttributeType == typeof(AuthorizeAttribute) ?
                   new InternalAuthorize(authorizeAttribute) : // No need to use Reflection.Emit when ASP.NET MVC built-in attribute is used
                   (IAuthorizeAttribute)builder.Build(currentAuthorizationAttributeType).Invoke(null);

            // Copy all properties
            objectCopier.Copy(authorizeAttribute, subclassedAttribute);

            return subclassedAttribute.IsAuthorized(controllerContext.HttpContext);
        }

        protected virtual ControllerDescriptor GetControllerDescriptor(Type controllerType)
        {
            ControllerDescriptor controllerDescriptor = null;
            if (typeof(IController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
            }
            else if (typeof(IAsyncController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedAsyncControllerDescriptor(controllerType);
            }
            return controllerDescriptor;
        }

        protected virtual ActionDescriptor GetActionDescriptor(string action, ControllerDescriptor controllerDescriptor, ControllerContext controllerContext)
        {
            ActionDescriptor actionDescriptor = null;
            var found = this.TryFindActionDescriptor(action, controllerContext, controllerDescriptor, out actionDescriptor);
            if (!found)
            {
                actionDescriptor = controllerDescriptor.GetCanonicalActions().Where(a => a.ActionName == action).FirstOrDefault();
            }
            return actionDescriptor;
        }

        protected virtual bool TryFindActionDescriptor(string action, ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, out ActionDescriptor actionDescriptor)
        {
            actionDescriptor = null;
            try
            {
                actionDescriptor = controllerDescriptor.FindAction(controllerContext, action);
                if (actionDescriptor != null)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        protected virtual ControllerContext CreateControllerContext(RouteData routes, Type controllerType, IControllerFactory controllerFactory, out bool factoryBuiltController)
        {
            var requestContext = this.httpContextFactory.CreateRequestContext(routes);
            ControllerBase controller = null;
            string controllerName = requestContext.RouteData.GetRequiredString("controller");

            // Whether controller is built by the ControllerFactory (or otherwise by Activator)
            factoryBuiltController = TryCreateController(requestContext, controllerName, controllerFactory, out controller);
            if (!factoryBuiltController)
            {
                TryCreateController(controllerType, out controller);
            }

            // Create controller context
            var controllerContext = httpContextFactory.CreateControllerContext(requestContext, controller);
            return controllerContext;
        }

        protected virtual bool TryCreateController(RequestContext requestContext, string controllerName, IControllerFactory controllerFactory, out ControllerBase controller)
        {
            controller = null;
            if (controllerFactory != null)
            {
                try
                {
                    controller = controllerFactory.CreateController(requestContext, controllerName) as ControllerBase;
                    if (controller != null)
                        return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        protected virtual bool TryCreateController(Type controllerType, out ControllerBase controller)
        {
            controller = null;
            try
            {
                controller = Activator.CreateInstance(controllerType) as ControllerBase;
                if (controller != null)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        #endregion

    }
}
