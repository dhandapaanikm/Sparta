using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sparta.Core.Entities;
using Sparta.Core.Enums;
using Sparta.Core.Interfaces.ApplicationServices;
using Sparta.Core.Interfaces.Entities;
using Sparta.Core.Interfaces.Repositories;

namespace Sparta.Core
{
    /// <summary>
    /// Generic repository class for implementing the repository pattern.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDataContext context;
        private readonly DbSet<TEntity> dbSet;
        private readonly Guid instanceId;

        #region Properties
        /// <summary>
        /// The id of the instance of the repository being used.
        /// </summary>
        public Guid InstanceId
        {
            get { return instanceId; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create the instance of the repository and instantiate the data context.
        /// </summary>
        /// <param name="context"></param>
        public Repository(IDataContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
            instanceId = Guid.NewGuid();
        }
        #endregion

        #region Public Methods
        public void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual IEnumerable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual void Insert(TEntity entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Added;
            dbSet.Attach(entity);
            context.SyncObjectState(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void InsertGraph(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Modified;
            dbSet.Attach(entity);
            context.SyncObjectState(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual void Delete(object id)
        {
            var entity = dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Deleted;
            dbSet.Attach(entity);
            context.SyncObjectState(entity);
        }

        public virtual IRepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);
            return repositoryGetFluentHelper;
        }
        #endregion

        #region Internal methods
        internal IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (includeProperties != null)
            {
                includeProperties.ForEach(i => query = query.Include(i));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1)*pageSize.Value).Take(pageSize.Value);
            }

            return query;
        }

        internal async Task<IQueryable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            return Get(filter, orderBy, includeProperties, page, pageSize);
        }
        #endregion
    }
}