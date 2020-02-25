using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace _RestoranWeb.Models
{
    public class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration() 
        {
            this.AutomaticMigrationsEnabled = true;
        }
    }
    
}