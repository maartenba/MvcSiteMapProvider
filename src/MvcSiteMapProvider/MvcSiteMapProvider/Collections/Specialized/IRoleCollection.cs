using System.Collections;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the roles.
    /// </summary>
    public interface IRoleCollection
        : IList<string>
    {
        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection.
        /// </summary>
        /// <param name="stringToSplit">The string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        void AddRange(string stringToSplit, char[] separator);

        /// <summary>
        /// Adds each element of a <see cref="System.Collections.IList"/> to the collection.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.IList"/> containing the values to add, or <b>null</b>.</param>
        void AddRange(IList collection);

        void CopyTo(IList<string> destination);
    }
}
