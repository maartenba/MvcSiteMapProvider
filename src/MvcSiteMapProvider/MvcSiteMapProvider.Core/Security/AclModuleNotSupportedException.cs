#region Using directives

using System;

#endregion

namespace MvcSiteMapProvider.Core.Security
{
    /// <summary>
    /// AclModuleNotSupportedException
    /// </summary>
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
