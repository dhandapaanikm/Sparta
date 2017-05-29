using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Enums;
using Sparta.Core.Interfaces.Entities;

namespace Sparta.Core.Entities
{
    /// <summary>
    ///     ABC for generic entities with Ids.
    /// </summary>
    public abstract class EntityBase : IObjectState
    {
        /// <summary>
        /// Used by the EF entity change tracker to determine which operation (INSERT, UPDATE, DELETE) needs to be
        ///     applied to the entity when being sent to the database.
        /// </summary>
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}
