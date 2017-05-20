using System;
using System.Linq;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class Issue4Test
    {
        [TestMethod]
        public void Contains_UsingSpecialCharacters_Succeeds()
        {
            foreach (string specialCharacter in new[]
            {
                "\"",
                "'",
                ")",
                "(",
                "%"
            })
            {
                using (var db = new MyDbContext())
                {
                    db.TestModel.FirstOrDefault();
                    db.Database.Log = ConsoleUtil.Write;
                    var text = FullTextSearchModelUtil.Contains(specialCharacter);
                    var query = db.TestModel
                        .Where(c => c.Name.Contains(text))
                        .ToList();
                }
            }
        }
    }
}