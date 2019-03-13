using System;

namespace DDD.Domain
{
    public class BaseModel : BaseModelCreate
    {
        /// <summary>
        /// 是否删除；0: 正常;1:删除,默认0正常
        /// </summary>
        public int Deleted { get; set; }
        /// <summary>
        /// 排序码，默认0
        /// </summary>
        public int SortNum { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditDate { get; set; }
        /// <summary>
        /// 编辑人唯一标识
        /// </summary>
        public Guid EditerId { get; set; }
        /// <summary>
        /// 编辑人名称
        /// </summary>
        public string EditerName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
