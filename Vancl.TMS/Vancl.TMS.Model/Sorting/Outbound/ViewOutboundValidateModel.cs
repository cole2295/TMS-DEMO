using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库View验证对象
    /// </summary>
    [Serializable]
    public class ViewOutboundValidateModel
    {
        private bool _isvalidate = false;
        private string _validatemsg;


        /// <summary>
        /// 当前操作人
        /// </summary>
        public SortCenterUserModel OpUser { get; set; }

        /// <summary>
        /// 分拣出库前置条件
        /// </summary>
        public OutboundPreConditionModel PreCondition { get; set; }

        public ViewOutboundValidateModel(SortCenterUserModel opuser, OutboundPreConditionModel precondition)
        {
            OpUser = opuser;
            PreCondition = precondition;
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
            if (PreCondition == null)
            {
                ValidateMsg = "未配置分拣业务流程!";
                return;
            }
            if (!OpUser.ExpressId.HasValue || OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                ValidateMsg = "请用分拣中心帐号操作入库!";
                return;
            }
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
