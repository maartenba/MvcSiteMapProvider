using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Change frequency for the node
    /// </summary>
    [Serializable]
    public enum ChangeFrequency
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Always
        /// </summary>
        Always = 1,

        /// <summary>
        /// Never
        /// </summary>
        Never = 2,

        /// <summary>
        /// Hourly
        /// </summary>
        Hourly = 3,

        /// <summary>
        /// Daily
        /// </summary>
        Daily = 4,

        /// <summary>
        /// Weekly
        /// </summary>
        Weekly = 5,

        /// <summary>
        /// Monthly
        /// </summary>
        Monthly = 6,

        /// <summary>
        /// Yearly
        /// </summary>
        Yearly = 7
    }
}