using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Security;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    [Serializable]
    public class ViewPackingBoxToModel
    {
        private int _selectedType;
        private string _selectStationValue;
        private string _validateMsg;
        private string _distributionCode;
        private Enums.CompanyFlag _companyFlag;
        private int _drrivalId;

        public ViewPackingBoxToModel(int selectedType, string selectStationValue)
        {
            _selectedType = selectedType;
            _selectStationValue = selectStationValue;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(_selectedType))
            {
                _validateMsg = "请选择站点类型!";
                return false;
            }
            if (string.IsNullOrEmpty(_selectStationValue) || _selectStationValue == "-1")
            {
                _validateMsg = "请选择站点!";
                return false;
            }

            string[] arrDecryptValue = null;
            if (_selectedType != (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                arrDecryptValue = DES.Decrypt3DES(_selectStationValue).Split(';');
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    _validateMsg = "数据解密错误!";
                    return false;
                }

                _drrivalId = Convert.ToInt32(arrDecryptValue[0]);
                _companyFlag = (Enums.CompanyFlag)int.Parse(arrDecryptValue[1]);
                _distributionCode = arrDecryptValue[2];
            }

            if (_selectedType == 0)
            {
                _companyFlag = Enums.CompanyFlag.DistributionStation;
                _drrivalId = Convert.ToInt32(_selectStationValue);
            }

            return true;
        }

        /// <summary>
        /// 验证结果
        /// </summary>
        public string ValidateMsg
        {
            get { return _validateMsg; }
        }

        /// <summary>
        /// 配送商
        /// </summary>
        public string DistributionCode
        {
            get { return _distributionCode; }
        }

        /// <summary>
        /// 目的地站点
        /// </summary>
        public int ArrivalId
        {
            get { return _drrivalId; }
        }

        public Enums.CompanyFlag CompanyFlag
        {
            get
            {
                return _companyFlag;
            }
        }
    }
}
