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
        public static string Contains(string search)
        {
            return string.Format("({0}{1})", FullTextContains, Convert(search));
        }
        public static string ContainsAll(string search)
        {
            return string.Format("({0}{1})", FullTextContainsAll, Convert(search));
        }
        public static string FreeText(string search)
        {
            return string.Format("({0}{1})", FullTextFreeText, Convert(search));
        }
        public static string FreeTextAll(string search)
        {
            return string.Format("({0}{1})", FullTextFreeTextAll, Convert(search));
        }

        private static string Convert(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return search;
            if (search.Contains(" "))
            {
                if (search.StartsWith("\"") && search.EndsWith("\""))
                    return search;
                var words = search.Split(new[] { ' ', '　' });
                return string.Join(" or ", words.Where(c => c != "or"));
            }
            else
            {
                return search;
            }
        }
    }
}