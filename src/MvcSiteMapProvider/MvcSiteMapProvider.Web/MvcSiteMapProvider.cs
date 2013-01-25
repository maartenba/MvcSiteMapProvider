// -----------------------------------------------------------------------
// <copyright file="MvcSiteMapProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using MvcSiteMapProvider.Core;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;
    

    /// <summary>
    /// MvcSiteMapProvider
    /// 
    /// This class plugs into the ASP.NET framework to supply a SiteMap tree.
    /// </summary>
    public class MvcSiteMapProvider 
        : StaticSiteMapProvider
    {
        private bool isInjected = false;
        //private ISiteMapSource siteMapSource = null;
        private ISiteMap siteMap = null;

        #region Base Overrides

        public override void Initialize(string name, NameValueCollection attributes)
        {
            Inject();
            base.Initialize(name, attributes);
        }

        public override SiteMapNode BuildSiteMap()
        {
            throw new NotImplementedException();
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return BuildSiteMap();
        }

        //public override SiteMapNode FindSiteMapNode(string rawUrl)
        //{
        //    throw new NotImplementedException();
        //}

        //public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public override SiteMapNode GetParentNode(SiteMapNode node)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Dependency Injection

        ///// <summary>
        ///// Gets or sets the site map source.
        ///// </summary>
        ///// <value>
        ///// The site map source.
        ///// </value>
        //public ISiteMapSource SiteMapSource
        //{
        //    get
        //    {
        //        return this.siteMapSource;
        //    }
        //    set
        //    {
        //        // Don't allow the value to be set to null
        //        if (value == null)
        //        {
        //            throw new ArgumentNullException("value");
        //        }
        //        // Don't allow the value to be set more than once
        //        if (this.siteMapSource != null)
        //        {
        //            throw new InvalidOperationException();
        //        }
        //        this.siteMapSource = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the site map source.
        /// </summary>
        /// <value>
        /// The site map source.
        /// </value>
        public ISiteMap SiteMap
        {
            get
            {
                return this.siteMap;
            }
            set
            {
                // Don't allow the value to be set to null
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                // Don't allow the value to be set more than once
                if (this.siteMap != null)
                {
                    throw new InvalidOperationException();
                }
                this.siteMap = value;
            }
        }



        /// <summary>
        /// Injects any dependencies using the DI container or
        /// poor man's DI.
        /// </summary>
        protected virtual void Inject()
        {
            if (!isInjected)
            {
                if (Core.IoC.DI.Container != null)
                {
                    Core.IoC.DI.Container.Inject(this);
                }
                else
                {
                    // TODO: load the dependencies using poor man's DI/ configuration
                }
            }
        }

        #endregion

    }
}
