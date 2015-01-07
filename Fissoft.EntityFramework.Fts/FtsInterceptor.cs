using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Text.RegularExpressions;
using System.Linq;
namespace Fissoft.EntityFramework.Fts
{
    public class FtsInterceptor : IDbCommandInterceptor
    {
        #region interface impl

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        #endregion

        public static void RewriteFullTextQuery(DbCommand cmd)
        {
            string text = cmd.CommandText;
            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                DbParameter parameter = cmd.Parameters[i];
                if (
                    new[] { DbType.String, DbType.AnsiString, DbType.StringFixedLength, DbType.AnsiStringFixedLength }
                        .Contains(parameter.DbType))
                {
                    if (parameter.Value == DBNull.Value)
                        continue;
                    var value = (string)parameter.Value;
                    if (value.IndexOf(FullTextSearchModelUtil.FullTextContains) >= 0)
                    {
                        parameter.Size = 4096;
                        parameter.DbType = DbType.AnsiStringFixedLength;
                        value = value.Replace(FullTextSearchModelUtil.FullTextContains, ""); // remove prefix we added n linq query
                        value = value.Substring(1, value.Length - 2);
                        // remove %% escaping by linq translator from string.Contains to sql LIKE
                        parameter.Value = value;
                        cmd.CommandText = Regex.Replace(text,
                            string.Format(
                                @"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{0}\s?(?:ESCAPE N?'~')",
                                parameter.ParameterName),
                            string.Format(@"contains([$1].[$2], @{0})",
                                parameter.ParameterName));
                        if (text == cmd.CommandText)
                            throw new Exception("FTS was not replaced on: " + text);
                        text = cmd.CommandText;
                    }
                }
            }

            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextContains);
            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextFreeText);
            ReplaceAll(cmd, FullTextSearchModelUtil.FullTextFreeTextAll);
            ReplaceAll(cmd, FullTextSearchModelUtil.FullTextContainsAll);
        }

        private static void ReplaceAll(DbCommand cmd, string flag)
        {
            var text = cmd.CommandText;
            var b = ReplaceAllStr(flag, text);
            if (text != b)
            {
                cmd.CommandText = b;
            }
        }

        public static string ReplaceAllStr(string flag, string text)
        {
            var b = text.Contains(flag);
            if (!b) return text;
            var regex = new Regex(string.Format(
                @"N'\*'\s*LIKE\s*N'%\({0}\s?([^\)]+)\)%'\s?(ESCAPE N?'~')?",
                flag), RegexOptions.Compiled);
            var matchs = regex.Matches(text);
            foreach (Match match in matchs)
            {
                if (match.Success)
                {
                    var value = match.Groups[1].Value;
                    if (match.Groups.Count > 2 && match.Groups[2].Value.StartsWith("ESCAPE"))
                    {
                        value = value.Replace("~", "");
                    }
                    text = text.Replace(match.Value,
                        string.Format(@"CONTAINS(*, N'{0}')", value)
                        );
                }
            }
            return text;
        }

        private static void ReplaceProperty(DbCommand cmd, string flag)
        {
            var text = cmd.CommandText;
            if (text.Contains(flag))
            {
                var regex = new Regex(string.Format(
                    @"\((\[\w*\]\.\[\w*\]\s*[\+]*\s*)+\s*LIKE\s*N'%\({0}\s?([^\)]+)\)%'\)\s?(ESCAPE N?'~')?",
                    flag), RegexOptions.Compiled);
                var matchs = regex.Matches(text);
                foreach (Match match in matchs)
                {
                    if (match.Success)
                    {
                        var value = match.Groups[2].Value;
                        if (match.Groups.Count > 3 && match.Groups[3].Value.StartsWith("ESCAPE"))
                        {
                            value = value.Replace("~", "");
                        }
                        var fields = match.Groups[0].Value.Trim('(')
                            .Split(new[] { "like", "LIKE" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        text = text.Replace(match.Value,
                            string.Format(@"(CONTAINS(({0}), N'{1}'))",
                                fields.Replace('+', ',').Trim(),
                                value)
                            );
                    }

                }
                var regex1 = new Regex(string.Format(
                    @"(\[\w*\].\[\w*\]\s*)\s*LIKE\s*N'%\({0}\s?([^\)]+)\)%'\s?(ESCAPE N?'~')?",
                    flag), RegexOptions.Compiled);
                var matchs1 = regex1.Matches(text);
                foreach (Match match in matchs1)
                {
                    if (match.Success)
                    {
                        var value = match.Groups[2].Value;
                        if (match.Groups.Count > 3 && match.Groups[3].Value.StartsWith("ESCAPE"))
                        {
                            value = value.Replace("~", "");
                        }
                        var fields = match.Groups[0].Value.Trim('(')
                            .Split(new[] { "like", "LIKE" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        text = text.Replace(match.Value,
                            string.Format(@"(CONTAINS(({0}), N'{1}'))",
                                fields.Replace('+', ',').Trim(),
                                value)
                            );
                    }

                }
                cmd.CommandText = text;
            }
        }
    }
}