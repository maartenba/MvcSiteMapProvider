using System;

// Startup using WebActivatorEx (which allows multiple things to be started up, as opposed to the System.Web version).
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(MvcSiteMapProvider.DI.Composer), "Compose")]
