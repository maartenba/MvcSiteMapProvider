using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Sitemap update priority
    /// </summary>
    [Serializable]
    public enum UpdatePriority
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 9999,

        /// <summary>
        /// Automatic
        /// </summary>
        Automatic = 50,

        /// <summary>
        /// Low
        /// </summary>
        Low = 0,

        /// <summary>
        /// Normal
        /// </summary>
        Normal = 50,

        /// <summary>
        /// High
        /// </summary>
        High = 75,

        /// <summary>
        /// Critical
        /// </summary>
        Critical = 100,

        /// <summary>
        /// Absolute value (0)
        /// </summary>
        Absolute_000 = 0,

        /// <summary>
        /// Absolute value (10)
        /// </summary>
        Absolute_010 = 10,

        /// <summary>
        /// Absolute value (20)
        /// </summary>
        Absolute_020 = 20,

        /// <summary>
        /// Absolute value (30)
        /// </summary>
        Absolute_030 = 30,

        /// <summary>
        /// Absolute value (40)
        /// </summary>
        Absolute_040 = 40,

        /// <summary>
        /// Absolute value (50)
        /// </summary>
        Absolute_050 = 50,

        /// <summary>
        /// Absolute value (60)
        /// </summary>
        Absolute_060 = 60,

        /// <summary>
        /// Absolute value (70)
        /// </summary>
        Absolute_070 = 70,

        /// <summary>
        /// Absolute value (80)
        /// </summary>
        Absolute_080 = 80,

        /// <summary>
        /// Absolute value (90)
        /// </summary>
        Absolute_090 = 90,

        /// <summary>
        /// Absolute value (100)
        /// </summary>
        Absolute_100 = 100
    }
}