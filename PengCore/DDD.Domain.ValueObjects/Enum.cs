using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain.ValueObjects
{
    public enum Status
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 等待
        /// </summary>
        Wait = 1,

        /// <summary>
        /// 通过
        /// </summary>
        Pass = 2,

        /// <summary>
        /// 拒绝
        /// </summary>
        Deny = 3,

        /// <summary>
        /// 锁定
        /// </summary>
        Lock = 4,
    }
}
