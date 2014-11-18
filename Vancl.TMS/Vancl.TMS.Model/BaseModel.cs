using System;
using Vancl.TMS.Model.DbAttributes;

namespace Vancl.TMS.Model
{
    [Serializable]
    public class BaseModel
    {
        /// <summary>
        /// 实体对应表名
        /// </summary>
        public virtual string ModelTableName { get { return "TMS_" + this.GetType().Name.Replace("Model", ""); } }

        public override string ToString()
        {
            return this.GetType().Name;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column]
        public int CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public int UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        [Column]
        public bool IsDeleted { get; set; }
    }
}
