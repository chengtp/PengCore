using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.DataValidation.Validators
{
    /// <summary>
    /// Compare Operator Value
    /// </summary>
    public class CompareOperatorValue
    {
        #region Propertys

        /// <summary>
        /// Source Value
        /// </summary>
        public dynamic SourceValue
        {
            get;set;
        }

        /// <summary>
        /// Compare Value
        /// </summary>
        public dynamic CompareValue
        {
            get;set;
        }

        #endregion
    }
}
