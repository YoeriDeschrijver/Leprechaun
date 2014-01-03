using System;

namespace Leprechaun.Api.BitStamp
{
    public static class Extensions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}