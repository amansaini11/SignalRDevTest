using SignalRDevTest.Dal.Context;
using SignalRDevTest.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Repository
{
   public class DevTestRepository : SignalRDevTest.Dal.Repository.Repository<DevTest>
    {        
        public DevTestRepository(DevTestContext context) : base(context)
        {            
        }
        
        public IQueryable<DevTest> GetDevTestQuerable()
        {
            return context.DevTests;
        }


    }
}
