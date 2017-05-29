using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Core.Interfaces.Repositories
{
    /// <summary>
    ///     To be used in services e.g. ICustomerService, does not expose Save()
    ///     or the ability to Commit unit of work
    /// </summary>
    public interface IUnitOfWorkForService : IDisposable
    {
        /// <summary>
        /// Instance of the repository pattern for the type of TEntity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase;
    }
}
