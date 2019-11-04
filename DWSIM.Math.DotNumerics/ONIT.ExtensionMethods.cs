using System.Collections.Generic;

namespace System
{
    using System.Runtime.CompilerServices;
    public static class NumericExtensionMethods
    {
        /// <summary>
        /// Только цифры в строке
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigitsOnly(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            int y = 0;
            foreach (byte code in value)
            {
                if (code < 48/*0*/ || code > 57/*9*/) return false;
            }

            return true;
        }
    }
}
