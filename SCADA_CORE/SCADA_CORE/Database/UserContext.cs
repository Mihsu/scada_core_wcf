using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCADA_CORE.Models;
using System.Data.Entity;

namespace SCADA_CORE.Database
{
    public class UserContext:DbContext
    {
        public DbSet<User> Users { get; set; }
         
    }
}