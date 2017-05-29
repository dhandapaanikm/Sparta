using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sparta.Core.Interfaces.ApplicationServices;
using Sparta.Core.Interfaces.Repositories;

namespace Sparta.Core.Interfaces
{
    public interface IUnitOfWork : IUnitOfWorkForService
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        Guid InstanceId { get; }

        /// <summary>
        /// 
        /// </summary>
        IDataContext DataContext { get; }
        #endregion

        /// <summary>
        /// Save the entities being tracked in the entity tracker.
        /// </summary>
        void Save();

        /// <summary>
        /// Asynchronously save the entities being tracked in the entity tracker.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();

        /// <summary>
        /// Asynchronously save the entities being tracked in the entity tracker, with the ability to cancel the action.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// /// <param name="disposing"></param>
        void Dispose(bool disposing);
    }
}