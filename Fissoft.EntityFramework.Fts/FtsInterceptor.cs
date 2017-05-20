/*
 * https://github.com/fissoft/Fissoft.EntityFramework.Fts
 */
using System;
using System.Collections.Generic;
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

        private static readonly Dictionary<string, FtsSetting> SettingDict = new Dictionary<string, FtsSetting>()
        {
            {
                FullTextSearchModelUtil.FullTextContains,
                new FtsSetting
                {
                    KeyWord = "CONTAINS",
                    Property = "[$1].[$2]"
                }
            },
            {
                FullTextSearchModelUtil.FullTextContainsAll,
                new FtsSetting
                {
                    KeyWord = "CONTAINS",
                    Property = "*"
                }
            },
            {
                FullTextSearchModelUtil.FullTextFreeText,
                new FtsSetting
                {
                    KeyWord = "FREETEXT",
                    Property = "[$1].[$2]"
                }
            },
            {
                FullTextSearchModelUtil.FullTextFreeTextAll,
                new FtsSetting
                {
                    KeyWord = "FREETEXT",
                    Property = "*"
                }
            },
        };

        public static void RewriteFullTextQuery(DbCommand cmd)
        {
            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextContains);
            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextFreeText);
            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextFreeTextAll);
            ReplaceProperty(cmd, FullTextSearchModelUtil.FullTextContainsAll);
        }

        private static void ReplaceProperty(DbCommand cmd, string flag)
        {
            var setting = SettingDict[flag];
            var keyword = setting.KeyWord;
            var text = cmd.CommandText;
            if (text.Contains(flag))
            {
                var regex = new Regex($@"\((\[\w*\]\.\[\w*\]\s*[\+]*\s*)+\s*LIKE\s*N'%\({flag}\s?([^\)]+)\)%'\)\s?(ESCAPE N?'~')?", RegexOptions.Compiled);
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
                            $@"({keyword}(({fields.Replace('+', ',').Trim()}), N'{value}'))"
                            );
                    }
                }
                var regex1 = new Regex($@"(\[\w*\].\[\w*\]\s*)\s*LIKE\s*N'%\({flag}\s?([^\)]+)\)%'\s?(ESCAPE N?'~')?", RegexOptions.Compiled);
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
                            $@"({keyword}(({fields.Replace('+', ',').Trim()}), N'{value}'))"
                            );
                    }
                }
            }
            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                DbParameter parameter = cmd.Parameters[i];
                if (
                    new[]
                        {
                            DbType.String,
                            DbType.AnsiString,
                            DbType.StringFixedLength,
                            DbType.AnsiStringFixedLength
                        }
                        .Contains(parameter.DbType))
                {
                    if (parameter.Value == DBNull.Value)
                        continue;
                    var value = (string)parameter.Value;
                    if (value.IndexOf(flag) >= 0)
                    {
                        parameter.Size = 4096;
                        parameter.DbType = DbType.AnsiStringFixedLength;
                        value = value.Replace(flag, "");
                        // remove prefix we added n linq query
                        value = value.Substring(1, value.Length - 2);
                        // remove %% escaping by linq translator from string.Contains to sql LIKE
                        parameter.Value = value;
                        text = Regex.Replace(text,
                            $@"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{parameter.ParameterName}\s?(?:ESCAPE N?'~')",
                            $@"{keyword}({setting.Property}, @{parameter.ParameterName})");
                        if (text == cmd.CommandText)
                            throw new Exception("FTS was not replaced on: " + text);
                    }
                }
            }
            cmd.CommandText = text;
        }
    }
}