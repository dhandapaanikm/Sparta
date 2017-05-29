using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sparta.Core.Interfaces.ApplicationServices;
using Sparta.Core.Interfaces.Entities;

namespace Sparta.Infrastructure
{
    /// <summary>
    ///     Main EF data context base class for all Sparta repository interactions.
    /// </summary>
    public class DataContext : DbContext, IDataContext
    {
        #region Fields
        private const string KEY_VALUE_SEPARATOR = "=";
        private const string KEY_VALUE_PAIR_SEPARATOR = ";";

        private readonly Guid instanceId;
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the auto generated Id (Guid) of this DataContext instance for tracking purposes.
        /// </summary>
        public Guid InstanceId
        {
            get { return instanceId; }
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     Initialize the DataContext and set the connection string.
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     The generic instance of the DbSet of entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        ///     The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();

            var changes = base.SaveChanges();

            SyncObjectsStatePostCommit();

            return changes;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public override Task<int> SaveChangesAsync()
        {
            SyncObjectsStatePreCommit();

            var changesAsync = base.SaveChangesAsync();

            SyncObjectsStatePostCommit();

            return changesAsync;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <param name="cancellationToken">
        ///     A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();

            var changesAsync = base.SaveChangesAsync(cancellationToken);

            SyncObjectsStatePostCommit();

            return changesAsync;
        }

        /// <summary>
        ///     Synchronize the state of a given entity with what it is to what it is in the DataContext change tracker.
        /// </summary>
        /// <param name="entity"></param>
        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).ObjectState);
        }

        /// <summary>
        ///     Execute a stored procedure and return a list of entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSqlCommand<T>(string storedProcedure, SqlParameter[] sqlParameters)
        {
            var execString = string.Format("EXEC {0} {1}", storedProcedure,
                string.Join(",", from p in sqlParameters select p.ParameterName));
            Database.CommandTimeout = 300;

            // ReSharper disable CoVariantArrayConversion
            return Database.SqlQuery<T>(execString, sqlParameters.ToArray());
            // ReSharper restore CoVariantArrayConversion
        }

        /// <summary>
        ///     Execute a stored procedure with the provided parameters.
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        public int ExecuteSqlCommand(string storedProcedure, SqlParameter[] sqlParameters)
        {
            var execString = string.Format("EXEC {0} {1}", storedProcedure,
                string.Join(",", from p in sqlParameters select p.ParameterName));
            Database.CommandTimeout = 300;

            // ReSharper disable CoVariantArrayConversion
            return Database.ExecuteSqlCommand(execString, sqlParameters.ToArray());
            // ReSharper restore CoVariantArrayConversion
        }


        public int ExecuteSqlCommandWithOutput(string storedProcedure, SqlParameter[] sqlParameters)
        {
            var execString = string.Format("EXEC {0} {1} OUTPUT", storedProcedure, string.Join(",", from p in sqlParameters select p.ParameterName));
            Database.CommandTimeout = 300;

            // ReSharper disable CoVariantArrayConversion
            return Database.ExecuteSqlCommand(execString, sqlParameters.ToArray());
            // ReSharper restore CoVariantArrayConversion
        }


        /// <summary>
        ///     Execute a SQL command string.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string command)
        {
            return Database.ExecuteSqlCommand(command);
        }

        /// <summary>
        ///     Asynchronously execute a stored procedure with the provided parameters.
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="sqlParameters"></param>
        public Task<int> ExecuteSqlCommandAsync(string storedProcedure, SqlParameter[] sqlParameters)
        {
            var execString = string.Format("EXEC {0} {1}", storedProcedure,
                string.Join(",", from p in sqlParameters select p.ParameterName));
            Database.CommandTimeout = 300;

            // ReSharper disable CoVariantArrayConversion
            return Database.ExecuteSqlCommandAsync(execString, sqlParameters);
            // ReSharper restore CoVariantArrayConversion
        }

        /// <summary>
        /// Use the database to encrypt data.
        /// </summary>
        /// <param name="unencryptedData"></param>
        /// <returns></returns>
        public byte[] Encrypt(string unencryptedData)
        {
            var sqlParams = new List<SqlParameter>
                            {
                                new SqlParameter
                                {
                                    ParameterName = "@unencryptedData",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = unencryptedData
                                }
                            };

            var execString = string.Format("EXEC [dbo].[Encrypt] {0} ", string.Join(",", from p in sqlParams select p.ParameterName));

            // ReSharper disable CoVariantArrayConversion
            return Database.SqlQuery<byte[]>(execString, sqlParams.ToArray()).First();
            // ReSharper restore CoVariantArrayConversion
        }

        /// <summary>
        /// Use the database to decrypt encrypted data.
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public string Decrypt(byte[] encryptedData)
        {
            var sqlParams = new List<SqlParameter>
                            {
                                new SqlParameter
                                {
                                    ParameterName = "@encryptedData",
                                    SqlDbType = SqlDbType.VarBinary,
                                    Value = encryptedData
                                }
                            };

            var execString = string.Format("EXEC [dbo].[Decrypt] {0} ", string.Join(",", from p in sqlParams select p.ParameterName));

            // ReSharper disable CoVariantArrayConversion
            return Database.SqlQuery<string>(execString, sqlParams.ToArray()).First();
            // ReSharper restore CoVariantArrayConversion
        }
        #endregion

        #region Private Methods
        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }

        private void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
        #endregion
    }
}