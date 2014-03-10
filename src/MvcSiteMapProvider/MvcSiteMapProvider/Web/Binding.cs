using System;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Represents a binding between host name, protocol, and port.
    /// This class can be used to determine the port when generating a URL by 
    /// matching the host name and protocol.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class Binding
        : IBinding
    {
        public Binding(
            string hostName,
            string protocol,
            int port
            )
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentNullException("hostName");
            if (string.IsNullOrEmpty(protocol))
                throw new ArgumentNullException("protocol");

            this.HostName = hostName;
            this.Protocol = protocol;
            this.Port = port;
        }

        #region IBinding Members

        public string HostName { get; private set; }

        public string Protocol { get; private set; }

        public int Port { get; private set; }

        #endregion
    }
}
