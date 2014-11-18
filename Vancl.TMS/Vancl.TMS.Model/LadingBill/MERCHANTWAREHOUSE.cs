using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LadingBill
{
    /// <summary>
    /// 商家仓库表
    /// </summary>
    [Serializable]
    public partial class MERCHANTWAREHOUSE
    {
        public MERCHANTWAREHOUSE()
        { }
        #region Model
        private decimal _warehouseid;
        private string _warehousecode;
        private string _warehousename;
        private string _warehouseaddress;
        private decimal _merchantid;
        private string _provinceid;
        private string _cityid;
        private string _areaid;
        private string _linkman;
        private string _phone;
        private string _fax;
        private string _email;
        private decimal? _warehousetype;
        private decimal? _sendgoodsseq;
        private decimal _isdeleted;
        private decimal? _createby;
        private DateTime _createtime;
        private decimal? _updateby;
        private DateTime _updatetime;

        private string _merchantname;

        public string MERCHANTNAME
        {
            set { _merchantname = value; }
            get { return _merchantname; }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public decimal WAREHOUSEID
        {
            set { _warehouseid = value; }
            get { return _warehouseid; }
        }
        /// <summary>
        /// 商家仓库编号
        /// </summary>
        public string WAREHOUSECODE
        {
            set { _warehousecode = value; }
            get { return _warehousecode; }
        }
        /// <summary>
        /// 商家仓库名称
        /// </summary>
        public string WAREHOUSENAME
        {
            set { _warehousename = value; }
            get { return _warehousename; }
        }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public string WAREHOUSEADDRESS
        {
            set { _warehouseaddress = value; }
            get { return _warehouseaddress; }
        }
        /// <summary>
        /// 商家ID
        /// </summary>
        public decimal MERCHANTID
        {
            set { _merchantid = value; }
            get { return _merchantid; }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public string PROVINCEID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string CITYID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 区县
        /// </summary>
        public string AREAID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LINKMAN
        {
            set { _linkman = value; }
            get { return _linkman; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PHONE
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string FAX
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EMAIL
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 仓库类型
        /// </summary>
        public decimal? WAREHOUSETYPE
        {
            set { _warehousetype = value; }
            get { return _warehousetype; }
        }
        /// <summary>
        /// 发货顺序
        /// </summary>
        public decimal? SENDGOODSSEQ
        {
            set { _sendgoodsseq = value; }
            get { return _sendgoodsseq; }
        }
        /// <summary>
        /// 有效标识（0、可用1、不可用）
        /// </summary>
        public decimal ISDELETED
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public decimal? CREATEBY
        {
            set { _createby = value; }
            get { return _createby; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        public decimal? UPDATEBY
        {
            set { _updateby = value; }
            get { return _updateby; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UPDATETIME
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        #endregion Model

    }
}
