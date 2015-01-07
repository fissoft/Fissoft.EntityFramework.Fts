using System.Data.Entity.Infrastructure.Interception;

namespace Fissoft.EntityFramework.Fts
{
    public class DbInterceptors
    {
        private static FtsInterceptor _ftsInterceptor = null;

        public static void Init()
        {
            if (_ftsInterceptor == null)
            {
                DbInterception.Add(_ftsInterceptor = new FtsInterceptor());
            }
        }
    }
}