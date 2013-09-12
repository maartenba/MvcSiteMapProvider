# MvcSiteMapProvider - Readme

MvcSiteMapProvider is a tool targeted at ASP.NET MVC that provides menus, site maps, site map path functionality, and more. It provides
the ability to configure a hierarchical navigation structure using a pluggable architecture that can be XML, database, or code driven.
We have moved beyond a mere ASP.NET SiteMapProvider implementation to provide support for multi-tenant applications, flexible caching,
dependency injection, and several interface-based extensibility points where virtually any part of the provider can be replaced with a
custom implementation.

Based on areas, controller and action method names rather than hardcoded URL references, sitemap nodes are completely dynamic based on
the routing engine used in an application. Search Engine Optimization support is also provided in the form of dynamic sitemaps XML,
canonical URL tags, and meta robots tags to ensure you send the search engines consistent - rather than conflicting - information about
your URLs.

## What just happened?

You have installed the MvcSiteMapProvider.Web package. This package adds several files to your project:

* The MvcSiteMapProvider.Core assembly reference contains all logic.
* Under Views, we have created several helpers. They allow you to customize the appearance of how a node is rendered.
* In Web.config, we have added some configuration values. See https://github.com/maartenba/MvcSiteMapProvider/wiki/Configuring-MvcSiteMapProvider.
* The XML sitemap definition, Mvc.sitemap, can be found in the root of your project.

## Getting started

Assuming you have a HomeController with an Index action method, you can edit your _Layout.cshtml or master view and add the following
where you would like to render a breadcrumb:

```
@Html.MvcSiteMap().SiteMapPath()
```

After running your project, you should now see breadcrumbs on every page that is defined in the Mvc.sitemap file.

Congratulations! You are ready to go.

A full tutorial can be found at http://www.shiningtreasures.com/post/2013/08/07/MvcSiteMapProvider-40-a-test-drive.

## More info?

Check our wiki at https://github.com/maartenba/mvcsitemapprovider/wiki