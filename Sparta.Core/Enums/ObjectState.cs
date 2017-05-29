using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta.Core.Enums
{
    /// <summary>
    /// The state of the entity in relation to the repository, used by the EF Entity Tracker.
    /// </summary>
    public enum ObjectState
    {
        /// <summary>
        /// The object is unchanged since being retrieved from its repository.
        /// </summary>
        Unchanged,

        /// <summary>
        /// The object is new, and did not come from a repository.
        /// </summary>
        Added,

        /// <summary>
        /// The object has been modified since being retrieve from its repository.
        /// </summary>
        Modified,

        /// <summary>
        /// The object is "marked" for deletion from its repository.
        /// </summary>
        Deleted
    }
}