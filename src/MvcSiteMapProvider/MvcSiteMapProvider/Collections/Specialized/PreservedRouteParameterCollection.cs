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
        /// Splits a string with the given separator characters and adds each element to the collection.
        /// </summary>
        /// <param name="stringToSplit">The string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        public void AddRange(string stringToSplit, char[] separator)
        {
            if (!string.IsNullOrEmpty(stringToSplit))
            {
                var collection = stringToSplit.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in collection)
                {
                    this.Add(item.Trim());
                }
            }
        }

        /// <summary>
        /// Adds each element of a <see cref="T:System.Collections.Generic.IEnumerable{string}"/> to the collection.
        /// </summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable{string}"/> containing the values to add, or <b>null</b>.</param>
        public override void AddRange(IEnumerable<string> collection)
        {
            if (collection != null)
            {
                base.AddRange(collection);
            }
        } 
    }
}
