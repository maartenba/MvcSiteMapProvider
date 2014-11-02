[![MvcSiteMapProvider Build Status](https://www.myget.org/BuildSource/Badge/mvcsitemapprovider?identifier=7a569f3b-9a67-4cd3-b702-daf7bf061ffb)](https://www.myget.org/gallery/mvcsitemapprovider)

# MvcSiteMapProvider
MvcSiteMapProvider is a tool that provides flexible menus, breadcrumb trails, and SEO features for the ASP.NET MVC framework, similar to the ASP.NET SiteMapProvider model.

## Upgrading from v3 to v4
If you are upgrading from v3 to v4 see the [upgrade guide](https://github.com/maartenba/MvcSiteMapProvider/wiki/Upgrading-from-v3-to-v4).

## What can it be used for?
MvcSiteMapProvider is a tool targeted at ASP.NET MVC that provides menus, site maps, site map path functionality, and more. It provides the ability to configure a hierarchical navigation structure using a pluggable architecture that can be XML, database, or code driven. We have moved beyond a mere ASP.NET SiteMapProvider implementation to provide support for multi-tenant applications, flexible caching, dependency injection, and several interface-based extensibility points where virtually any part of the provider can be replaced with a custom implementation.

Based on areas, controller and action method names rather than hardcoded URL references, sitemap nodes are completely dynamic based on the routing engine used in an application. Search Engine Optimization support is also provided in the form of dynamic sitemaps XML, canonical URL tags, and meta robots tags to ensure you send the search engines consistent - rather than conflicting - information about your URLs.

## Documentation
See the [documentation](https://github.com/maartenba/MvcSiteMapProvider/wiki)

## Get it on NuGet!

Stable releases:

    Install-Package MvcSiteMapProvider.MVC5
                   - or -
    Install-Package MvcSiteMapProvider.MVC4
                   - or -
    Install-Package MvcSiteMapProvider.MVC3
                   - or -
    Install-Package MvcSiteMapProvider.MVC2
                   - or -
Use one of our [dependency injection packages] (http://www.nuget.org/packages?q=mvcsitemapprovider.mvc*.di&prerelease=&sortOrder=package-download-count).

Prefer continuous integration builds?

    Install-Package MvcSiteMapProvider.MVC5 -IncludePrerelease -Source http://www.myget.org/F/mvcsitemapprovider
                   - or -
    Install-Package MvcSiteMapProvider.MVC4 -IncludePrerelease -Source http://www.myget.org/F/mvcsitemapprovider
                   - or -
    Install-Package MvcSiteMapProvider.MVC3 -IncludePrerelease -Source http://www.myget.org/F/mvcsitemapprovider
                   - or -
    Install-Package MvcSiteMapProvider.MVC2 -IncludePrerelease -Source http://www.myget.org/F/mvcsitemapprovider

## License
[MS-PL License](https://github.com/maartenba/MvcSiteMapProvider/blob/master/LICENSE.md)

## Building the source
After cloning the repository, run build.cmd.

MvcSiteMapProvider used the psake build engine to build the project. Psake is a Powershell based engine and if it is the first time you execute powershell scripts on your system you may need to allow script execution by running the following command as adminstrator:

    Set-ExecutionPolicy RemoteSigned

## Contributions

Please read our [Contributing to MvcSiteMapProvider](CONTRIBUTING.md) guide.

## Acknowledgements

The downloads page features an example application. The example code is all based on the excellent [ASP.NET MVC Music Store sample application](http://www.asp.net/mvc/videos/mvc-2/music-store/mvc-music-store-part-1-intro,-tools,-and-project-structure) by [Jon Galloway](http://weblogs.asp.net/jgalloway/).

## Unofficial Documentation and Resources

Other places around the web have some documentation that is helpful for getting started and finding answers that are not found here.

### Tutorials and Demos

#### Version 4.x

- [MvcSiteMapProvider 4.0 - A Test Drive] (http://www.shiningtreasures.com/post/2013/08/07/MvcSiteMapProvider-40-a-test-drive)
- [MvcSiteMapProvider 4.0 - SEO Features Tutorial] (http://www.shiningtreasures.com/post/2013/08/10/mvcsitemapprovider-4-seo-features)
- [How to Make MvcSiteMapProvider Remember a User’s Position] (http://www.shiningtreasures.com/post/2013/09/02/how-to-make-mvcsitemapprovider-remember-a-user-position)
- [MvcSiteMapProvider 4.0 - Cache Configuration] (http://www.shiningtreasures.com/post/2013/08/11/mvcsitemapprovider-4-cache-configuration)
- [MvcSiteMapProvider 4.0 - Extending the Cache] (http://www.shiningtreasures.com/post/2013/08/13/mvcsitemapprovider-4-extending-the-cache)
- [MvcSiteMapProvider 4.0 - Unit Testing with the SiteMaps Static Methods] (http://www.shiningtreasures.com/post/2013/08/14/mvcsitemapprovider-4-unit-testing-with-the-sitemaps-static-methods)
- [Debugging an MvcSiteMapProvider Configuration] (http://www.shiningtreasures.com/post/2013/08/21/debugging-an-mvcsitemapprovider-configuration)
- [Converting from C# to Vb MvcSiteMapProvider] (http://www.developerfusion.com/thread/112710/converting-from-c-to-vb-mvcsitemapprovider/)
- [ASP.NET MVC Menu using Site Map Provider & Bootstrap Navbar] (http://joeylicc.wordpress.com/2014/10/03/asp-net-mvc-5-menu-using-site-map-provider-bootstrap-3-navbar/)
- [NightOwl888's MvcSiteMapProvider Demos] (https://github.com/NightOwl888?tab=repositories) - Filter for "MvcSiteMapProvider" to see the most relevant.

#### Version 3.x

- [MvcSiteMapProvider Tutorial and Examples] (http://edspencer.me.uk/2011/02/10/mvc-sitemap-provider-tutorial/)
- [MvcSiteMapProvider Tutorial 2 - Breadcrumbs] (http://edspencer.me.uk/2011/09/20/mvc-sitemap-provider-tutorial-2-breadcrumbs/)
- [Getting Started with MvcSiteMapProvider] (http://blog.danstuken.com/2011/04/29/getting-started-with-mvcsitemapprovider/)
- [Inside the MvcSiteMapProvider - Part 1] (http://xharze.blogspot.com/2012/04/inside-mvcsitemapprovider-part-1.html)
- [Inside the MvcSiteMapProvider - Part 2: Dynamic node providers] (http://xharze.blogspot.com/2012/04/inside-mvcsitemapprovider-part-2.html)
- [Inside the MvcSiteMapProvider - Part 3: The ISiteMapVisibilityProvider] (http://xharze.blogspot.com/2012/04/inside-mvcsitemapprovider-part-3.html)
- [Inside the MvcSiteMapProvider - Part 4: The IAclModule] (http://xharze.blogspot.com/2012/04/inside-mvcsitemapprovider-part-4.html)
- [Inside the MvcSiteMapProvider - Part 5: The ISiteMapNodeUrlResolver] (http://xharze.blogspot.com/2012/04/inside-mvcsitemapprovider-part-5.html)
- [Styling MvcSiteMapProvider with CSS] (http://tutsblog.net/styling-mvc-sitemap-provider-with-css/)
- [Using MvcSiteMapProvider with Twitter Bootstrap] (http://codingit.wordpress.com/2013/05/03/using-mvcsitemapprovider-with-twitter-bootstrap/)
- [ASP.NET MVC Menu using Site Map Provider & Bootstrap Navbar] (http://joeylicc.wordpress.com/2013/06/04/asp-net-mvc-menu-using-site-map-provider-bootstrap-navbar/)

### Forums and Q & A Sites

- [StackOverflow MvcSiteMapProvider] (http://stackoverflow.com/questions/tagged/mvcsitemapprovider)
- [StackOverflow MvcSiteMap] (http://stackoverflow.com/questions/tagged/mvcsitemap)
- [StackOverflow ASP.NET MVC SiteMap] (http://stackoverflow.com/questions/tagged/asp.net-mvc-sitemap)
- [CodePlex Discussion Forum (no longer maintained)] (http://mvcsitemap.codeplex.com/discussions/topics/general?size=2147483647)
- [Telerik Forum] (http://www.telerik.com/search.aspx?insection=False&start=0&client=telerik_developer_tools&q=MvcSiteMapProvider&sid=1)

### Other Blog Posts

- [maartenba's blog] (http://blog.maartenballiauw.be/search.aspx?q=mvcsitemapprovider)
- [NightOwl888's blog] (http://www.shiningtreasures.com/category/MvcSiteMapProvider)
