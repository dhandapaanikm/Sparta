using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sparta.Core.Interfaces.ApplicationServices
{
  /// <summary>
    ///     Defines operations for working with the Data Context.
    /// </summary>
    public interface IDataContext : IDisposable
    {
        #region Properties
        /// <summary>
        ///     The unique identifier of the instance of the DataContext class.
        /// </summary>
        Guid InstanceId { get; }
        #endregion

        /// <summary>
        ///     The generic instance of the DbSet of entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        ///     Persist any changes to the database.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        ///     Asynchronously persist any changes to the database.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        ///     Asynchronously persist any changes to the database, allowing for cancellation.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Synchronize the state of a given entity with what it is to what it is in the DataContext change tracker.
        /// </summary>
        /// <param name="entity"></param>
        void SyncObjectState(object entity);

        /// <summary>
        ///     Execute a stored procedure and return a list of entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteSqlCommand<T>(string storedProcedure, SqlParameter[] sqlParameters);

        /// <summary>
        ///     Execute a stored procedure with the provided parameters.
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        int ExecuteSqlCommand(string storedProcedure, SqlParameter[] sqlParameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommandWithOutput(string storedProcedure, SqlParameter[] sqlParameters);

        /// <summary>
        ///     Asynchronously execute a stored procedure with the provided parameters.
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        Task<int> ExecuteSqlCommandAsync(string storedProcedure, SqlParameter[] sqlParameters);

        /// <summary>
        ///     Execute a SQL command string.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string command);

        /// <summary>
        /// Use the database to encrypt data.
        /// </summary>
        /// <param name="unencryptedData"></param>
        /// <returns></returns>
        byte[] Encrypt(string unencryptedData);

        /// <summary>
        /// Use the database to decrypt encrypted data.
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        string Decrypt(byte[] encryptedData);
    }
}