using Chloe;

using System.Threading.Tasks;

namespace System.Data.Chloe
{
    public abstract class DBBase : IDisposable
    {
        private IDbContext _dbContext;

        protected DBBase()
        {
        }

        protected IDbContext DbContext
        {
            get
            {
                if (this._dbContext == null)
                {
                    this._dbContext = DbContextFactory.CreateContext();
                }

                return this._dbContext;
            }
            set => this._dbContext = value;
        }

        protected Task DoAsync(Action<IDbContext> act, bool? startTransaction = null)
        {
            return Task.Run(() =>
            {
                using (var dbContext = DbContextFactory.CreateContext())
                {
                    if (startTransaction.HasValue && startTransaction.Value == true)
                    {
                        dbContext.DoWithTransaction(() =>
                        {
                            act(dbContext);
                        });
                    }
                    else
                    {
                        act(dbContext);
                    }
                }
            });
        }

        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
            }
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}