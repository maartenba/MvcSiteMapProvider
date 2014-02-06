using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// MvcSiteMapException
    /// </summary>
    [Serializable]
    public class MvcSiteMapException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MvcSiteMapException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public MvcSiteMapException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public MvcSiteMapException(string message, Exception innerException) : base(message, innerException) { }
    }

}
