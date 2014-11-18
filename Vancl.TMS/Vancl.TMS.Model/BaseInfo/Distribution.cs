using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo
{
    public class Distribution : BaseModel
    {
        public string ModelTableName { get { return "ps_pms." + this.GetType().Name.Replace("Model", ""); } }

        /// <summary>
        /// 主键
        /// </summary>		
        private Int32 _distributionid;
        public Int32 DistributionID
        {
            get { return _distributionid; }
            set { _distributionid = value; }
        }
        /// <summary>
        /// 配送商注册编码
        /// </summary>		
        private String _distributioncode;
        public String DistributionCode
        {
            get { return _distributioncode; }
            set { _distributioncode = value; }
        }
        /// <summary>
        /// 配送商名称
        /// </summary>		
        private String _distributionname;
        public String DistributionName
        {
            get { return _distributionname; }
            set { _distributionname = value; }
        }
        /// <summary>
        /// 配送商全称
        /// </summary>		
        private String _distributionallname;
        public String DistributionAllName
        {
            get { return _distributionallname; }
            set { _distributionallname = value; }
        }
        /// <summary>
        /// 拼音简写
        /// </summary>		
        private String _distributionspell;
        public String DistributionSpell
        {
            get { return _distributionspell; }
            set { _distributionspell = value; }
        }
        /// <summary>
        /// 所在大区
        /// </summary>		
        private String _districtid;
        public String DistrictID
        {
            get { return _districtid; }
            set { _districtid = value; }
        }
        /// <summary>
        /// 所在省份
        /// </summary>		
        private String _provinceid;
        public String ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        /// 所在城市
        /// </summary>		
        private String _cityid;
        public String CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>		
        private String _address;
        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// 主要联系人
        /// </summary>		
        private String _linkman;
        public String LinkMan
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        /// <summary>
        /// 联系人手机
        /// </summary>		
        private String _cellphone;
        public String CellPhone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }
        /// <summary>
        /// 联系人固定电话
        /// </summary>		
        private String _telphone;
        public String TelPhone
        {
            get { return _telphone; }
            set { _telphone = value; }
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>		
        private String _email;
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// 最大峰值
        /// </summary>		
        private String _limitnum;
        public String LimitNum
        {
            get { return _limitnum; }
            set { _limitnum = value; }
        }
        /// <summary>
        /// LOGO的存放路径URL
        /// </summary>		
        private String _distributionlogo;
        public String DistributionLogo
        {
            get { return _distributionlogo; }
            set { _distributionlogo = value; }
        }
        /// <summary>
        /// 合作级别0站点级1城市级2区域级
        /// </summary>		
        private Int32? _distributionlevel;
        public Int32? DistributionLevel
        {
            get { return _distributionlevel; }
            set { _distributionlevel = value; }
        }
        /// <summary>
        /// 停用标志
        /// </summary>		
        private Boolean? _isdelete;
        public Boolean? IsDelete
        {
            get { return _isdelete; }
            set { _isdelete = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private System.DateTime? _creattime;
        public System.DateTime? CreatTime
        {
            get { return _creattime; }
            set { _creattime = value; }
        }
        /// <summary>
        /// 创建人
        /// </summary>		
        private Int32? _creatby;
        public Int32? CreatBy
        {
            get { return _creatby; }
            set { _creatby = value; }
        }
        /// <summary>
        /// 创建人所在部门
        /// </summary>		
        private Int32? _creatstation;
        public Int32? CreatStation
        {
            get { return _creatstation; }
            set { _creatstation = value; }
        }
        /// <summary>
        /// 更新人
        /// </summary>		
        private Int32? _updateby;
        public Int32? UpdateBy
        {
            get { return _updateby; }
            set { _updateby = value; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>		
        private System.DateTime? _updatetime;
        public System.DateTime? UpdateTime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }
        /// <summary>
        /// 更新人所在部门
        /// </summary>		
        private Int32? _updatestation;
        public Int32? UpdateStation
        {
            get { return _updatestation; }
            set { _updatestation = value; }
        }


        /// <summary>
        /// GIS使用情况.0不分配1gis2手工
        /// </summary>		
        private Int32? _isSupportGIS;
        public Int32? IsSupportGIS
        {
            get { return _isSupportGIS; }
            set { _isSupportGIS = value; }
        }

        private String _distributionGroupCode;
        public String DistributionGroupCode
        {
            get { return _distributionGroupCode; }
            set { _distributionGroupCode = value; }
        }

        /// <summary>
        /// 配送商模式
        /// </summary>
        public int DistributionType
        {
            get;
            set;
        }
    }
}
