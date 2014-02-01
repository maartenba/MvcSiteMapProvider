using System;
using System.Collections;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the preserved route parameters.
    /// </summary>
    public interface IPreservedRouteParameterCollection
        : IList<string>
    {
        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection as a new preserved route parameter.
        /// </summary>
        /// <param name="preservedRouteParameters">The preserved route parameters string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        void AddRange(string preservedRouteParameters, char[] separator);

        /// <summary>
        /// Adds each element of a <see cref="System.Collections.Generic.IEnumerable{string}"/> to the collection as a new preserved route parameter.
        /// </summary>
        /// <param name="preservedRouteParameters">The <see cref="System.Collections.Generic.IEnumerable{string}"/> containing the values to add, or <b>null</b>.</param>
        void AddRange(IEnumerable<string> preservedRouteParameters);

        void CopyTo(IList<string> destination);
    }
}
