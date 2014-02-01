using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized dictionary for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public interface IRouteValueDictionary
        : IDictionary<string, object>
    {
        /// <summary>
        /// Adds a new element to the dictionary with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void Add(string key, object value, bool throwIfReservedKey);

        /// <summary>
        /// Adds a new element to the dictionary with the values specified in the KeyValuePair.
        /// </summary>
        /// <param name="item">The KeyValuePair object that contains the key and value to add.</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void Add(KeyValuePair<string, object> item, bool throwIfReservedKey);

        /// <summary>
        /// Adds the elements from a <see cref="System.Collections.Generic.IDictionary{string, object}"/>.
        /// </summary>
        /// <param name="items">The <see cref="System.Collections.Generic.IDictionary{string, object}"/> of items to add.</param>
        void AddRange(IDictionary<string, object> items);

        /// <summary>
        /// Adds the elements from a <see cref="System.Collections.Generic.IDictionary{string, object}"/>.
        /// </summary>
        /// <param name="items">The <see cref="System.Collections.Generic.IDictionary{string, object}"/> of items to add.</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void AddRange(IDictionary<string, object> items, bool throwIfReservedKey);

        /// <summary>
        /// Adds the elements from a JSON string representing the attributes.
        /// </summary>
        /// <param name="jsonString">A JSON string that represents a dictionary of key-value pairs. Example: @"{ ""key-1"": ""value-1""[, ""key-x"": ""value-x""] }". 
        /// The value may be a string or primitive type (by leaving off the quotes).</param>
        void AddRange(string jsonString);

        /// <summary>
        /// Adds the elements from a JSON string representing the attributes.
        /// </summary>
        /// <param name="jsonString">A JSON string that represents a dictionary of key-value pairs. Example: @"{ ""key-1"": ""value-1""[, ""key-x"": ""value-x""] }". 
        /// The value may be a string or primitive type (by leaving off the quotes).</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void AddRange(string jsonString, bool throwIfReservedKey);

        /// <summary>
        /// Adds the elements from a given <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <param name="xmlNode">The <see cref="System.Xml.Linq.XElement"/> that represents the siteMapNode element in XML.</param>
        void AddRange(XElement xmlNode);

        /// <summary>
        /// Adds the elements from a given <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <param name="xmlNode">The <see cref="System.Xml.Linq.XElement"/> that represents the siteMapNode element in XML.</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void AddRange(XElement xmlNode, bool throwIfReservedKey);

        /// <summary>
        /// Adds the elements from a given <see cref="System.Collections.Specialized.NameValueCollection"/>.
        /// </summary>
        /// <param name="nameValueCollection">The <see cref="System.Collections.Specialized.NameValueCollection"/> to retrieve the values from.</param>
        void AddRange(NameValueCollection nameValueCollection);

        /// <summary>
        /// Adds the elements from a given <see cref="System.Collections.Specialized.NameValueCollection"/>.
        /// </summary>
        /// <param name="nameValueCollection">The <see cref="System.Collections.Specialized.NameValueCollection"/> to retrieve the values from.</param>
        /// <param name="throwIfReservedKey"><c>true</c> to throw an exception if one of the keys being added is a reserved key name; otherwise, <c>false</c>.</param>
        void AddRange(NameValueCollection nameValueCollection, bool throwIfReservedKey);

        bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues);

        void CopyTo(IDictionary<string, object> destination);
    }
}
