// (c) Copyright 2002-2010 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

namespace Telerik.Web.Mvc.Infrastructure.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// ObjectCopier
    /// </summary>
    public static class ObjectCopier
    {
        public static void Copy(object source, object destination, params string[] excludedMembers)
        {
            bool hasExcludedMembers = ((excludedMembers != null) && (excludedMembers.Length > 0));
            Type sourceType = source.GetType();

            IEnumerable<FieldInfo> fields =
                sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField).Where(field => !field.IsInitOnly);

            if (hasExcludedMembers)
            {
                fields = fields.Where(field => !excludedMembers.Any(name => field.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
            }

            foreach (FieldInfo field in fields)
            {
                field.SetValue(destination, field.GetValue(source));
            }

            IEnumerable<PropertyInfo> properties =
                sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(
                    p => p.CanRead && p.GetGetMethod() != null && p.CanWrite && p.GetSetMethod() != null);

            if (hasExcludedMembers)
            {
                properties = properties.Where(property => !excludedMembers.Any(name => property.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
            }

            foreach (PropertyInfo property in properties)
            {
                if (property.GetIndexParameters().Length == 0)
                {
                    property.SetValue(destination, property.GetValue(source, null), null);
                }
            }
        }
    }
}