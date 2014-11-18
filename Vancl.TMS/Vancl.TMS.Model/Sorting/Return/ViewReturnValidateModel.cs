using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 退货View验证对象
    /// </summary>
    [Serializable]
    public class ViewReturnValidateModel
    {
        private bool _isvalidate = false;
        private string _validatemsg;


        /// <summary>
        /// 当前操作人
        /// </summary>
        public SortCenterUserModel OpUser { get; set; }

        public ViewReturnValidateModel(SortCenterUserModel opuser)
        {
            OpUser = opuser;
            Validate();
        }

        /// <summary>
        /// 验证
        /// </summary>
        private void Validate()
        {
            if (OpUser == null)
            {
                ValidateMsg = "当前操作人不存在!";
                return;
            }
            //if (!OpUser.ExpressId.HasValue || OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            //{
            //    ValidateMsg = "请用分拣中心帐号操作退货!";
            //    return;
            //}
            //初始化验证成功
            IsValidate = true;
        }

        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _isvalidate;
            }
            private set
            {
                _isvalidate = value;
            }
        }

        /// <summary>
        /// 验证消息
        /// </summary>
        public string ValidateMsg
        {
            get
            {
                return _validatemsg;
            }
            private set
            {
                _validatemsg = value;
            }
        }
    }
}
