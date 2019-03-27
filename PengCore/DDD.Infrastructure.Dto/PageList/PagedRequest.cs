using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.PageList
{
    /// <summary>
    /// 请求分页实体
    /// </summary>
    public class PagedRequest
    {
        /// <summary>
        /// 字段名(全部字段为*)
        /// </summary>
        public string FieldsStr { get; set; } = "*";
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Order { get; set; } = "CreateDate";
        /// <summary>
        /// 条件语句(不用加where)
        /// </summary>
        public string WhereString { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; } = 10;

    }
}
