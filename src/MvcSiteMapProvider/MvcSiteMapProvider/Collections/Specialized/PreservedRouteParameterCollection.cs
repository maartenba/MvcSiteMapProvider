using System;
using System.Collections.Generic;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Specialized string collection for providing business logic that manages
    /// the behavior of the preserved route parameters.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class PreservedRouteParameterCollection
        : LockableList<string>, IPreservedRouteParameterCollection
    {
        public PreservedRouteParameterCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection as a new preserved route parameters.
        /// </summary>
        /// <param name="preservedRouteParameters">The preserved route parameters string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        public void AddRange(string preservedRouteParameters, char[] separator)
        {
            if (!string.IsNullOrEmpty(preservedRouteParameters))
            {
                var parameters = preservedRouteParameters.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var parameter in parameters)
                {
                    this.Add(parameter.Trim());
                }
            }
        }

        /// <summary>
        /// Adds each element of a <see cref="System.Collections.Generic.IEnumerable{string}"/> to the collection as a new preserved route parameter.
        /// </summary>
        /// <param name="preservedRouteParameters">The <see cref="System.Collections.Generic.IEnumerable{string}"/> containing the values to add, or <b>null</b>.</param>
        public override void AddRange(IEnumerable<string> preservedRouteParameters)
        {
            if (preservedRouteParameters != null)
            {
                base.AddRange(preservedRouteParameters);
            }
        } 
    }
}
