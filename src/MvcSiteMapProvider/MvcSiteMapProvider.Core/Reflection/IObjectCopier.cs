using System;

namespace MvcSiteMapProvider.Core.Reflection
{
    public interface IObjectCopier
    {
        void Copy(object source, object destination, params string[] excludedMembers);
    }
}
