using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fissoft.EntityFramework.Fts.Tests.Models
{
    class MyDbContext : DbContext
    {
        public MyDbContext():base("DefaultConnection")
        {

        }
        public DbSet<TestModel> TestModel { get; set; }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
    }
}
