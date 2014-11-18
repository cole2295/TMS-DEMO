using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// 动态字典类
    /// </summary>
    public class DynamicDictionaryModel : DynamicObject
    {
        private Dictionary<string, object> _dic = new Dictionary<string, object>();
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dic.ContainsKey(binder.Name))
            {
                _dic[binder.Name] = value;
            }
            else
            {
                _dic.Add(binder.Name, value);
            }
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dic.ContainsKey(binder.Name))
            {
                result = _dic[binder.Name];
            }
            else
            {
                result = null;
            }
            return true;
        }
    }
}
