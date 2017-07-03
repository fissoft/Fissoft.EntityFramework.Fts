using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fissoft.EntityFramework.Fts.Tests
{
    public class TestBase
    {
        [TestInitialize]
        public void Init()
        {
            DbInterceptors.Init();
        }
    }
}