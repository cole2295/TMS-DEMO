using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.Core.Logging
{
    public class UpdateLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable, new()
    {
        public override string GetLogNote()
        {
            if (!string.IsNullOrEmpty(_note))
            {
                return _note;
            }
            if (_nowModel is IForceLog)
            {
                var custAttr = typeof(T).GetCustomAttributes(typeof(LogNameAttribute), false);
                if (custAttr != null && custAttr.Length > 0)
                {
                    _note = string.Format(Consts.NULL_UPDATE_OPERATE_LOG_NOTE, (custAttr[0] as LogNameAttribute).Name);
                }
                return _note;
            }
            if (_nowModel == null || _pastModel == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] pisNow = _nowModel.GetType().GetProperties();
            object oNow;
            object oPast;
            foreach (PropertyInfo pi in pisNow)
            {
                if (Consts.OPERATE_LOG_NOT_COMPARE_COLUMNS.ToLower().Contains(pi.Name.ToLower()))
                {
                    continue;
                }
                var custNotlogAttr = pi.GetCustomAttributes(typeof(ColumnNot4LogAttribute), false);
                if (custNotlogAttr != null && custNotlogAttr.Length > 0)
                {
                    continue;
                }
                oNow = pi.GetValue(_nowModel, null);
                oPast = pi.GetValue(_pastModel, null);
                if (!ValueEquals(pi, oNow, oPast))
                {
                    ConvertToLogValue(pi, ref oNow, ref oPast);
                    var custlogAttr = pi.GetCustomAttributes(typeof(LogNameAttribute), false);
                    if (custlogAttr == null || custlogAttr.Length <= 0)
                    {
                        sb.AppendFormat(Consts.UPDATE_OPERATE_LOG_NOTE, pi.Name, oPast, oNow);
                        continue;
                    }
                    sb.AppendFormat(Consts.UPDATE_OPERATE_LOG_NOTE, (custlogAttr[0] as LogNameAttribute).Name, oPast, oNow);
                }
            }
            _note = sb.ToString();
            return _note;
        }

        /// <summary>
        /// 值比较
        /// </summary>
        /// <param name="pi">属性对象</param>
        /// <param name="nowValue">当前操作对象</param>
        /// <param name="pastValue">前一操作对象</param>
        /// <returns></returns>
        private bool ValueEquals(PropertyInfo pi,  object nowValue,  object pastValue)
        {
            Type tp = pi.PropertyType;
            if (tp == typeof(DateTime)
                || tp == typeof(DateTime?))
            {
                var logTimeCompare = pi.GetCustomAttributes(typeof(LogTimeCompareAttribute), false);
                if (logTimeCompare != null && logTimeCompare.Length > 0)
                {
                    //仅当都有值的时候采用时间部分对比，否者默认对象对比方式
                    if (nowValue != null && pastValue != null)
                    {
                        DateTime nowtime = DateTime.Parse(nowValue.ToString());
                        DateTime pasttime = DateTime.Parse(pastValue.ToString());
                        return nowtime.TimeOfDay.Equals(pasttime.TimeOfDay);
                    }
                }
                var logDateCompare = pi.GetCustomAttributes(typeof(LogDateCompareAttribute), false);
                if (logDateCompare != null && logDateCompare.Length > 0)
                {
                    //仅当都有值的时候采用日期部分对比，否者默认对象对比方式
                    if (nowValue != null && pastValue != null)
                    {
                        DateTime nowdate = DateTime.Parse(nowValue.ToString());
                        DateTime pastdate = DateTime.Parse(pastValue.ToString());
                        return nowdate.Date.Equals(pastdate.Date);
                    }
                }
            }
            if (tp == typeof(String))
            {
                //如果前对象和当前对象都是null或者空白字符则对比认为相同
                if ( (nowValue == null || String.IsNullOrWhiteSpace(nowValue.ToString()))
                    && (pastValue == null || String.IsNullOrWhiteSpace(pastValue.ToString()))
                    )
                {
                    return true;
                }
            }
            return Equals(nowValue,pastValue);
        }

        /// <summary>
        /// 转换为日志记录值
        /// </summary>
        /// <param name="pi">实体对象属性</param>
        /// <param name="nowValue"></param>
        /// <param name="pastValue">修改之前的Value</param>
        private void ConvertToLogValue(PropertyInfo pi, ref object nowValue, ref object pastValue)
        {
            Type tp = pi.PropertyType;
            if (tp == typeof(bool))
            {
                nowValue = Convert.ToBoolean(nowValue) ? "是" : "否";
                pastValue = Convert.ToBoolean(pastValue) ? "是" : "否";
                return;
            }
            if (tp.IsEnum)
            {
                nowValue = EnumHelper.GetDescription(tp, (int)nowValue);
                pastValue = EnumHelper.GetDescription(tp, (int)pastValue);
                return;
            }
            if (tp == typeof(DateTime)
                || tp == typeof(DateTime?))
            {
                var logTimeCompare = pi.GetCustomAttributes(typeof(LogTimeCompareAttribute), false);
                if (logTimeCompare != null && logTimeCompare.Length > 0)
                {
                    if (nowValue != null)
                    {
                        DateTime nowtime = DateTime.Parse(nowValue.ToString());
                        nowValue = nowtime.ToString("HH:mm");
                    }
                    if (pastValue != null)
                    {
                        DateTime pasttime = DateTime.Parse(pastValue.ToString());
                        pastValue = pasttime.ToString("HH:mm");
                    }
                    return;
                }
                var logDateCompare = pi.GetCustomAttributes(typeof(LogDateCompareAttribute), false);
                if (logDateCompare != null && logDateCompare.Length > 0)
                {
                    if (nowValue != null)
                    {
                        DateTime nowdate = DateTime.Parse(nowValue.ToString());
                        nowValue = nowdate.ToString("yyyy-MM-dd");
                    }
                    if (pastValue != null)
                    {
                        DateTime pastdate = DateTime.Parse(pastValue.ToString());
                        pastValue   = pastdate.ToString("yyyy-MM-dd");
                    }
                    return;
                }
            }
            //other extend

        }

        public override bool IsDoOperation()
        {
            if (typeof(T).Name == typeof(IForceLog).Name)
            {
                return true;
            }
            if (string.IsNullOrEmpty(GetLogNote()))
            {
                return false;
            }
            return true;
        }
    }
}
