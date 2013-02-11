using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Mvc.Filters
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
        private const string OriginalTitleKey = "F6409AA4-2870-407D-BC2A-31AC3A64F87A";

        /// <summary>
        /// Gets or sets the original title.
        /// </summary>
        /// <value>The original title.</value>
        protected string OriginalTitle
        {
            get
            {
                if (HttpContext.Current.Items[OriginalTitleKey] != null)
                {
                    return HttpContext.Current.Items[OriginalTitleKey].ToString();
                }
                return "";
            }
            set { HttpContext.Current.Items[OriginalTitleKey] = value; }
        }

        /// <summary>
        /// Property name of ViewData to look in
        /// </summary>
        protected readonly string PropertyName;

        /// <summary>
        /// Gets or sets the site map title target.
        /// </summary>
        /// <value>The site map title target.</value>
        [Obsolete("The SiteMapTitleTarget property is obsolete. Use the Target property instead.")]
        public SiteMapTitleTarget SiteMapTitleTarget
        {
            get { return (SiteMapTitleTarget)(int)Target; }
            set { Target = (AttributeTarget)(int)value; }
        }

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
                    var currentSiteMap = SiteMaps.Current;

                    if (currentSiteMap.CurrentNode != null)
                    {
                        if (Target == AttributeTarget.ParentNode && currentSiteMap.CurrentNode.ParentNode != null)
                        {
                            OriginalTitle = currentSiteMap.CurrentNode.ParentNode.Title;
                            currentSiteMap.CurrentNode.ParentNode.Title = target.ToString();
                            //((MvcSiteMapProvider.Core.SiteMap.SiteMapNode)currentSiteMap.CurrentNode.ParentNode).SetTitle(target.ToString());
                        }
                        else
                        {
                            OriginalTitle = currentSiteMap.CurrentNode.Title;
                            currentSiteMap.CurrentNode.Title = target.ToString();
                            //((MvcSiteMapProvider.Core.SiteMap.SiteMapNode)currentSiteMap.CurrentNode).SetTitle(target.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called by the MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                if (!string.IsNullOrEmpty(OriginalTitle))
                {
                    var currentSiteMap = SiteMaps.Current;
                    if (currentSiteMap.CurrentNode != null)
                    {
                        if (Target == AttributeTarget.ParentNode && currentSiteMap.CurrentNode.ParentNode != null)
                        {
                            currentSiteMap.CurrentNode.ParentNode.Title = OriginalTitle;
                            //((MvcSiteMapProvider.Core.SiteMap.SiteMapNode)currentSiteMap.CurrentNode.ParentNode).SetTitle(OriginalTitle);
                        }
                        else
                        {
                            currentSiteMap.CurrentNode.Title = OriginalTitle;
                            //((MvcSiteMapProvider.Core.SiteMap.SiteMapNode)currentSiteMap.CurrentNode).SetTitle(OriginalTitle);
                        }
                        OriginalTitle = "";
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
                var lambdaExpression = MvcSiteMapProvider.Core.Linq.DynamicExpression.ParseLambda(new[] { parameter }, null, "target." + expression);
                return lambdaExpression.Compile().DynamicInvoke(target);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}