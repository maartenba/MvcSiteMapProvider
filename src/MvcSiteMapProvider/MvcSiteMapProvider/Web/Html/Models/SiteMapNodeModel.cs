using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html.Models
{
    /// <summary>
    /// SiteMapNodeModel
    /// </summary>
    public class SiteMapNodeModel
    {
        private ISiteMapNode _node;
        private IDictionary<string, object> _sourceMetadata;
        private int _maxDepth;
        private bool _drillDownToCurrent;
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        public SiteMapNodeModel()
        {
            RouteValues = new Dictionary<string, object>();
            Attributes = new Dictionary<string, string>();
            //Children = new SiteMapNodeModelList();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        public SiteMapNodeModel(ISiteMapNode node, IDictionary<string, object> sourceMetadata) : this(node, sourceMetadata, Int32.MaxValue, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        public SiteMapNodeModel(ISiteMapNode node, IDictionary<string, object> sourceMetadata, int maxDepth, bool drillDownToCurrent)
        {
            _node = node;
            _sourceMetadata = sourceMetadata;
            _maxDepth = maxDepth;
            _drillDownToCurrent = drillDownToCurrent;
            Area = (node != null ? node.Area : "");
            Controller = (node != null ? node.Controller : "");
            Action = (node != null ? node.Action : "");
            Title = (node != null ? node.Title : "");
            Description = (node != null ? node.Description : "");
            TargetFrame = (node == null ? "" : node.TargetFrame);
            ImageUrl = (node == null ? "" : node.ImageUrl);
            Url = (node != null ? node.Url : "/");
            CanonicalUrl = (node != null ? node.CanonicalUrl : "");
            MetaRobotsContent = (node != null ? node.GetMetaRobotsContentString() : "");
            IsCurrentNode = (node != null ? node == node.SiteMap.CurrentNode : false);
            IsInCurrentPath = (node != null ? node.IsInCurrentPath() : true);
            IsRootNode = (node != null ? node == node.SiteMap.RootNode : false);
            IsClickable = (node == null || node.Clickable);
            RouteValues = (node != null ? (IDictionary<string, object>)node.RouteValues : new Dictionary<string, object>());
            Attributes = (node != null ? (IDictionary<string, string>)node.Attributes : new Dictionary<string, string>());
            SourceMetadata = sourceMetadata;
        }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public string Area { get; set; }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <value>The canonical URL.</value>
        public string CanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the content value of the meta robots tag.
        /// </summary>
        /// <value>The content value of the meta robots tag.</value>
        public string MetaRobotsContent { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the target frame.
        /// </summary>
        /// <value>The target frame.</value>
        public string TargetFrame { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current node; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentNode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in current path.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is in current path; otherwise, <c>false</c>.
        /// </value>
        public bool IsInCurrentPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is root node; otherwise, <c>false</c>.
        /// </value>
        public bool IsRootNode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clickable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is clickable; otherwise, <c>false</c>.
        /// </value>
        public bool IsClickable { get; set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the meta attributes.
        /// </summary>
        /// <value>The meta attributes.</value>
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the source metadata generated by the HtmlHelper.
        /// </summary>
        /// <value>The source metadata.</value>
        public IDictionary<string, object> SourceMetadata { get; set; }

        /// <summary>
        /// for storing the children
        /// </summary>
        private SiteMapNodeModelList _children;

        /// <summary>
        /// Gets the children.
        /// </summary>
        public SiteMapNodeModelList Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new SiteMapNodeModelList();
                    if (ReachedMaximalNodelevel(_maxDepth, _node, _drillDownToCurrent) && _node.HasChildNodes)
                    {
                        foreach (SiteMapNode child in _node.ChildNodes)
                        {
                            if (child.IsAccessibleToUser() && child.IsVisible(_sourceMetadata))
                                _children.Add(new SiteMapNodeModel(child, _sourceMetadata, _maxDepth - 1, _drillDownToCurrent));
                        }
                    }
                }
                return _children;
            }
        }

        /// <summary>
        /// Gets the descendants.
        /// </summary>
        public IEnumerable<SiteMapNodeModel> Descendants
        {
            get
            {
                return GetDescendants(this);
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        public SiteMapNodeModel Parent
        {
            get
            {
                return _node.ParentNode == null ? null : new SiteMapNodeModel(_node.ParentNode, _sourceMetadata, _maxDepth - 1, _drillDownToCurrent);
            }
        }

        /// <summary>
        /// Test if the maximal nodelevel has not been reached
        /// </summary>
        /// <param name="maxDepth">The normal max depth.</param>
        /// <param name="node">The starting node</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns></returns>
        private bool ReachedMaximalNodelevel(int maxDepth, ISiteMapNode node, bool drillDownToCurrent)
        {
            if (maxDepth > 0)
                return true;
            if (!drillDownToCurrent)
                return false;
            if (node.IsInCurrentPath())
                return true;
            if (node.ParentNode == node.SiteMap.CurrentNode)
                return true;
            foreach (ISiteMapNode sibling in node.ParentNode.ChildNodes)
            {
                if (sibling.IsInCurrentPath())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieve all descendant children    
        /// </summary>
        /// <param name="node">the node</param>
        /// <returns></returns>
        private IEnumerable<SiteMapNodeModel> GetDescendants(SiteMapNodeModel node)
        {
            foreach (var child in node.Children)
            {
                yield return child;
                foreach (var deeperchild in GetDescendants(child))
                {
                    yield return deeperchild;
                }
            }
        }
    }
}
