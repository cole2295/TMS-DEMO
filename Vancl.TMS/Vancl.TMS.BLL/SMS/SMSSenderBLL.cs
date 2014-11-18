using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.SMS;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.SMS;
using Vancl.TMS.BLL.SMSService;

namespace Vancl.TMS.BLL.SMS
{
    /// <summary>
    /// 短信发送
    /// </summary>
    public class SMSSenderBLL : BaseBLL, ISMSSender
    {
        #region ISMSSender 成员

        /// <summary>
        /// 短信系统名称
        /// </summary>
        private static readonly String DepartName = "凡客城际运输系统";

        /// <summary>
        /// 短信签名
        /// </summary>
        private static readonly String Signer = "如风达";

        /// <summary>
        /// 取得Key
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        private String GetKey(String PhoneNumber)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(PhoneNumber + "|COMMONMSG", "md5");
        }

        public ResultModel Send(SMSMessage msg)
        {
            ResultModel Result = new ResultModel();
            if (msg == null) throw new ArgumentNullException("msg is null");
            if (String.IsNullOrWhiteSpace(msg.FormCode))
            {
                throw new ArgumentNullException("无系统单号");
            }
            if (String.IsNullOrWhiteSpace(msg.PhoneNumber))
            {
                throw new ArgumentNullException("无手机号码");
            }
            if (String.IsNullOrWhiteSpace(msg.Content))
            {
                throw new ArgumentNullException("短信内容为空");
            }
            if (String.IsNullOrWhiteSpace(msg.Title))
            {
                throw new ArgumentNullException("短信标题为空");
            }
            try
            {
                using (SMSService.CommonMsgSoapClient sms = new CommonMsgSoapClient())
                {
                    sms.AddMsgBySign(msg.PhoneNumber, msg.Content, msg.Title, DepartName, Signer, GetKey(msg.PhoneNumber));
                }
                return Result.Succeed();
            }
            catch(Exception ex)
            {
                return Result.Failed(String.Format("短信接口异常：{0},发送不成功", ex.StackTrace));
            }
        }

        #endregion
    }
}
