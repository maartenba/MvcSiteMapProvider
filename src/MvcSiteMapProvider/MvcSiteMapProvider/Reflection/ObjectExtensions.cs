using System;
using System.Reflection;

namespace MvcSiteMapProvider.Reflection
{
    /// <summary>
    /// Extensions to the System.Object data type.
    /// </summary>
    /// <remarks>
    /// Source: http://stackoverflow.com/questions/1565734/is-it-possible-to-set-private-property-via-reflection#answer-1565766
    /// </remarks>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is located</param>
        /// <param name="propertyName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivatePropertyValue<T>(this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            PropertyInfo pi = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null) throw new ArgumentOutOfRangeException("propertyName", string.Format(Resources.Messages.ObjectPropertyNotFound, propertyName, obj.GetType().FullName));
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Returns a private Field Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Field is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Field</typeparam>
        /// <param name="obj">Object from where the Field Value is located</param>
        /// <param name="fieldName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string fieldName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("fieldName", string.Format(Resources.Messages.ObjectFieldNotFound, fieldName, obj.GetType().FullName));
            return (T)fi.GetValue(obj);
        }
    }
}
