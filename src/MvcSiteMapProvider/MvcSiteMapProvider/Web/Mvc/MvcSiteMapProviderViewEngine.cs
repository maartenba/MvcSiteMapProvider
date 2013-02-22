using System;
using System.Security;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// MvcSiteMapProviderViewEngine class
    /// </summary>
    internal class MvcSiteMapProviderViewEngine
        : VirtualPathProviderViewEngine
    {
        /// <summary>
        /// Initializes the <see cref="MvcSiteMapProviderViewEngine"/> class.
        /// </summary>
        static MvcSiteMapProviderViewEngine()
        {
            ViewEngines.Engines.Insert(ViewEngines.Engines.Count, new MvcSiteMapProviderViewEngine());

            try
            {
                MvcSiteMapProviderViewEngineVirtualPathProvider pathProvider = new MvcSiteMapProviderViewEngineVirtualPathProvider();
                HostingEnvironment.RegisterVirtualPathProvider(pathProvider);
            }
            catch (SecurityException)
            {
                // Partial trust... No support for this!
            }
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        internal static void Register()
        {
            // just a dummy to make sure the static constructor is called
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcSiteMapProviderViewEngine"/> class.
        /// </summary>
        public MvcSiteMapProviderViewEngine()
        {
            base.ViewLocationFormats = new string[] { "~/__MVCSITEMAPPROVIDER/{0}.ascx" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            base.AreaPartialViewLocationFormats = base.ViewLocationFormats;
            base.MasterLocationFormats = base.ViewLocationFormats;
            base.AreaMasterLocationFormats = base.ViewLocationFormats;
        }

        /// <summary>
        /// Creates the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialPath">The partial path for the new partial view.</param>
        /// <returns>A reference to the partial view.</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
#if MVC2
            return (WebFormView)Activator.CreateInstance(typeof(WebFormView), partialPath, null);
#else
            return new WebFormView(controllerContext, partialPath, null);
#endif
        }

        /// <summary>
        /// Creates the specified view by using the controller context, path of the view, and path of the master view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The path of the view.</param>
        /// <param name="masterPath">The path of the master view.</param>
        /// <returns>A reference to the view.</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
#if MVC2
            return (WebFormView)Activator.CreateInstance(typeof(WebFormView), viewPath, masterPath);
#else
            return new WebFormView(controllerContext, viewPath, masterPath);
#endif
        }
    }
}
