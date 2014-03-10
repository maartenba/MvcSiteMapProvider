using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.Web.Administration;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Provides bindings from the IIS server, if running on IIS. 
    /// </summary>
    /// <remarks>
    /// To determine if IIS is used, we are using the SERVER_SOFTWARE server variable.
    /// Do note that it is possible to overwrite this value so prying eyes cannot see what, 
    /// web server you are running. If you overwrite the value so that it does not start with 
    /// Microsoft-IIS, this class will stop functioning.
    /// 
    /// This class should be configured as a singleton in the DI container, so 
    /// the settings are only retrieved at application start, rather than on every request.
    /// </remarks>
    public class BindingProvider
        : IBindingProvider
    {
        public BindingProvider(
            IBindingFactory bindingFactory,
            IMvcContextFactory mvcContextFactory
            )
        {
            if (bindingFactory == null)
                throw new ArgumentNullException("bindingFactory");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            this.bindingFactory = bindingFactory;
            this.mvcContextFactory = mvcContextFactory;
        }

        protected readonly IBindingFactory bindingFactory;
        protected readonly IMvcContextFactory mvcContextFactory;

        #region IBindingProvider Members

        public IEnumerable<IBinding> GetBindings()
        {
            if (this.Bindings == null)
                this.LoadBindings();

            return this.Bindings;
        }

        #endregion

        protected HttpContextBase HttpContext { get { return this.mvcContextFactory.CreateHttpContext(); } }

        protected virtual bool IsIISServer
        {
            get
            {
                var serverSoftware = this.HttpContext.Request.ServerVariables["SERVER_SOFTWARE"] as string;
                return string.IsNullOrEmpty(serverSoftware) ? false : serverSoftware.StartsWith("Microsoft-IIS/");
            }
        }

        protected IEnumerable<IBinding> Bindings { get; set; }

        /// <summary>
        /// Loads the IIS bindings for the current application, if running on IIS.
        /// </summary>
        protected virtual void LoadBindings()
        {
            IList<IBinding> result = new List<IBinding>();

            if (this.IsIISServer)
            {
                // Get the current Site Name
                var siteName = HostingEnvironment.SiteName;

                // Get the sites section from the AppPool.config
                var sitesSection = WebConfigurationManager.GetSection(null, null, "system.applicationHost/sites");

                var site = sitesSection.GetCollection().Where(x => string.Equals((string)x["name"], siteName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (site != null)
                {
                    foreach (var iisBinding in site.GetCollection("bindings"))
                    {
                        var protocol = iisBinding["protocol"] as string;
                        var bindingInfo = iisBinding["bindingInformation"] as string;

                        string[] parts = bindingInfo.Split(':');
                        if (parts.Length == 3)
                        {
                            string ip = parts[0]; // May be "*" or the actual IP
                            string port = parts[1]; // Always a port number (even if default port)
                            string hostHeader = parts[2]; // Optional - may be "". We can't rely on this entirely.

                            // Guess what the hostName will be depending on host header/IP address
                            var hostName = string.IsNullOrEmpty(hostHeader) ? ip : hostHeader;

                            result.Add(this.bindingFactory.Create(hostName, protocol, int.Parse(port)));
                        }
                    }
                }
            }

            this.Bindings = result;
        }
    }
}
