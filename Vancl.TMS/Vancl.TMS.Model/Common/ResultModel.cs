using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// 操作返回结果对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class ResultModel
    {
        /// <summary>
        /// 不更新Lms ChangeLog 同步标志
        /// </summary>
        [DataMember]
        public bool NoUpdateLmsLog { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 成功或其他返回信息到客户端显示
        /// </summary>
        [DataMember]
        public object PromptMessage { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [IgnoreDataMember]
        [ScriptIgnore]
        public Exception Exception { get; set; }

        /// <summary>
        /// 静态创建结果对象
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel Create(bool isSuccess, string message = null, Exception ex = null)
        {
            return new ResultModel { IsSuccess = isSuccess, Message = message, Exception = ex };
        }
        /// <summary>
        /// 静态创建结果对象
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel<T> Create<T>(T data, bool isSuccess, string message = null, Exception ex = null)
        {
            return new ResultModel<T> { DataBag = data, IsSuccess = isSuccess, Message = message, Exception = ex };
        }

        /// <summary>
        /// 实例创建失败结果对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultModel Failed(String message, Exception ex = null)
        {
            this.IsSuccess = false;
            this.Message = message;
            this.Exception = ex;
            return this;
        }

        /// <summary>
        /// 实例创建成功结果对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultModel Succeed(String message,object promptMessage)
        {
            this.IsSuccess = true;
            this.Message = message;
            this.PromptMessage = promptMessage;
            return this;
        }

        /// <summary>
        /// 实例创建成功结果对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultModel Succeed(String message)
        {
            this.IsSuccess = true;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 实例创建成功结果对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultModel Succeed()
        {
            this.IsSuccess = true;
            return this;
        }

        /// <summary>
        /// 实例创建成功结果对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultModel Succeed(object promptMessage)
        {
            this.IsSuccess = true;
            this.PromptMessage = promptMessage;
            return this;
        }

        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        public virtual bool HasData()
        {
            return false;
        }
    }

    public class ResultModel<T> : ResultModel
    {
        public T DataBag { get; set; }
        public override bool HasData()
        {
            return DataBag != null;
        }
    }
}
