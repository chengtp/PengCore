using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Common
{
    public class PageParamters
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Order { get; set; }
    }
}
