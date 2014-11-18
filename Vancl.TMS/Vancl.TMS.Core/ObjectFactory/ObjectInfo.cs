using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Core.ObjectFactory
{
    internal class ObjectInfo
    {
        private string _name;
        private string _interfaceName;
        private string _classFullName;
        private string _loadFrom;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                    throw new Exception("名称不能为空!");
                else
                    _name = value;
            }
        }

        /// <summary>
        /// 接口
        /// </summary>
        public string InterFaceName
        {
            get { return _interfaceName; }
            set
            {
                if (value == null)
                    throw new Exception("接口不能为空!");
                else
                    _interfaceName = value;
            }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassFullName
        {
            get { return _classFullName; }
            set
            {
                if (value == null)
                    throw new Exception("类名不能为空!");
                else
                    _classFullName = value;
            }
        }

        /// <summary>
        /// 程序集加载路径
        /// </summary>
        public string LoadFrom
        {
            get { return _loadFrom; }
            set
            {
                if (value == null)
                    throw new Exception("Load路径不能为空!");
                else
                    _loadFrom = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Type ClassType { get; set; }
    }
}
