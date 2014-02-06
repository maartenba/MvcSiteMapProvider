using System;

namespace MvcSiteMapProvider
{
    static class EnumHelper
    {
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
        {
#if NET35
            try
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                return true;
            }
            catch
            {
            }
            result = default(TEnum);
            return false;
#else
            return Enum.TryParse<TEnum>(value, ignoreCase, out result);
#endif
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return EnumHelper.TryParse<TEnum>(value, false, out result);
        }
    }
}

