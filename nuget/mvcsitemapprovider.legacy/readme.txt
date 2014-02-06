The MvcSiteMapProvider package is now obsolete. NuGet doesn't provide a way 
to support multiple versions of ASP.NET MVC in a single package, so it was
necessary to create a package per MVC version that we support.

This package mainly exists to provide an upgrade path from older versions 
of MvcSiteMapProvider. It targets the latest version of MVC and will change 
each time the highest supported MVC version we support is incremented. 
For this reason, we do not recommend you leave it installed in your project 
because it could force an unwanted MVC version upgrade during a future update.

If the current version of MVC is appropriate for your project, uninstall 
this package by running the following command from Package Manager Console:

PM> Uninstall-Package MvcSiteMapProvider

This will remove only the obsolete package, but keep all of the other 
MvcSiteMapProvider and dependent packages in place.

If you are unsure or you know you will need to target a specific version of MVC, 
you can remove this package and all of its dependencies, and then reinstall the
appropriate package.

PM> Uninstall-Package MvcSiteMapProvider -RemoveDependencies

PM> Install-Package MvcSiteMapProvider.MVC[x]

Replace the [x] in the above line with the version of MVC you wish to target.
To see the currently supported MVC versions of MvcSiteMapProvider, visit the 
following URL:

http://www.nuget.org/packages?q=mvcsitemapprovider.mvc