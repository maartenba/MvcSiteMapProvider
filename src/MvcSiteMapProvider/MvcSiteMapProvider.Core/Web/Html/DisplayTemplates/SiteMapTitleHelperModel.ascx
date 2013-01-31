<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Core.Web.Html.Models.SiteMapTitleHelperModel,MvcSiteMapProvider.Core]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Core.Web.Html.Models" %>

<%=Model.CurrentNode.Title%>