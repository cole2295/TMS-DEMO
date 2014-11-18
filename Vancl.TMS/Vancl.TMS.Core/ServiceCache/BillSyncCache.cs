using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Core.ServiceCache
{
    public class BillSyncCache
    {
        private Dictionary<string, BillSyncCacheModel> _dic = null;
        private static BillSyncCache _cache = null;
        private static readonly object _lockObj = new object();
        private BillSyncCache()
        {
            _dic = new Dictionary<string, BillSyncCacheModel>();
        }

        /// <summary>
        /// 获得单例对象
        /// </summary>
        /// <returns></returns>
        public static BillSyncCache GetInstance()
        {
            if (_cache == null)
            {
                lock (_lockObj)
                {
                    if (_cache == null)
                    {
                        _cache = new BillSyncCache();
                    }
                }
            }
            return _cache;
        }

        /// <summary>
        /// 根据单号获得缓存对象
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public BillSyncCacheModel Get(string formCode)
        {
            if (_dic.ContainsKey(formCode))
            {
                return _dic[formCode];
            }
            BillSyncCacheModel model = new BillSyncCacheModel();
            _dic.Add(formCode, model);
            return model;
        }

        /// <summary>
        /// 根据单号获得处理次数
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public int GetTryTimes(string formCode)
        {
            return Get(formCode).TryTimes;
        }

        /// <summary>
        /// 根据单号获得错误列表
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public List<Exception> GetExceptions(string formCode)
        {
            return Get(formCode).Exceptions;
        }

        /// <summary>
        /// 根据单号获得最后一个错误
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public Exception GetLastException(string formCode)
        {
            List<Exception> list = Get(formCode).Exceptions;
            if (list.Count > 0)
            {
                return list.Last();
            }
            return null;
        }

        /// <summary>
        /// 根据单号获得错误内容
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public string GetExceptionMessage(string formCode)
        {
            List<Exception> list = Get(formCode).Exceptions;
            if (list.Count > 0)
            {
                return list.Last().Message;
            }
            return "";
        }

        /// <summary>
        /// 设置缓存订单处理次数
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="tryTimes"></param>
        public void SetTryTimes(string formCode, int tryTimes)
        {
            if (_dic.ContainsKey(formCode))
            {
                _dic[formCode].TryTimes = tryTimes;
            }
            else
            {
                _dic.Add(formCode, new BillSyncCacheModel() { TryTimes = tryTimes });
            }
        }

        /// <summary>
        /// 设置缓存订单错误列表
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="ex"></param>
        public void SetExceptions(string formCode, Exception ex)
        {
            if (!_dic.ContainsKey(formCode))
            {
                _dic.Add(formCode, new BillSyncCacheModel());
            }
            _dic[formCode].Exceptions.Add(ex);
        }

        /// <summary>
        /// 移除运单
        /// </summary>
        /// <param name="formCode"></param>
        public void RemoveBill(string formCode)
        {
            if (_dic.ContainsKey(formCode))
            {
                _dic.Remove(formCode);
            }
        }

        /// <summary>
        /// 是否有运单
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public bool HasBill(string formCode)
        {
            return _dic.ContainsKey(formCode);
        }
    }
}
