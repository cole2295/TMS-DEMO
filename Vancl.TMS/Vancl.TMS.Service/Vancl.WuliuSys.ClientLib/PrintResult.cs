using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Vancl.WuliuSys.ClientLib
{
    /// <summary>
    /// 方法返回结果信息类
    /// </summary>
    [Serializable]
    [DataContract]
    public class PrintModel
    {
        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="dataBag"></param>
        public PrintModel(ResultType result, string message = null, Exception exception = null, object dataBag = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "未能成功执行当前操作！";
            }

            this.Result = result;
            this.Message = message;
            this.Exception = exception;
            this.DataBag = dataBag;
        }
        /// <summary>
        /// 默认参数为0的构造函数（默认返回值为"失败"）
        /// </summary>
        public PrintModel()
            : this(ResultType.Failed)
        {

        }

        #endregion

        #region 静态创建

        /// <summary>
        /// 静态创建方法
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="dataBag"></param>
        /// <returns></returns>
        public static PrintModel Create(ResultType result, string message = null, Exception exception = null, object dataBag = null)
        {
            return new PrintModel(result, message, exception, dataBag);
        }

        public static PrintModel Create(bool isSuccess, string message = null, Exception exception = null, object dataBag = null)
        {
            return Create(isSuccess ? ResultType.Success : ResultType.Failed, message, exception, dataBag);
        }

        /// <summary>
        /// 创建成功返回
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="dataBag"></param>
        /// <returns></returns>
        public static PrintModel CreateSuccessResult(string message = null, Exception exception = null, object dataBag = null)
        {
            return new PrintModel(ResultType.Success, message, exception, dataBag);
        }

        /// <summary>
        /// 创建失败返回
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="dataBag"></param>
        /// <returns></returns>
        public static PrintModel CreateFailedResult(string message = null, Exception exception = null, object dataBag = null)
        {
            return new PrintModel(ResultType.Failed, message, exception, dataBag);
        }

        /// <summary>
        /// 创建未知返回
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="dataBag"></param>
        /// <returns></returns>
        public static PrintModel CreateUnknownResult(string message = null, Exception exception = null, object dataBag = null)
        {
            return new PrintModel(ResultType.Unknown, message, exception, dataBag);
        }


        #endregion

        #region 属性

        /// <summary>
        /// 返回消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        [DataMember]
        public ResultType Result { get; set; }

        //  [NonSerialized]
        private Exception _exception;

        /// <summary>
        /// 异常信息
        /// </summary>
        [DataMember]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
            }
        }

        /// <summary>
        /// 动态数据
        /// </summary>
      //  [DataMember]
        public object DataBag { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>  
        [DataMember]
        public bool IsSuccess
        {
            get
            {
                return Result == ResultType.Success;
            }
            set
            {

            }
        }

        /// <summary>
        /// 是否抛出有异常
        /// </summary>
        public bool IsThrowException
        {
            get
            {
                return Exception != null;
            }
        }

        #endregion

        public override string ToString()
        {
            return base.ToString();
           // DataContractJsonSerializer json = new DataContractJsonSerializer(this.GetType());
           // Stream stream = new MemoryStream();
           // json.WriteObject(stream, this);
           // stream.Position = 0;
           //StreamReader reader = new StreamReader(stream);
           //stream.Position = 0;
           // string text = reader.ReadToEnd();
           // return text;
        }
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    [DataContract]
    public enum ResultType : int
    {
        /// <summary>
        /// 失败
        /// </summary>
        [EnumMember]
        Failed = -1,

        /// <summary>
        /// 未知
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [EnumMember]
        Success = 1,
    }
}
