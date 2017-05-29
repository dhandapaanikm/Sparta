using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sparta.Core.Entities;
using Sparta.Core.Interfaces;
using Sparta.Core.Interfaces.ApplicationServices;
using Sparta.Core.Interfaces.Repositories;

namespace Sparta.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Fields
        private readonly IDataContext context;
        private readonly Guid instanceId;
        private bool disposed;
        private Hashtable repositories;
        #endregion Private Fields

        #region Constuctor/Dispose
        public UnitOfWork(IDataContext context)
        {
            this.context = context;
            instanceId = Guid.NewGuid();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                context.Dispose();
            }

            disposed = true;
        }
        #endregion Constuctor/Dispose

        public Guid InstanceId
        {
            get { return instanceId; }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
        public IDataContext DataContext { get { return context; } }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)repositories[type];
            }

            var repositoryType = typeof(Repository<>);
            repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context));

            return (IRepository<TEntity>)repositories[type];
        }



    }
}