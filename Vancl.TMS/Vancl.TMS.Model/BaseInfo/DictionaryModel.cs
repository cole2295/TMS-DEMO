using System;

namespace Vancl.TMS.Model.BaseInfo
{
    [Serializable]
    public class DictionaryModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long DID { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>
        public int DicType { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        public int DicCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string DicName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DicDesc { get; set; }
        /// <summary>
        /// 顺序号(排序用)
        /// </summary>
        public int SeqNo { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DICTIONARY_DID"; }
        }

        #endregion
    }
}
