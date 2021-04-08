using SCADA_CORE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SCADA_CORE.Database
{
    public class AlarmsContext:DbContext
    {
        public DbSet<Alarm> Alarms { get; set; }
    }
}