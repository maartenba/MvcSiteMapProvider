using System;
using System.Reflection;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// IAuthorizeAttributeBuilder interface
    /// </summary>
    public interface IAuthorizeAttributeBuilder
    {
        ConstructorInfo Build(Type parentType);
    }
}
