using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Core.Interfaces.Repositories
{
    /// <summary>
    /// Defines the operations for working with repositories
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryQuery<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Filter the entities in the repository. (WHERE clause)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Order the entities in the repository. (ORDER BY clause)
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        
        /// <summary>
        /// Include sub-entities and collections of the top level entity. (Essentially a JOIN statement).
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression);
        
        /// <summary>
        /// Get a paged list of entities from the repository.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetPage(int page, int pageSize, out int totalCount);

        /// <summary>
        /// Execute the query against the repository and retrieve the entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Get();
        
        /// <summary>
        /// Asynchronously execute the query against the repository and retrieve the entities.
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetAsync();

        /// <summary>
        /// Execute a SQL query and return a list of entities.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}
