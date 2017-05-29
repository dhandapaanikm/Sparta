using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Enums;

namespace Sparta.Core.Interfaces.Entities
{
    /// <summary>
    /// Interface for entities that have need to track their state in terms of CRUD operations for the EF Entity Tracker.
    /// </summary>
    public interface IObjectState
    {
        /// <summary>
        /// The entities state.
        /// </summary>
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}