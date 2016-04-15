using SignalRDevTest.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Context
{
    public class DevTestContext : DbContext
    {
        public DevTestContext()
        : base("DevTestConnection")         
        {
        }
        public DbSet<DevTest> DevTests { get; set; }
    }
}
