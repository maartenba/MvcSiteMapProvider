<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Core.Web.Html.Models.SiteMapNodeModel,MvcSiteMapProvider.Core]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Core.Web.Html.Models" %>

<% if (Model.IsCurrentNode && Model.SourceMetadata["HtmlHelper"].ToString() != "MvcSiteMapProvider.Core.Web.Html.MenuHelper")  { %>
    <%=Model.Title %>
<% } else if (Model.IsClickable) { %>
    <a href="<%=Model.Url %>"><%=Model.Title %></a>
<% } else { %>
    <%=Model.Title %>
<% } %>