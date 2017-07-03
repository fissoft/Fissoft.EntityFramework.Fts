using System.Linq;
using Fissoft.EntityFramework.Fts.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    [TestClass]
    public class Issue10Test: TestBase
    {
        [TestMethod]
        public void Issue10()
        {
            using (var db = new MyDbContext())
            {
                var first = db.TestModel.FirstOrDefault();

                db.Database.Log = ConsoleUtil.Write;

                var text = FullTextSearchModelUtil.Contains("web", true);
                var query = db.TestModel
                    .Where(c => c.Name.Contains(text))
                    .ToList();

            }
        }
    }

}