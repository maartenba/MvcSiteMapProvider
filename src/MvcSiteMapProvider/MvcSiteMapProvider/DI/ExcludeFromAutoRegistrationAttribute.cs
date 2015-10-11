using System;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// An attribute that can be used to specify not to implicitly register a class for an 
    /// external DI container even if the class conforms to one of the conventions. This can be 
    /// useful if the class is meant to be instantiated by a factory or its purpose otherwise 
    /// requires it to be registered explicitly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExcludeFromAutoRegistrationAttribute
        : Attribute
    {
    }
}
