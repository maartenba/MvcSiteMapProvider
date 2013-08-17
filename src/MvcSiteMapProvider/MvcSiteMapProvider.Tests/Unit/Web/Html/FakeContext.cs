using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    class FakeHttpContext 
        : HttpContextBase
    {
        private Dictionary<object, object> dictionary = new Dictionary<object, object>();
        public virtual System.Collections.IDictionary Item { get { return this.dictionary; } }
    }

    class FakeIView
        : IViewDataContainer
    {
        private ViewDataDictionary viewData = new ViewDataDictionary();
        public ViewDataDictionary ViewData
        {
            get { return this.viewData; }
            set { this.viewData = value; }
        }
    }
}
