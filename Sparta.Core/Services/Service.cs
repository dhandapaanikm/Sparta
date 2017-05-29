using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Interfaces;

namespace Sparta.Core.Services
{
    /// <summary>
    ///     ABC for all Core.Services classes that need UoW/Repository support.
    /// </summary>
    public abstract class Service
    {
        #region Fields
        /// <summary>
        ///     The UnitOfWork instance to use for Repository access.
        /// </summary>
        public IUnitOfWork UnitOfWork;
        #endregion

        #region Properties
        /// <summary>
        ///     The executing caller's user id (a.k.a. ModifiedBy).
        /// </summary>
        
        #endregion

        #region Constructors
        /// <summary>

        protected Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

        }
        #endregion
    }
}
