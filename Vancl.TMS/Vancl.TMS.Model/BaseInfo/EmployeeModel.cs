using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo
{
    public class EmployeeModel
    {
        /// <summary>
        /// 员工编号（内码）
        /// </summary>
        public int EmployeeID { get; set; }

        /// <summary>
        /// 员工代号
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 员工代号（原来老的编号）
        /// </summary>
        public string EmployeeOldCode { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }
    }
}
