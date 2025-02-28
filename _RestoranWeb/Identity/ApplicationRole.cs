﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace _RestoranWeb.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public ApplicationRole()
        {

        }
        public ApplicationRole(string roleName, string description)
            : base(roleName)
        {
            this.Description = description;
        }
    }
}