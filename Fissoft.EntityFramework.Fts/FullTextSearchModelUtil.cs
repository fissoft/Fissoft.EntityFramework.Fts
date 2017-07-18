/*
 * https://github.com/fissoft/Fissoft.EntityFramework.Fts
 */

using System.Linq;

namespace Fissoft.EntityFramework.Fts
{
    public class FullTextSearchModelUtil
    {
        public const string FullTextContains = "-FTSFULLTEXTCONTAINS-";
        public const string FullTextContainsAll = "-FTSFULLTEXTCONTAINSALL-";
        public const string FullTextFreeText = "-FTSFULLTEXTFREETEXT-";
        public const string FullTextFreeTextAll = "-FTSFULLTEXTFREETEXTALL-";

        public static string Contains(string search, bool matchAllWords = false)
        {
            return $"({FullTextContains}{Convert(search, matchAllWords)})";
        }

        public static string ContainsAll(string search, bool matchAllWords = false)
        {
            return $"({FullTextContainsAll}{Convert(search, matchAllWords)})";
        }

        public static string FreeText(string search, bool matchAllWords = false)
        {
            return $"({FullTextFreeText}{Convert(search, matchAllWords)})";
        }

        public static string FreeTextAll(string search, bool matchAllWords = false)
        {
            return $"({FullTextFreeTextAll}{Convert(search, matchAllWords)})";
        }

        private static string Convert(string search, bool matchAllWords = false)
        {
            if (string.IsNullOrWhiteSpace(search))
                return search;
            if (!search.Contains(" ")) return search;
            if (search.StartsWith("\"") && search.EndsWith("\""))
                return search;
            var words = search.Split(' ', '　');
            return matchAllWords
                ? string.Join(" and ", words.Where(c => c != "and"))
                : string.Join(" or ", words.Where(c => c != "or"));
        }
    }
}