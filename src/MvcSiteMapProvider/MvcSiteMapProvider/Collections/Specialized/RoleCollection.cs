using MvcSiteMapProvider.DI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Specialized string collection for providing business logic that manages
    /// the behavior of the roles.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class RoleCollection
        : LockableList<string>, IRoleCollection
    {
        public RoleCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

        public override void AddRange(IEnumerable<string> collection)
        {
            if (collection != null)
            {
                base.AddRange(collection);
            }
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
                var values = stringToSplit.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var value in values)
                {
                    this.Add(value.Trim());
                }
            }
        }

        /// <summary>
        /// Adds each element of a <see cref="System.Collections.IList"/> to the collection.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.IList"/> containing the values to add, or <b>null</b>.</param>
        public void AddRange(IList collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    this.Add(item.ToString());
                }
            }
        }
    }
}
