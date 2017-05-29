using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;
using Sparta.Core.Interfaces.Repositories;

namespace Sparta.Core
{
    /// <summary>
    /// Class for querying a repository of entities.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class RepositoryQuery<TEntity> : IRepositoryQuery<TEntity> where TEntity : EntityBase
    {
        #region Fields
        private readonly List<Expression<Func<TEntity, object>>> includeProperties;
        private readonly Repository<TEntity> repository;
        private Expression<Func<TEntity, bool>> filter;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByQuerable;
        private int? page;
        private int? pageSize;
        #endregion

        #region Constructors
        /// <summary>
        /// Create the instance and initialize the Repository.
        /// </summary>
        /// <param name="repository"></param>
        public RepositoryQuery(Repository<TEntity> repository)
        {
            this.repository = repository;
            includeProperties = new List<Expression<Func<TEntity, object>>>();
        }
        #endregion

        /// <summary>
        /// Filter the entities in the repository. (WHERE clause)
        /// </summary>
        /// <param name="entityFilter"></param>
        /// <returns></returns>
        public RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            filter = entityFilter;
            return this;
        }

        /// <summary>
        /// Order the entities in the repository. (ORDER BY clause)
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            orderByQuerable = orderBy;
            return this;
        }

        /// <summary>
        /// Include sub-entities and collections of the top level entity. (Essentially a JOIN statement).
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            includeProperties.Add(expression);
            return this;
        }

        /// <summary>
        /// Get a paged list of entities from the repository.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPage(int pageNumber, int pageSize, out int totalCount)
        {
            page = pageNumber;
            this.pageSize = pageSize;
            totalCount = repository.Get(filter).Count();

            return repository.Get(filter, orderByQuerable, includeProperties, page, this.pageSize);
        }

        /// <summary>
        /// Execute the query against the repository and retrieve the entities.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Get()
        {
            return repository.Get(filter, orderByQuerable, includeProperties, page, pageSize);
        }

        /// <summary>
        /// Asynchronously execute the query against the repository and retrieve the entities.
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetAsync()
        {
            return await repository.GetAsync(filter, orderByQuerable, includeProperties, page, pageSize);
        }

        /// <summary>
        /// Execute a SQL query and return a list of entities.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return repository.SqlQuery(query, parameters).AsQueryable();
        }
    }
}