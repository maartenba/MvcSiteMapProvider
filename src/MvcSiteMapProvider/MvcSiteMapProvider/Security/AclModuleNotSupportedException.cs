using System;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// AclModuleNotSupportedException
    /// </summary>
    [Serializable]
    public class AclModuleNotSupportedException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AclModuleNotSupportedException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public AclModuleNotSupportedException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public AclModuleNotSupportedException(string message, Exception innerException) : base(message, innerException) { }
    }

}
