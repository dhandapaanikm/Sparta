using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Enums;

namespace Sparta.Infrastructure
{
    /// <summary>
    /// Static utility class for converting ObjectState to EntitySate and back again (used for the EF Entity Change Tracker).
    /// </summary>
    public static class StateHelper
    {
        /// <summary>
        /// Convert object state to entity state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        /// <summary>
        /// Convert entity state to object state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static ObjectState ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Detached:
                    return ObjectState.Unchanged;
                case EntityState.Unchanged:
                    return ObjectState.Unchanged;
                case EntityState.Added:
                    return ObjectState.Added;
                case EntityState.Deleted:
                    return ObjectState.Deleted;
                case EntityState.Modified:
                    return ObjectState.Modified;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}