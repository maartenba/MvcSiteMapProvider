using System;

namespace MvcSiteMapProvider.Reflection
{
    public interface IObjectCopier
    {
        void Copy(object source, object destination, params string[] excludedMembers);
    }
}
