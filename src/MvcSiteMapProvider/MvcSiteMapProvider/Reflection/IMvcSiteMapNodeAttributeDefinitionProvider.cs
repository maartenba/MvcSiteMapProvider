using MvcSiteMapProvider.Builder;
using System.Collections.Generic;
using System.Reflection;

namespace MvcSiteMapProvider.Reflection
{
    public interface IMvcSiteMapNodeAttributeDefinitionProvider
    {
        IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetMvcSiteMapNodeAttributeDefinitions(IEnumerable<Assembly> assemblies);
    }
}
