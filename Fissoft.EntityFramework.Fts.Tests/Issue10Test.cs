using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class Issue10Test: TestBase
    {
        [TestMethod]
        public void Issue10RegexTest()
        {
            var sql= @"SELECT 
    [Extent1].[Id] AS [Id], 
    [Extent1].[Text] AS [Text], 
    [Extent1].[Name] AS [Name]
    FROM [dbo].[TestModels] AS [Extent1]
    WHERE [Extent1].[Name] + [Extent1].[Text] LIKE @p__linq__0 ESCAPE N'~'";
            var matches = Regex.Matches(sql, @"(\[(?<t>\w*)\].\[(?<p>\w*)\]\s*\+?\s*)+LIKE\s*@p__linq__0\s?(?:ESCAPE N?'~')");
            Console.WriteLine(matches);
            var rep = Regex.Replace(sql,
                @"(\[(?<t>\w*)\].\[(?<p>\w*)\]\s*\+?\s*)+LIKE\s*@p__linq__0\s?(?:ESCAPE N?'~')", (match) =>
                {
                    var sb = new StringBuilder();
                    sb.Append("contains((");
                    foreach (Capture capture in match.Groups[1].Captures)
                    {
                        sb.Append(capture.Value.Replace("+", ","));
                    }
                    sb.Append("),@p__linq__0)");
                    //$@"{keyword}({setting.Property}, @{parameter.ParameterName})");
                    return sb.ToString();
                });
            Console.WriteLine(rep);
        }
        [TestMethod]
        public void Issue10()
        {
            using (var db = new MyDbContext())
            {
                var first = db.TestModel.FirstOrDefault();

                db.Database.Log = ConsoleUtil.Write;
                db.Configuration.UseDatabaseNullSemantics = true;
                var keyword = "web";
                var text = FullTextSearchModelUtil.Contains(keyword, true);
                var queryOrg = db.TestModel
                    .Where(c => (c.Name +c.Text).Contains(keyword))
                    .ToList();
                var query = db.TestModel
                    .Where(c => (c.Name+c.Text).Contains(text))
                    .ToList();

            }
        }
    }

}