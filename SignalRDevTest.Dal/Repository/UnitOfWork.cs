using SignalRDevTest.Dal.Context;
using SignalRDevTest.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Repository
{
    public class UnitOfWork : IDisposable
    {
        public DevTestContext context = new DevTestContext();
        private Repository<DevTest> genericRepository;
        private DevTestRepository devTestRepository;

        public Repository<DevTest> GenericRepository
        {
            get
            {

                if (this.GenericRepository == null)
                {
                    this.genericRepository = new Repository<DevTest>(context);
                }
                return GenericRepository;
            }
        }

        public DevTestRepository DevTestRepository
        {
            get
            {

                if (this.devTestRepository == null)
                {
                    this.devTestRepository = new DevTestRepository(context);
                }
                return devTestRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
