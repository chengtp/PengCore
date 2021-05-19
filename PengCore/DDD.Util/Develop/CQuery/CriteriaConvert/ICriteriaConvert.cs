using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Util.CQuery.CriteriaConvert
{
    public interface ICriteriaConvert
    {
        /// <summary>
        /// convert type
        /// </summary>
        CriteriaConvertType Type { get; set; }
    }
}
