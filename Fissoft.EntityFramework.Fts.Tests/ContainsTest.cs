using System;
using System.Linq;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class ContainsTest : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new MyDbContext())
            {
                db.Database.CreateIfNotExists();
            }
        }

        [TestMethod]
        public void TestParameter()
        {
            using (var db = new MyDbContext())
            {
                var first = db.TestModel.FirstOrDefault();

                db.Database.Log = ConsoleUtil.Write;
                Console.WriteLine("Signle Word");
                var text = FullTextSearchModelUtil.Contains("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
                var orWord = FullTextSearchModelUtil.Contains("a b");
                var query1 = db.TestModel
                    .Where(c => c.Name.Contains(orWord))
                    .ToList();
                var andWord = FullTextSearchModelUtil.Contains("a b", true);
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
                var text = FullTextSearchModelUtil.ContainsAll("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
                var orWord = FullTextSearchModelUtil.ContainsAll("a b");
                var query1 = db.TestModel
                    .Where(c => c.Name.Contains(orWord))
                    .ToList();
                var andWord = FullTextSearchModelUtil.ContainsAll("a b", true);
                var query2 = db.TestModel
                        .Where(c => c.Name.Contains(andWord)).ToList()
                    ;
            }
        }
    }
}