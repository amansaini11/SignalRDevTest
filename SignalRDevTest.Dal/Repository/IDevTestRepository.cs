using SignalRDevTest.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Repository
{
    public interface IDevTestRepository : IDisposable
    {
        IQueryable<DevTest> GetDevTestQuerable();
    }
}
