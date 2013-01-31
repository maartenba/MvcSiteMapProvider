<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Core.Web.Html.Models.SiteMapNodeModelList,MvcSiteMapProvider.Core]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Core.Web.Html.Models" %>

<ul>
<% foreach (var node in Model) { %>
    <li><%=Html.DisplayFor(m => node)%>
    <% if (node.Children.Any()) { %>
        <%=Html.DisplayFor(m => node.Children)%>
    <% } %>
    </li>
<% } %>
</ul>