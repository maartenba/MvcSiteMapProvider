<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Web.Html.Models.SiteMapNodeModel,MvcSiteMapProvider]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Web.Html.Models" %>

<% if (Model.IsCurrentNode && Model.SourceMetadata["HtmlHelper"].ToString() != "MvcSiteMapProvider.Web.Html.MenuHelper")  { %>
    <%=Model.Title %>
<% } else if (Model.IsClickable) { %>
    <% if (string.IsNullOrEmpty(Model.Description)) { %>
        <a href="<%=Model.Url%>"><%=Model.Title %></a>
    <% } else { %>
        <a href="<%=Model.Url%>" title="<%=Model.Description%>"><%=Model.Title %></a>
    <% } %>
<% } else { %>
    <%=Model.Title %>
<% } %>