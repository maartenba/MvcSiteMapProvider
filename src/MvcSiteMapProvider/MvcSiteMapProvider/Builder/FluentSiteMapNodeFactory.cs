using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Linq;

namespace MvcSiteMapProvider.Builder
{
    public class FluentSiteMapNodeFactory
        : IFluentSiteMapNodeFactory
    {
        private readonly IFluentFactory _fluentFactory;
        private readonly IList<IFluentSiteMapNodeBuilder> _result;
        private readonly ISiteMapNodeHelper _siteMapNodeHelper;

        public FluentSiteMapNodeFactory(IFluentFactory fluentFactory, IList<IFluentSiteMapNodeBuilder> result, ISiteMapNodeHelper siteMapNodeHelper)
        {
            if(fluentFactory == null)
                throw new ArgumentNullException("fluentFactory");
            if(result == null)
                throw new ArgumentNullException("result");
            if(siteMapNodeHelper == null)
                throw new ArgumentNullException("siteMapNodeHelper");

            _fluentFactory = fluentFactory;
            _result = result;
            _siteMapNodeHelper = siteMapNodeHelper;
        }

        #region IFluentSiteMapNodeFactory Members

        /// <summary>
        /// Start the build of a fluent SiteMapNode
        /// </summary>
        /// <returns></returns>
        public IFluentSiteMapNodeBuilder Add()
        {
            var builder = _fluentFactory.CreateSiteMapNodeBuilder(_siteMapNodeHelper);
            _result.Add(builder);
            return builder;
        }

        #endregion
    }
}
