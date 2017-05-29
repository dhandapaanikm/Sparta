using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Infrastructure
{
   public class SpartaContext : DataContext
    {
        static SpartaContext()
        {
            Database.SetInitializer<SpartaContext>(null);
        }

        public SpartaContext()
            : base("name = SpartaContext")
        {

        }

        //public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Configuration.LazyLoadingEnabled = false;

            modelBuilder.Entity<Region>().Map(m => m.ToTable("Regions"));

        }

       // public System.Data.Entity.DbSet<Inventory.Core.Entities.Customer> Customers { get; set; }
    }
}
