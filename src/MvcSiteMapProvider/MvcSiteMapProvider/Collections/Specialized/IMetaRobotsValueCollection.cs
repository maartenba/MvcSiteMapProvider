using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the meta robots content attribute.
    /// </summary>
    public interface IMetaRobotsValueCollection
        : IList<string>
    {
        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection as a new value. Duplicates will be ignored.
        /// </summary>
        /// <param name="stringToSplit">The string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        void AddRange(string stringToSplit, char[] separator);

        /// <summary>
        /// Adds each element of a <see cref="T:System.Collections.Generic.IEnumerable{string}"/> to the collection as a new meta robots value. Duplicates will be ignored.
        /// </summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable{string}"/> containing the values to add, or <b>null</b>.</param>
        void AddRange(IEnumerable<string> collection);

        string GetMetaRobotsContentString();
        bool HasNoIndexAndNoFollow { get; }
        void CopyTo(IList<string> destination);
    }
}
