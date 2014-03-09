<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Web.Html.Models.CanonicalHelperModel,MvcSiteMapProvider]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Web.Html.Models" %>

<% if (!string.IsNullOrEmpty(Model.CurrentNode.CanonicalUrl)) { %>
    <link rel="canonical" href="<%=Model.CurrentNode.CanonicalUrl%>" />
<% } %>