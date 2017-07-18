/*
 * https://github.com/fissoft/Fissoft.EntityFramework.Fts
 */

using System.Data.Entity.Infrastructure.Interception;

namespace Fissoft.EntityFramework.Fts
{
    public class DbInterceptors
    {
        private static FtsInterceptor _ftsInterceptor;

        public static void Init()
        {
            if (_ftsInterceptor == null)
                DbInterception.Add(_ftsInterceptor = new FtsInterceptor());
        }
    }
}