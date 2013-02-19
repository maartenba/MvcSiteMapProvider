using System;

namespace MvcSiteMapProvider.Reflection
{
    /// <summary>
    /// Contract for the object copier.
    /// </summary>
    public interface IObjectCopier
    {
        void Copy(object source, object destination, params string[] excludedMembers);
    }
}
