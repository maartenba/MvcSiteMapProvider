using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Web.Mvc.Filters
{
    /// <summary>
    /// SiteMapTitle attribute
    /// Can be used for setting sitemap title on an action method.
    /// Credits go to Kenny Eliasson - http://mvcsitemap.codeplex.com/Thread/View.aspx?ThreadId=67056
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SiteMapTitleAttribute
        : ActionFilterAttribute
    {
        /// <summary>
        /// Property name of ViewData to look in
        /// </summary>
        protected readonly string PropertyName;

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public AttributeTarget Target { get; set; }

        /// <summary>
        /// Creates a new SiteMapTitleAttribute instance.
        /// </summary>
        /// <param name="propertyName">Property in ViewData used as the SiteMap.CurrentNode.Title</param>
        public SiteMapTitleAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
            this.Target = AttributeTarget.CurrentNode;
        }

        /// <summary>
        /// Called by the MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult result = filterContext.Result as ViewResult;
            if (result != null)
            {
                var target = (ResolveTarget(result.ViewData.Model, PropertyName) ??
                              ResolveTarget(result.ViewData, PropertyName)) ??
                             result.ViewData[PropertyName];

                if (target != null)
                {
                    var siteMap = SiteMaps.Current;

                    if (siteMap.CurrentNode != null)
                    {
                        if (Target == AttributeTarget.ParentNode && siteMap.CurrentNode.ParentNode != null)
                        {
                            siteMap.CurrentNode.ParentNode.Title = target.ToString();
                        }
                        else
                        {
                            siteMap.CurrentNode.Title = target.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve target
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="expression">Target expression</param>
        /// <returns>
        /// A target represented as a <see cref="object"/> instance 
        /// </returns>
        internal static object ResolveTarget(object target, string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return target;
            }

            try
            {
                var parameter = Expression.Parameter(target.GetType(), "target");
                var lambdaExpression = MvcSiteMapProvider.Linq.DynamicExpression.ParseLambda(new[] { parameter }, null, "target." + expression);
                return lambdaExpression.Compile().DynamicInvoke(target);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}