using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// UnknownSiteMapException
    /// </summary>
    [Serializable]
    public class UnknownSiteMapException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnknownSiteMapException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public UnknownSiteMapException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public UnknownSiteMapException(string message, Exception innerException) : base(message, innerException) { }
    }
}
