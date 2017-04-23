using System.Data.Entity;

namespace Fissoft.EntityFramework.Fts.Tests.Models
{
    class MyDbContext : DbContext
    {
        public MyDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<TestModel> TestModel { get; set; }
    }
}
