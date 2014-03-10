using System;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for a class that represents a binding between host name, protocol, and port.
    /// </summary>
    public interface IBinding
    {
        string HostName { get; }
        string Protocol { get; }
        int Port { get; }
    }
}
