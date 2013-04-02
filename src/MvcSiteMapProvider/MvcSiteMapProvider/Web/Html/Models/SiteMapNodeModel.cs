using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html.Models
{
    /// <summary>
    /// SiteMapNodeModel
    /// </summary>
    public class SiteMapNodeModel
    {
        SiteMapNode _node;
        MvcSiteMapNode _mvcNode;
        IDictionary<string, object> _sourceMetadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        public SiteMapNodeModel()
        {
            RouteValues = new Dictionary<string, object>();
            MetaAttributes = new Dictionary<string, string>();
            Children = new SiteMapNodeModelList();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="mvcNode">The MVC node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        public SiteMapNodeModel(SiteMapNode node, MvcSiteMapNode mvcNode, IDictionary<string, object> sourceMetadata)
        {
            _node = node;
            _mvcNode = mvcNode;
            _sourceMetadata = sourceMetadata;
            var mvcNodeExist = mvcNode != null;
            Area = (mvcNodeExist ? mvcNode.Area : "");
            Controller = (mvcNodeExist ? mvcNode.Controller : "");
            Action = (mvcNodeExist ? mvcNode.Action : "");
            Title = node.Title;
            Description = node.Description;
            TargetFrame = (!mvcNodeExist ? "" : mvcNode.TargetFrame);
            Url = node.Url;
            IsCurrentNode = node == node.Provider.CurrentNode;
            IsInCurrentPath = node.IsInCurrentPath();
            IsRootNode = node == node.Provider.RootNode;
            IsClickable = (!mvcNodeExist || mvcNode.Clickable);
            RouteValues = (mvcNodeExist ? mvcNode.RouteValues : new Dictionary<string, object>());
            MetaAttributes = (mvcNodeExist ? mvcNode.MetaAttributes : new Dictionary<string, string>());
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
        public IDictionary<string, string> MetaAttributes { get; set; }

        /// <summary>
        /// Gets or sets the source metadata generated by the HtmlHelper.
        /// </summary>
        /// <value>The source metadata.</value>
        public IDictionary<string, object> SourceMetadata { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public SiteMapNodeModelList Children
        {
            get
            {
                var list = new SiteMapNodeModelList();
                if (_node.HasChildNodes)
                {
                    foreach (SiteMapNode child in _node.ChildNodes)
                    {
                        list.Add(new SiteMapNodeModel(child, child as MvcSiteMapNode, _sourceMetadata));
                    }
                }
                return list;
            }
        }
        
        /// <summary>
        /// Gets the parent
        /// </summary>
        public SiteMapNodeModel Parent
        {
            get
            {
                return _node.ParentNode == null ? null : new SiteMapNodeModel(_node.ParentNode, _node.ParentNode as MvcSiteMapNode, _sourceMetadata);
            }
        }
    }
}
