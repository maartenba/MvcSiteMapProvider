using System.Collections.Generic;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Provides extension methods for the <see cref="T:System.Collections.Specialized.NameValueCollection"/>.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        public static void AddWithCaseCorrection(this NameValueCollection nameValueCollection, string key, string value, IEnumerable<string> correctCaseKeyset)
        {
            var loweredKey = key.ToLowerInvariant();
            foreach (var item in correctCaseKeyset)
            {
                if (item.ToLowerInvariant().Equals(loweredKey))
                {
                    // Add item with corrected case key
                    nameValueCollection.Add(item, value);
                    break;
                }
            }
        }
    }
}
