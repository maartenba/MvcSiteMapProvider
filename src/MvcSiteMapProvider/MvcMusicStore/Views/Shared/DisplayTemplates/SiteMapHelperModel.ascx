<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MvcSiteMapProvider.Core.Web.Html.Models.SiteMapHelperModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Core.Web.Html.Models" %>

<ul class="siteMap">
<% foreach (var node in Model.Nodes) { %>
    <li><%=Html.DisplayFor(m => node)%>
    <% if (node.Children.Any()) { %>
        <%=Html.DisplayFor(m => node.Children)%>
    <% } %>
    </li>
<% } %>
</ul>