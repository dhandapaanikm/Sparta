using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Abstraction for implementations of the Repository pattern.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        #region Properties
        /// <summary>
        /// The id of the instance of the repository being used.
        /// </summary>
        Guid InstanceId { get; }
        #endregion

        /// <summary>
        ///     Allows for the execution of a stored procedure(, should the need arise)
        /// </summary>
        /// <param name="procedureCommand"></param>
        /// <param name="sqlParams"></param>
        void ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams);

        /// <summary>
        /// Find an entity by its key.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity Find(params object[] keyValues);

        /// <summary>
        /// Asynchronously find an entity by its key.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] keyValues);

        /// <summary>
        /// Asynchronously find an entity by its key, allowing for cancellation.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);

        /// <summary>
        /// Insert a single entity into the repository.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert multiple entities into the repository.
        /// </summary>
        /// <param name="entities"></param>
        void InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert a single object graph.
        /// </summary>
        /// <param name="entity"></param>
        void InsertGraph(TEntity entity);

        /// <summary>
        /// Insert multiple objects graph.
        /// </summary>
        /// <param name="entities"></param>
        void InsertGraphRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update an exisitng entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Update multiple entities in the repository.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete an existing entity by its id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// Delete an exisitng entity from the repository.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Query the repository.
        /// </summary>
        /// <returns></returns>
        IRepositoryQuery<TEntity> Query();

        /// <summary>
        /// Execute a SQL Query using the stored proc and parameters.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}