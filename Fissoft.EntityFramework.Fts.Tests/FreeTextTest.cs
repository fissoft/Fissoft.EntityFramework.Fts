using System;
using System.Linq;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class FreeTextTest:TestBase
    {
    
        [TestMethod]
        public void Test()
        {
            using (var db = new MyDbContext())
            {
                var first = db.TestModel.FirstOrDefault();

                db.Database.Log = ConsoleUtil.Write; 
                Console.WriteLine("Signle Word");
                var text = FullTextSearchModelUtil.FreeText("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
                var orWord = FullTextSearchModelUtil.FreeText("a b");
                var query1 = db.TestModel
                    .Where(c => c.Name.Contains(orWord))
                    .ToList();
                var andWord = FullTextSearchModelUtil.FreeText("a b", true);
                var query2 = db.TestModel
                        .Where(c => c.Name.Contains(andWord)).ToList()
                    ;
            }
        }
        [TestMethod]
        public void TestAll()
        {
            using (var db = new MyDbContext())
            {
                var first = db.TestModel.FirstOrDefault();

                db.Database.Log = ConsoleUtil.Write;
                Console.WriteLine("Signle Word");
                var text = FullTextSearchModelUtil.FreeTextAll("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
                var orWord = FullTextSearchModelUtil.FreeTextAll("a b");
                var query1 = db.TestModel
                    .Where(c => c.Name.Contains(orWord))
                    .ToList();
                var andWord = FullTextSearchModelUtil.FreeTextAll("a b", true);
                var query2 = db.TestModel
                        .Where(c => c.Name.Contains(andWord)).ToList()
                    ;
            }
        }
    }
}
