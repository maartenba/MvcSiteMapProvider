using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MvcSiteMapProvider 4.0.0.0")]
[assembly: AssemblyDescription("An ASP.NET SiteMapProvider implementation for the ASP.NET MVC framework.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("MvcSiteMapProvider")]
[assembly: AssemblyProduct("MvcSiteMapProvider 4.0.0.0")]
[assembly: AssemblyCopyright("Copyright © Maarten Balliauw 2009 - 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyDelaySignAttribute(false)]
[assembly: CLSCompliantAttribute(false)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]

#if !NET35
// Startup using WebActivatorEx (which allows multiple things to be started up, as opposed to the System.Web version).
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(MvcSiteMapProvider.DI.Composer), "Compose")]

#endif