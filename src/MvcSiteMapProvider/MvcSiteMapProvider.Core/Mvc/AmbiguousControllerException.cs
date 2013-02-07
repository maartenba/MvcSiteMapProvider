using System;

namespace MvcSiteMapProvider.Core.Mvc
{
    /// <summary>
    /// AmbiguousControllerException
    /// </summary>
    [Serializable]
    public class AmbiguousControllerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmbiguousControllerException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public AmbiguousControllerException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public AmbiguousControllerException(string message, Exception innerException) : base(message, innerException) { }
    }

}
