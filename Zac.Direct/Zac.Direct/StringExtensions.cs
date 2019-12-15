using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct
{
    public static class StringExtensions
    {
        /// <summary>
        /// Will always return a new string instance and never throws NullExceptions.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringOrEmpty(this string input)
        {
            return input == null ? "" : new string(input.ToCharArray());
        }

        /// <summary>
        /// Will always return a new string instance and never throws NullExceptions.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringOrEmpty(this object input)
        {
            return input == null ? "" : new string(input.ToString().ToCharArray());
        }
    }
}
