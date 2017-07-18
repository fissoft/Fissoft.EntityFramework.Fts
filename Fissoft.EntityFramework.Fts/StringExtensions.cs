using System;

namespace Fissoft.EntityFramework.Fts
{
    internal static class StringExtensions
    {
        private static readonly string[] LikeSpliter =
        {
            "like", "LIKE"
        };

        public static string GetSplitLikeFirst(this string str)
        {
            return str.Split(LikeSpliter, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}