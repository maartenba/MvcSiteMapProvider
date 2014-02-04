using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// An ACL module that determines whether the current user has access to a given node based on the MVC AuthorizeAttribute.
    /// </summary>
    public class AuthorizeAttributeAclModule
        : IAclModule
    {
        public AuthorizeAttributeAclModule(
            IMvcContextFactory mvcContextFactory,
            IControllerDescriptorFactory controllerDescriptorFactory,
            IControllerBuilder controllerBuilder,
            IGlobalFilterProvider filterProvider
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (controllerDescriptorFactory == null)
                throw new ArgumentNullException("controllerDescriptorFactory");
            if (controllerBuilder == null)
                throw new ArgumentNullException("controllerBuilder");
            if (filterProvider == null)
                throw new ArgumentNullException("filterProvider");

            this.mvcContextFactory = mvcContextFactory;
            this.controllerDescriptorFactory = controllerDescriptorFactory;
            this.controllerBuilder = controllerBuilder;
            this.filterProvider = filterProvider;
        }

        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly IControllerDescriptorFactory controllerDescriptorFactory;
        protected readonly IControllerBuilder controllerBuilder;
        protected readonly IGlobalFilterProvider filterProvider;

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
            // Clickable? Always accessible.
            if (node.Clickable == false)
            {
                return true;
            }

            var httpContext = mvcContextFactory.CreateHttpContext();

            // Is it an external Url?
            if (node.HasExternalUrl(httpContext))
            {
                return true;
            }

            return this.VerifyNode(siteMap, node, httpContext);
        }

        #endregion

        #region Protected Members

        protected virtual bool VerifyNode(ISiteMap siteMap, ISiteMapNode node, HttpContextBase httpContext)
        {
            // Time to delve into the AuthorizeAttribute defined on the node.
            // Let's start by getting all metadata for the controller...
            var controllerType = siteMap.ResolveControllerType(node.Area, node.Controller);
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
            var controllerFactory = controllerBuilder.GetControllerFactory();

            // Create controller context
            bool factoryBuiltController = false;
            var controllerContext = this.CreateControllerContext(node, routes, controllerType, controllerFactory, out factoryBuiltController);
            try
            {
                // Fixes #271 - set controller's ControllerContext property for MVC
                controllerContext.Controller.ControllerContext = controllerContext;

                return this.VerifyControllerAttributes(node, controllerType, controllerContext);
            }
            finally
            {
                // Release the circular reference between Controller-ControllerContext, so it can be GC'd
                controllerContext.Controller.ControllerContext = null;

                // Release controller
                if (factoryBuiltController)
                    controllerFactory.ReleaseController(controllerContext.Controller);
            }
        }

        protected virtual RouteData FindRoutesForNode(ISiteMapNode node, string originalPath, HttpContextBase httpContext)
        {
            var routes = mvcContextFactory.GetRoutes();
            var originalRoutes = routes.GetRouteData(httpContext);
            var nodeUrl = node.Url;
            httpContext.RewritePath(nodeUrl, true);

            RouteData routeData = node.GetRouteData(httpContext);
            if (routeData != null)
            {
                foreach (var routeValue in node.RouteValues)
                {
                    routeData.Values[routeValue.Key] = routeValue.Value;
                }
                if (originalRoutes != null && (!routeData.Route.Equals(originalRoutes.Route) || originalPath != nodeUrl || node.Area == String.Empty))
                {
                    routeData.DataTokens.Remove("area");
                    //routeData.DataTokens.Remove("Namespaces");
                    //routeData.Values.Remove("area");
                }
            }
            return routeData;
        }

        protected virtual bool VerifyControllerAttributes(ISiteMapNode node, Type controllerType, ControllerContext controllerContext)
        {
            // Get controller descriptor
            var controllerDescriptor = controllerDescriptorFactory.Create(controllerType);
            if (controllerDescriptor == null)
                return true;

            // Get action descriptor
            var actionDescriptor = this.GetActionDescriptor(node.Action, controllerDescriptor, controllerContext);
            if (actionDescriptor == null)
                return true;

            // Verify security
            var authorizeAttributes = this.GetAuthorizeAttributes(actionDescriptor, controllerContext);
            return this.VerifyAuthorizeAttributes(authorizeAttributes, controllerContext, actionDescriptor);
        }

        protected virtual bool VerifyAuthorizeAttributes(IEnumerable<AuthorizeAttribute> authorizeAttributes, ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            // Verify all attributes
            foreach (var authorizeAttribute in authorizeAttributes)
            {
                try
                {
                    var authorized = this.VerifyAuthorizeAttribute(authorizeAttribute, controllerContext, actionDescriptor);
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
            var filters = filterProvider.GetFilters(controllerContext, actionDescriptor);

            return filters
                    .Where(f => typeof(AuthorizeAttribute).IsAssignableFrom(f.Instance.GetType()))
                    .Select(f => f.Instance as AuthorizeAttribute);
        }
#endif

        protected virtual bool VerifyAuthorizeAttribute(AuthorizeAttribute authorizeAttribute, ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var authorizationContext = this.mvcContextFactory.CreateAuthorizationContext(controllerContext, actionDescriptor);
            authorizeAttribute.OnAuthorization(authorizationContext);
            if (authorizationContext.Result != null)
                return false;
            return true;
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

        protected virtual ControllerContext CreateControllerContext(ISiteMapNode node, RouteData routes, Type controllerType, IControllerFactory controllerFactory, out bool factoryBuiltController)
        {
            var requestContext = this.mvcContextFactory.CreateRequestContext(node, routes);
            ControllerBase controller = null;
            string controllerName = requestContext.RouteData.GetRequiredString("controller");

            // Whether controller is built by the ControllerFactory (or otherwise by Activator)
            factoryBuiltController = TryCreateController(requestContext, controllerName, controllerFactory, out controller);
            if (!factoryBuiltController)
            {
                TryCreateController(controllerType, out controller);
            }

            // Create controller context
            var controllerContext = mvcContextFactory.CreateControllerContext(requestContext, controller);
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
