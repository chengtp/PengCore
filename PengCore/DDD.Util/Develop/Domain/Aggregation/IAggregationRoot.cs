using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Domain.Aggregation
{
    /// <summary>
    /// AggregationRoot Interface
    /// </summary>
    public interface IAggregationRoot<in T> where T : IAggregationRoot<T>
    {
        #region Propertys

        /// <summary>
        /// allow to save
        /// </summary>
        bool CanBeSave { get; }

        /// <summary>
        /// allow to remove
        /// </summary>
        bool CanBeRemove { get; }

        /// <summary>
        /// save by add
        /// </summary>
        bool SaveByAdd { get; }

        /// <summary>
        /// Life Status
        /// </summary>
        LifeStatus LifeStatus { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Mark Object Is New
        /// </summary>
        bool MarkNew();

        /// <summary>
        /// Mark Object Is Remove
        /// </summary>
        bool MarkRemove();

        /// <summary>
        /// Mark Object Is Modify
        /// </summary>
        bool MarkModify();

        /// <summary>
        /// Mark Object Is Stored
        /// </summary>
        /// <returns></returns>
        bool MarkStored();

        /// <summary>
        /// Save Object
        /// </summary>
        void Save();

        /// <summary>
        /// Save Object
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Remove Object
        /// </summary>
        void Remove();

        /// <summary>
        /// Remove Object
        /// </summary>
        Task RemoveAsync();

        #endregion
    }
}
