using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using _RestoranWeb.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace _RestoranWeb.Models
{
    public class IdentityContext :  IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
            : base("DataModel", false) //  "Context", throwIfV1Schema: false << ConnectionString  property has not been initialized. .. ConnectionString özelliği başlatılmamış.
        {
            Database.SetInitializer(new  
                MigrateDatabaseToLatestVersion<IdentityContext , Configuration>("DataModel"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

        }



    }
    
}