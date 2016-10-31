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
                db.Database.Log = (s =>
                {
                    Console.WriteLine(s);
                });
                var text = FullTextSearchModelUtil.Contains("a");
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();
                
            }
        }
    }
}
