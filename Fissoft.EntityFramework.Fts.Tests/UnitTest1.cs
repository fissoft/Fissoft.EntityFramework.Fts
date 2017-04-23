using System;
using System.Linq;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            DbInterceptors.Init();
        }
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new MyDbContext())
            {
                db.Database.CreateIfNotExists();
            }
        }
        [TestMethod]
        public void MyTestMethod()
        {
            using (var db = new MyDbContext())
            {
                db.Database.Log = (Console.WriteLine);
                var text = FullTextSearchModelUtil.Contains("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
            }
        }

        [TestMethod]
        public void MyTestMethod_MultiWords()
        {
            using (var db = new MyDbContext())
            {
                db.Database.Log = (Console.WriteLine);
                var text = FullTextSearchModelUtil.Contains("a b", true);
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList(); // Should return results that contain BOTH words. For the second param = false, should return records with either of the words
            }
        }
    }
}
