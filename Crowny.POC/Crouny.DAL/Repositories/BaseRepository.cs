using Crouny.DAL.EntityModel;
using System;

namespace Crouny.DAL.Repositories
{
    public abstract class BaseRepository: IDisposable
    {
        protected CrounyEntities Context { get; }

        protected BaseRepository(CrounyEntities context) {
            Context = context;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
