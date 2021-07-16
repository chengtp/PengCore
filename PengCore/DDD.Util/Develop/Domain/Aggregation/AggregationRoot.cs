using DDD.Util.DataValidation;
using DDD.Util.Domain.Repository;
using DDD.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Util.Domain.Aggregation
{
    /// <summary>
    /// AggregationRoot
    /// </summary>
    public abstract class AggregationRoot<T> : IAggregationRoot<T> where T : AggregationRoot<T>
    {
        /// <summary>
        /// life status
        /// </summary>
        protected LifeStatus _lifeStatus = LifeStatus.New;
        /// <summary>
        /// batch return
        /// </summary>
        protected bool _batchReturn = false;

        //private Dictionary<string, T> storeDataList = new Dictionary<string, T>();
        /// <summary>
        /// enable lazy load
        /// </summary>
        protected bool _loadLazyMember = true;

        //allow load propertys
        protected Dictionary<string, bool> _allowLoadPropertys = new Dictionary<string, bool>();
        //static string typeFullName = typeof(T).FullName;

        #region Propertys

        /// <summary>
        /// allow to save
        /// </summary>
        public bool CanBeSave
        {
            get
            {
                return SaveValidation();
            }
        }

        /// <summary>
        /// allow to remove
        /// </summary>
        public bool CanBeRemove
        {
            get
            {
                return RemoveValidation();
            }
        }

        /// <summary>
        /// Model Life Status
        /// </summary>
        public LifeStatus LifeStatus
        {
            get
            {
                return _lifeStatus;
            }
            private set
            {
                _lifeStatus = value;
            }
        }

        /// <summary>
        /// LifStatus Is New
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _lifeStatus == LifeStatus.New;
            }
        }

        /// <summary>
        /// LifStatus Is Remove
        /// </summary>
        public bool IsRemove
        {
            get
            {
                return _lifeStatus == LifeStatus.Remove;
            }
        }

        /// <summary>
        /// LifStatus Is Modify
        /// </summary>
        public bool IsModify
        {
            get
            {
                return _lifeStatus == LifeStatus.Modify;
            }
        }

        /// <summary>
        /// Is Batch Return
        /// </summary>
        protected bool BatchReturn
        {

            get
            {
                return _batchReturn;
            }
            private set
            {
                _batchReturn = value;
                _loadLazyMember = !value;
            }
        }

        /// <summary>
        /// Allow Lazy Data Load
        /// </summary>
        protected bool LoadLazyMember
        {

            get
            {
                return _loadLazyMember;
            }
        }

        /// <summary>
        /// Allow Load Data Propertys
        /// </summary>
        protected Dictionary<string, bool> LoadPropertys
        {
            get
            {
                return _allowLoadPropertys;
            }
            private set
            {
                _allowLoadPropertys = value;
            }
        }

        /// <summary>
        /// Stored Data
        /// </summary>
        private T StoredData { get; set; } = default(T);

        /// <summary>
        /// Save by Add
        /// </summary>
        public bool SaveByAdd
        {
            get
            {
                return IsNew || IsRemove;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save Validation
        /// </summary>
        /// <returns></returns>
        protected virtual bool SaveValidation()
        {
            if (this == null)
            {
                return false;
            }
            //validate object primary value
            if (PrimaryValueIsNone())
            {
                if (SaveByAdd)
                {
                    InitPrimaryValue();
                }
                else
                {
                    throw new Exception("the identity value for the object to be saved is not specified");
                }
            }
            var verifyResults = ValidationManager.Validate(this);
            string[] errorMessages = verifyResults.GetErrorMessage();
            if (errorMessages != null && errorMessages.Length > 0)
            {
                throw new Exception(string.Join("\n", errorMessages));
            }
            return true;
        }

        /// <summary>
        /// Remove Validation
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveValidation()
        {
            return this != null;
        }

        /// <summary>
        /// Mark Object LifeStatus
        /// </summary>
        protected void MarkLifeStatus(LifeStatus status)
        {
            _lifeStatus = status;
            switch (status)
            {
                case LifeStatus.Stored:
                    StoredData = (T)MemberwiseClone();
                    break;
                case LifeStatus.New:
                case LifeStatus.Remove:
                    StoredData = default(T);
                    break;
            }
        }

        /// <summary>
        /// Mark Object Is New
        /// </summary>
        public virtual bool MarkNew()
        {
            MarkLifeStatus(LifeStatus.New);
            return true;
        }

        /// <summary>
        /// Mark Object Is Remove
        /// </summary>
        public virtual bool MarkRemove()
        {
            MarkLifeStatus(LifeStatus.Remove);
            return true;
        }

        /// <summary>
        /// Mark Object Is Modify
        /// </summary>
        public virtual bool MarkModify()
        {
            MarkLifeStatus(LifeStatus.Modify);
            return true;
        }

        /// <summary>
        /// Mark Object Is Stored
        /// </summary>
        /// <returns></returns>
        public virtual bool MarkStored()
        {
            MarkLifeStatus(LifeStatus.Stored);
            return true;
        }

        /// <summary>
        /// Set Load Propertys
        /// </summary>
        /// <param name="loadPropertys">propertys</param>
        /// <returns></returns>
        public virtual void SetLoadPropertys(Dictionary<string, bool> loadPropertys)
        {
            if (loadPropertys == null)
            {
                return;
            }
            _allowLoadPropertys = _allowLoadPropertys ?? new Dictionary<string, bool>();
            foreach (var property in loadPropertys)
            {
                if (_allowLoadPropertys.ContainsKey(property.Key))
                {
                    _allowLoadPropertys[property.Key] = property.Value;
                }
                else
                {
                    _allowLoadPropertys.Add(property.Key, property.Value);
                }
            }
        }

        /// <summary>
        /// Set Load Propertys
        /// </summary>
        /// <param name="property">property</param>
        /// <param name="allowLoad">allow load</param>
        public virtual void SetLoadPropertys(Expression<Func<T, dynamic>> property, bool allowLoad = true)
        {
            if (property == null)
            {
                return;
            }
            Dictionary<string, bool> propertyDic = new Dictionary<string, bool>()
            {
                { ExpressionHelper.GetExpressionPropertyName(property.Body),allowLoad}
            };
            SetLoadPropertys(propertyDic);
        }

        /// <summary>
        /// Close Lazy Data Load
        /// </summary>
        public virtual void CloseLazyMemberLoad()
        {
            _loadLazyMember = false;
        }

        /// <summary>
        /// Open Lazy Data Load
        /// </summary>
        public virtual void OpenLazyMemberLoad()
        {
            _loadLazyMember = true;
        }

        /// <summary>
        /// Allow Lazy Load
        /// </summary>
        /// <param name="property">property</param>
        /// <returns>wheather allow load property</returns>
        protected virtual bool AllowLazyLoad(string property)
        {
            if (!_loadLazyMember || _allowLoadPropertys == null || !_allowLoadPropertys.ContainsKey(property))
            {
                return false;
            }
            return _allowLoadPropertys[property];
        }

        /// <summary>
        /// Allow Lazy Load
        /// </summary>
        /// <param name="property">property</param>
        /// <returns>wheather allow load property</returns>
        protected virtual bool AllowLazyLoad(Expression<Func<T, dynamic>> property)
        {
            if (property == null)
            {
                return false;
            }
            return AllowLazyLoad(ExpressionHelper.GetExpressionPropertyName(property.Body));
        }

        /// <summary>
        /// Save
        /// </summary>
        public virtual void Save()
        {
            SaveAsync().Wait();
        }

        /// <summary>
        /// Save
        /// </summary>
        public abstract Task SaveAsync();

        /// <summary>
        /// Remove
        /// </summary>
        public virtual void Remove()
        {
            RemoveAsync().Wait();
        }

        /// <summary>
        /// Remove
        /// </summary>
        public abstract Task RemoveAsync();

        /// <summary>
        /// Init From SimilarObject
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="similarObject">similar object</param>
        /// <returns></returns>
        public virtual void InitFromSimilarObject<DT>(DT similarObject) where DT : AggregationRoot<T>, T
        {
            if (similarObject == null)
            {
                return;
            }
            MarkLifeStatus(similarObject.LifeStatus);
            CopyDataFromSimilarObject(similarObject);//copy data
            //merge data
            if (similarObject.StoredData != null)
            {
                if (StoredData == null)
                {
                    StoredData = similarObject.StoredData;
                }
                else
                {
                    StoredData.CopyDataFromSimilarObject(similarObject.StoredData);
                }
            }
        }

        /// <summary>
        /// Copy Data From SimilarObject
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="similarObject">similar object</param>
        /// <param name="excludePropertys">not copy propertys</param>
        protected virtual void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null) where DT : T
        {
        }

        /// <summary>
        /// Init Primary Value
        /// </summary>
        public virtual void InitPrimaryValue()
        {

        }

        /// <summary>
        /// Primary Value Is None
        /// </summary>
        /// <returns></returns>
        public abstract bool PrimaryValueIsNone();

        /// <summary>
        /// Value Is Changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">property</param>
        /// <returns></returns>
        protected virtual bool ValueIsChanged(Func<T, dynamic> property)
        {
            if (property == null || StoredData == null)
            {
                return true;
            }
            var newValue = property((T)this);
            var oldValue = property(StoredData);
            if (newValue == null && oldValue == null)
            {
                return false;
            }
            if (newValue == null || oldValue == null)
            {
                return true;
            }
            return !newValue.Equals(oldValue);
        }

        #endregion
    }
}
