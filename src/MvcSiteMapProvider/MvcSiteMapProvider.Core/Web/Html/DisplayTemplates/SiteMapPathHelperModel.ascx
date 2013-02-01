<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Core.Web.Html.Models.SiteMapPathHelperModel,MvcSiteMapProvider.Core]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Core.Web.Html.Models" %>

<% foreach (var node in Model) { %>
    <%=Html.DisplayFor(m => node)%>
    <% if (node != Model.Last()) { %>
        &gt;
    <% } %>
<% } %>