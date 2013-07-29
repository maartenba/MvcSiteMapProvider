# MvcSiteMapProvider
MvcSiteMapProvider is a tool that provides flexible menus, breadcrumb trails, and SEO features for the ASP.NET MVC framework, similar to the ASP.NET SiteMapProvider model.

## Upgrading from v3 to v4
If you are upgrading from v3 to v4 see the [upgrade guide)[https://github.com/maartenba/MvcSiteMapProvider/wiki/Upgrading-from-v3-to-v4].

## What can it be used for?
MvcSiteMapProvider is a tool targeted at ASP.NET MVC that provides menus, site maps, site map path functionality, and more. It provides the ability to configure a hierarchical navigation structure using a pluggable architecture that can be XML, database, or code driven. We have moved beyond a mere ASP.NET SiteMapProvider implementation to provide support for multi-tenant applications, flexible caching, dependency injection, and several interface-based extensibility points where virtually any part of the provider can be replaced with a custom implementation.

Based on areas, controller and action method names rather than hardcoded URL references, sitemap nodes are completely dynamic based on the routing engine used in an application. Search Engine Optimization support is also provided in the form of dynamic sitemaps XML, canonical URL tags, and meta robots tags to ensure you send the search engines consistent - rather than conflicting - information about your URLs.

## Documentation
See the [documentation](https://github.com/maartenba/MvcSiteMapProvider/wiki)

## Get it on NuGet!

Stable releases:

    Install-Package MvcSiteMapProvider
	
Prefer continuous integration builds?

    Install-Package MvcSiteMapProvider -IncludePrerelease -Source http://www.myget.org/F/mvcsitemapprovider

## License
[MS-PL License](https://github.com/maartenba/MvcSiteMapProvider/blob/master/LICENSE.md)

## Building the source
After cloning the repository, run build.cmd.

MvcSiteMapProvider used the psake build engine to build the project. Psake is a Powershell based engine and if it is the first time you execute powershell scripts on your system you may need to allow script execution by running the following command as adminstrator:

    Set-ExecutionPolicy unrestricted

## Acknowledgements
The downloads page features an example application. The example code is all based on the excellent [ASP.NET MVC Music Store sample application](http://www.asp.net/mvc/videos/mvc-2/music-store/mvc-music-store-part-1-intro,-tools,-and-project-structure) by [Jon Galloway](http://weblogs.asp.net/jgalloway/).