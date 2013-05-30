namespace ExtensibleSiteMap
{
    using System;

    internal static class IntExtensions
    {
        public static TimeSpan Hours(this int theNumber)
        {
            return TimeSpan.FromHours(theNumber);
        }
    }
}