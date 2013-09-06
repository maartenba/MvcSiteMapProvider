using System;
using System.Collections.Generic;
using System.Reflection;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider.Reflection
{
    public interface IMvcSiteMapNodeAttributeDefinitionProvider
    {
        IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetMvcSiteMapNodeAttributeDefinitions(IEnumerable<Assembly> assemblies);
    }
}
