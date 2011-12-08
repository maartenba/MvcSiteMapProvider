#region Using directives

using System;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// UnknownSiteMapProviderException
    /// </summary>
    public class UnknownSiteMapProviderException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnknownSiteMapProviderException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public UnknownSiteMapProviderException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public UnknownSiteMapProviderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
