# MvcSiteMapProvider
An ASP.NET MVC SiteMapProvider implementation for the ASP.NET MVC framework.

## Note to contributors
We are working on v4 of MvcSiteMapProvider and will be discontinuing development on the current version. If you want to provide contributions, please work on the v4 branch. Pull requests on the master branch will not be accepted for now.

## Like this project?
[<img src="https://www.paypal.com/en_US/i/btn/btn_donate_SM.gif">](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=C8GLSG8E33NA4) via [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=C8GLSG8E33NA4).

## What can it be used for?
MvcSiteMapProvider is, as the name implies, an ASP.NET MVC SiteMapProvider implementation for the ASP.NET MVC framework. Targeted at ASP.NET MVC 2, it provides sitemap XML functionality and interoperability with the classic ASP.NET sitemap controls, like the SiteMapPath control for rendering breadcrumbs and the Menu control.

Based on areas, controller and action method names rather than hardcoded URL references, sitemap nodes are completely dynamic based on the routing engine used in an application. The dynamic character of ASP.NET MVC is followed in the MvcSiteMapProvider: there are numerous extensibility points that allow you to extend the basic functionality offered.

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