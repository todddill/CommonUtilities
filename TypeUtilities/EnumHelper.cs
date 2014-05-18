using System;

namespace TypeUtilities
{
    public static class EnumHelper
    {
        /// <summary>
        /// Converts the string to enum.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>enumeration</returns>
        public static T ConvertStringToEnum<T>(string value) where T : struct 
        {
            return ConvertStringToEnum<T>(value, true);
        }

        /// <summary>
        /// Converts the string to enum.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static T ConvertStringToEnum<T>(string value, bool ignoreCase) where T : struct
        {
            T retValue = default(T);
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<T>(value, ignoreCase, out retValue))
                return retValue;

            return default(T);
        }
    }
}
