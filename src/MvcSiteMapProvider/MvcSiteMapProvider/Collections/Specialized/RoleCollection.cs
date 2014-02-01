using System;
using System.Collections;
using System.Collections.Generic;
using MvcSiteMapProvider.DI;

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

        public override void AddRange(IEnumerable<string> roles)
        {
            if (roles != null)
            {
                base.AddRange(roles);
            }
        }

        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection as a new role.
        /// </summary>
        /// <param name="roles">The roles string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        public void AddRange(string roles, char[] separator)
        {
            if (!string.IsNullOrEmpty(roles))
            {
                var localRoles = roles.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var role in localRoles)
                {
                    this.Add(role.Trim());
                }
            }
        }

        /// <summary>
        /// Adds each element of a <see cref="System.Collections.IList"/> to the collection as a new role.
        /// </summary>
        /// <param name="roles">The <see cref="System.Collections.IList"/> containing the values to add, or <b>null</b>.</param>
        public void AddRange(IList roles)
        {
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    this.Add(role.ToString());
                }
            }
        }
    }
}
