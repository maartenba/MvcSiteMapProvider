<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Web.Html.Models.MetaRobotsHelperModel,MvcSiteMapProvider]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Web.Html.Models" %>

<% if (!string.IsNullOrEmpty(Model.CurrentNode.MetaRobotsContent)) { %>
    <meta name="robots" content="<%=Model.CurrentNode.MetaRobotsContent%>" />
<% } %>